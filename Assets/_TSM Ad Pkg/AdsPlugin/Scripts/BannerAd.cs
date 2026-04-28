using System;
using System.Collections;
using System.Collections.Generic;
using ByteBrewSDK;
using FirebaseSDKAdType;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class BannerAd : MonoBehaviour
{
    private BannerView AdView;
    public List<String> AdUnitID;
    int AdCount = 0;
    bool Consent = true;
    bool AdLoading = false;
    private bool AdLoadedSuccessfully = false;

    AdSize BannerSize = AdSize.SmartBanner;
    AdPosition BannerPos = AdPosition.Top;
    bool AdHideCalled = false;
    public bool TopBanner = false;

    public void LoadAd()
    {
        Consent = PlayerPrefs.GetInt("userConsent") == 0 ? false : true;

        AdLoading = true;
        if (this.AdView != null)
        {
            this.AdView.OnBannerAdLoadFailed -= AdFailedtoLoad;
            this.AdView.OnBannerAdLoaded -= AdLoaded;
            this.AdView.Destroy();
        }

        this.AdView = new BannerView(AdUnitID[AdCount], BannerSize, BannerPos);
        this.AdView.OnBannerAdLoadFailed += AdFailedtoLoad;
        this.AdView.OnBannerAdLoaded += AdLoaded;
        this.AdView.OnAdPaid += AdPaidEvent;
        AdRequest request = new AdRequest();
        this.AdView.LoadAd(request);
    }

    private void AdPaidEvent(AdValue adValue)
    {
        try
        {
            // Get adapter response info from the banner instance
            AdapterResponseInfo loadedAdapterResponseInfo = null;
            if (AdView != null && AdView.GetResponseInfo() != null)
            {
                loadedAdapterResponseInfo = AdView.GetResponseInfo().GetLoadedAdapterResponseInfo();
            }

            // Log to Firebase and ByteBrew using Banner type
            FbAnalytics.Instance.HandleOnPaidEvent(adValue, FirebaseAdType.Banner);
            ByteBrewManager.Instance.LogAdsAnaytics_Admobe(ByteBrewAdTypes.Banner, loadedAdapterResponseInfo, adValue);
            ByteBrewManager.Instance.LogAdsType_Admobe(ByteBrewAdTypes.Banner, loadedAdapterResponseInfo, adValue);

            Debug.Log("Admob_Banner_Revenue");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error logging banner revenue: " + ex);
        }
    }

    private void AdLoaded()
    {
        AdLoadedSuccessfully = true; // Mark the ad as successfully loaded
        if (AdHideCalled)
        {
            AdLoading = false;
            AdView.Destroy();
        }
    }

    private void AdFailedtoLoad(LoadAdError obj)
    {
        AdLoadedSuccessfully = false; // Mark the ad as not successfully loaded
        if (AdCount < AdUnitID.Count - 1)
        {
            AdCount++;
            LoadAd();
            return;
        }
        this.AdView.OnBannerAdLoadFailed -= AdFailedtoLoad;

        AdLoading = false;
        AdCount = 0;
    }

    public void ShowAd(AdPosition adPosition, AdSize _adSize)
    {
        if (AdLoading)
        {
            return;
        }

        BannerSize = _adSize;
        BannerPos = adPosition;

        AdHideCalled = false;
        AdCount = 0;
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 1)
            return;

        LoadAd();
    }

    public void HideAd()
    {
        AdHideCalled = true;
        AdLoading = false;
        if (this.AdView != null)
        {
            this.AdView.Destroy();
        }
    }

    public bool IsAdAvailable
    {
        get { return AdView != null && AdLoadedSuccessfully; }
    }
}
