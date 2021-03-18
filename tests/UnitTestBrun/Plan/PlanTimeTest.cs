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
    }
}
