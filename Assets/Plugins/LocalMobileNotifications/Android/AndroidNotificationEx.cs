#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

namespace Noa.LocalMobileNotification.Android
{
    public class AndroidNotificationEx : iOSNotification, IMobileNotificationEx
    {
        public int Id { get; set; }
        public DateTime RegisterUtcDate { get; set; }
        public DateTime TriggerUtcDate { get; set; }
    }
}
#endif