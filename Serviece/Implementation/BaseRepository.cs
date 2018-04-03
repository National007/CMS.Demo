using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using EntityModels;
using Serviece.Interface;
using WebModels;

namespace Serviece.Implementation
{
   public class BaseRepository<T>:IBaseRepository<T> where T : class,new() //限制T的类型为class或者对象
    {
        #region 查询普通实现方案(基于Lambda表达式的Where查询)
          
       public virtual IEnumerable<T> GetAll()
        {
            using (OpenAuthDBEntities db=new OpenAuthDBEntities())
            {
                return db.Set<T>().ToList();
            }
        }
      
        #endregion
    }
}
