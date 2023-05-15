using Autofac;
using FluentValidation;
using MessageCore.Application.Admin.Commands.BlockingCommand.Validators;
using MessageCore.Application.Admin.DataTransferModels;
using MessageCore.Application.Admin.Queries;
using MessageCore.Application.Admin.Queries.Impl;
using MessageCore.Application.Base;
using MessageCore.Application.SendingServices;
using MessageCore.Application.SendingServices.Base;
using MessageCore.Application.Services;
using MessageCore.Application.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Ioc.AutofacModule
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the Command's Validators (Validators based on FluentValidation library)
            builder.RegisterAssemblyTypes(typeof(CreateBlockingCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            //Queries
            builder.RegisterType<BlockingQueries>().As<IBlockingQueries>().InstancePerLifetimeScope();
            builder.RegisterType<IssuerQueries>().As<IIssuerQueries>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberQueries>().As<ISubscriberQueries>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeQueries>().As<IThemeQueries>().InstancePerLifetimeScope();

            builder.RegisterType<TemplateQueries>().As<ITemplateQueries>().InstancePerLifetimeScope();
            builder.RegisterType<SendingServiceQueries>().As<ISendingServiceQueries>().InstancePerLifetimeScope();
            builder.RegisterType<SendingOrderQueries>().As<ISendingOrderQueries>().InstancePerLifetimeScope();

            //Service
            builder.RegisterType<AttachmentService>().As<IAttachmentService>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateAnalysisService>().As<ITemplateAnalysisService>().InstancePerLifetimeScope();
            builder.RegisterType<SendMessagesService>().As<ISendMessagesService>().InstancePerLifetimeScope();
            builder.RegisterType<PortalService>().As<IPortalService>().InstancePerLifetimeScope();

            //SendingServices
            //获取对应的程序集
            var mailKitEmailServiceAssemblies = Assembly.GetAssembly(typeof(MailKitEmailService));
            builder.RegisterAssemblyTypes(mailKitEmailServiceAssemblies).As<IEmailService>().SingleInstance();
            builder.RegisterAssemblyTypes(mailKitEmailServiceAssemblies).As<IEnterpriseWeChatService>().SingleInstance();
            builder.RegisterAssemblyTypes(mailKitEmailServiceAssemblies).As<ISMSService>().SingleInstance();

        }
    }
}
