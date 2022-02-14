using chatsolution.Core;
using chatsolution.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChatTest
{
    [TestClass]
    public class CommandMessageTest
    {
        Mock<IStockService> mockStockService = new Mock<IStockService>();
        Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        Mock<ILogger> mockLogger = new Mock<ILogger>();
        Mock<IQueuePublisherService> mockQueuePublisher = new Mock<IQueuePublisherService>();


        [TestMethod]
        public void ShouldReturnACommandText()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/test=0", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.IsNotNull(commandMessage);
            Assert.IsInstanceOfType(commandMessage, typeof(CommandMessage));
        }

        [TestMethod]
        public void ShouldSaveValidCommandMessageToDataBase()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/test=0", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.IsFalse(commandMessage.ShouldSaveToDatabase());
        }

        [TestMethod]
        public void ShouldIdentifyInvalidCommand()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/test=0", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.IsTrue(commandMessage.IsUnKnownCommand());
            Assert.IsTrue(commandMessage.GetTextMessage().StartsWith("Command unknown (/test=0)."));
        }

        [TestMethod]
        public void ShouldIdentifyValidComand()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/stock=0000", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.IsFalse(commandMessage.IsUnKnownCommand());
        }

        [TestMethod]
        public void ShouldNotSaveValidCommandMessageToDataBase()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/stock=AAPL.US", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.IsFalse(commandMessage.ShouldSaveToDatabase());
        }

        [TestMethod]
        public void ValidCommandMessageShouldInformThatIsInProcess()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/stock=AAPL.US", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.AreEqual(commandMessage.GetTextMessage(), "Command is in process, wait until it finishes.");
        }

        [TestMethod]
        public void InvalidCommandMessageShouldReturnTextAlert()
        {
            //arrange
            var commandMessage = new CommandMessage("me", "/stock=AAPL.US", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            //assert
            Assert.AreEqual(commandMessage.GetTextMessage(), "Command is in process, wait until it finishes.");
        }

        [TestMethod]
        public async Task StockCommandShouldCallStockService()
        {
            //arrange
            string code = "AAPL.US";
            var stooqReturn = GetSampleOfStooq("0.00");

            var commandMessage = new CommandMessage("me", "/stock=AAPL.US", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            mockStockService.Setup(s => s.QueryByCodeAsync(code)).Returns(Task.FromResult(stooqReturn));

            //act
            await commandMessage.DoWorkAsync();

            //assert
            mockStockService.Verify(f => f.QueryByCodeAsync(code));
        }
        
        [TestMethod]
        public async Task ShouldSendToQueueQuotePriceFormatted()
        {
            
            //arrange
            string code = "AAPL.US";
            var stooqReturn = GetSampleOfStooq("167.37");

            var commandMessage = new CommandMessage("me", "/stock=AAPL.US", DateTime.Now, mockStockService.Object,
                mockConfiguration.Object, mockQueuePublisher.Object, mockLogger.Object);

            mockStockService.Setup(s => s.QueryByCodeAsync(code)).Returns(Task.FromResult(stooqReturn));

            //act
            await commandMessage.DoWorkAsync();

            //assert
            mockQueuePublisher.Verify(f => f.EnQueueMessage(It.Is<TextMessage>(m => m.Text.Equals("AAPL.US quote is 167.37 per share."))));
        }

        private static string GetSampleOfStooq(string value)
        {
            return "Symbol,Date,Time,Open,High,Low,Close,Volume" + Environment.NewLine +
                   $"AAPL.US,2022-02-14,17:39:36,{value},169.19,167.09,168.74,17211913";
        }
    }
}