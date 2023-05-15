using Autofac;
using Autofac.Core;
using Framework.Infrastructure.UOW;
using Framework.Repository.MongoDB;
using Framework.Repository.MongoDB.Configurations;
using Framework.Repository.MongoDB.Models;
using Framework.Repository.MongoDB.UOW;
using MessageCore.Domain.Repositories;
using MessageCore.Domain.Repositories.ReadOnly;
using MessageCore.Repository.MongoDB;
using MessageCore.Repository.MongoDB.Managements;
using MessageCore.Repository.MongoDB.ReadOnly;
using Microsoft.Extensions.Options;

namespace MessageCore.Infrastructure.Ioc.AutofacModule
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //unitofwork
            builder.RegisterType<MongoDBUnitOfWork>().As<IUnitOfWork>().SingleInstance();
            //migration
            builder.RegisterType<MessageCoreConfigurationManager>().As<MongoDBConfigurationManager>().InstancePerLifetimeScope();


            //默认用于读写
            builder.RegisterType<MongoDBContext>()
                .As<IMongoDBContext>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IOptions<MongoDBConnectionOptions>),
                    (pi, ctx) => Options.Create(ctx.Resolve<IOptionsMonitor<MongoDBConnectionOptions>>().Get("MongoDBForWrite"))))
                .InstancePerLifetimeScope();

            //用于只读
            builder.RegisterType<MongoDBContext>()
                .Named<IMongoDBContext>("ReadOnly")
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IOptions<MongoDBConnectionOptions>),
                    (pi, ctx) => Options.Create(ctx.Resolve<IOptionsMonitor<MongoDBConnectionOptions>>().Get("MongoDBForRead"))))
                .InstancePerLifetimeScope();

            /*
             * 配置可写可读领域仓储
             */
            builder.RegisterType<BlockingRepository>().As<IBlockingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<IssuerRepository>().As<IIssuerRepository>().InstancePerLifetimeScope();

            builder.RegisterType<OrderFilterRepository>().As<IOrderFilterRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SendingAttachmentRepository>().As<ISendingAttachmentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SendingOrderRepository>().As<ISendingOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SendingServiceRepository>().As<ISendingServiceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberRepository>().As<ISubscriberRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateRepository>().As<ITemplateRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeRepository>().As<IThemeRepository>().InstancePerLifetimeScope();


            /*
             * 配置只读领域仓储
             */
            builder.RegisterType<ReadOnlyBlockingRepository>().As<IReadOnlyBlockingRepository>()
                 .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();
            builder.RegisterType <ReadOnlyIssuerRepository > ().As<IReadOnlyIssuerRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyOrderFilterRepository>().As<IReadOnlyOrderFilterRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyOrderRepository>().As<IReadOnlyOrderRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlySendingOrderRepository>().As<IReadOnlySendingOrderRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlySendingServiceRepository>().As<IReadOnlySendingServiceRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlySendingAttachmentRepository>().As<IReadOnlySendingAttachmentRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlySubscriberRepository>().As<IReadOnlySubscriberRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyTemplateRepository>().As<IReadOnlyTemplateRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyThemeRepository>().As<IReadOnlyThemeRepository>()
                .WithParameter(ResolvedParameter.ForNamed<IMongoDBContext>("ReadOnly")).InstancePerLifetimeScope();

        }
    }
}
