using System;
using System.Collections.Generic;

namespace Noa.LocalMobileNotification
{
    /// <summary>
    /// PackageManager - Mobile Notifications -
    /// </summary>
    public interface IMobileNotification
    {
        Dictionary<int, IMobileNotificationEx> NotificationTable { get; set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Init();

        /// <summary>
        /// メッセージの送信
        /// </summary>
        void SendMessage(string title, string subTitle, string message, int second, int id);

        /// <summary>
        /// 通知センターからの通知を削除する
        /// </summary>
        void CancelMessage(params int[] channelIds);

        /// <summary>
        /// メッセージを全て削除する
        /// </summary>
        void CancelALLMessage(bool isForce);

        /// <summary>
        /// メッセージを登録する
        /// </summary>
        void Register();

        /// <summary>
        /// メッセージの送信をする
        /// </summary>
        IMobileNotificationEx SendMessage(IMobileNotificationEx mobileEx, int channelId, int second);
    }
}