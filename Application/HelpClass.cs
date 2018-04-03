
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Application
{
    public static  class HelpClass
    {
        public static T ChangeType<T>(object obj)
        {
            if (obj == null)
            {
                return (T)Convert.ChangeType("", typeof(T));
            }
            else
            {
                if (obj.GetType() == typeof(T))
                    return (T)obj;
            }
            return (T)Convert.ChangeType(obj, typeof(T));
        }
        public static T[] ChangeArray<T>(object[] objArray)
        {
            T[] outArray = new T[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                outArray[i] = ChangeType<T>(objArray[i]);
            }
            return outArray;
        }
        public static int[] ChangeToIntArray(object[] objArray)
        {
            int[] outArray = new int[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                outArray[i] = ChangeType<int>(objArray[i]);
            }
            return outArray;
        }
        public static T[] StrConvertToArray<T>(string obj, char[] splitChar)
        {
            string[] strArray = obj.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            return ChangeArray<T>(strArray);
        }

        public static string GetEnumDescription<T>(object val)
        {

            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception("不是枚举类");
            }
            string enumItem = Enum.GetName(type, Convert.ToInt32(val));
            if (enumItem == null) { return string.Empty; }

            object[] attres = type.GetField(enumItem).GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute item in attres)
            {
                name = item.Description;
            }
            return name;
        }
       
    }
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string attrStr)
        {
            this.Description = attrStr;
        }
        public string Description { get; set; }

       
    }
}
