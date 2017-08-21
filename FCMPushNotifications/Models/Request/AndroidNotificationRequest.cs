namespace ACB.FCMPushNotifications.Models.Request
{
    /// <summary>
    /// Describes the android notification to be sent. 
    /// </summary>
    public class AndroidNotificationRequest : NotificationRequest
    {
        /// <summary>
        /// The notification's channel id (new in Android O). The app must 
        /// create a channel with this ID before any notification with this 
        /// key is received. If you don't send this key in the request, or 
        /// if the channel id provided has not yet been created by your app, 
        /// FCM uses the channel id specified in app manifest.
        /// </summary>
        public string AndroidChannelId { get; set; }

        /// <summary>
        /// The notification's icon. Sets the notification icon to myicon 
        /// for drawable resource myicon.If you don't send this key in the 
        /// request, FCM displays the launcher icon specified in app manifest. 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Identifier used to replace existing notifications in the 
        /// notification drawer. If not specified, each request creates 
        /// a new notification. If specified and a notification with the 
        /// same tag is already being shown, the new notification replaces 
        /// the existing one in the notification drawer.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The notification's icon color, expressed in #rrggbb format.
        /// </summary>
        public string Color { get; set; }
    }
}
