using System.Collections.Generic;

namespace ACB.FCMPushNotifications.Models
{
    internal class NotificationPayload
    {
        /// <summary>
        /// Indicates notification title. This field is not visible on iOS phones and tablets.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates notification body text.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The sound to play when the device receives the notification.
        /// <para>Android: Supports "default" or the filename of a sound 
        /// resource bundled in the app.Sound files must reside in /res/raw/</para>
        /// <para>iOS: Sound files can be in the main bundle of the client 
        /// app or in the Library/Sounds folder of the app's data container.</para>
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// Indicates the action associated with a user click on the notification.
        /// <para>iOS: Corresponds to category in the APNs payload.</para>
        /// <para>Android: When this is set, an activity with a matching intent 
        /// filter is launched when user clicks the notification.</para>
        /// </summary>
        public string ClickAction { get; set; }

        /// <summary>
        /// The key to the body string in the app's string resources 
        /// to use to localize the body text to the user's current 
        /// localization. 
        /// <para>iOS: Corresponds to loc-key in the APNs payload.</para>
        /// </summary>
        public string BodyLocKey { get; set; }

        /// <summary>
        /// Variable string values to be used in place of the format 
        /// specifiers in body_loc_key to use to localize the body text 
        /// to the user's current localization. 
        /// <para>iOS: Corresponds to loc-args in the APNs payload.</para>
        /// </summary>
        public List<string> BodyLocArgs { get; set; }

        /// <summary>
        /// The key to the title string in the app's string resources 
        /// to use to localize the title text to the user's current 
        /// localization. 
        /// <para>iOS: Corresponds to title-loc-key in the APNs payload.</para>
        /// </summary>
        public string TitleLocKey { get; set; }

        /// <summary>
        /// Variable string values to be used in place of the format 
        /// specifiers in body_loc_key to use to localize the body text 
        /// to the user's current localization. 
        /// <para>iOS: Corresponds to loc-args in the APNs payload.</para>
        /// </summary>
        public List<string> TitleLocArgs { get; set; }

        /// <summary>
        /// The value of the badge on the home screen app icon. 
        /// If not specified, the badge is not changed.
        /// If set to 0, the badge is removed.
        /// <para>iOS only. Will be ignored in case of Android notification</para>
        /// </summary>
        public string Badge { get; set; }

        /// <summary>
        /// The notification's channel id (new in Android O). The app must 
        /// create a channel with this ID before any notification with this 
        /// key is received. If you don't send this key in the request, or 
        /// if the channel id provided has not yet been created by your app, 
        /// FCM uses the channel id specified in app manifest.
        /// <para>Android only. Will be ignored in case of iOS notification</para>
        /// </summary>
        public string AndroidChannelId { get; set; }

        /// <summary>
        /// The notification's icon. Sets the notification icon to myicon 
        /// for drawable resource myicon.If you don't send this key in the 
        /// request, FCM displays the launcher icon specified in app manifest. 
        /// <para>Android only. Will be ignored in case of iOS notification</para>
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Identifier used to replace existing notifications in the 
        /// notification drawer. If not specified, each request creates 
        /// a new notification. If specified and a notification with the 
        /// same tag is already being shown, the new notification replaces 
        /// the existing one in the notification drawer.
        /// <para>Android only. Will be ignored in case of iOS notification</para>
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The notification's icon color, expressed in #rrggbb format.
        /// <para>Android only. Will be ignored in case of iOS notification</para>
        /// </summary>
        public string Color { get; set; }
    }
}