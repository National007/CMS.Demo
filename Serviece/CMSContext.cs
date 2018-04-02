using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Serviece
{
   public class CMSContext
    {
        /// <summary>
        /// 服务层 AutoFac容器
        /// </summary>
        public IContainer ServiceAutoFacContainer { get; set; }

        #region 单例
        private static CMSContext instance;
        private CMSContext()
        {

        }

        public static CMSContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CMSContext();
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// AutoFac解析服务层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ServiceResolve<T>()
        {
            using (var scope = ServiceAutoFacContainer.BeginLifetimeScope())
            {
                T t = scope.Resolve<T>();
                return t;
            }
        }
    }
}
