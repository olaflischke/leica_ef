using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal.Extensions
{
    public static class ExtensionMethods
    {
        public static bool IsNumeric(this string text)
        {
            return double.TryParse(text, out double _);
        }
    }
}
