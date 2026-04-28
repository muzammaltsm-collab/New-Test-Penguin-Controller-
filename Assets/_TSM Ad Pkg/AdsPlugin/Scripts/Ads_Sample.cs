using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ads_Sample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // optional: warm up or sanity-check AdsHandler
        if (AdsHandler.Instance == null)
        {
            Debug.LogWarning("[Ads_Sample] AdsHandler.Instance is null. Make sure AdsHandler is present in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // ------------------- Wrappers that delegate to AdsHandler -------------------

    // Show a rewarded ad and pass reward / no-reward callbacks to AdsHandler
    public void ShowRewarded(Action onReward = null, Action onNoReward = null)
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowRewardedVideo(onReward, onNoReward);
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowRewarded: AdsHandler.Instance is null.");
        }
    }

    // Show an interstitial ad
    public void ShowInterstitial()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowInterstitial();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowInterstitial: AdsHandler.Instance is null.");
        }
    }

    // Show App Open ad
    public void ShowAppOpen()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowAppOpen();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowAppOpen: AdsHandler.Instance is null.");
        }
    }

    // Show a smart banner. If no position is provided, AdsHandler will use its configured default.
    public void ShowSmartBanner(SmartBannerPosition? position = null)
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowSmartBanner(position);
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowSmartBanner: AdsHandler.Instance is null.");
        }
    }

    // Show a rect (MRec) banner. If no position is provided, AdsHandler will use its configured default.
    public void ShowRectBanner(RectBannerPosition? position = null)
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowRectBanner(position);
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowRectBanner: AdsHandler.Instance is null.");
        }
    }

    // Hide banners
    public void HideBanner()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.HideBanner();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] HideBanner: AdsHandler.Instance is null.");
        }
    }

    public void HideRectBanner()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.HideRectBanner();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] HideRectBanner: AdsHandler.Instance is null.");
        }
    }

    // Show/hide ad background (visual overlay used by AdsHandler)
    public void ShowAdBackground()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.ShowAdBackground();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] ShowAdBackground: AdsHandler.Instance is null.");
        }
    }

    public void HideAdBackground()
    {
        if (AdsHandler.Instance != null)
        {
            AdsHandler.Instance.HideAdBackground();
        }
        else
        {
            Debug.LogWarning("[Ads_Sample] HideAdBackground: AdsHandler.Instance is null.");
        }
    }

    // ------------------- UI-friendly methods (hook these to Unity Buttons) -------------------

    // Example: hooks for UI Buttons that want to grant a simple reward
    public void UI_ShowRewarded()
    {
        ShowRewarded(
            onReward: () => Debug.Log("[Ads_Sample] UI_ShowRewarded: Reward granted."),
            onNoReward: () => Debug.Log("[Ads_Sample] UI_ShowRewarded: No reward (ad failed or skipped).")
        );
    }

    public void UI_ShowInterstitial()
    {
        ShowInterstitial();
    }

    public void UI_ShowBannerTopCenter()
    {
        ShowSmartBanner(SmartBannerPosition.TopCenter);
    }

    public void UI_HideBanner()
    {
        HideBanner();
    }

    // PSEUDOCODE / PLAN (detailed):
    // 1. Define a UI-facing method `UI_LoadAllAdsAndNotify` that will be wired to a Unity Button.
    // 2. If AdsHandler.Instance is null -> log a warning and return.
    // 3. Attempt to invoke any "load" methods on AdsHandler via reflection:
    //    - Prefer known names: "LoadAllAds", "PreloadAllAds", "PreloadAds", "LoadAds", "LoadAll".
    //    - If none match, find any parameterless method whose name starts with "Load" and invoke it.
    // 4. After invoking load methods, attempt to determine load completion by checking for boolean
    //    properties or parameterless methods that indicate loaded state (names containing "Loaded" or starting with "Is").
    //    - Collect all boolean indicators and poll them periodically (e.g., every 0.25s) up to a timeout (e.g., 5s).
    //    - If at least one indicator was found, require all found indicators to be true to consider "all ads loaded".
    //    - If no indicators found, just log that load was initiated but status cannot be determined.
    // 5. Log success ("All ads loaded.") or timeout/failure accordingly.
    // 6. Make the polling coroutine safe and non-blocking by using Unity's coroutine (System.Collections.IEnumerator).

    // Note: Reflection is used to avoid compile-time dependencies on exact AdsHandler API names.

    public void UI_LoadAllAdsAndNotify()
    {
        // Start the load-and-notify coroutine
        if (AdsManagerWrapper.Instance == null)
        {
            Debug.LogWarning("[Ads_Sample] UI_LoadAllAdsAndNotify: AdsHandler.Instance is null.");
            return;
        }
        AdsManagerWrapper.Instance.LoadAllAds();

    }

   
}