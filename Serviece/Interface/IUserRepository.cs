using EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebModels;

namespace Serviece.Interface
{
    public interface IUserRepository:IBaseRepository<User>
    {
        List<User> GetList();

        List<User> GetListAll();

        List<User> GetAll(string field, string order);
        List<UserModels> GetAllModel(string field, string order);

        string GetType(string Type);
    }
}
