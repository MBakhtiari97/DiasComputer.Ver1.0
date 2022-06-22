using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Compute
{
    public static class Compute
    {
        public static decimal ComputeSalePrice(int price, int? salePercent)
        {
            try
            {
                var priceAmount = (decimal)price;
                var saleAmount = (decimal)salePercent / 100;
                var priceAfterSale = priceAmount - (priceAmount * saleAmount);
                priceAfterSale = Math.Floor(priceAfterSale);

                return priceAfterSale;
            }
            catch
            {
                return 0;
            }

        }
    }
}
