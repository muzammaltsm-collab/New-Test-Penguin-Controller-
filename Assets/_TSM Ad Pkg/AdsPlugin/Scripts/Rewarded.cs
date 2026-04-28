using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using ByteBrewSDK;
using FirebaseSDKAdType;

public class Rewarded : MonoBehaviour
{
    private RewardedAd AdView;
    public List<string> AdUnitID;
    private int AdCount = 0;
    private bool AdLoading = false;

    private Action rewardCallback;
    private Action noRewardCallback;

    // Tracks whether reward flow has already been processed (granted or denied).
    private bool rewardProcessed = false;

    private void Start()
    {
        LoadAd();
    }

    public bool IsAdAvailable => AdView != null && AdView.CanShowAd();

    public void LoadAd()
    {
        if (AdLoading) return;

        AdLoading = true;
        AdView = null;

        AdRequest request = new AdRequest();

        RewardedAd.Load(AdUnitID[AdCount], request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogWarning("Rewarded failed to load. Trying next ID.");

                if (AdCount < AdUnitID.Count - 1)
                {
                    AdCount++;
                    LoadAd();
                }
                else
                {
                    Debug.LogError("All Rewarded Ad IDs Failed.");
                    AdCount = 0;
                    AdLoading = false;
                }

                return;
            }

            AdView = ad;
            RegisterEventHandlers(ad);
            Debug.Log("Rewarded Loaded Successfully: " + AdCount);

            AdLoading = false;
            AdCount = 0;
        });
    }

    public void ShowAd(Action _Reward, Action _NoReward)
    {
        rewardCallback = _Reward;
        noRewardCallback = _NoReward;

        if (!IsAdAvailable)
        {
            Debug.Log("Rewarded NOT Available.");
            noRewardCallback?.Invoke();
            LoadAd();
            return;
        }

        try
        {
            AdsHandler.Instance.ShowAdBackground();
            AdsHandler.Instance.AdShown = true;

            // reset processed flag for this ad show flow
            rewardProcessed = false;

            AdView.Show((GoogleMobileAds.Api.Reward reward) =>
            {
                // Reward callback — immediately process reward to avoid race with close event.
                rewardProcessed = true;
                Debug.Log("Rewarded Granted Callback!");
                try
                {
                    rewardCallback?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Exception invoking reward callback: " + ex);
                }
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception Showing Rewarded Ad: " + ex);
            noRewardCallback?.Invoke();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Closed");

            // If reward flow wasn't processed (no reward granted), call noRewardCallback.
            if (!rewardProcessed)
            {
                try
                {
                    noRewardCallback?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Exception invoking no-reward callback: " + ex);
                }
            }

            LoadAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded Failed to Show: " + error);
            try
            {
                noRewardCallback?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception invoking no-reward callback: " + ex);
            }
            LoadAd();
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {
            var response = ad.GetResponseInfo().GetLoadedAdapterResponseInfo();

            FbAnalytics.Instance.HandleOnPaidEvent(adValue, FirebaseAdType.Reward);
            ByteBrewManager.Instance.LogAdsAnaytics_Admobe(ByteBrewAdTypes.Reward, response, adValue);
            ByteBrewManager.Instance.LogAdsType_Admobe(ByteBrewAdTypes.Reward, response, adValue);

            Debug.Log("Rewarded Revenue Logged");
        };
    }
}
