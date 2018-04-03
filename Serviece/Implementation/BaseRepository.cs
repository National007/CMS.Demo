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
           /// <summary>
           /// 获取所有Entity
          /// </summary>
         /// <param name="exp">Lambda条件的where</param>
         /// <returns>返回IEnumerable类型</returns>
        public virtual IEnumerable<T> GetEntities(Func<T, bool> exp)
         {
             using (Entities db = new Entities())
             {
                  return db.Set<T>().Where(exp).ToList();
             }
  

        }
        /// <summary>
        /// 计算总个数(分页)
        /// </summary>
         /// <param name="exp">Lambda条件的where</param>
         /// <returns></returns>
          public virtual int GetEntitiesCount(Func<T, bool> exp)
          {
              using (Entities db = new Entities())
             {
                  return db.Set<T>().Where(exp).ToList().Count();
 
             }
         }
         /// <summary>
         /// 分页查询(Linq分页方式)
          /// </summary>
          /// <param name="pageNumber">当前页</param>
         /// <param name="pageSize">页码</param>
         /// <param name="orderName">lambda排序名称</param>
         /// <param name="sortOrder">排序(升序or降序)</param>
         /// <param name="exp">lambda查询条件where</param>
         /// <returns></returns>
          public virtual IEnumerable<T> GetEntitiesForPaging(int pageNumber, int pageSize, Func<T, string> orderName, string sortOrder, Func<T, bool> exp)
          {
             using (Entities db = new Entities())
             {
                  if (sortOrder == "asc") //升序排列
                  {
                      return db.Set<T>().Where(exp).OrderBy(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                  }
                  else
                  {
                      return db.Set<T>().Where(exp).OrderByDescending(orderName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                  }
             }
 
          }
          /// <summary>
         /// 根据条件查找满足条件的一个entites
         /// </summary>
         /// <param name="exp">lambda查询条件where</param>
          /// <returns></returns>
          public virtual T GetEntity(Func<T, bool> exp)
         {
              using (Entities db = new Entities())
              {
                  return db.Set<T>().Where(exp).SingleOrDefault();
              }
          }
          #endregion
  
          #region 增改删实现
          /// <summary>
          /// 插入Entity
         /// </summary>
          /// <param name="model"></param>
          /// <returns></returns>
          public virtual bool Insert(T entity)
          {
              using (Entities db = new Entities())
             {
                 var obj = db.Set<T>();
                 obj.Add(entity);
                 return db.SaveChanges() > 0;
 
             }

          }
        /// 更新Entity(注意这里使用的傻瓜式更新,可能性能略低)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         public virtual bool Update(T entity)
        {
            using (Entities db = new Entities())
           {
                var obj = db.Set<T>();
                obj.Attach(entity);
                 db.Entry(entity).State =EntityState.Modified;
                return db.SaveChanges() > 0;
             }
 
 
         }
         /// <summary>
         /// 删除Entity
        /// </summary>
        /// <param name="entity"></param>
       /// <returns></returns>
        public virtual bool Delete(T entity)
        {
           using (Entities db = new Entities())
           {
                var obj = db.Set<T>();
                if (entity != null)
                {
                    obj.Attach(entity);
                     db.Entry(entity).State = EntityState.Deleted;
                     obj.Remove(entity);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
 
         }
        #endregion
    }
}
