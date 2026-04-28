using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using GoogleMobileAds.Api;
using System;
using System.Threading.Tasks;
using UnityEngine;
using FirebaseSDKAdType;
public class FbAnalytics : MonoBehaviour
{
    public static FbAnalytics Instance;
    //public FBNotifier fBNotifier;
    //public FirebaseRemoteConfigManager FbRemoteConfig;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public bool firebaseInitialized = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                Crashlytics.IsCrashlyticsCollectionEnabled = true;
            }
            else
            {
                ////Debug.LogError(
                //"Could not resolve all Firebase dependencies: " + dependencyStatus);
                //FbRemoteConfig.gameObject.SetActive(true);
            }
        });
    }

    void InitializeFirebase()
    {
        DebugLog("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);


        // Set the user's sign up method.

        firebaseInitialized = true;
        AnalyticsLogin();
        //fBNotifier.Initilize();
        //FbRemoteConfig.gameObject.SetActive(true);
        DebugLog("Firebase Init Done.");
    }

    public void DebugLog(string s)
    {
        // if (IsLogEnabled)
        //     print(s);
    }
    public void HandleOnPaidEvent(AdValue adValue, FirebaseAdType adType)
    {
        if (adValue == null)
        {
            Debug.LogWarning("AdValue is null. OnPaidEvent aborted.");
            return;
        }

        string adUnitId = "unknown";

        // Detect which ad type was triggered
        switch (adType)
        {
            case FirebaseAdType.Banner:
                adUnitId = AdsManagerWrapper.Instance._banner.AdUnitID[0];
                break;

            case FirebaseAdType.Interstitial:
                adUnitId = AdsManagerWrapper.Instance._inter.AdUnitID[0];
                break;

            case FirebaseAdType.Reward:
                adUnitId = AdsManagerWrapper.Instance._rewarded.AdUnitID[0];
                break;
           
        }

        // Send Firebase OnPaid event
        FirebaseAnalytics.LogEvent(
            "ad_impression",
            new Parameter[]
            {
            new Parameter("ad_type", adType.ToString()),                // 👈 NEW PARAM
            new Parameter("value", adValue.Value / 1_000_000f),         // Micros → USD
            new Parameter("currency", adValue.CurrencyCode),
            new Parameter("ad_unit_id", adUnitId),
            new Parameter("precision", adValue.Precision.ToString()),
            }
        );

        Debug.Log(
            $"AdPaid ({adType}): {adValue.Value} micros | {adValue.CurrencyCode} | {adUnitId}"
        );
    }

    /// <summary>
    /// any event detail of Analytics will pass through here
    /// </summary>
    /// <param name="info"></param>
    public void LogEvent(string info)
    {
        FirebaseAnalytics.LogEvent(info);

        //      ////Debug.LogError(info);



    }

    /// <summary>
    /// log with int parameter
    /// </summary>
    /// <param name="info"></param>
    /// <param name="parameter"></param>
    public void LogEvent(string info, string paramenterName, int parameterValue)
    {
        FirebaseAnalytics.LogEvent(info, paramenterName, parameterValue);
    }
    //    public void 
    public void AnalyticsLogin()
    {
        // Log an event with no parameters.
        DebugLog("Logging a login event.");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    public void AnalyticsProgress()
    {
        // Log an event with a float.
        DebugLog("Logging a progress event.");
        FirebaseAnalytics.LogEvent("progress", "percent", 0.4f);
    }

    public void AnalyticsScore()
    {
        // Log an event with an int parameter.
        DebugLog("Logging a post-score event.");
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventPostScore,
            FirebaseAnalytics.ParameterScore,
            42);
    }



    public void AnalyticsLevelUp()
    {
        // Log an event with multiple parameters.
        DebugLog("Logging a level up event.");
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelUp,
            new Parameter(FirebaseAnalytics.ParameterLevel, 5),
            new Parameter(FirebaseAnalytics.ParameterCharacter, "mrspoon"),
            new Parameter("hit_accuracy", 3.14f));
    }

    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
        DebugLog("Reset analytics data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }
}