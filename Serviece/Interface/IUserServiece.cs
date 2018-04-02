using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebModels;

namespace Serviece.Interface
{
   public interface IUserServiece:IBaseRepository<UserModels>
    {
        IEnumerable<UserModels> GetList();
    }
}
