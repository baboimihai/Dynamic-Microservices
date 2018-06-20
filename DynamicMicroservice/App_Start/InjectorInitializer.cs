using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using GlobalServices;

namespace DynamicMicroservice.App_Start
{
    public class InjectorInitializer
    {
        public static Container InjectorContainer { get; set; }
        public static void Initialize()
        {
            var container = new Container();
            InjectorContainer = container;
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            var webReqLifeCycle = new WebRequestLifestyle();
            DependencyContainerMapper.InitializeContainer(container, webReqLifeCycle);
        }
    }
}