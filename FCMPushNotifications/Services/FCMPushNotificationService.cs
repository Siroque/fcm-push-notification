﻿using ACB.FCMPushNotifications.Models;
using ACB.FCMPushNotifications.Models.Request;
using ACB.FCMPushNotifications.Utils;
using ACB.FCMPushNotifications.Models.Response;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ACB.FCMPushNotifications.Services
{
    class FCMPushNotificationService : IPushNotificationService
    {
        private HttpClient _Http { get; set; }

        private string FCMServerToken { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public FCMPushNotificationService(IOptions<PushNotificationServiceOptions> options)
        {
            if (string.IsNullOrWhiteSpace(options.Value.FCMServerToken))
            {
                throw new Exception("FCM Server Key is required");
            }

            FCMServerToken = options.Value.FCMServerToken;

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
            var notification = MapRequestToMessage(request);

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

            Debug.WriteLine(jsonPayload);

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
                        var userToken = request.UserIds[i];

                        results.Add(new NotificationResult
                        {
                            UserId = userToken,
                            Success = !result.Error.HasValue,
                            Error = result.Error
                        });
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
                Data = request.Data,
                RegistrationIds = request.UserIds
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
    }
}
