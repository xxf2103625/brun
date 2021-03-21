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
    public class MouthComputerTest
    {
        [TestMethod]
        public void TestAny()
        {
            MonthComputer hourComputer = new MonthComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Month, "*");
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
            MonthComputer hourComputer = new MonthComputer();
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Month, "5");
            hourCloumn.SetStrategy(TimeStrategy.Number);
            var tcs = new List<TimeCloumn>()
            {
                hourCloumn,
            };
            DateTimeOffset? next = hourComputer.Compute(DateTime.Parse("2021-3-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-5-18 0:0:0"), next);
            DateTimeOffset? next2 = hourComputer.Compute(DateTime.Parse("2022-6-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2023-5-18 0:0:0"), next2);
        }
        [TestMethod]
        public void TestTo()
        {
            MonthComputer hourComputer = new MonthComputer();
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Month, "5-8");
            hourCloumn.SetStrategy(TimeStrategy.To);
            var tcs = new List<TimeCloumn>()
            {
                hourCloumn,
            };
            DateTimeOffset? next = hourComputer.Compute(DateTime.Parse("2021-3-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-5-18 0:0:0"), next);

            DateTimeOffset? next2 = hourComputer.Compute(DateTime.Parse("2022-5-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2022-5-18 0:0:0"), next2);

            DateTimeOffset? next3 = hourComputer.Compute(DateTime.Parse("2022-8-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next3);
            Assert.AreEqual(DateTime.Parse("2022-8-18 0:0:0"), next3);

            DateTimeOffset? next4 = hourComputer.Compute(DateTime.Parse("2022-9-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next4);
            Assert.AreEqual(DateTime.Parse("2023-5-18 0:0:0"), next4);

            DateTimeOffset? next5 = hourComputer.Compute(DateTime.Parse("2022-7-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next5);
            Assert.AreEqual(DateTime.Parse("2022-7-18 0:0:0"), next5);
        }

        [TestMethod]
        public void TestStep()
        {
            MonthComputer hourComputer = new MonthComputer();
            TimeCloumn hourCloumn = new TimeCloumn(TimeCloumnType.Month, "5-11/3");
            hourCloumn.SetStrategy(TimeStrategy.Step);
            var tcs = new List<TimeCloumn>()
            {
                hourCloumn,
            };
            DateTimeOffset? next = hourComputer.Compute(DateTime.Parse("2021-3-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-5-18 0:0:0"), next);

            DateTimeOffset? next2 = hourComputer.Compute(DateTime.Parse("2022-6-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2022-8-18 0:0:0"), next2);

            DateTimeOffset? next3 = hourComputer.Compute(DateTime.Parse("2022-8-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next3);
            Assert.AreEqual(DateTime.Parse("2022-8-18 0:0:0"), next3);

            DateTimeOffset? next4 = hourComputer.Compute(DateTime.Parse("2022-12-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next4);
            Assert.AreEqual(DateTime.Parse("2023-5-18 0:0:0"), next4);

            DateTimeOffset? next5 = hourComputer.Compute(DateTime.Parse("2022-7-18 0:0:0"),new PlanTime(tcs));
            Console.WriteLine(next5);
            Assert.AreEqual(DateTime.Parse("2022-8-18 0:0:0"), next5);
        }
    }
}
