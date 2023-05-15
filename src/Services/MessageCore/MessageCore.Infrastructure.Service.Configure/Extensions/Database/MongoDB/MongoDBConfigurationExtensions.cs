using Framework.Repository.MongoDB.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Infrastructure.Service.Configure.Extensions.Database.MongoDB
{
    public static class MongoDBConfigurationExtensions
    {
        public static void ConfigMongoDB(this IHost webHost, bool needMigration)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var mongoDbManager = services.GetRequiredService<MongoDBConfigurationManager>();

                mongoDbManager.ConfigDatabase();

                if (needMigration)
                {
                    mongoDbManager.Migration();
                }
            }
        }
    }
}
