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
    public class DayComputerTest
    {
        [TestMethod]
        public void TestNumber()
        {
            DayComputer dayComputer = new DayComputer();
            DateTimeOffset start = DateTime.Parse("2021-3-18 0:0:59");
            TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "3");
            dayCloumn.SetStrategy(TimeStrategy.Number);
            var tcs = new List<TimeCloumn>()
            {
                dayCloumn,
            };
            DateTimeOffset? next = dayComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next);
            Assert.AreEqual(DateTime.Parse("2021-4-3 0:1:0"), next);
            DateTimeOffset? next2 = dayComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
            Console.WriteLine(next2);
            Assert.AreEqual(DateTime.Parse("2021-4-3 0:1:1"), next2);
            DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-4-18 4:1:1"),new PlanTime(tcs));
            Console.WriteLine(next3);
            Assert.AreEqual(DateTime.Parse("2021-5-3 4:1:1"), next3);
        }
        [TestMethod]
        public void TestNumber_2()
        {
            {
                DayComputer dayComputer = new DayComputer();
                DateTimeOffset start = DateTime.Parse("2021-1-18 0:0:59");
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "31");
                dayCloumn.SetStrategy(TimeStrategy.Number);
                var tcs = new List<TimeCloumn>()
                {
                    dayCloumn,
                };
                DateTimeOffset? next = dayComputer.Compute(start.AddSeconds(1),new PlanTime(tcs));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-1-31 0:1:0"), next);

                DateTimeOffset? next2 = dayComputer.Compute(next.Value.AddSeconds(1),new PlanTime(tcs));
                Console.WriteLine(next2);
                Assert.AreEqual(DateTime.Parse("2021-1-31 0:1:1"), next2);

                DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-2-1 0:1:1"),new PlanTime(tcs));
                Console.WriteLine(next3);
                //2月没有31号，快进到3月1号，再重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:1:1"), next3);

                DateTimeOffset? next4 = dayComputer.Compute(DateTime.Parse("2021-2-28 0:1:1"),new PlanTime(tcs));
                Console.WriteLine(next4);
                //2月没有31号，快进到3月1号，再重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:1:1"), next4);
            }

            {
                DayComputer dayComputer = new DayComputer();
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "30");
                dayCloumn.SetStrategy(TimeStrategy.Number);
                var tcs = new List<TimeCloumn>()
                {
                dayCloumn,
                };
                DateTimeOffset? next5 = dayComputer.Compute(DateTime.Parse("2021-1-31 0:1:1"),new PlanTime(tcs));
                Console.WriteLine(next5);
                //2月没有31号，快进到3月1号，再重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:1:1"), next5);
            }
        }
        [TestMethod]
        public void TestTo()
        {
            {
                DayComputer dayComputer = new DayComputer();
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "28-31");
                dayCloumn.SetStrategy(TimeStrategy.To);
                List<TimeCloumn> tcs = new List<TimeCloumn>()
                {
                dayCloumn,
                };
                DateTimeOffset? next = dayComputer.Compute(DateTime.Parse("2021-2-18 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-2-28 0:0:0"), next);

                DateTimeOffset? next2 = dayComputer.Compute(DateTime.Parse("2021-3-1 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next2);
                Assert.AreEqual(DateTime.Parse("2021-3-28 0:0:0"), next2);

                DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-4-29 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next3);
                Assert.AreEqual(DateTime.Parse("2021-4-29 0:0:0"), next3);
            }
            {
                DayComputer dayComputer = new DayComputer();
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "30-31");
                dayCloumn.SetStrategy(TimeStrategy.To);
                List<TimeCloumn> tcs = new List<TimeCloumn>()
                {
                dayCloumn,
                };
                DateTimeOffset? next = dayComputer.Compute(DateTime.Parse("2021-2-18 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next);
                //2月没有31号，快进到3月1号，再重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:0:0"), next);

                DateTimeOffset? next2 = dayComputer.Compute(DateTime.Parse("2021-3-30 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next2);
                Assert.AreEqual(DateTime.Parse("2021-3-30 0:0:0"), next2);

                DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-4-29 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next3);
                Assert.AreEqual(DateTime.Parse("2021-4-30 0:0:0"), next3);
            }
        }
        [TestMethod]
        public void TestStep()
        {
            {
                DayComputer dayComputer = new DayComputer();
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "5/7");
                dayCloumn.SetStrategy(TimeStrategy.Step);
                List<TimeCloumn> tcs = new List<TimeCloumn>()
                {
                dayCloumn,
                };
                DateTimeOffset? next = dayComputer.Compute(DateTime.Parse("2021-2-18 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-2-19 0:0:0"), next);

                DateTimeOffset? next2 = dayComputer.Compute(DateTime.Parse("2021-2-28 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next2);
                //快进到3/1 需要重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:0:0"), next2);

                DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-3-6 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next3);
                Assert.AreEqual(DateTime.Parse("2021-3-12 0:0:0"), next3);
            }
            {
                DayComputer dayComputer = new DayComputer();
                TimeCloumn dayCloumn = new TimeCloumn(TimeCloumnType.Day, "5-30/8");
                dayCloumn.SetStrategy(TimeStrategy.Step);
                List<TimeCloumn> tcs = new List<TimeCloumn>()
                {
                dayCloumn,
                };
                DateTimeOffset? next = dayComputer.Compute(DateTime.Parse("2021-2-18 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-2-21 0:0:0"), next);

                DateTimeOffset? next2 = dayComputer.Compute(DateTime.Parse("2021-2-22 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next2);
                //到了2月29 修正成3月1号，等待重新计算
                Assert.AreEqual(DateTime.Parse("2021-3-1 0:0:0"), next2);


                DateTimeOffset? next3 = dayComputer.Compute(DateTime.Parse("2021-4-6 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next3);
                Assert.AreEqual(DateTime.Parse("2021-4-13 0:0:0"), next3);

                DateTimeOffset? next4 = dayComputer.Compute(DateTime.Parse("2021-3-30 0:0:0"),new PlanTime(tcs));
                Console.WriteLine(next4);
                // 修正成4-1号，等待重新计算
                Assert.AreEqual(DateTime.Parse("2021-4-1 0:0:0"), next4);
            }
        }

    }
}
