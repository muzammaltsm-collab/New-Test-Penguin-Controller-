using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.UI;

public class AppOpen : MonoBehaviour
{
    // Start is called before the first frame update
    #region OpenAd

    private AppOpenAd AdView;
    public List<String> AdUnitID;
    int AdCount = 0;
    bool Consent = true;
    [HideInInspector]
    public bool AdLoading = false;



    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            ShowAd();
        }
    }
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
        //  AdsManagerWrapper.Instance.Log("Open Ad Loading ID Number" + AdCount);
        // Load an app open ad for portrait orientation
        AppOpenAd.Load(AdUnitID[AdCount], request, ((appOpenAd, error) =>
        {
            if (error != null)
            {

                // Handle the error.
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
            //  AdsManagerWrapper.Instance.Log("Open Ad Loaded ID Number" + AdCount);
            // App open ad is loaded.
            AdView = appOpenAd;
            RegisterEventHandlers(AdView);
        }));
    }
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {

        };
    }
    public void ShowAd()
    {

        if (IsAdAvailable)
        {
            AdsHandler.Instance.ShowAdBackground();

            AdLoading = false;
            AdView.Show();

        }

        if (!AdLoading)
        {
            AdCount = 0;
            LoadAd();
        }
    }

    #endregion
}
