using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/"
                                     + pc.GetMonth(value).ToString("00") + "/"
                                     + pc.GetDayOfMonth(value).ToString("00");
        }
        public static DateTime ToMiladi(string date)
        {
            string[] std = date.Split("/");
            var miladiDate = new DateTime(int.Parse(std[0]),
                int.Parse(std[1]),
                int.Parse(std[2]),
                new PersianCalendar()
            );

            return miladiDate;
        }
    }
}
