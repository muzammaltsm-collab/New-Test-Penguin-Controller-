using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SmartBannerPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
public enum RectBannerPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
public class AdsHandler : MonoBehaviour
{

    // Start is called before the first frame update
    public static AdsHandler Instance;
    private static DateTime Time1ForAds;
    public bool AdShown = false;
    private bool isBackAppOpen = false;

    public bool IsAdshowing = false;

    public GameObject AdBg, NoAds;

    // ------------------- Banner & MRec Settings -------------------
    [Header("Banner & MRec Settings")]

    // Smart Banner Position (TopCenter by default)
    public SmartBannerPosition smartBannerPosition = SmartBannerPosition.TopCenter;

    // Rect Banner (MRec) Position (BottomRight by default)
    public RectBannerPosition rectBannerPosition = RectBannerPosition.BottomRight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        adsboolReset = StartCoroutine(Wfs());

        setTime1ForAds();

    }


    // ------------------- Interstitial -------------------
    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;

        IsAdshowing = true;

        AdsManagerWrapper.Instance.ShowInterstitial();                        // show Admobe Interstial Ad

    }

    // ------------------- Rewarded Video -------------------

    public void ShowRewardedVideo(Action _Reward, Action _NoReward)
    {

        IsAdshowing = true;


        if (AdsManagerWrapper.Instance.IsRewardedVideoAvailable())
        {
            AdsManagerWrapper.Instance.ShowRewardedVideo(_Reward, _NoReward);
        }


        else
        {
            NoAds.gameObject.SetActive(true);
            Invoke(nameof(NoAdsOff), 2f);
        }

    }
    void NoAdsOff()
    {
        NoAds.gameObject.SetActive(false);
    }

    // ------------------- AppOpen -------------------

    public void ShowAppOpen()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
            return;


        AdsManagerWrapper.Instance.ShowAppOpen();                           // show admobe App Open



        // FirebaseRemoteConfigManager.Instance.configData
    }

    // ------------------- SMART BANNER -------------------

    public void ShowSmartBanner(SmartBannerPosition? position = null)
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;

        SmartBannerPosition posToUse = position ?? smartBannerPosition;


        AdPosition admobPos = ConvertToAdmobPosition(posToUse);
        AdsManagerWrapper.Instance.ShowBanner(admobPos);

    }


    // ------------------- RECT BANNER (MBR) -------------------
    public void ShowRectBanner(RectBannerPosition? position = null)
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) return;
        RectBannerPosition posToUse = position ?? rectBannerPosition;
        AdPosition admobPos = ConvertToAdmobPosition(posToUse);

        AdsManagerWrapper.Instance.ShowRectBanner(admobPos);
    }
    // ------------------- HIDE METHODS -------------------

    public void HideBanner()
    {
            AdsManagerWrapper.Instance.HideBanner();
    }

    public void HideRectBanner()
    {
            AdsManagerWrapper.Instance.HideRectBanner();
    }


    // ------------------- POSITION CONVERTERS -------------------

  

    private AdPosition ConvertToAdmobPosition(SmartBannerPosition position)
    {
        switch (position)
        {
            case SmartBannerPosition.TopLeft: return AdPosition.TopLeft;
            case SmartBannerPosition.TopCenter: return AdPosition.Top;
            case SmartBannerPosition.TopRight: return AdPosition.TopRight;
            case SmartBannerPosition.BottomLeft: return AdPosition.BottomLeft;
            case SmartBannerPosition.BottomCenter: return AdPosition.Bottom;
            case SmartBannerPosition.BottomRight: return AdPosition.BottomRight;
            default: return AdPosition.Top; // default Top Center
        }
    }
    // --- Rect Banner Position Conversion ---

   
    private AdPosition ConvertToAdmobPosition(RectBannerPosition position)
    {
        switch (position)
        {
            case RectBannerPosition.TopLeft: return AdPosition.TopLeft;
            case RectBannerPosition.TopCenter: return AdPosition.Top;
            case RectBannerPosition.TopRight: return AdPosition.TopRight;
            case RectBannerPosition.BottomLeft: return AdPosition.BottomLeft;
            case RectBannerPosition.BottomCenter: return AdPosition.Bottom;
            case RectBannerPosition.BottomRight: return AdPosition.BottomRight;
            default: return AdPosition.BottomRight; // default Bottom Right
        }
    }

    public void setTime1ForAds()
    {
        Time1ForAds = DateTime.UtcNow;
    }
    public bool isDealayCompleteForAds()
    {


        double secondsPassed = (DateTime.UtcNow - Time1ForAds).TotalSeconds;

        if (secondsPassed > 15)
        {
            setTime1ForAds();
            return true;
        }

        return false;

    }


    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            //  Debug.LogError("AppOpen Show" + "IsAdshowing" + IsAdshowing);
            if (!IsAdshowing)
            {
                if (AdShown)
                {
                    //   Debug.LogError("AdShown" + AdShown);
                    AdShown = false;
                    return;
                }

                if (isDealayCompleteForAds())
                {
                    //  Debug.LogError("AppOpen_Ad_onReturnFromBackground");


                    ShowAppOpen();
                    isBackAppOpen = true;



                }
            }
            StartAdCoroutine();

        }
    }

    private Coroutine adsboolReset;

    public void StartAdCoroutine()
    {
        // If there's an existing coroutine running, stop it
        if (adsboolReset != null)
        {
            StopCoroutine(adsboolReset);
        }

        // Start a new coroutine and store its reference
        adsboolReset = StartCoroutine(Wfs());
    }

    public IEnumerator Wfs()
    {
        yield return new WaitForSecondsRealtime(5f);
        IsAdshowing = false;

        // After the coroutine completes, reset the adsboolReset variable
        adsboolReset = null;
    }
    private Coroutine adCoroutine;

    public void ShowAdBackground()
    {
        if (adCoroutine != null)
            StopCoroutine(adCoroutine);

        adCoroutine = StartCoroutine(ShowAdBgCoroutine());
    }

    public void HideAdBackground()
    {
        if (AdBg != null)
            AdBg.SetActive(false);
    }

    private IEnumerator ShowAdBgCoroutine()
    {
        if (AdBg != null)
        {
            AdBg.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            HideAdBackground();
        }
        else
        {
            Debug.LogWarning("AdBg is not assigned!");
        }
    }


    public void RemoveAdsBtnClick()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //InAppPurchaser.instance.progressbar.SetActive(true);

           
                IsAdshowing = true;
            
        }
    }
}
