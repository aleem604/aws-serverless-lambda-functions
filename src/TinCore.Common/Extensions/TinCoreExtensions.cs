using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TinCore.Common
{
   public static class TCoreExts
    {
        public static string Serialize(this string str)
        {
            if (!string.IsNullOrEmpty(str)){
                return JsonConvert.SerializeObject(str);
            }
            return str;
        }
        public static dynamic DeSerialize(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return JsonConvert.DeserializeObject(str);
            }
            return str;
        }
        public static short ToInt(this Enum enumValue)
        {
            return (short)((object)enumValue);
        }

    }
}
