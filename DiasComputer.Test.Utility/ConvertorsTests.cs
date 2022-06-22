using System;
using DiasComputer.Utility.Convertors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiasComputer.Test.Utility
{
    [TestClass]
    public class ConvertorsTests
    {
        [TestMethod]
        public void TestToTomanConvertorMethod()
        {
            //Arrange
            var expected = "1,000,000 تومان";

            //Act
            var actual = CurrencyConvertor.ToToman(1000000);

            //Assert
            Assert.AreEqual(expected,actual);

        }

        [TestMethod]
        public void TestToShamsiConvertorMethod()
        {
            //Arrange
            var expected = "1401/03/27";

            //Act
            var actual = DateConvertor.ToShamsi(DateTime.Parse("06/17/2022"));

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestToMiladiConvertorMethod()
        {
            //Arrange
            var expected = "6/17/2022 12:00:00 AM";

            //Act
            var actual = DateConvertor.ToMiladi("1401/03/27").ToString();

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestGetMonthStartDateMethod()
        {
            //Arrange
            var expected = DateTime.Parse(DateTime.Now.Month + "/"
                                                             + "01"
                                                             + "/" + DateTime.Now.Year);

            //Act
            var actual = GetCustomDates.GetMonthStartDate();

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestGetMonthEndDateMethod()
        {
            //Arrange
            var expected = DateTime.Parse(DateTime.Now.Month + "/"
                                                             + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/"
                                                             + DateTime.Now.Year);

            //Act
            var actual = GetCustomDates.GetMonthEndDate();

            //Assert
            Assert.AreEqual(expected, actual);

        }
    }
}