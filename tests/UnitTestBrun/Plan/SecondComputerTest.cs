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
    public class SecondComputerTest
    {
        [TestMethod]
        public void TestAny()
        {
            SecondComputer secondComputer = new SecondComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "*");
            timeCloumn.SetStrategy(TimeStrategy.Any);
            var tcs = new List<TimeCloumn>()
            {
                timeCloumn
            };
            DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
            Console.WriteLine(next);
            Assert.AreEqual(start.AddSeconds(1), next);
        }
        [TestMethod]
        public void TestAnd()
        {
            SecondComputer secondComputer = new SecondComputer();
            {
                DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:3");
                TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "5,31");
                timeCloumn.SetStrategy(TimeStrategy.And);
                var tcs = new List<TimeCloumn>()
                {
                timeCloumn
                };
                DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-3-18 0:0:5"), next);
            }
            {
                DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:6");
                TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "5,31");
                timeCloumn.SetStrategy(TimeStrategy.And);
                var tcs = new List<TimeCloumn>()
                {
                timeCloumn
                };
                DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-3-18 0:0:31"), next);
            }
            {
                DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:50");
                TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "5,31");
                timeCloumn.SetStrategy(TimeStrategy.And);
                var tcs = new List<TimeCloumn>()
                {
                timeCloumn
                };
                DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:5"), next);
            }
            {
                DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:5");
                TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "5,31");
                timeCloumn.SetStrategy(TimeStrategy.And);
                var tcs = new List<TimeCloumn>()
                {
                timeCloumn
                };
                DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-3-18 0:0:31"), next);
            }
        }
        [TestMethod]
        public void TestTo()
        {
            SecondComputer secondComputer = new SecondComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "10-12");
            timeCloumn.SetStrategy(TimeStrategy.To);
            var tcs = new List<TimeCloumn>()
            {
                timeCloumn
            };
            DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:10"), next);
            var next2= secondComputer.Compute(next.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:11"), next2);
            var next3= secondComputer.Compute(next2.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:12"), next3);
            var next4= secondComputer.Compute(next3.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:2:10"), next4);
            var next5= secondComputer.Compute(next4.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:2:11"), next5);
        }
        [TestMethod]
        public void TestStep()
        {
            SecondComputer secondComputer = new SecondComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn timeCloumn = new TimeCloumn(TimeCloumnType.Second, "*/5");
            timeCloumn.SetStrategy(TimeStrategy.Step);
            var tcs = new List<TimeCloumn>()
            {
                timeCloumn
            };
            DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:0"), next);
            var next2= secondComputer.Compute(next.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:5"), next2);
            var next3 = secondComputer.Compute(next2.Value.AddSeconds(1), tcs);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:10"), next3);
        }
    }
}
