using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Convertors
{
    public static class GetCustomDates
    {
        public static DateTime GetMonthStartDate()
        {
            var startDate = DateTime.Parse(DateTime.Now.Month + "/"
                                                              + "01"
                                                              + "/" + DateTime.Now.Year);
            return startDate;
        }

        public static DateTime GetMonthEndDate()
        {
            var endDate = DateTime.Parse(DateTime.Now.Month + "/"
                                                            + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/"
                                                            + DateTime.Now.Year);

            return endDate;
        }
    }
}
