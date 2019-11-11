using System;
using System.Collections;
using System.Collections.Generic;

namespace Noa.LocalMobileNotification
{
    public static class MobileNotificationExtension
    {
        public static T SetTimer<T>(this T iMobileNotificationEx, int channelId, int second) where T : IMobileNotificationEx
        {
            iMobileNotificationEx.Id = channelId;

            iMobileNotificationEx.RegisterUtcDate = DateTime.UtcNow;

            iMobileNotificationEx.TriggerUtcDate = DateTime.UtcNow.AddSeconds(second);

            return iMobileNotificationEx;
        }
    }
}