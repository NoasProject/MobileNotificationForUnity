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
        /// プッシュ通知待ちの数
        /// </summary>
        public Dictionary<int, IMobileNotificationEx> NotificationTable { get; set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            this.NotificationTable = new Dictionary<int, IMobileNotificationEx>();
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

            if (this.ShowInForeground)
            {
                channel.ShowInForeground = true;
                channel.ForegroundPresentationOption = PresentationOption.Sound | PresentationOption.Alert;
            }

            // iOSのPush通知設定
            // iOSNotificationCenter.ScheduleNotification(channel);

            // 登録を削除する
            this.CancelMessage(channelId);

            // 重複するチャンネルを取得する
            this.NotificationTable[channelId] = channel;

            return channel;
        }

        /// <summary>
        /// 登録するロジック
        /// </summary>
        public void Register()
        {
            Debug.Log("メッセージの登録処理を行う");

            // Triggerが発生している場合は、削除する
            this.NotificationTable = this.NotificationTable.Where(w => w.Value.TriggerUtcDate > DateTime.UtcNow).ToDictionary(d => d.Key, d => d.Value);

            // 登録するKeyの一覧を取得し、並び替える
            int[] registerKeys = this.NotificationTable.Keys.OrderBy(o => this.NotificationTable[o].TriggerUtcDate).ToArray();

            // 登録数
            int cnt = registerKeys.Length;

            // バッチ番号
            int number = 0;

            for (int i = 0; i < cnt; i++)
            {
                int channelId = registerKeys[i];

                var channel = (iOSNotificationEx)this.NotificationTable[channelId];

                // Triggerの時間を更新する
                channel.Trigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = (channel.TriggerUtcDate - DateTime.UtcNow)
                };

                // 登録を削除する
                this.CancelRegister(channelId);

                // バッチに加算する
                number++;

                // バッチ番号を設定
                channel.Badge = number;

                // iOSのPush通知設定
                iOSNotificationCenter.ScheduleNotification(channel);
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        public void CancelMessage(params int[] channelIDs)
        {
            this.CancelRegister(channelIDs);

            // 登録チャンネルを削除する
            this.NotificationTable = this.NotificationTable.Where(w => !channelIDs.Contains(w.Key)).ToDictionary(d => d.Key, d => d.Value);
        }

        public void CancelRegister(params int[] channelIDs)
        {
            foreach (int channelId in channelIDs)
            {
                iOSNotificationCenter.RemoveDeliveredNotification(channelId.ToString());
                iOSNotificationCenter.RemoveScheduledNotification(channelId.ToString());
            }
        }

        /// <summary>
        /// 全てのメッセージを削除する
        /// </summary>
        public void CancelALLMessage(bool isForce = false)
        {
            this.NotificationTable.Clear();

            this.CancelALLRegister();
        }

        /// <summary>
        /// 登録のみを削除
        /// </summary>
        public void CancelALLRegister()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.ApplicationBadge = 0;
        }
    }
}
#endif