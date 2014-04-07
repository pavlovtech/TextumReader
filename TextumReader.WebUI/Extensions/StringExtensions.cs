using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextumReader.WebUI.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this String str) 
        {
            return (T)Enum.Parse(typeof(T), str, true);
        }
    }
}