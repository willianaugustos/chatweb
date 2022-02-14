using chatsolution.Core;
using chatsolution.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTest
{
    [TestClass]
    public class MessageFactoryTest
    {
        Mock<IStockService> mockStockService = new Mock<IStockService>();
        Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        Mock<ILogger> mockLogger = new Mock<ILogger>();
        Mock<IQueuePublisherService> mockQueuePublisher = new Mock<IQueuePublisherService>();

        [TestMethod]
        public void ShouldReturnTextMessage()
        {
            var msg = MessageFactory.Create("me", "hello", DateTime.Now, mockStockService.Object, mockConfiguration.Object,
                mockQueuePublisher.Object, mockLogger.Object);

            Assert.IsFalse(msg is CommandMessage);
            Assert.IsInstanceOfType(msg, typeof(TextMessage));
        }

        [TestMethod]
        public void ShouldReturnCommandMessage()
        {
            var msg = MessageFactory.Create("me", "/action=1234", DateTime.Now, mockStockService.Object, mockConfiguration.Object,
                mockQueuePublisher.Object, mockLogger.Object);

            Assert.IsInstanceOfType(msg, typeof(CommandMessage));
        }
    }
}
