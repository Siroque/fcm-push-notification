using ACB.FCMPushNotifications.Data;
using System;
using System.Collections.Generic;

namespace ACB.FCMPushNotifications.Models.Request
{
    /// <summary>
    /// Describes the notification to be sent. 
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// Indicates notification title. This field is not visible on 
        /// iOS phones and tablets.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Indicates notification body text.
        /// </summary>
        public string Message { get; set; }

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
        /// localization. If both, BodyLocKey and BodyLocArgs, are 
        /// given they would owerride <seealso cref="Message"/> property.
        /// <para>iOS: Corresponds to loc-key in the APNs payload.</para>
        /// </summary>
        public string BodyLocKey { get; set; }

        /// <summary>
        /// Variable string values to be used in place of the format 
        /// specifiers in body_loc_key to use to localize the body text 
        /// to the user's current localization. If both, BodyLocKey and 
        /// BodyLocArgs, are given they would owerride <seealso cref="Message"/> property.
        /// <para>iOS: Corresponds to loc-args in the APNs payload.</para>
        /// </summary>
        public List<string> BodyLocArgs { get; set; }

        /// <summary>
        /// The key to the title string in the app's string resources 
        /// to use to localize the title text to the user's current 
        /// localization. If both, TitleLocKey and TitleLocArgs, are 
        /// given they would owerride <seealso cref="Title"/> property.
        /// <para>iOS: Corresponds to title-loc-key in the APNs payload.</para>
        /// </summary>
        public string TitleLocKey { get; set; }

        /// <summary>
        /// Variable string values to be used in place of the format 
        /// specifiers in body_loc_key to use to localize the body text 
        /// to the user's current localization. If both, TitleLocKey and 
        /// TitleLocArgs, are given they would owerride <seealso cref="Title"/> property.
        /// <para>iOS: Corresponds to loc-args in the APNs payload.</para>
        /// </summary>
        public List<string> TitleLocArgs { get; set; }

        /// <summary>
        /// Specifies how long (in seconds) the message should be kept in 
        /// FCM storage if the device is offline. The maximum time to live 
        /// supported is 4 weeks.
        /// </summary>
        public TimeSpan? TimeToLive { get; set; }

        /// <summary>
        /// Set to true to test a request without actually sending a message.
        /// </summary>
        public bool DryRun { get; set; }

        /// <summary>
        /// (Optional) Target devices on a single platform.
        /// </summary>
        public DevicePlatform? LimitByPlatform { get; set; }

        /// <summary>
        /// Ids of users to target
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Specifies the custom key-value pairs of the message's payload.
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
    }
}
