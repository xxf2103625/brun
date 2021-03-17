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
    public class MinuteComputerTest
    {
        [TestMethod]
        public void TestAny()
        {
            SecondComputer secondComputer = new SecondComputer();
            MinuteComputer minuteComputer = new MinuteComputer();
            secondComputer.SetNext(minuteComputer);
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn secondCloumn = new TimeCloumn(TimeCloumnType.Second, "5");
            TimeCloumn minuteCloumn = new TimeCloumn(TimeCloumnType.Minute, "*");
            secondCloumn.SetStrategy(TimeStrategy.Number);
            minuteCloumn.SetStrategy(TimeStrategy.Any);
            var tcs = new List<TimeCloumn>()
            {
                secondCloumn,
                minuteCloumn
            };
            DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:5"), next);
            DateTimeOffset? next2 = secondComputer.Compute(next.Value.AddSeconds(1), tcs);
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:2:5"), next2);
        }
        [TestMethod]
        public void TestNumber()
        {
            SecondComputer secondComputer = new SecondComputer();
            MinuteComputer minuteComputer = new MinuteComputer();
            secondComputer.SetNext(minuteComputer);
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn secondCloumn = new TimeCloumn(TimeCloumnType.Second, "5");
            TimeCloumn minuteCloumn = new TimeCloumn(TimeCloumnType.Minute, "10");
            secondCloumn.SetStrategy(TimeStrategy.Number);
            minuteCloumn.SetStrategy(TimeStrategy.Number);
            var tcs = new List<TimeCloumn>()
            {
                secondCloumn,
                minuteCloumn
            };
            DateTimeOffset? next = secondComputer.Compute(start.AddSeconds(1), tcs);
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:10:5"), next);
            DateTimeOffset? next2 = secondComputer.Compute(next.Value.AddSeconds(1), tcs);
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 1:10:5"), next2);
        }
    }
}
