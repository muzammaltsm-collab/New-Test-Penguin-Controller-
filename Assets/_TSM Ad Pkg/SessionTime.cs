using ByteBrewSDK;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SessionTime : MonoBehaviour
{
    public static SessionTime instance;

    private float timer;
    private float timerInSeconds;
    public float totalSeconds = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            totalSeconds = PlayerPrefs.GetFloat("SessionTime", 0);
            totalSeconds += timer;
            PlayerPrefs.SetFloat("SessionTime", totalSeconds);
            if ((int)totalSeconds % 15 == 0)
            {

                var eventParameters = new Dictionary<string, string>()
            {
                 { "Time",  ((int)totalSeconds).ToString()},


            };

                var GAeventParameters = new Dictionary<string, object>()
            {
                 { "Time",  ((int)totalSeconds).ToString()},


            };

                ByteBrew.NewCustomEvent("PlayTime", eventParameters);
            }

            timer = 0f;
        }
    }


    public void LevelComplete(int id)
    {
        Logs.Print("LevelComplete " + id);

        var eventParameters = new Dictionary<string, string>()
                {
                    { "Level",  id.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent("Level_Completed", eventParameters);

    }
    public void CustomEvent(string PlaceHolder)
    {
        Logs.Print("CustomEvent " + PlaceHolder);

        var eventParameters = new Dictionary<string, string>()
                {

                  { "CustomEvent",  PlaceHolder.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent(PlaceHolder, eventParameters);

    }
    public void InterstitialAdWatched()
    {
        Logs.Print("InterstitialAdWatched");

        var eventParameters = new Dictionary<string, string>()
    {
        { "SessionTime", SessionTime.instance.totalSeconds.ToString() },
        { "UserRetentionDay", SessionTime.instance.GetCurrentDay().ToString() },
    };

        ByteBrew.NewCustomEvent("Interstitial_Ad_Watched", eventParameters);
    }
    public void RewardedAdWatched()
    {
        Logs.Print("RewardedAdWatched");

        var eventParameters = new Dictionary<string, string>()
    {
        { "SessionTime", SessionTime.instance.totalSeconds.ToString() },
        { "UserRetentionDay", SessionTime.instance.GetCurrentDay().ToString() },
    };

        ByteBrew.NewCustomEvent("Rewarded_Ad_Watched", eventParameters);
    }
    public void StepComplete(int id)
    {
        Logs.Print("StepComplete " + id);

        var eventParameters = new Dictionary<string, string>()
                {
                    { "Step",  id.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent("Step_Complete", eventParameters);


    }
    public void LevelRetry(int id)
    {
        Logs.Print("LevelRetry " + id);

        var eventParameters = new Dictionary<string, string>()
                {
                    { "Level",  id.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent("Level_Retry", eventParameters);

    }
    public void LevelFailed(int id)
    {
        Logs.Print("LevelFailed " + id);

        var eventParameters = new Dictionary<string, string>()
                {
                    { "Level",  id.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent("Level_Failed", eventParameters);

    }
    public void LevelStart(int id)
    {
        Logs.Print("LevelStart " + id );

        var eventParameters = new Dictionary<string, string>()
                {
                    { "Level",  id.ToString()},
                    { "SessionTime",  SessionTime.instance.totalSeconds.ToString()},
                    { "UserRetentionDay",  SessionTime.instance.GetCurrentDay().ToString()},
                };
        ByteBrew.NewCustomEvent("Level_Start", eventParameters);
    }
    public int GetCurrentDay()
    {
        System.TimeSpan timeSinceInstallation = System.DateTime.UtcNow - InstallDate;
        int daysSinceInstall = timeSinceInstallation.Days;

        return daysSinceInstall;

    }
    private System.DateTime InstallDate
    {
        get
        {
            // Assuming you store the install date in PlayerPrefs at the first launch
            if (!PlayerPrefs.HasKey("install_date"))
            {
                PlayerPrefs.SetString("install_date", System.DateTime.UtcNow.ToString());
            }
            return System.DateTime.Parse(PlayerPrefs.GetString("install_date"));
        }
    }
}
