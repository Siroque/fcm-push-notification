using ACB.FCMPushNotifications.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for FCMUserPushNotificationService
    /// </summary>
    public static class PushNotificationServiceCollectionExtensions
    {
        /// <summary>
        /// Register and configure scoped FCMUserPushNotificationService class. 
        /// Inject using IUserPushNotificationService interface.
        /// </summary>
        public static void AddFCMPushNotificationService(this IServiceCollection services, Action<PushNotificationServiceOptions> configure)
        {
            services.AddScoped<IUserPushNotificationService, FCMUserPushNotificationService>()
                .Configure(configure);
        }
    }
}
