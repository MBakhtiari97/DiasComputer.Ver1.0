using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Generator
{
    public static class ReferCodeGenerator
    {
        public static int Generate()
        {
            Random random = new Random();
            var rndNumber = random.Next(10000000,99999999);
            return rndNumber;
        }
    }
}
