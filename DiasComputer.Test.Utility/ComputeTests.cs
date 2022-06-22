using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Utility.Compute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiasComputer.Test.Utility
{
    [TestClass]
    public class ComputeTests
    {
        [TestMethod]
        public void GetNewPriceWithPercentSale()
        {
            //Arrange
            var expected = 900000;

            //Act
            var actual = Compute.ComputeSalePrice(1000000,10);

            //Assert
            Assert.AreEqual(expected,actual);
        }
    }
}
