using ACB.FCMPushNotifications.Data;
using ACB.FCMPushNotifications.Models;
using ACB.FCMPushNotifications.Models.Request;
using ACB.FCMPushNotifications.Utils;
using ACB.FCMPushNotifications.Models.Response;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ACB.FCMPushNotifications.Services
{
    /// <summary>
    /// Service class to send push notifications using FCM.
    /// </summary>
    public class FCMPushNotificationService : IPushNotificationService
    {
        private HttpClient _Http { get; set; }
        private NotifServerDbContext _db { get; set; }

        private string FCMServerToken { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FCMPushNotificationService(IOptions<PushNotificationServiceOptions> options,
            NotifServerDbContext db)
        {
            if (string.IsNullOrWhiteSpace(options.Value.FCMServerToken))
            {
                throw new Exception("FCM Server Key is required");
            }

            FCMServerToken = options.Value.FCMServerToken;

            _db = db;

            _Http = new HttpClient
            {
                BaseAddress = new Uri("https://fcm.googleapis.com/fcm/")
            };
            _Http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={FCMServerToken}");
        }

        /// <summary>
        /// Send notification to users
        /// </summary>
        public async Task<List<NotificationResult>> NotifyAsync(NotificationRequest request)
        {
            var userTokensQuery = _db.UserDeviceTokens.Where(ut => request.UserIds.Contains(ut.UserId));
            if (request.LimitByPlatform.HasValue)
            {
                userTokensQuery = userTokensQuery.Where(ut => ut.Platform == request.LimitByPlatform.Value);
            }

            var userTokens = userTokensQuery.ToList();

            if (userTokens.Count == 0) return request.UserIds.Select(uid => new NotificationResult()
            {
                UserId = uid,
                Success = false,
                Error = NotificationResultError.UnknownUserIdentifier
            }).ToList();

            var notification = MapRequestToMessage(request);
            notification.RegistrationIds = userTokens.Select(r => r.Token).ToList();

            var jsonPayload = await Task.Run(() =>
                JsonConvert.SerializeObject(
                    notification,
                    Formatting.None,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new SnakeCasePropertyNameContractResolver()
                    }
                )
            );

            var payload = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _Http.PostAsync("send", payload);
            var content = await response.Content.ReadAsStringAsync();
            var json = await Task.Run(() => JsonConvert.DeserializeObject<FCMResponse>(content));

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:

                    var tasks = new List<Task>();
                    var results = new List<NotificationResult>();

                    for (var i = 0; i < json.Results.Count; i++)
                    {
                        var result = json.Results[i];
                        var userToken = userTokens[i];

                        results.Add(new NotificationResult
                        {
                            UserId = userToken.UserId,
                            Success = !result.Error.HasValue,
                            Error = result.Error
                        });

                        if (!string.IsNullOrWhiteSpace(result.RegistrationId))
                        {
                            tasks.Add(UnregisterUserAsync(userToken.UserId, userToken.Token));
                            tasks.Add(
                                RegisterUserAsync(
                                    userToken.UserId, result.RegistrationId, userToken.Platform
                                )
                            );
                        }
                        else if (result.Error.HasValue)
                        {
                            tasks.Add(HandleResponseError(userToken, result));
                        }
                    }

                    await Task.WhenAll(tasks.ToArray());
                    return results;

                case (HttpStatusCode)400:
                    throw new Exception("Invalid JSON");

                case (HttpStatusCode)401:
                    throw new Exception("Authentication Error");

                default:
                case (HttpStatusCode)500:
                    throw new Exception($"Internal Server Error: Multicast Id: {json?.MulticastId}");
            }
        }

        private NotificationMessage MapRequestToMessage(NotificationRequest request)
        {
            var ttl = request.TimeToLive?.TotalSeconds ?? TimeSpan.FromDays(28).TotalSeconds;
            var notification = new NotificationMessage
            {
                DryRun = request.DryRun,
                TimeToLive = Math.Max(0, ttl),
                Data = request.Data
            };

            var notificationPayload = new NotificationPayload
            {
                Title = request.Title,
                Body = request.Message,
                Sound = request.Sound,
                ClickAction = request.ClickAction,
                BodyLocKey = request.BodyLocKey,
                BodyLocArgs = request.BodyLocArgs,
                TitleLocKey = request.TitleLocKey,
                TitleLocArgs = request.TitleLocArgs
            };

            if (request is ApnsNotificationRequest apnsRequest)
            {
                notification.ContentAvailable = apnsRequest.ContentAvailable;
                notification.MutableContent = apnsRequest.MutableContent;
                notificationPayload.Badge = apnsRequest.Badge;
            }

            if (request is AndroidNotificationRequest androidRequest)
            {
                notificationPayload.Icon = androidRequest.Icon;
                notificationPayload.AndroidChannelId = androidRequest.AndroidChannelId;
                notificationPayload.Tag = androidRequest.Tag;
                notificationPayload.Color = androidRequest.Color;
            }

            notification.Notification = notificationPayload;
            return notification;
        }

        private Task HandleResponseError(UserDeviceToken userToken, FCMResponse.Result result)
        {
            switch (result.Error)
            {
                case NotificationResultError.InvalidRegistration:
                case NotificationResultError.NotRegistered:
                    return UnregisterUserAsync(userToken.UserId, userToken.Token);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Save user device token
        /// </summary>
        public async Task RegisterUserAsync(string userId, string userToken, DevicePlatform platform)
        {
            var isDuplicate = _db.UserDeviceTokens.Any(ur => ur.UserId == userId
                                                          && ur.Token == userToken);

            if (!isDuplicate)
            {
                _db.UserDeviceTokens.Add(new UserDeviceToken
                {
                    UserId = userId,
                    Token = userToken,
                    Platform = platform
                });

                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete user device token
        /// </summary>
        public async Task UnregisterUserAsync(string userId, string userToken)
        {
            var userRegId = _db.UserDeviceTokens.FirstOrDefault(ur => ur.UserId == userId
                                                                   && ur.Token == userToken);
            if (userRegId != null)
            {
                _db.UserDeviceTokens.Remove(userRegId);
                await _db.SaveChangesAsync();
            }
        }
    }
}
