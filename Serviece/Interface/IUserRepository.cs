using EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Serviece.Interface
{
    public interface IUserRepository:IBaseRepository<User>
    {
        List<User> GetList();

        List<User> GetListAll();

        string GetType(string Type);
    }
}
