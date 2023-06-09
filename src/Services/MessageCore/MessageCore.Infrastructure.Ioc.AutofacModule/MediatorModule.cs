﻿using Autofac;
using MediatR;
using MediatR.Pipeline;
using MessageCore.Application.Admin.Commands.BlockingCommand;
using MessageCore.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Ioc.AutofacModule
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //自动装配 IMediator 所在程序集中的所有的公共的，具体类将被注册。
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                //命令与领域事件相关
                //api
                builder.RegisterAssemblyTypes(typeof(CreateBlockingCommand).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

                //web
                //builder.RegisterAssemblyTypes(typeof(CreateSupplierCommand).GetTypeInfo().Assembly)
                //   .AsClosedTypesOf(mediatrOpenType)
                //   .AsImplementedInterfaces();

                //pushback
                //builder.RegisterAssemblyTypes(typeof(LogisticsTrackingWebhookNotificationInformation).GetTypeInfo().Assembly)
                //   .AsClosedTypesOf(mediatrOpenType)
                //   .AsImplementedInterfaces();

            }

            builder.Register<ServiceFactory>(ctx =>
            {
                var context = ctx.Resolve<IComponentContext>();
                return t => context.Resolve(t);
            });

            LoadBehaviors(builder);
        }

        /// <summary>
        /// 配置管道行为
        /// </summary>
        private void LoadBehaviors(ContainerBuilder builder)
        {
            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            //泛型注入日志行为
            //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
           // builder.RegisterGeneric(typeof(Admin.Application.Behaviors.CommandValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RetryBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(CommandValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
