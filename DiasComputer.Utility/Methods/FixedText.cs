using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Methods
{
    public class FixedText
    {
        public static string FixEmail(string email)
        {
            return email.Trim()
                .ToLower();
        }
    }
}
