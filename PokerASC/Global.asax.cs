
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using PokerASC.Models;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace PokerASC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public object UnityBootstrap { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //DependencyResolver.SetResolver(new UnityDependencyResolver(UnityBootstrap.BuildUnityContainer()));

            var container = new SimpleInjector.Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<ApplicationDbContext, ApplicationDbContext>(Lifestyle.Scoped);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
