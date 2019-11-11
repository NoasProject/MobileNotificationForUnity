using System;
using System.Collections;
using System.Collections.Generic;

namespace Noa.LocalMobileNotification
{
    public interface IMobileNotificationEx
    {
        /// <summary>
        /// Unique - id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 登録時間
        /// </summary>
        DateTime RegisterUtcDate { get; set; }

        /// <summary>
        /// 発生時間
        /// </summary>
        DateTime TriggerUtcDate { get; set; }

        /// <summary>
        /// バッジ処理
        /// </summary>
        bool IsAutoBadge { get; set; }
    }
}