using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serviece.Interface;

namespace Serviece.Implementation
{
   public class UserRepository:BaseRepository<User>,IUserRepository
    {
        public List<User> GetList()
        {
            return GetAll().ToList();
        }
    }
}
