#if UNITY_IOS
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;
using Noa.LocalMobileNotification.iOS;

namespace Noa.LocalMobileNotification
{
    public sealed class iOSMobileNotification : IMobileNotification
    {
        private bool ShowInForeground = false;

        /// <summary>
        /// バッチの数
        /// </summary>
        public List<IMobileNotificationEx> NotificationList { get; set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            this.NotificationList = new List<IMobileNotificationEx>();
        }

        /// <summary>
        /// メッセージの送信
        /// </summary>
        public void SendMessage(string title, string subTitle, string message, int second, int channelId)
        {
            this.CancelMessage(channelId);

            var channel = new iOSNotificationEx()
            {
                Identifier = channelId.ToString(),
                Title = title,
                Subtitle = subTitle,
                Body = message,
                RegisterUtcDate = DateTime.UtcNow,
                TriggerUtcDate = DateTime.UtcNow.AddSeconds(second),
            };

            this.SendMessage(channel, channelId, second);
        }

        public IMobileNotificationEx SendMessage(IMobileNotificationEx mobileEx, int channelId, int second)
        {
            iOSNotificationEx channel = null;

            if ((mobileEx is iOSNotificationEx))
            {
                channel = (iOSNotificationEx)mobileEx;
            }
            else
            {
                return null;
            }

            channel = channel.SetTimer(channelId, second);

            // 時間をトリガーにする
            channel.Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = TimeSpan.FromSeconds(second),
                Repeats = false
            };

            if (this.ShowInForeground)
            {
                channel.ShowInForeground = true;
                channel.ForegroundPresentationOption = PresentationOption.Sound | PresentationOption.Alert;
            }

            // iOSのPush通知設定
            // iOSNotificationCenter.ScheduleNotification(channel);

            this.NotificationList.Add(channel);

            return channel;
        }

        /// <summary>
        /// 登録するロジック
        /// </summary>
        public void Register()
        {
            // メッセージを削除する
            this.CancelALLMessage();

            // Triggerが発生している場合は、削除する
            this.NotificationList = this.NotificationList.Where(w => w.TriggerUtcDate > DateTime.UtcNow).OrderBy(o => o.TriggerUtcDate).ToList();

            int cnt = this.NotificationList.Count;

            for (int i = 0; i < cnt; i++)
            {
                var channel = (iOSNotificationEx)this.NotificationList[i];

                int number = (i + 1);

                channel.Badge = number;

                // iOSのPush通知設定
                iOSNotificationCenter.ScheduleNotification(channel);
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void CancelMessage(params int[] channelIDs)
        {
            foreach (int channelId in channelIDs)
            {
                iOSNotificationCenter.RemoveDeliveredNotification(channelId.ToString());
                iOSNotificationCenter.RemoveScheduledNotification(channelId.ToString());
            }

            this.NotificationList = this.NotificationList.Where(w => !channelIDs.Contains(w.Id)).ToList();
        }

        public void CancelALLMessage()
        {
            this.NotificationList.Clear();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.ApplicationBadge = 0;
        }
    }
}
#endif