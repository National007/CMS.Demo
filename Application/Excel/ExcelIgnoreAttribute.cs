using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Excel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ExcelIgnoreAttribute : Attribute
    {
        public ExcelIgnoreAttribute() { }
    }
}
