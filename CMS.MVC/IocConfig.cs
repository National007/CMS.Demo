using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serviece.Interface;
using Serviece.Implementation;
using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;
using EntityModels;

namespace CMS.MVC
{
	public class IocConfig
	{
        public static void RegisterAutofac()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            #region IOC注册区域
            //倘若需要默认注册所有的，请这样写（主要参数需要修改）
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //   .AsImplementedInterfaces();

            //Admin
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerHttpRequest();


            #endregion
            // then
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}