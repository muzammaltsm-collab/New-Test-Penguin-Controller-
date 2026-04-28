using System;
using System.Collections.Generic;
using ByteBrewSDK;
using GoogleMobileAds.Api;
using UnityEngine;

public class RewardedInterstitial : MonoBehaviour
{
    private RewardedInterstitialAd AdView;
    public List<string> AdUnitID;
    private int AdCount = 0;
    private bool AdLoading = false;
    public bool isRewarded = false;

    public bool IsAdAvailable => AdView != null;

    // Method to check if the rewarded interstitial ad is available
    public bool IsRewardedInterstitialAvailable()
    {
        return IsAdAvailable;
    }

    public void LoadAd()
    {
        AdLoading = true;
        AdView = null;

        // Create an AdRequest directly
        AdRequest request = new AdRequest();
        RewardedInterstitialAd.Load(AdUnitID[AdCount], request, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                if (AdCount < AdUnitID.Count - 1)
                {
                    AdCount++;
                    LoadAd();
                }
                else
                {
                    AdLoading = false;
                    AdCount = 0;
                }
                return;
            }
            AdView = ad;
            RegisterEventHandlers(AdView);
        });
    }

    public void ShowAd(Action _Reward)
    {
        if (IsRewardedInterstitialAvailable())
        {
            RewardHandle = _Reward;
            AdLoading = false;
            AdView.Show((GoogleMobileAds.Api.Reward reward) => { isRewarded = true; });
           // Debug.LogError("Showing Ad");
        }

        if (!AdLoading)
        {
            AdCount = 0;
            LoadAd();
        }
    }

    public Action RewardHandle;
    private void Update()
    {

        if (isRewarded)
        {
            isRewarded = false;
            if (RewardHandle != null)
            {
                RewardHandle.Invoke();
            }

        }


    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
      
            AdapterResponseInfo loadedAdapterResponseInfo = AdView.GetResponseInfo().GetLoadedAdapterResponseInfo();

         


            Debug.Log("Admobe_Reward_Revnue");
        
    }
}