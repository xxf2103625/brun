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
            //SecondComputer secondComputer = new SecondComputer();
            MinuteComputer minuteComputer = new MinuteComputer();
            //secondComputer.SetNext(minuteComputer);
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            //TimeCloumn secondCloumn = new TimeCloumn(TimeCloumnType.Second, "5");
            TimeCloumn minuteCloumn = new TimeCloumn(TimeCloumnType.Minute, "*");
            //secondCloumn.SetStrategy(TimeStrategy.Number);
            minuteCloumn.SetStrategy(TimeStrategy.Any);
            var tcs = new List<TimeCloumn>()
            {
                //secondCloumn,
                minuteCloumn
            };
            DateTimeOffset? next = minuteComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:0"), next);
            DateTimeOffset? next2 = minuteComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:1:1"), next2);
        }
        [TestMethod]
        public void TestNumber()
        {
            //SecondComputer secondComputer = new SecondComputer();
            MinuteComputer minuteComputer = new MinuteComputer();
            //secondComputer.SetNext(minuteComputer);
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            //TimeCloumn secondCloumn = new TimeCloumn(TimeCloumnType.Second, "5");
            TimeCloumn minuteCloumn = new TimeCloumn(TimeCloumnType.Minute, "10");
            //secondCloumn.SetStrategy(TimeStrategy.Number);
            minuteCloumn.SetStrategy(TimeStrategy.Number);
            var tcs = new List<TimeCloumn>()
            {
                //secondCloumn,
                minuteCloumn
            };
            DateTimeOffset? next = minuteComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:10:0"), next);
            DateTimeOffset? next2 = minuteComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:10:1"), next2);
        }
        [TestMethod]
        public void TestAnd()
        {
            //SecondComputer secondComputer = new SecondComputer();
            MinuteComputer minuteComputer = new MinuteComputer();
            //secondComputer.SetNext(minuteComputer);
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            //TimeCloumn secondCloumn = new TimeCloumn(TimeCloumnType.Second, "5");
            TimeCloumn minuteCloumn = new TimeCloumn(TimeCloumnType.Minute, "10,12,15");
            //secondCloumn.SetStrategy(TimeStrategy.Number);
            minuteCloumn.SetStrategy(TimeStrategy.And);
            var tcs = new List<TimeCloumn>()
            {
                //secondCloumn,
                minuteCloumn
            };
            DateTimeOffset? next = minuteComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:10:0"), next);
            DateTimeOffset? next2 = minuteComputer.Compute(DateTime.Parse("2021-3-18 0:11:5"),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:12:5"), next2);
            DateTimeOffset? next3 = minuteComputer.Compute(DateTime.Parse("2021-3-18 0:13:5"),new PlanTime(tcs));
            Console.WriteLine(next3);
            Assert.AreEqual(DateTime.Parse("2021-3-18 0:15:5"), next3);
            DateTimeOffset? next4 = minuteComputer.Compute(DateTime.Parse("2021-3-18 0:18:5"),new PlanTime(tcs));
            Console.WriteLine(next4);
            Assert.AreEqual(DateTime.Parse("2021-3-18 1:10:5"), next4);
        }
    }
}
