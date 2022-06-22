using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Generator
{
    public class StringGenerator
    {
        public static string GenerateUniqueCode()
        {
            return Guid.NewGuid()
                .ToString()
                .Replace("-", "");
        }
    }
}
