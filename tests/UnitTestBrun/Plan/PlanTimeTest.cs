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
    public class PlanTimeTest
    {
        [TestMethod]
        public void TestCompute()
        {
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();

            {
                planTime.Parse("0 0 0 1 *");
                computer.SetPlanTime(planTime);
                DateTimeOffset? next = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-4-1 0:0:0"), next);

                planTime.Parse("31 1 1 1 8 *");
                computer.SetPlanTime(planTime);
                var next2 = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-8-1 1:1:31"), next2);
            }
            {
                planTime.Parse("31 1 1 1 8 2059");
                computer.SetPlanTime(planTime);
                var next3 = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2059-8-1 1:1:31"), next3);
            }
            {
                planTime.Parse("31 1 1 31 * *");
                computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-3-31 1:1:31"), n1);

                var n2 = computer.GetNextTime(n1.Value);
                Assert.AreEqual(DateTime.Parse("2021-5-31 1:1:31"), n2);

                var n3 = computer.GetNextTime(n2.Value);
                Assert.AreEqual(DateTime.Parse("2021-7-31 1:1:31"), n3);
            }
            {
                planTime.Parse("10-50/15 1 1 * * *");
                computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:10"), n1);

                var n2 = computer.GetNextTime(n1.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:25"), n2);

                var n3 = computer.GetNextTime(n2.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:40"), n3);

                var n4 = computer.GetNextTime(n3.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-19 1:1:10"), n4);
            }
            {
                planTime.Parse("10-50/15 1 1 * * 2010");
                computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(null, n1);
            }
        }
        [TestMethod]
        public void TestComputeMouth()
        {
            List<DateTime> rs = new List<DateTime>()
            {
                DateTime.Parse("02/29/2024 00:00:00 +08:00"),
                DateTime.Parse("02/29/2028 00:00:00 +08:00"),
                DateTime.Parse("02/29/2032 00:00:00 +08:00"),
                DateTime.Parse("02/29/2036 00:00:00 +08:00"),
                DateTime.Parse("02/29/2040 00:00:00 +08:00"),
                DateTime.Parse("02/29/2044 00:00:00 +08:00"),
                DateTime.Parse("02/29/2048 00:00:00 +08:00"),
                DateTime.Parse("02/29/2052 00:00:00 +08:00"),
                DateTime.Parse("02/29/2056 00:00:00 +08:00"),
                DateTime.Parse("02/29/2060 00:00:00 +08:00"),
            };
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            planTime.Parse("0 0 0 29 2");
            computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Now;// = computer.GetNextTime();
            for (int i = 0; i < 10; i++)
            {
                if (next != null)
                {
                    next = computer.GetNextTime(next.Value);
                    Console.WriteLine(next);
                    Assert.AreEqual(rs[i], next);
                }
            }
        }
        [TestMethod]
        public void TestComputeMouth_31()
        {
            List<DateTime> rs = new List<DateTime>()
            {
                DateTime.Parse("03/31/2021 00:00:00 +08:00"),
                DateTime.Parse("05/31/2021 00:00:00 +08:00"),
                DateTime.Parse("07/31/2021 00:00:00 +08:00"),
                DateTime.Parse("08/31/2021 00:00:00 +08:00"),
                DateTime.Parse("10/31/2021 00:00:00 +08:00"),
                DateTime.Parse("12/31/2021 00:00:00 +08:00"),
                DateTime.Parse("01/31/2022 00:00:00 +08:00"),
                DateTime.Parse("03/31/2022 00:00:00 +08:00"),
                DateTime.Parse("05/31/2022 00:00:00 +08:00"),
                DateTime.Parse("07/31/2022 00:00:00 +08:00"),
            };
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            planTime.Parse("0 0 0 31 *");
            computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {

                if (next != null)
                {
                    next = computer.GetNextTime(next.Value);
                    Console.WriteLine(next);
                    Assert.AreEqual(rs[i], next);
                }
            }
        }
    }
}
