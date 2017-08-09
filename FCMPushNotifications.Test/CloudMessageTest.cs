using ACB.FCMPushNotifications.Data;
using ACB.FCMPushNotifications.Services;
using ACB.FCMPushNotifications.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using System.Collections.Generic;
using ACB.FCMPushNotifications.Models.Response;
using System.Threading.Tasks;

namespace FCMPushNotifications.Test
{
    public class CloudMessageTest
    {
        [Fact]
        public async Task CloudMessage_CanDryRun()
        {
            // Arrange
            PushNotificationServiceOptions pushOptions = new PushNotificationServiceOptions()
            {
                FCMServerToken = ""
            };
            IOptions<PushNotificationServiceOptions> options = Options.Create(pushOptions);
            IPushNotificationService pushService = new FCMPushNotificationService(options, GetContextWithData());
            NotificationRequest apnsRequest = GetApnsRequest();

            //Act
            List <NotificationResult> result = await pushService.NotifyAsync(apnsRequest);

            //Assert
            Assert.NotEmpty(result);
        }

        private NotifServerDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<NotifServerDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new NotifServerDbContext(options);

            var user = new UserInfo {
                UserId = "",
                Token = "",
                Platform = DevicePlatform.iOS
            };
            context.UserDeviceTokens.Add(user);
            context.SaveChanges();

            return context;
        }

        private NotificationRequest GetApnsRequest()
        {
            return new ApnsNotificationRequest()
            {
                Title = "TestTitle",
                Message = "TestMessage",
                DryRun = true,
                UserIds = new List<string>() { "" },
                Sound = "default",
                BodyLocKey = "",
                BodyLocArgs = new List<string>() { },
                TitleLocKey = "",
                TitleLocArgs = new List<string>() { },
                Badge = "100",
                Data = new Dictionary<string, string>()
                {

                }
            };
        }
    }
}
