#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

namespace Noa.LocalMobileNotification.Android
{
    public class AndroidNotificationEx : IMobileNotificationEx
    {
        public AndroidNotification Notification { get; set; }
        public int Id { get; set; }
        public DateTime RegisterUtcDate { get; set; }
        public DateTime TriggerUtcDate { get; set; }

        public bool IsAutoBadge { get; set; } = true;
    }
}
#endif