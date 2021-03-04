using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class TimeWorkerTest
    {
        [TestMethod]
        public void TestDateTime()
        {
            DateTimeOffset offset1 = DateTime.Now;
            DateTimeOffset offset2 = DateTime.UtcNow;
            Console.WriteLine("off1:{0},off2:{1}", offset1.ToString(), offset2.ToString());
            Assert.AreNotEqual(offset1.ToLocalTime(), offset2.ToLocalTime());
        }
    }
}
