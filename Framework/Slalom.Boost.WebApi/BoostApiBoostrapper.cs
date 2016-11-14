using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Serilog.Core;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.RuntimeBinding.Configuration;
using Swashbuckle.Application;

namespace Slalom.Boost.WebApi
{
    public abstract class BoostApiBoostrapper
    {
        public IContainer Container;

        public virtual void Configuration(IAppBuilder application)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };

            var configuration = new HttpConfiguration();

            this.ConfigureSecurity(application, configuration);

            this.ConfigureCors(application);

            Container = this.ConfigureContainer(configuration);

            this.ConfigureLogging(configuration);

            this.ConfigureFormatting(configuration);

            this.ConfigureSwagger(configuration);

            configuration.MapHttpAttributeRoutes();

            application.UseWebApi(configuration);
        }

        protected virtual IContainer ConfigureContainer(HttpConfiguration configuration)
        {
            var unityContainer = new BoostUnityContainer();
            var target = unityContainer.AutoConfigure(this.GetBindingFilters().ToArray());
            configuration.DependencyResolver = new BoostDependencyResolver(new UnityContainerAdapter(unityContainer));
            return target;
        }

        protected virtual void ConfigureCors(IAppBuilder application)
        {
            application.UseCors(CorsOptions.AllowAll);
        }

        protected virtual void ConfigureFormatting(HttpConfiguration configuration)
        {
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        protected virtual void ConfigureLogging(HttpConfiguration configuration)
        {
            this.Container.Register<IEnumerable<IDestructuringPolicy>>(new[] { new LoggingDestructuringPolicy() });
            configuration.Filters.Add(Container.Resolve<WebApiExceptionFilter>());

            var instrumentationKey = ConfigurationManager.AppSettings["ApplicationInsights:InstrumentationKey"];
            if (!String.IsNullOrWhiteSpace(instrumentationKey))
            {
                TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
            }
        }

        protected virtual void ConfigureSecurity(IAppBuilder application, HttpConfiguration configuration)
        {
            application.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            configuration.Filters.Add(new AuthorizeClaims());
        }

        protected virtual void ConfigureSwagger(HttpConfiguration configuration)
        {
            var assembly = this.GetType().Assembly.GetName().Name;
            var documents = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}\\bin", this.GetType().Assembly.FullName.Split('.')[0] + "*.xml", SearchOption.TopDirectoryOnly);

            configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", ConfigurationManager.AppSettings["Swagger:ApiTitle"] ?? assembly);
                foreach (var document in documents)
                {
                    c.IncludeXmlComments(document);
                }
                c.DescribeAllEnumsAsStrings();
                c.OperationFilter<MultipleOperationsWithSameVerbFilter>();
                c.SchemaFilter<OmitCommandProperties>();
            }).EnableSwaggerUi();
        }

        protected virtual IEnumerable<BindingFilter> GetBindingFilters()
        {
            return new BindingFilter[] { AssemblyFilter.Include(e => e.FullName.StartsWith(this.GetType().Assembly.FullName.Split('.')[0])) };
        }
    }
}