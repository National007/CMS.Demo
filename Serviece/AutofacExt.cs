using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Serviece.Implementation;
using Serviece.Interface;

namespace Serviece
{
    public class AutofacExt
    {
        private static IContainer _container;
        private ContainerBuilder builder;
        public AutofacExt()
        {
            //builder
            builder = new ContainerBuilder();
        }

        public void Init()
        {
            //注册类别
            RegisterTypes();
            //创建容器
            _container = builder.Build();
            //将容器放在应用程序上下文中
            CMSContext.Instance.ServiceAutoFacContainer = _container;
        }

        /// <summary>
        /// 注册类别
        /// 增加了新的Service要在此处注册关系
        /// </summary>
        private void RegisterTypes()
        {
            builder.RegisterType<UserServiece>().As<IUserServiece>();
        }


    }
}
