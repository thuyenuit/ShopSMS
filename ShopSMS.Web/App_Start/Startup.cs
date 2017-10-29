using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using Autofac;
using ShopSMS.Service.Services;
using ShopSMS.DAL.Repositories;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ShopSMS.DAL.Infrastructure.Implements;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL;
using Microsoft.AspNet.Identity;
using ShopSMS.Model.Model;
using Microsoft.Owin.Security.DataProtection;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

[assembly: OwinStartup(typeof(ShopSMS.Web.App_Start.Startup))]

namespace ShopSMS.Web.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigAutofac(app);
            ConfigurationAuth(app);
        }

        public void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // register web api controller
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<ShopSMSDbcontext>().AsSelf().InstancePerRequest();

            // ASP.NET Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(x => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(x => app.GetDataProtectionProvider()).InstancePerRequest();

            #region // repository
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(ProductCategoryRepository).Assembly)
               .Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            #endregion

            #region // service
            builder.RegisterAssemblyTypes(typeof(ProductService).Assembly)
                .Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(ProductCategoryService).Assembly)
                .Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();
            #endregion

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);

        }
    }
}
