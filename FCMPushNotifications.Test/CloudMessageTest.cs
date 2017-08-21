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
                FCMServerToken = "AAAAHNWuXwM:APA91bE5z_JvXHMHjG9NvaNG0SzmnETma4_HORQsAcveh2_vKj7gKapem4apGEuOTVg9brxL2ut94rQHVDDgceeB6Bk7ma9b0U0DngvFTcDsQI52CvPMkOK6c7UcdMVl7Mx-LxUMTeF6"
            };
            IOptions<PushNotificationServiceOptions> options = Options.Create(pushOptions);
            IUserPushNotificationService pushService = new FCMUserPushNotificationService(options, GetContextWithData());
            NotificationRequest apnsRequest = GetApnsRequest();

            //Act
            List <NotificationResult> result = await pushService.NotifyUserAsync(apnsRequest);

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
                UserId = "zgv5ed",
                Token = "fMuDvtvuMBI:APA91bFtVLS7iR-MhbixwUvIssH6qeZQEm9kLEnF7-2l0sJTryGZtLYMou2Bw5nPUeIemHJrEQXEfzMgqcusa8dMWh4uI4zK25MMJl-ZtoYYqTe29MUzqKQLiexYh1SZVFexLcokz-Sp",
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
                    { "service", "callUpdate" },
                    { "expectation", "value2" }
                }
            };
        }
    }
}
