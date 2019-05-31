using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void DateTimeUnixConversion()
        {
            DateTime expectedDt = DateTime.UtcNow;
            var dt = expectedDt.GetUnixTime().GetDate();
            Assert.AreEqual(expectedDt.Date, dt.Date);
            Assert.AreEqual(expectedDt.Second, dt.Second);
            Assert.AreEqual(expectedDt.Minute, dt.Minute);
            Assert.AreEqual(expectedDt.Hour, dt.Hour);

            // Millisecond part is going to differ obviously
        }
    }
}
