using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using ByteBrewSDK;
using FirebaseSDKAdType;
public class Interstitial : MonoBehaviour
{

    // Start is called before the first frame update
    #region OpenAd

    private InterstitialAd AdView;
    public List<String> AdUnitID;
    int AdCount = 0;
    bool Consent = true;
    bool AdLoading = false;
    private static DateTime Time1ForAds;
    bool isInterClosed;
    Action InterCloseHandle;
    private void Start()
    {
        //   AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }
    //private void OnAppStateChanged(AppState state)
    //{
    //    // Display the app open ad when the app is foregrounded.
    //    UnityEngine.Debug.Log("App State is " + state);
    //    if (state == AppState.Foreground)
    //    {
    //        ShowAd();
    //    }
    //}
    public bool IsAdAvailable
    {
        get
        {
            return AdView != null;
        }
    }



    public void LoadAd()
    {



        AdLoading = true;
        AdView = null;
        AdRequest request = new AdRequest();
        // Load an app open ad for portrait orientation
        InterstitialAd.Load(AdUnitID[AdCount], request,
        (InterstitialAd ad, LoadAdError error) =>
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
                    // AdsManagerWrapper.Instance.Log("App Open Failed to Load");
                    AdLoading = false;
                    AdCount = 0;
                }

                return;

            }
            //AdsManagerWrapper.Instance.Log("Open Ad Loaded ID Number" + AdCount);
            // App open ad is loaded.
            AdView = ad;
            RegisterEventHandlers(AdView);
        });
    }
    public void ShowAd(Action _closeAction = null)
    {

        if (IsAdAvailable)
        {
            try
            {
                AdsHandler.Instance.ShowAdBackground();
                InterCloseHandle = _closeAction;
                AdsHandler.Instance.AdShown = true;
                AdLoading = false;

                if (AdView != null)
                {
                    Debug.Log("AdView is not null. Attempting to show ad...");

                    AdView.Show();

                    Debug.Log("AdView.Show() called successfully.");
                }
                else
                {
                    Debug.LogError("AdView is null. Cannot show interstitial ad.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during AdView.Show(): " + ex);
            }
        }
        else
        {
            Debug.LogWarning("IsAdAvailable is false. Ad not shown.");
        }
        if (!AdLoading)
        {
            AdCount = 0;
            LoadAd();
        }
    }

    private void Update()
    {

        if (isInterClosed)
        {
            isInterClosed = false;
            if (InterCloseHandle != null)
            {
                InterCloseHandle.Invoke();
            }

        }


    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {


        ad.OnAdFullScreenContentClosed += () =>
        {
            isInterClosed = true;
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {

            AdapterResponseInfo loadedAdapterResponseInfo = ad.GetResponseInfo().GetLoadedAdapterResponseInfo();

            FbAnalytics.Instance.HandleOnPaidEvent(adValue, FirebaseAdType.Interstitial);
            ByteBrewManager.Instance.LogAdsAnaytics_Admobe(ByteBrewAdTypes.Interstitial, loadedAdapterResponseInfo, adValue);
            ByteBrewManager.Instance.LogAdsType_Admobe(ByteBrewAdTypes.Interstitial, loadedAdapterResponseInfo, adValue);

            Debug.Log("Admobe_Interstial_Revnue");

        };
    }

    #endregion
}