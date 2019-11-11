#if UNITY_IOS
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;

namespace Noa.LocalMobileNotification.iOS
{
    public class iOSNotificationEx : iOSNotification, IMobileNotificationEx
    {
        public int Id { get; set; }
        public DateTime RegisterUtcDate { get; set; }
        public DateTime TriggerUtcDate { get; set; }

        public bool IsAutoBadge { get; set; } = true;
    }
}
#endif