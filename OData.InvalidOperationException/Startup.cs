using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace OData.InvalidOperationException
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(config =>
            {
                config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

                var conventions = ODataRoutingConventions
                    .CreateDefaultWithAttributeRouting("odata", config);

                conventions.Insert(0, new RoutingConventions());

                config.MapODataServiceRoute("odata", "odata", GetModel(), new DefaultODataPathHandler(), conventions);
            });
        }

        private static IEdmModel GetModel()
        {
            var builder = new ODataConventionModelBuilder();
            return builder.GetEdmModel();
        }
    }

    public class RoutingConventions : EntitySetRoutingConvention
    {
        public override string SelectAction(RouteContext routeContext, SelectControllerResult controllerResult, IEnumerable<ControllerActionDescriptor> actionDescriptors)
        {
            return base.SelectAction(routeContext, controllerResult, actionDescriptors);
        }
    }
}
