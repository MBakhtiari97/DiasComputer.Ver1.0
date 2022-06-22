using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Utility.Compute;
using DiasComputer.Utility.Methods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiasComputer.Test.Utility
{
    [TestClass]
    public class MethodsTests
    {
        [TestMethod]
        public void GetFixedEmailAddress()
        {
            //Arrange
            var expected = "test@test.com";

            //Act
            var actual = FixedText.FixEmail("TeSt@TesT.Com ");

            //Assert
            Assert.AreEqual(expected,actual);
        }
    }
}
