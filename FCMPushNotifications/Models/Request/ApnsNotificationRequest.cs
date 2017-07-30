
namespace ACB.FCMPushNotifications.Models.Request
{
    class ApnsNotificationRequest : NotificationRequest
    {
        /// <summary>
        /// Used to represent content-available in the APNs payload. 
        /// When a notification or message is sent and this is set to true, 
        /// an inactive client app is awoken.
        /// </summary>
        public string ContentAvailable { get; set; }

        /// <summary>
        /// Used to represent mutable-content in the APNS payload. 
        /// When a notification is sent and this is set to true, 
        /// the content of the notification can be modified before it is displayed, 
        /// using a Notification Service app extension.
        /// </summary>
        public string MutableContent { get; set; }

        /// <summary>
        /// The value of the badge on the home screen app icon. 
        /// If not specified, the badge is not changed.
        /// If set to 0, the badge is removed.
        /// </summary>
        public string Badge { get; set; }
    }
}
