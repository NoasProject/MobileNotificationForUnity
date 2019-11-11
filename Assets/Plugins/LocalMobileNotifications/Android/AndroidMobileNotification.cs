#if UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;
using Noa.LocalMobileNotification.Android;
using Unity.Notifications.Android;

namespace Noa.LocalMobileNotification
{
    public sealed class AndroidMobileNotification : IMobileNotification, IAndroidMobileNotification
    {
        private MobileNotificationIcon _notificationIcon = new MobileNotificationIcon()
        {
            SmallIcon = "small",
            LargeIcon = "large",
        };

        public MobileNotificationIcon NotificationIcon { get { return this._notificationIcon; } }

        // チャンネル名
        public string ChannelName { get { return Application.productName; } }
        // チャンネル名
        public Importance ChannelImportance { get { return Importance.High; } }
        // チャンネル名
        public string ChannelDescription { get { return Application.productName; } }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            this.RegisterChannel(1);
        }

        public void RegisterChannel(int channelId)
        {
            var channel = new AndroidNotificationChannel
            {
                Id = channelId.ToString(),
                Name = this.ChannelName,
                Importance = Importance.High,
                Description = this.ChannelDescription,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        /// <summary>
        /// メッセージの送信
        /// </summary>
        public void SendMessage(string title, string subTitle, string message, int second, int channelId)
        {
            this.CancelMessage(channelId);

            // iOSのPush通知設定
            AndroidNotificationCenter.SendNotification(new AndroidNotification()
            {
                Title = title,
                Text = message,
                SmallIcon = this._notificationIcon.SmallIcon,
                LargeIcon = this._notificationIcon.LargeIcon,

                // 時間をトリガーにする
                FireTime = System.DateTime.Now.AddSeconds(second)
            }, channelId.ToString());
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void CancelMessage(params int[] channelIDs)
        {
            foreach (int channelId in channelIDs)
            {
                var channel = AndroidNotificationCenter.GetNotificationChannel(channelId.ToString());
                // if (channel != null)
                // {
                //     AndroidNotificationCenter.CancelNotification(channel.Id);
                // }
            }
        }

        public void CancelALLMessage()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
    }
}
#endif