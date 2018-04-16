using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Serviece.Interface
{
    public interface IBaseRepository<T>
    {
        #region 查询普通实现方案(基于Lambda表达式的Where查询)

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        bool IsExist(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 获取整个实体的数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// 新增某个实体
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        void BatchAdd(T[] entities);

        /// <summary>
        /// 修改某个实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
         void Delete(T entity);

        IEnumerable<T> Sort<T>(IEnumerable<T> source, string propertyName, bool asc);
        #endregion

        #region  //存储过程、执行脚本
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql">sql命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回执行结果</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> ExecuteProcedureList<T>(string commandText, params object[] parameters);
        #endregion

        #region 跨服务器访问数据库表
        IEnumerable<T> ExecuteSql<T>(string tableName, string condition);
        #endregion

    }
}
