using ACB.FCMPushNotifications.Models.Request;
using ACB.FCMPushNotifications.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACB.FCMPushNotifications.Services
{
    interface IPushNotificationService
    {
        /// <summary>
        /// Send notification to devices
        /// </summary>
        /// <param name="request">Define notification content and a set of FCM tokens</param>
        /// <returns></returns>
        Task<List<NotificationResult>> NotifyAsync(NotificationRequest request);
    }
}
