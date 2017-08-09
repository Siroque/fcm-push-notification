using ACB.FCMPushNotifications.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using ACB.FCMPushNotifications.Models.Request;
using ACB.FCMPushNotifications.Models.Response;

namespace ACB.FCMPushNotifications.Services
{
    /// <summary>
    /// Push Notification Service interface
    /// </summary>
    public interface IPushNotificationService
    {
        /// <summary>
        /// Send notification to users
        /// </summary>
        /// <param name="request">Define notification content and audience</param>
        /// <returns></returns>
        Task<List<NotificationResult>> NotifyAsync(NotificationRequest request);

        /// <summary>
        /// Save user info object. <seealso cref="UserInfo">
        /// See UserInfo object description.</seealso>
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> RegisterUserAsync(UserInfo userInfo);

        /// <summary>
        /// Delete user device token. <seealso cref="UserInfo">
        /// See UserInfo object description.</seealso>
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true if successfully unregistered</returns>
        Task<bool> UnregisterUserAsync(UserInfo userInfo);

        /// <summary>
        /// Check if userId is already known. <seealso cref="UserInfo">
        /// See UserInfo object description.</seealso>
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true if userId is already exist</returns>
        bool IsKnownUser(UserInfo userInfo);

        /// <summary>
        /// Check if user with specific userId is already using 
        /// a specific platform. <seealso cref="UserInfo">
        /// See UserInfo object description.</seealso>
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true if user with specific userId is already using 
        /// a specific platform</returns>
        bool IsUsingPlatform(UserInfo userInfo);

        /// <summary>
        /// Checks if a specific userId + token combination is 
        /// already persisted. <seealso cref="UserInfo">
        /// See UserInfo object description.</seealso>
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true if userId is already associated with token given</returns>
        bool IsKnownToken(UserInfo userInfo);
    }
}
