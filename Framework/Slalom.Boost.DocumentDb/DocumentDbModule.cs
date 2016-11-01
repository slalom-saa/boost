//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using Slalom.Boost.Reflection;

//namespace Slalom.Boost.DocumentDb
//{
//    public class DocumentDbDomainModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            base.Load(builder);

//            builder.Register(c => new DocumentDbMappingsManager(c.Resolve<IDiscoverTypes>()))
//                   .As<DocumentDbMappingsManager>()
//                   .SingleInstance();

//            builder.Register(c => new DocumentDbConnectionManager(c.Resolve<DocumentDbMappingsManager>(), c.Resolve<DocumentDbOptions>()))
//                   .As<DocumentDbConnectionManager>()
//                   .SingleInstance();
//        }
//    }
//}
