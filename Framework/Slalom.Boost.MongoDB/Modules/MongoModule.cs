using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Slalom.Boost.MongoDB.Modules
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new MongoDbContext())
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
        }
    }
}
