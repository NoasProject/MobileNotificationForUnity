using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Noa;

namespace Noa.LocalMobileNotification
{
    public class MobileNotificationManager : SingletonMonoBehaviour<MobileNotificationManager>
    {
        private IMobileNotification iMobileNotification = null;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // 初期化処理
#if UNITY_ANDROID
            this.iMobileNotification = new AndroidMobileNotification();
#endif
#if UNITY_IOS
            this.iMobileNotification = new iOSMobileNotification();
#endif

            this.iMobileNotification.Init();
        }


        public void SendMessage(string message, int second, int channelId)
        {
            this.SendMessage(Application.productName, message, second, channelId);
        }

        public void SendMessage(string title, string message, int second, int channelId)
        {
            this.SendMessage(title, string.Empty, message, second, channelId);
        }

        public void SendMessage(string title, string subTitle, string message, int second, int channelId)
        {
            this.iMobileNotification.SendMessage(title, subTitle, message, second, channelId);
        }

        public void OnApplicationPause(bool paused)
        {
            // バックグラウンドへ行く
            if (paused)
            {
                Debug.Log("バックグラウンドへ行く - プッシュ通知を登録する");
                this.iMobileNotification.Register();
            }

            // 戻ってくる
            else
            {
                Debug.Log("アプリに戻ってくる - プッシュ通知を登録する");
                CancelALLMessage();
            }
        }

        public void OnApplicationQuit()
        {
            this.iMobileNotification.Register();
        }

        //すべてのローカルメッセージをクリアする
        public void CancelALLMessage(bool isForce = false)
        {
            Debug.Log("ローカルメッセージを削除する");
            this.iMobileNotification.CancelALLMessage(isForce);
        }
    }
}