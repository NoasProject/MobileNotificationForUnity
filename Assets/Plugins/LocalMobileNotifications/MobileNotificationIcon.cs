#if UNITY_ANDROID
namespace Noa.LocalMobileNotification.Android
{
    /// <summary>
    /// AndroidのPush通知アイコンの設定クラス
    /// </summary>
    public sealed class MobileNotificationIcon
    {
        public string SmallIcon;
        public string LargeIcon;
    }
}
#endif