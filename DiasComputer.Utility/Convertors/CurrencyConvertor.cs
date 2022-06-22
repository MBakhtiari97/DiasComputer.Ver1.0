using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Convertors
{
    public static class CurrencyConvertor
    {
        public static string ToToman(this int value)
        {
            return value.ToString("#,0") + " تومان";
        }
    }
}
