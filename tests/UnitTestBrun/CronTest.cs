using Brun.Plan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class CronTest
    {
        [TestMethod]
        public void TestSimp()
        {
            //string cron = "1-2 * * * * ? ";
            //CronSchedule cronSchedule = new CronSchedule(cron);
            ////cronSchedule.isValid
            //bool isTime= cronSchedule.isTime(DateTime.Parse("2021-03-13 20:16:01"));
            //bool isTime2 = cronSchedule.isTime(DateTime.Parse("2021-03-13 20:16:04"));
            //bool isVali= cronSchedule.isValid("0/1 0,11 0 L 0 1L *");
            //Assert.IsTrue(isTime);
            //Console.WriteLine("一:{0},二:{1},三:{2},四:{3},五:{4},六:{5}", cronSchedule.seconds.Count, cronSchedule.minutes.Count, cronSchedule.hours.Count, cronSchedule.days_of_month.Count, cronSchedule.months.Count, cronSchedule.days_of_week.Count);
            //Assert.IsFalse(isTime2);
            //Assert.IsTrue(isVali);

            //string cron2 = "0 0 0 0 0 1L *";
            //CronSchedule c2 = new CronSchedule(cron2);
            //Console.WriteLine("秒:{0},分:{1},时:{2},日:{3},月:{4},日/周:{5}", Join(c2.seconds), Join(c2.minutes), Join(c2.hours), Join(c2.days_of_month), Join(c2.months), Join(c2.days_of_week));

        }
        private string Join(List<int> source)
        {
            return string.Join(",", source);
        }
        [TestMethod]
        public void TestSortSet()
        {
            PlanTime planTime = new PlanTime();
            planTime.Parse("1 0 0 1 1");
            Assert.IsTrue(planTime.IsSuccess);
            //planTime.Parse("1 0 0 0 0");
            //Assert.AreEqual(5, planTime.Times);
            DateTimeOffset? next= planTime.GetNextTime();
            Console.WriteLine(next);
            //Assert.AreEqual(3, cron.Seconds.Count);
        }
    }
}
