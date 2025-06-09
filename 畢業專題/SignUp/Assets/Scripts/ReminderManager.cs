using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class ReminderManager:MonoBehaviour
{
    [Header("UI")]
    public InputField timeInputField; // User time input (e.g. "16:00")
    public Text feedbackText;         // Optional: show success/failure messages

    private TimeSpan notifyTime;

    public void OnConfirmTime()
    {
        string input = timeInputField.text;

        if (TimeSpan.TryParse(input, out notifyTime))
        {
            Debug.Log("? Time parsed: " + notifyTime);
            feedbackText.text = $"每日提醒設定 {notifyTime:hh\\:mm} .";

#if UNITY_ANDROID
            ScheduleAndroidNotification(notifyTime);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            StartCoroutine(WindowsReminderChecker(notifyTime));
#else
            Debug.LogWarning("This platform does not support reminders.");
#endif
        }
        else
        {
            Debug.LogWarning("? Invalid time format. Use HH:mm");
            feedbackText.text = "格式不正確，輸入格式範例 HH:mm .";
        }
    }

#if UNITY_ANDROID
    void ScheduleAndroidNotification(TimeSpan time)
    {
        // Register channel once
        var channel = new AndroidNotificationChannel()
        {
            Id = "reminder_channel",
            Name = "Daily Reminders",
            Importance = Importance.Default,
            Description = "Reminder to use the app!",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // Calculate next fire time
        var now = DateTime.Now;
        var fireTime = new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0);
        if (fireTime < now)
        {
            fireTime = fireTime.AddDays(1); // Schedule for tomorrow if it's in the past today
        }

        var notification = new AndroidNotification()
        {
            Title = "手語時間到!",
            Text = "別忘了每天練習手語喔! ??",
            FireTime = fireTime,
            RepeatInterval = TimeSpan.FromDays(1)
        };

        AndroidNotificationCenter.SendNotification(notification, "reminder_channel");

        Debug.Log("?? Android notification scheduled at: " + fireTime);
    }
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    IEnumerator WindowsReminderChecker(TimeSpan targetTime)
    {
        Debug.Log("?? Starting Windows reminder loop...");
        while (true)
        {
            var now = DateTime.Now;
            if (now.Hour == targetTime.Hours && now.Minute == targetTime.Minutes)
            {
                Debug.Log("?? [Windows] It's time to use the app!");
                feedbackText.text = "手語學習時間到!";
                yield return new WaitForSeconds(60); // Wait a minute before rechecking
            }
            yield return new WaitForSeconds(10); // Check every 10 seconds
        }
    }
#endif
}
