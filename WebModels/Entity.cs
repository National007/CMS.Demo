using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
   public class Entity
    {
        public string Id { get; set; }
        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
