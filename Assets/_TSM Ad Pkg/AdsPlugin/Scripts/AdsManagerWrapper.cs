using ByteBrewSDK;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdsManagerWrapper : MonoBehaviour
{

    public static AdsManagerWrapper Instance;

    ConsentForm _consentForm;

    public RewardedInterstitial rewardedInterstitial;
    public Interstitial _inter;
    public Rewarded _rewarded;
    public BannerAd _banner;
    public BannerAd _rectBanner;
    public AppOpen _appOpen;
    //public  Interstitial _cp;

    private static DateTime Time1ForAds;

    public AdSize adaptiveSize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
    #region init

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //  DontDestroyOnLoad(this.gameObject);
    }



    #endregion

    #region UserConsentForm
    void Start()
    {
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
        {
            "3CB10DE262B4F50B8F94398D73192169"
        }
        };

        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };

        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.LogError("ConsentInfoUpdate ErrorCode: " + error.ErrorCode);
            Debug.LogError("ConsentInfoUpdate Message: " + error.Message);
            return;
        }

        if (ConsentInformation.IsConsentFormAvailable())
        {
            ConsentForm.Load(OnLoadConsentForm);
        }
        else
        {
            InitializeAds();
        }
    }

    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            Debug.LogError("LoadConsentForm ErrorCode: " + error.ErrorCode);
            Debug.LogError("LoadConsentForm Message: " + error.Message);
            InitializeAds();
            return;
        }

        _consentForm = consentForm;

        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
        else
        {
            InitializeAds();
        }
    }

    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            Debug.LogError("ShowForm ErrorCode: " + error.ErrorCode);
            Debug.LogError("ShowForm Message: " + error.Message);
            return;
        }

        InitializeAds();
    }

    void InitializeAds()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
            LoadAllAds();
        });
    }
    #endregion

    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;

        _inter.ShowAd();
    }



    public bool IsInterstitialAvailable()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return false;

        return _inter.IsAdAvailable;
    }

    public void ShowRewardedVideo(Action _Reward, Action _NoReward)
    {
        _rewarded.ShowAd(_Reward, _NoReward);
    }



    public bool IsRewardedVideoAvailable()
    {
        return _rewarded.IsAdAvailable;
    }
    public void ShowBanner(AdPosition adPosition)
    {


        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;


        _banner.ShowAd(adPosition, AdsManagerWrapper.Instance.adaptiveSize);
    }
    public void HideBanner()
    {

        _banner.HideAd();

    }

    public void ShowRectBanner(AdPosition adPosition)
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;

        _rectBanner.ShowAd(adPosition, AdSize.MediumRectangle);

    }
    public void HideRectBanner()
    {

        _rectBanner.HideAd();

    }
    public bool isBannerAvailable()
    {
        return _banner.IsAdAvailable;
    }

    public void ShowAppOpen()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;

        _appOpen.ShowAd();
        AdsHandler.Instance.AdShown = true;

    }


    public bool isAppOpenAvailable()
    {
        return _appOpen.IsAdAvailable;
    }



    public void showTestInter()
    {
        ShowInterstitial();

    }

    public void showTestRewarded()
    {
        ShowRewardedVideo(null, null
            );
    }
    //public void showTestRewardedInter()
    //{
    //    ShowRewardedInter(null);
    //}




    public void LoadAllAds()
    {


        _inter.LoadAd();



        _rewarded.LoadAd();




        _appOpen.LoadAd();

        //rewardedInterstitial.LoadAd();
    }

}





















