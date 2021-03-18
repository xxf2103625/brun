using Brun.Plan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun.Plan
{
    [TestClass]
    public class PlanTimeParseTest
    {
        [TestMethod]
        public void TestParse()
        {
            PlanTime planTime = new PlanTime();
            planTime.Parse("10 * * * *");
            Assert.IsTrue(planTime.IsSuccess);
            Assert.AreEqual(5, planTime.Times.Count);
            planTime.Parse("10 * * * * * ");
            Assert.IsTrue(planTime.IsSuccess);
            Assert.AreEqual(6, planTime.Times.Count);
        }
        [TestMethod]
        public void TestDateTimeOffSet()
        {
            DateTimeOffset date = new DateTimeOffset(DateTime.Now);
            Console.WriteLine(date);
            Console.WriteLine(date.LocalDateTime);
            var date2 = date.AddTicks(TimeSpan.FromSeconds(80).Ticks);
            Console.WriteLine(date);
            Console.WriteLine(date2);
            Console.WriteLine(date2 - date);
            Console.WriteLine((date2 - date).Ticks);
            Assert.AreEqual(TimeSpan.FromSeconds(80), (date2 - date));
        }
    }
}
