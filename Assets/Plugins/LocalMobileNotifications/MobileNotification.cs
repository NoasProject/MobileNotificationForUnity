// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Notifications.iOS;

// namespace Noa.LocalMobileNotification.iOS
// {
//     public sealed class MobileNotification
//     {
//         public MobileNotification()
//         {
//             this.RegisterUtcDate = System.DateTime.UtcNow;
//         }

//         public int ChannelId { get; set; }

//         public DateTime RegisterUtcDate { get; }

//         public DateTime TriggerUtcDate { get; set; }

//         public TimeSpan? TimeInterval { get; set; } = null;

//         public bool IsAutoBadge { get; set; } = true;

//         public string Data { get; set; }
//     }
// }