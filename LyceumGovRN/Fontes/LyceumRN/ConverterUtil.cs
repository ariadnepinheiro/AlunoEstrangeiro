using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public static class ConverterUtil
    {
        public static decimal? ToDecimal(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                decimal ret = 0;
                if (decimal.TryParse(str, out ret))
                    return ret;
                else
                    return null;

            }
        }

        public static decimal? ToDecimal(this string str, out bool isInvalid)
        {
            if (string.IsNullOrEmpty(str))
            {
                isInvalid= false;
                return null;
            }
            else
            {
                decimal ret = 0;
                if (isInvalid=!decimal.TryParse(str, out ret))
                    return null;
                else
                    return ret;

            }
        }
    }
}
