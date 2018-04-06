using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels.OtherServer
{
   public class T_User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public int IsAdmin { get; set; }
    }
}
