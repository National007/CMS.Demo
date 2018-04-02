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
   public class UserServiece:BaseRepository<UserModels>,IUserServiece
    {

        public IEnumerable<UserModels> GetList()
        {
            var list = new List<UserModels>();
            //var list = this.Find().ToList();
            using (var ent=new OpenAuthDBEntities())
            {
                list = ent.User.ToList().Select(s=>
                {
                    var m = new UserModels();
                    m.Account = s.Account;
                    m.Password = s.Password;
                    m.Name = s.Name;
                    return m;
                }).ToList();
            }

            return list;
        }
    }
}
