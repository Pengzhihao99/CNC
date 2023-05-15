using Autofac;
using Framework.Infrastructure.Adapter.AutoMapper;
using Framework.Infrastructure.Crosscutting.Adapter;
using Framework.Infrastructure.Crosscutting.IdGenerators;
using Framework.Repository.MongoDB.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Ioc.AutofacModule
{
    public class CommonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //domain Id生成器
            builder.RegisterType<MongoDBObjectIdIdentityGenerator>().As<IIdentityGenerator<string>>().SingleInstance();

            //注册类型适配转换器
            builder.RegisterType<AutoMapperTypeAdapter>().As<ITypeAdapter>().SingleInstance();

        }
    }
}
