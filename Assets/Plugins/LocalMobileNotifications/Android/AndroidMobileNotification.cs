#if UNITY_ANDROID
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noa.LocalMobileNotification.Android;
using Unity.Notifications.Android;

namespace Noa.LocalMobileNotification
{
    public sealed class AndroidMobileNotification : IMobileNotification, IAndroidMobileNotification
    {
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
            AndroidNotificationCenter.Initialize();
        }

        /// <summary>
        /// プッシュ通知アイコンの設定
        /// </summary>
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
        /// チャンネルを登録する
        /// </summary>
        public void RegisterChannel(int channelId)
        {
            if (this.IsEnableChannel(channelId))
            {
                return;
            }

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
        /// チャンネルを削除する
        /// </summary>
        public bool IsEnableChannel(int channelId)
        {
            var channel = AndroidNotificationCenter.GetNotificationChannel(channelId.ToString());

            return channel.Enabled;
        }

        /// <summary>
        /// メッセージの送信
        /// </summary>
        public void SendMessage(string title, string subTitle, string message, int second, int channelId)
        {
            this.CancelMessage(channelId);

            var channel = new AndroidNotificationEx()
            {
                Notification = new AndroidNotification()
                {
                    Title = title,
                    Text = message,
                    SmallIcon = this.NotificationIcon.SmallIcon,
                    LargeIcon = this.NotificationIcon.LargeIcon,

                    // 時間トリガーを設定する
                    FireTime = System.DateTime.Now.AddSeconds(second)
                }
            };

            this.SendMessage(channel, channelId, second);
        }

        public IMobileNotificationEx SendMessage(IMobileNotificationEx mobileEx, int channelId, int second)
        {
            AndroidNotificationEx channel = null;

            if ((mobileEx is AndroidNotificationEx))
            {
                channel = (AndroidNotificationEx)mobileEx;
            }
            else
            {
                return null;
            }

            channel = channel.SetTimer(channelId, second);

            // 重複するチャンネルを削除する
            this.CancelMessage(channelId);

            this.NotificationTable[channelId] = channel;

            return channel;
        }

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

                var channel = (AndroidNotificationEx)this.NotificationTable[channelId];

                TimeSpan span = channel.TriggerUtcDate - DateTime.UtcNow;

                // 登録を削除する
                this.CancelRegister(channelId);

                // バッチに加算する
                number++;

                // チャンネルを取得する
                var preChannel = channel.Notification;

                // バッチ番号を設定
                preChannel.Number = number;

                // チャンネルを登録する
                this.RegisterChannel(channelId);

                AndroidNotificationCenter.SendNotification(channel.Notification, channelId.ToString());
            }
        }

        /// <summary>
        /// 初期化処理
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
                AndroidNotificationCenter.CancelDisplayedNotification(channelId);
                AndroidNotificationCenter.CancelScheduledNotification(channelId);
                AndroidNotificationCenter.CancelNotification(channelId);
                AndroidNotificationCenter.DeleteNotificationChannel(channelId.ToString());
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
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();
            AndroidNotificationCenter.CancelAllNotifications();
        }
    }
}
#endif