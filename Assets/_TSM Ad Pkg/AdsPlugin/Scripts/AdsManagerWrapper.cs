using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class AdsManagerWrapper : MonoBehaviour
{
    public static AdsManagerWrapper Instance;

    [Header("ATT Panel")]
    [SerializeField] private GameObject attPanel;

    [Header("Ad Scripts")]
    public RewardedInterstitial rewardedInterstitial;
    public Interstitial _inter;
    public Rewarded _rewarded;
    public BannerAd _banner;
    public BannerAd _rectBanner;
    public AppOpen _appOpen;

    private ConsentForm _consentForm;

    public bool IsATTFlowFinished { get; private set; } = false;
    public bool IsAdsSetupComplete { get; private set; } = false;

    private bool adsInitialized = false;
    private bool attChoiceDone = false;

    public AdSize adaptiveSize =
        AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (attPanel != null)
            attPanel.SetActive(false);

#if UNITY_IOS && !UNITY_EDITOR
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            if (attPanel != null)
            {
                attPanel.SetActive(true);
                return;
            }
        }
#endif

        IsATTFlowFinished = true;
        RequestUMPConsent();
    }

    public void OnATTContinueButton()
    {
        if (attChoiceDone) return;

        attChoiceDone = true;

        if (attPanel != null)
            attPanel.SetActive(false);

        IsATTFlowFinished = true;

#if UNITY_IOS && !UNITY_EDITOR
        ATTrackingStatusBinding.RequestAuthorizationTracking();
        StartCoroutine(ContinueAfterATT());
#else
        RequestUMPConsent();
#endif
    }

    public void OnATTNotNowButton()
    {
        if (attChoiceDone) return;

        attChoiceDone = true;

        if (attPanel != null)
            attPanel.SetActive(false);

        IsATTFlowFinished = true;
        RequestUMPConsent();
    }

    private IEnumerator ContinueAfterATT()
    {
        yield return new WaitForSeconds(1.5f);
        RequestUMPConsent();
    }

    private void RequestUMPConsent()
    {
        ConsentRequestParameters request;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                "3CB10DE262B4F50B8F94398D73192169"
            }
        };

        request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings
        };
#else
        request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false
        };
#endif

        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    private void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.LogError("Consent Update Error: " + error.Message);
            InitializeAds();
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

    private void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            Debug.LogError("Consent Form Load Error: " + error.Message);
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

    private void OnShowForm(FormError error)
    {
        if (error != null)
            Debug.LogError("Consent Form Show Error: " + error.Message);

        InitializeAds();
    }

    private void InitializeAds()
    {
        if (adsInitialized) return;

        adsInitialized = true;
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
            LoadAllAds();
            IsAdsSetupComplete = true;
        });
    }

    public void LoadAllAds()
    {
        if (_inter != null) _inter.LoadAd();
        if (_rewarded != null) _rewarded.LoadAd();
        if (_appOpen != null) _appOpen.LoadAd();

        // if (rewardedInterstitial != null)
        //     rewardedInterstitial.LoadAd();
    }

    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;

        if (_inter != null && _inter.IsAdAvailable)
            _inter.ShowAd();
    }

    public bool IsInterstitialAvailable()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return false;
        return _inter != null && _inter.IsAdAvailable;
    }

    public void ShowRewardedVideo(Action reward, Action noReward)
    {
        if (_rewarded != null && _rewarded.IsAdAvailable)
            _rewarded.ShowAd(reward, noReward);
        else
            noReward?.Invoke();
    }

    public bool IsRewardedVideoAvailable()
    {
        return _rewarded != null && _rewarded.IsAdAvailable;
    }

    public void ShowBanner(AdPosition adPosition)
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;

        if (_banner != null)
            _banner.ShowAd(adPosition, adaptiveSize);
    }

    public void HideBanner()
    {
        if (_banner != null)
            _banner.HideAd();
    }

    public void ShowRectBanner(AdPosition adPosition)
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;

        if (_rectBanner != null)
            _rectBanner.ShowAd(adPosition, AdSize.MediumRectangle);
    }

    public void HideRectBanner()
    {
        if (_rectBanner != null)
            _rectBanner.HideAd();
    }

    public bool isBannerAvailable()
    {
        return _banner != null && _banner.IsAdAvailable;
    }

    public void ShowAppOpen()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;

        if (_appOpen != null && _appOpen.IsAdAvailable)
        {
            _appOpen.ShowAd();

            if (AdsHandler.Instance != null)
                AdsHandler.Instance.AdShown = true;
        }
    }

    public bool isAppOpenAvailable()
    {
        return _appOpen != null && _appOpen.IsAdAvailable;
    }

    public void showTestInter()
    {
        ShowInterstitial();
    }

    public void showTestRewarded()
    {
        ShowRewardedVideo(null, null);
    }
}