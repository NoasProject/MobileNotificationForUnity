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

        /// <summary>
        /// チャンネル登録をする
        /// </summary>
        void RegisterChannel(int channelId);

        /// <summary>
        /// チャンネルが存在するか確認をする
        /// </summary>
        bool IsEnableChannel(int channelId);
    }
}
#endif