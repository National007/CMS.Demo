using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;
using Serviece.Interface;
using EntityModels;

namespace Serviece.Implementation
{
   public class UserServiece:IUserServiece
    {
        private OpenAuthDBEntities context = new OpenAuthDBEntities();
        //public List<UserModels> GetList()
        //{
        //    var list = context.User.Select(s =>
        //    new UserModels()
        //    {
        //        Account = s.Account,
        //        Name=s.Name
        //    }).ToList();

        //    return list;
        //}
        public List<User> GetList()
        {
            return context.User.ToList();
        }
    }
}
