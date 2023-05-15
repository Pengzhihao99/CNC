using Framework.Domain.Core.Events;
using Framework.Repository.MongoDB;
using Framework.Repository.MongoDB.Models;
using MediatR;
using MessageCore.Application.DataTransferModels;
using MessageCore.Application.SendingServices;
using MessageCore.Application.Services;
using MessageCore.Domain.AggregatesModels.SendingOrderAggregate;
using MessageCore.Domain.Common.Enum;
using MessageCore.Domain.Repositories;
using MessageCore.Repository.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;

namespace MessageCore.BackgroundTask.Test
{
    public class SendMessagesJobTest
    {
        /// <summary>
        /// Xunit����̨���
        /// </summary>
        private ITestOutputHelper OutPut { get; set; }
        private readonly ISendingServiceRepository _sendingServiceRepository;
        private readonly ISendingAttachmentRepository _sendingAttachmentRepository;
        private readonly Mock<ILogger<MongoDBContext>> _mockLogger;
        private readonly IConfigurationRoot Configuration;
        private readonly IMongoDBContext _mongoDbContext;
        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly IDomainEventBus _domainEventBus;

        public SendMessagesJobTest(ITestOutputHelper outPut, IDomainEventBus domainEventBus)
        {
            OutPut = outPut;
            _domainEventBus = domainEventBus;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            this.Configuration = builder.Build();

            var options = Options.Create(new MongoDBConnectionOptions()
            {
                ConnectionString = Configuration.GetSection("MongoDBForWrite")["ConnectionString"],
                Database = Configuration.GetSection("MongoDBForWrite")["Database"]
            });

            var mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Publish<INotification>(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
                .Returns(new Task(() => { })) //<-- return Task to allow await to continue
                .Verifiable("Notification was not sent.");

            _mockLogger = new Mock<ILogger<MongoDBContext>>();
            //ILogger �ܶ���չ������������չ�����޷�moq��ֱ��moq��ײ�
            _mockLogger.Setup(m => m.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<MongoDBContext>(),
                It.IsAny<System.Exception>(), It.IsAny<Func<MongoDBContext, Exception, string>>()));
            _mockLogger.Setup(m => m.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

            _serviceProvider = new Mock<IServiceProvider>();
            _serviceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new { });

            _mongoDbContext = new MongoDBContext(_mockLogger.Object, options);
            _sendingServiceRepository = new SendingServiceRepository(_mongoDbContext, _domainEventBus, _serviceProvider.Object);
            _sendingAttachmentRepository = new SendingAttachmentRepository(_mongoDbContext, _domainEventBus, _serviceProvider.Object);
        }

        [Fact]
        public async void SendEmailShouldPass()
        {
            var service = new SendMessagesService(_sendingServiceRepository, _sendingAttachmentRepository,
                new Lazy<IEnumerable<Application.SendingServices.Base.IEmailService>>(() => new List<Application.SendingServices.Base.IEmailService>() { new MailKitEmailService() }),
                null,
                null);
            var sendingOrder = new SendingOrder("123123123", "Ref20230404A16", "6358fd98fb775453612a4f4b", "Emain_test", "62ff63169d76539faf917772", "ģ��222", "SRM", "zhihao.peng@chukou1.com", "�ͻ�TEST�˼�֪ͨ",
           "Hello Test", "<h1 data-v-4cc04517=\"\">���⣺�ͻ������˼�֪ͨ</h1>\r\n<div data-v-4cc04517=\"\">Hello ������Ա��\r\n<p><strong>�ر�����</strong>�������Ա����˵����κ����飬��������յ����ʼ���7����������ͨ��������ϵ���ǡ���������δ��ϵ���ǣ�����Ϊ�����顣</p>\r\n<p><strong>Items:</strong><br /><br />&nbsp;* 1 x 1kg carrots - 4.99<br /><br />&nbsp;* 1 x 2L Milk - 3.5</p>\r\n<p>Thanks,</p>\r\nBFE�����Ŷ�</div>", "BFE �����Ŷ�", new List<string>() { "6359f6f753663cdf81c45b8f" },
           TimerType.Immediately, SendingOrderStatus.Ready);
            await service.SendAsync(sendingOrder);
        }
    }
}