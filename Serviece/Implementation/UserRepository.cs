using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serviece.Interface;
using System.Data.SqlClient;
using EntityModels.OtherServer;
using WebModels;
using Application.AutoMapper;

namespace Serviece.Implementation
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        //跨域数据库
        public List<User> GetList()
        {
            var list = this.ExecuteSql<T_User>("T_User", "where UserName like '%汪%'");
            return GetAll().ToList();
        }

        public List<User> GetListAll()
        {
            string sql = string.Format("select * from [dbo].[User]");
            return SqlQuery<User>(sql).ToList();
        }

        public List<User> GetAll(string field,string order)
        {
            var list = GetAll();
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(order))
            {
                if (order.ToLower() == "asc")
                {
                    list = Sort<User>(list, field, true);
                }
                else
                {
                    list = Sort<User>(list, field, false);
                }
               
            }
            return list.ToList();
        }

        public List<UserModels> GetAllModel(string field, string order)
        {
            var list = GetAll().Select(s=>
            {
                var m = s.ToModel();
                return m;
            });
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(order))
            {
                if (order.ToLower() == "asc")
                {
                    list = Sort<UserModels>(list, field, true);
                }
                else
                {
                    list = Sort<UserModels>(list, field, false);
                }

            }
            return list.ToList();
        }

        //执行存储过程
        public string GetType(string Type)
        {
            using (System.Transactions.TransactionScope ts=new System.Transactions.TransactionScope())
            {
                ts.Complete();
            }

            var str = ExecuteProcedureList<string>("Proc_Test", new SqlParameter("type", Type))[0];
            return str;
        }

    }
}
