using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
   public class UserModels:Entity
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public int Status { get; set; }
        public string BizCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string CrateId { get; set; }

        public string TypeName { get; set; }
        public string TypeId { get; set; }
    }
}
