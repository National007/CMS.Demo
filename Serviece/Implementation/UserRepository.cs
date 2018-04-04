using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serviece.Interface;
using System.Data.SqlClient;

namespace Serviece.Implementation
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public List<User> GetList()
        {
            return GetAll().ToList();
        }

        public List<User> GetListAll()
        {
            string sql = string.Format("select * from [dbo].[User]");
            return SqlQuery<User>(sql).ToList();
        }

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
