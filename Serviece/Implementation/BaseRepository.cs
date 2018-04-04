using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        OpenAuthDBEntities db = new OpenAuthDBEntities();

        #region 查询普通实现方案(基于Lambda表达式的Where查询)

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsExist(Expression<Func<T, bool>> exp)
        {
            return db.Set<T>().Any(exp);
        }

        /// <summary>
        /// 获取整个实体的数据
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return db.Set<T>().ToList();
        }
        /// <summary>
        /// 新增某个实体
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            db.Set<T>().Add(entity);
            Save();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void BatchAdd(T[] entities)
        {
            db.Set<T>().AddRange(entities);
            Save();
        }

        /// <summary>
        /// 修改某个实体
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            var entry = this.db.Entry(entity);
            entry.State = System.Data.Entity.EntityState.Modified;
            Save();
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
            Save();
        }
        private void Save()
        {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }
        }
        #endregion

        #region  //存储过程、执行脚本
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return db.Database.SqlQuery<T>(sql, parameters).AsQueryable<T>().ToList();
        }
        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql">sql命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回执行结果</returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return db.Database.ExecuteSqlCommand(sql, parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> ExecuteProcedureList<T>(string commandText, params object[] parameters)
        {
            try
            {
                //add parameters to command
                if (parameters != null && parameters.Length > 0)
                {
                    for (int i = 0; i <= parameters.Length - 1; i++)
                    {
                        var p = parameters[i] as DbParameter;
                        if (p == null)
                            throw new Exception("Not support parameter type");

                        commandText += i == 0 ? " " : ", ";

                        commandText += "@" + p.ParameterName;
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            //output parameter
                            commandText += " output";
                        }
                    }
                }

                var result = SqlQuery<T>(commandText, parameters).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
