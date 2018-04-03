using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Serviece
{
   public class Entities:DbContext
    {
        public Entities() 
            : base("DBcontext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除EF的表名公约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


        }

    }
}
