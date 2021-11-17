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
        public void TestComputeSimple()
        {
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();

            {
                planTime.Parse("0 0 0 1 *");
                //computer.SetPlanTime(planTime);
                DateTimeOffset? next = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Console.WriteLine(next);
                Assert.AreEqual(DateTime.Parse("2021-4-1 0:0:0"), next);

                planTime.Parse("31 1 1 1 8 *");
                //computer.SetPlanTime(planTime);
                var next2 = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-8-1 1:1:31"), next2);
            }
            {
                planTime.Parse("31 1 1 1 8 2059");
                //computer.SetPlanTime(planTime);
                var next3 = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2059-8-1 1:1:31"), next3);
            }
            {
                planTime.Parse("31 1 1 31 * *");
                //computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-3-31 1:1:31"), n1);

                var n2 = computer.GetNextTime(planTime, n1.Value);
                Assert.AreEqual(DateTime.Parse("2021-5-31 1:1:31"), n2);

                var n3 = computer.GetNextTime(planTime, n2.Value);
                Assert.AreEqual(DateTime.Parse("2021-7-31 1:1:31"), n3);
            }
            {
                planTime.Parse("10-50/15 1 1 * * *");
                //computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:10"), n1);

                var n2 = computer.GetNextTime(planTime, n1.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:25"), n2);

                var n3 = computer.GetNextTime(planTime, n2.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-18 1:1:40"), n3);

                var n4 = computer.GetNextTime(planTime, n3.Value);
                Assert.AreEqual(DateTime.Parse("2021-3-19 1:1:10"), n4);
            }
            {
                planTime.Parse("10-50/15 1 1 * * 2010");
                //computer.SetPlanTime(planTime);
                var n1 = computer.GetNextTime(planTime, DateTime.Parse("2021-3-18 0:0:59"));
                Assert.AreEqual(null, n1);
            }
        }
        [TestMethod]
        public void TestComputeMouth_2_29()
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
            //computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Now;// = computer.GetNextTime();
            for (int i = 0; i < 10; i++)
            {
                if (next != null)
                {
                    next = computer.GetNextTime(planTime, next.Value);
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
            //computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
            for (int i = 0; i < 10; i++)
            {
                if (next != null)
                {
                    next = computer.GetNextTime(planTime, next.Value);
                    Console.WriteLine(next);
                    Assert.AreEqual(rs[i], next);
                }
            }
        }
        [TestMethod]
        public void TestComputeDayLast()
        {
            List<DateTime> rs = new List<DateTime>()
            {
                DateTime.Parse("03/31/2021 00:00:00 +08:00"),
                DateTime.Parse("04/30/2021 00:00:00 +08:00"),
                DateTime.Parse("05/31/2021 00:00:00 +08:00"),
                DateTime.Parse("06/30/2021 00:00:00 +08:00"),
                DateTime.Parse("07/31/2021 00:00:00 +08:00"),
                DateTime.Parse("08/31/2021 00:00:00 +08:00"),
                DateTime.Parse("09/30/2021 00:00:00 +08:00"),
                DateTime.Parse("10/31/2021 00:00:00 +08:00"),
                DateTime.Parse("11/30/2021 00:00:00 +08:00"),
                DateTime.Parse("12/31/2021 00:00:00 +08:00"),
                DateTime.Parse("01/31/2022 00:00:00 +08:00"),
                DateTime.Parse("02/28/2022 00:00:00 +08:00"),
            };
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            planTime.Parse("0 0 0 L *");
            //computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Parse( "03/3/2021 00:00:00 +08:00");
            for (int i = 0; i < 12; i++)
            {

                if (next != null)
                {
                    next = computer.GetNextTime(planTime, next.Value);
                    Console.WriteLine(next);
                    Assert.AreEqual(rs[i], next);
                }
            }
            Console.WriteLine("---------------");
            {
                List<DateTime> rs2 = new List<DateTime>()
                {
                DateTime.Parse("05/01/2021 00:00:00 +08:00"),
                DateTime.Parse("07/01/2021 00:00:00 +08:00"),
                DateTime.Parse("08/01/2021 00:00:00 +08:00"),
                DateTime.Parse("10/01/2021 00:00:00 +08:00"),
                DateTime.Parse("12/01/2021 00:00:00 +08:00"),
                DateTime.Parse("01/01/2022 00:00:00 +08:00"),
                DateTime.Parse("03/01/2022 00:00:00 +08:00"),
                DateTime.Parse("05/01/2022 00:00:00 +08:00"),
                DateTime.Parse("07/01/2022 00:00:00 +08:00"),
                DateTime.Parse("08/01/2022 00:00:00 +08:00"),
                DateTime.Parse("10/01/2022 00:00:00 +08:00"),
                DateTime.Parse("12/01/2022 00:00:00 +08:00"),
                };
                planTime.Parse("0 0 0 30L *");
                DateTimeOffset? next2 = DateTime.Parse("04/23/2021 00:00:00 +08:00");
                for (int i = 0; i < 12; i++)
                {

                    if (next2 != null)
                    {
                        next2 = computer.GetNextTime(planTime, next2.Value);
                        Console.WriteLine(next2);
                        Assert.AreEqual(rs2[i], next2);
                    }
                }
            }

        }

        [TestMethod]
        public void TestComputeAnd()
        {
            List<DateTime?> rs = new List<DateTime?>()
            {
                DateTime.Parse("03/31/2021 00:00:00 +08:00"),
                DateTime.Parse("06/01/2021 00:00:00 +08:00"),
                DateTime.Parse("06/05/2021 00:00:00 +08:00"),
                DateTime.Parse("03/01/2023 00:00:00 +08:00"),
                DateTime.Parse("03/05/2023 00:00:00 +08:00"),
                DateTime.Parse("03/31/2023 00:00:00 +08:00"),
                DateTime.Parse("06/01/2023 00:00:00 +08:00"),
                DateTime.Parse("06/05/2023 00:00:00 +08:00"),
                null,
                null,
                null,
                null,
            };
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            planTime.Parse("0 0 0 1,5,31 3,6 2021,2023");
            //computer.SetPlanTime(planTime);
            DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine(next);
                if (next != null)
                {
                    next = computer.GetNextTime(planTime, next.Value);

                    Assert.AreEqual(rs[i], next);
                }
            }
        }
        [TestMethod]
        public void TestComputeWeekNumber()
        {
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            //computer.SetPlanTime(planTime);
            {
                planTime.Parse("0 0 0 x1 3,6 2021,2023");
                DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
                Console.WriteLine("start week:{0}", (int)next.Value.DayOfWeek);
                next = computer.GetNextTime(planTime, next.Value);
                Console.WriteLine(next + ",week:{0}", (int)next.Value.DayOfWeek);
                Assert.AreEqual(DateTime.Parse("03/21/2021 00:00:00 +08:00"), next);
            }
            {
                planTime.Parse("0 0 0 x7 3,6 2021,2023");
                DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
                Console.WriteLine("start week:{0}", (int)next.Value.DayOfWeek);
                next = computer.GetNextTime(planTime,next.Value);
                Console.WriteLine(next + ",week:{0}", (int)next.Value.DayOfWeek);
                Assert.AreEqual(DateTime.Parse("03/27/2021 00:00:00 +08:00"), next);
            }
            {
                planTime.Parse("0 0 0 x3 3,6 2021,2023");//周2
                DateTimeOffset? next = DateTime.Parse("03/24/2021 07:38:14 +08:00");//周三
                Console.WriteLine("start week:{0}", (int)next.Value.DayOfWeek);
                next = computer.GetNextTime(planTime,next.Value);
                Console.WriteLine(next + ",week:{0}", (int)next.Value.DayOfWeek);
                Assert.AreEqual(DateTime.Parse("03/30/2021 00:00:00 +08:00"), next);//周二
            }
        }
        [TestMethod]
        public void TestComputeWeekAnd()
        {
            List<DateTimeOffset> r = new List<DateTimeOffset>()
            {
                DateTimeOffset.Parse("03/21/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/23/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/25/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/27/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/28/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/30/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/01/2021  00:00:00 +08:00"),
                DateTimeOffset.Parse("04/03/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/04/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/06/2021 00:00:00 +08:00"),
            };

            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            //computer.SetPlanTime(planTime);
            {
                planTime.Parse("0 0 0 x1,3,5,7 *");
                DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
                for (int i = 0; i < 10; i++)
                {
                    if (next != null)
                    {
                        Console.WriteLine(next + ",星期DayOfWeek：{0}", (int)next.Value.DayOfWeek);
                    }
                    next = computer.GetNextTime(planTime,next.Value);
                    Assert.AreEqual(r[i], next);
                }
            }
        }
        [TestMethod]
        public void TestComputeWeekTo()
        {
            List<DateTimeOffset> r = new List<DateTimeOffset>()
            {
                DateTimeOffset.Parse("03/23/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/24/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/25/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/26/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/27/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/30/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("03/31/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/01/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/02/2021 00:00:00 +08:00"),
                DateTimeOffset.Parse("04/03/2021 00:00:00 +08:00"),
            };
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            //computer.SetPlanTime(planTime);
            {
                planTime.Parse("0 0 0 x3-7 *");
                DateTimeOffset? next = DateTime.Parse("03/20/2021 07:38:14 +08:00");
                for (int i = 0; i < 10; i++)
                {
                    if (next != null)
                    {
                        Console.WriteLine(next + ",星期DayOfWeek：{0}", (int)next.Value.DayOfWeek);
                    }
                    next = computer.GetNextTime(planTime,next.Value);
                    Assert.AreEqual(r[i], next);
                }
            }
        }
        [TestMethod]
        public void TestComputeWeekAny()
        {
            List<DateTimeOffset> r = new List<DateTimeOffset>()
            {
                DateTimeOffset.Parse("02/21/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/22/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/23/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/24/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/25/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/26/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/27/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/28/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("03/01/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("03/02/2021 00:00:00 +08:00"),
            };
            /*
DateTimeOffset.Parse("02/21/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/22/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/23/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/24/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/25/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/26/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/27/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("02/28/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("03/01/2021 00:00:00 +08:00"),
DateTimeOffset.Parse("03/02/2021 00:00:00 +08:00"),
             */
            PlanTimeComputer computer = new PlanTimeComputer();
            PlanTime planTime = new PlanTime();
            //computer.SetPlanTime(planTime);
            {
                planTime.Parse("0 0 0 x* *");
                DateTimeOffset? next = DateTime.Parse("02/20/2021 07:38:14 +08:00");
                for (int i = 0; i < 10; i++)
                {
                    if (next != null)
                    {
                        next = computer.GetNextTime(planTime,next.Value);
                        Assert.AreEqual(r[i], next);
                        Console.WriteLine(next + ",星期DayOfWeek：{0}", (int)next.Value.DayOfWeek);
                    }
                }
            }
        }
    }
}
