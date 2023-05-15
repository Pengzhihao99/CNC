using Autofac;
using Framework.Domain.Core.Entities;
using Framework.Domain.Core.Events;
using MessageCore.Domain.AggregatesModels.BlockingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Ioc.AutofacModule
{
    public class DomainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatRDomainEventBus>().As<IDomainEventBus>().SingleInstance();
        }
    }
}
