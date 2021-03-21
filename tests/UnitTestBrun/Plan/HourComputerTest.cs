using Brun.Plan;
using Brun.Plan.TimeComputers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun.Plan
{
    [TestClass]
    public class HourComputerTest
    {
        [TestMethod]
        public void TestAny()
        {
            HourComputer hourComputer = new HourComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Hour, "*");
            hourCloumn.SetStrategy(TimeStrategy.Any);
            var tcs = new List<TimeCloumn>()
            {
                hourCloumn,
            };
            DateTimeOffset? next = hourComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:0"), next);
            DateTimeOffset? next2 = hourComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:1"), next2);
        }
        [TestMethod]
        public void TestNumber()
        {
            HourComputer hourComputer = new HourComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Hour, "3");
            hourCloumn.SetStrategy(TimeStrategy.Number);
            var tcs = new List<TimeCloumn>()
            {
                hourCloumn,
            };
            DateTimeOffset? next = hourComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 3:1:0"), next);
            DateTimeOffset? next2 = hourComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 3:1:1"), next2);
            DateTimeOffset? next3 = hourComputer.Compute(DateTime.Parse("2021-3-18 4:1:1"),new PlanTime(tcs));
            Console.WriteLine(next3);
            Assert.AreEqual(DateTime.Parse("2021-3-19 3:1:1"), next3);
        }
    }
}
