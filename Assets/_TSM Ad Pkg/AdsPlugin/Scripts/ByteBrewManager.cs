using ByteBrewSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using GoogleMobileAds.Api;

public class ByteBrewManager : MonoBehaviour
{
    public static ByteBrewManager Instance;
    int interstialAds = 0;
    int rewardedAds = 0;
    float levelTime = 0;
    string currentAdDetail = string.Empty;
    string CurrentLocation = string.Empty;


    int CurrentLevelNo;
    //public int CurrentTask;
    // public string CurrentTaskLocation;

    int totalSink = 0;
    int totalSource = 0;

    private void OnEnable()
    {
        if (Instance != this || Instance == null)
        {
            Instance = this;


        }

    }

    private void Start()
    {
        if (!ByteBrew.IsInitilized)
        {
            ByteBrew.InitializeByteBrew();
        }
    }

    public void LogAdsAnaytics_Admobe(ByteBrewAdTypes adTypes, AdapterResponseInfo impressionData, AdValue adValue)
    {
        if (ByteBrew.IsInitilized)
        {

            ByteBrewAdTypes byteBrewAd = adTypes;
            AdWatched(byteBrewAd);

            var levelNo = (100 + CurrentLevelNo).ToString();

            ByteBrew.TrackAdEvent(byteBrewAd, impressionData.AdSourceName, impressionData.AdSourceId, adValue.Value / 1000000f);

            if (byteBrewAd == ByteBrewAdTypes.Interstitial || byteBrewAd == ByteBrewAdTypes.Reward)
            {
                var parameter = new Dictionary<string, string>
                {


                    { "ad_type", byteBrewAd.ToString() },
                    { "network_name", impressionData.AdSourceName },
                    { "ad_unit_name", impressionData.AdSourceId },
                    { "sublocation", CurrentLocation },
                    { "location", "LVL"/* CurrentTaskLocation*/ },
                    { "environment", "Gameplay" }
                };

                ByteBrew.NewCustomEvent(currentAdDetail, parameter);

            }

        }
    }



    public void LogAdsType_Admobe(ByteBrewAdTypes adTypes, AdapterResponseInfo impressionData, AdValue adValue)
    {

        if (ByteBrew.IsInitilized)
        {

            ByteBrewAdTypes byteBrewAd = adTypes;

            var levelNo = (100 + CurrentLevelNo).ToString();


            ByteBrew.TrackAdEvent(byteBrewAd, impressionData.AdSourceName, impressionData.AdSourceId, adValue.Value / 1000000f);

            if (byteBrewAd == ByteBrewAdTypes.Interstitial)
            {
                var parameter = new Dictionary<string, string>
                {
                    { "ad_type", byteBrewAd.ToString() },
                    { "network_name", impressionData.AdSourceName },
                    { "ad_unit_name", impressionData.AdSourceId },
                    { "sublocation", CurrentLocation },
                   { "location", "LVL"/* CurrentTaskLocation*/ },
                    { "environment", "Gameplay" }
                };

                ByteBrew.NewCustomEvent("ByteBrewEventTags", parameter);

            }
            else if (byteBrewAd == ByteBrewAdTypes.Reward)
            {
                var parameter = new Dictionary<string, string>
                {
                    { "ad_type", byteBrewAd.ToString() },
                    { "network_name", impressionData.AdSourceName },
                    { "ad_unit_name", impressionData.AdSourceId },
                    { "sublocation", CurrentLocation },
                   { "location", "LVL"/* CurrentTaskLocation*/ },
                    { "environment", "Gameplay" }
                };

                ByteBrew.NewCustomEvent("ByteBrewEventTags_REWARDEDAD", parameter);
            }
            else if (byteBrewAd == ByteBrewAdTypes.Banner)
            {
                var parameter = new Dictionary<string, string>
                {
                    { "ad_type", byteBrewAd.ToString() },
                    { "network_name", impressionData.AdSourceName },
                    { "ad_unit_name", impressionData.AdSourceId },
                    { "sublocation", CurrentLocation },
                   { "location", "LVL"/* CurrentTaskLocation*/ },
                    { "environment", "Gameplay" }
                };

                ByteBrew.NewCustomEvent("ByteBrewEventTags_BANNERAD", parameter);
            }

        }
    }




    public void SetCurrentLocation(string Sublocation)
    {
        CurrentLocation = Sublocation;
    }


    public void AdWatched(ByteBrewAdTypes adTypes)
    {
        if (adTypes == ByteBrewAdTypes.Interstitial)
        {
            interstialAds++;
        }
        else if (adTypes == ByteBrewAdTypes.Reward)
        {
            rewardedAds++;
        }
    }

    public void ResetAds()
    {
        interstialAds = 0;
        rewardedAds = 0;
    }

    public string GetCompletionTime()
    {
        var completeTime = Time.time - levelTime;
        return Mathf.Round(completeTime).ToString();
    }






    public void AnalyticEvent(string eventName = " ")
    {
        //UnityEngine.////////////////////////////////Debug.LogError("Event_"+ eventName);
        ByteBrew.NewCustomEvent(eventName);
    }


}

