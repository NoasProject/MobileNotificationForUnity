#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using Unity.Notifications.Android;
using Noa.LocalMobileNotification.Android;

namespace Noa.LocalMobileNotification
{
    /// <summary>
    /// PackageManager - Android
    /// </summary>
    public interface IAndroidMobileNotification
    {
        /// <summary>
        /// プッシュ通知アイコンの設定
        /// </summary>
        MobileNotificationIcon NotificationIcon { get; }

        string ChannelName { get; }

        Importance ChannelImportance { get; }

        string ChannelDescription { get; }

        // Dictionary<int, AA>
        /*
        var c = new AndroidNotificationChannel
    {
        Id = m_channelId,
        Name = "【ここにチャンネル名】",
        Importance = Importance.High,
        Description = "【ここに説明文】",
    };
    AndroidNotificationCenter.RegisterNotificationChannel(c );
    */
        /// <summary>
        /// チャンネル登録をする
        /// </summary>
        void RegisterChannel(int channelId);
    }
}
#endif