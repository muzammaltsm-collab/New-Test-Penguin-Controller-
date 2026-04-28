using UnityEngine;
#if UNITY_IOS || UNITY_IPHONE
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

public class DailyNotification : MonoBehaviour
{
    public static bool isOnceDone = false;

#if UNITY_ANDROID
    [HideInInspector] public string channelID = "DailyNotification";
    [HideInInspector] public string channelName = "DailyNotificationCH";
#endif

    [Header("Notification 1 (One-time after X hours)")]
    public int interval1Hours = 1;
    public string message1 = "Time to check back in!";

    [Header("Notification 2 (One-time after X hours)")]
    public int interval2Hours = 6;
    public string message2 = "Don't miss out on rewards!";

    [Header("Notification 3 (Repeats every X hours)")]
    public int interval3Hours = 9;
    public string message3 = "Come back for a bonus!";

#if UNITY_IOS || UNITY_IPHONE
    public string SubTitleMessageText = "Are You Ready?";
#endif

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (!isOnceDone)
        {
#if UNITY_IOS || UNITY_IPHONE
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            StartCoroutine(RequestAuthorization());
#elif UNITY_ANDROID
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");

            var channel = new AndroidNotificationChannel()
            {
                Id = channelID,
                Name = channelName,
                Importance = Importance.Default,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            AndroidNotificationCenter.CancelAllNotifications();
            ScheduleCustom();
#endif
            isOnceDone = true;
        }
    }

#if UNITY_IOS || UNITY_IPHONE
    System.Collections.IEnumerator RequestAuthorization()
    {
        using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound, true))
        {
            while (!req.IsFinished)
                yield return null;

            Debug.Log("Notification Authorization: " + req.Granted);
            ScheduleCustom();
        }
    }
#endif

    public void ScheduleCustom()
    {
        try
        {
#if UNITY_IOS || UNITY_IPHONE
            // One-time
            ScheduleiOSNotification("Reminder 1", message1, interval1Hours, "notif_1hr", false);
            ScheduleiOSNotification("Reminder 2", message2, interval2Hours, "notif_6hr", false);
            // Repeating
            ScheduleiOSNotification("Reminder 3", message3, interval3Hours, "notif_9hr", true);
#elif UNITY_ANDROID
            // One-time
            ScheduleAndroidNotification("💥 Power-Ups Ready! ", message1, interval1Hours, 1001, false);
            ScheduleAndroidNotification("💡 Miss Popping Bubbles?  ", message2, interval2Hours, 1002, false);
            // Repeating
            ScheduleAndroidNotification("🔥 Weekend Bubble Blast! ", message3, interval3Hours, 1003, true);
#endif
            Debug.Log("Notifications: Scheduled with correct repetition.");
        }
        catch (System.Exception e)
        {
            Debug.Log("Notification: Exception in Creating..." + e.ToString());
        }
    }

#if UNITY_IOS || UNITY_IPHONE
    void ScheduleiOSNotification(string title, string body, int hours, string id, bool repeat)
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(hours, 0, 0),
            Repeats = repeat
        };

        var notification = new iOSNotification()
        {
            Identifier = id,
            Title = title,
            Body = body,
            Subtitle = SubTitleMessageText,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "GSSDailyNotification",
            ThreadIdentifier = "MainGameThread",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif

#if UNITY_ANDROID
    void ScheduleAndroidNotification(string title, string message, int hours, int notifId, bool repeat)
    {
        var notification = new AndroidNotification()
        {
            Title = title,
            Text = message,
            FireTime = System.DateTime.Now.AddHours(hours),
            RepeatInterval = repeat ? System.TimeSpan.FromHours(hours) : new System.TimeSpan(0)
        };

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, channelID, notifId);
    }
#endif
}

