//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Brun.Plan
//{
//    /*
//     * Cron表达式是一个字符串，字符串以5或6个空格隔开，分为6或7个域，每一个域代表一个含义，Cron有如下两种语法格式：
//Seconds Minutes Hours DayofMonth Month DayofWeek Year或
//Seconds Minutes Hours DayofMonth Month DayofWeek
//每一个域可出现的字符如下：
//Seconds:可出现", - * /"四个字符，有效范围为0-59的整数
//Minutes:可出现", - * /"四个字符，有效范围为0-59的整数
//Hours:可出现", - * /"四个字符，有效范围为0-23的整数
//DayofMonth:可出现", - * / ? L W C"八个字符，有效范围为0-31的整数
//Month:可出现", - * /"四个字符，有效范围为1-12的整数或JAN-DEc
//DayofWeek:可出现", - * / ? L C #"八个字符，有效范围为1-7的整数或SUN-SAT两个范围。1表示星期天，2表示星期一， 依次类推
//Year:可出现", - * /"四个字符，有效范围为1970-2099年

//    每一个域都使用数字，但还可以出现如下特殊字符，它们的含义是：
//(1)*：表示匹配该域的任意值，假如在Minutes域使用*, 即表示每分钟都会触发事件。
//(2)?:只能用在DayofMonth和DayofWeek两个域。它也匹配域的任意值，但实际不会。因为DayofMonth和DayofWeek会相互影响。例如想在每月的20日触发调度，不管20日到底是星期几，则只能使用如下写法： 13 13 15 20 * ?, 其中最后一位只能用？，而不能使用*，如果使用*表示不管星期几都会触发，实际上并不是这样。
//(3)-:表示范围，例如在Minutes域使用5-20，表示从5分到20分钟每分钟触发一次
//(4)/：表示起始时间开始触发，然后每隔固定时间触发一次，例如在Minutes域使用5/20,则意味着5分钟触发一次，而25，45等分别触发一次.
//(5),:表示列出枚举值值。例如：在Minutes域使用5,20，则意味着在第5和第20分钟分别触发一次。
//(6)L:表示最后，只能出现在DayofWeek和DayofMonth域，如果在DayofWeek域使用5L,意味着在最后的一个星期四触发。
//(7)W:表示有效工作日(周一到周五),只能出现在DayofMonth域，系统将在离指定日期的最近的有效工作日触发事件。例如：在 DayofMonth使用5W，如果5日是星期六，则将在最近的工作日：星期五，即4日触发。如果5日是星期天，则在6日(周一)触发；如果5日在星期一到星期五中的一天，则就在5日触发。另外一点，W的最近寻找不会跨过月份
//(8)LW:这两个字符可以连用，表示在某个月最后一个工作日，即最后一个非周六周末的日期。
//(9)#:用于确定每个月第几个星期几，只能出现在DayofWeek域。例如在4#2，表示某月的第二个星期三。
//     */
//    /// <summary>
//    /// Cron
//    /// </summary>
//    public class CronTime
//    {
//        /// <summary>
//        /// 可出现", - * /"四个字符，有效范围为0-59的整数
//        /// </summary>
//        public SortedSet<int> Seconds { get; set; }
//        /// <summary>
//        /// Minutes:可出现", - * /"四个字符，有效范围为0-59的整数
//        /// </summary>
//        public SortedSet<int> Minutes { get; set; }
//        /// <summary>
//        /// Hours:可出现", - * /"四个字符，有效范围为0-23的整数
//        /// </summary>
//        public SortedSet<int> Hours { get; set; }
//        /// <summary>
//        /// DayofMonth:可出现", - * / ? L W C"八个字符，有效范围为0-31的整数
//        /// </summary>
//        public SortedSet<int> DayOfMonth { get; set; }
//        /// <summary>
//        /// Month:可出现", - * /"四个字符，有效范围为1-12的整数或JAN-DEc
//        /// </summary>
//        public SortedSet<int> Month { get; set; }
//        /// <summary>
//        /// DayofWeek:可出现", - * / ? L C #"八个字符，有效范围为1-7的整数或SUN-SAT两个范围。1表示星期天，2表示星期一， 依次类推
//        /// </summary>
//        public SortedSet<int> DayOfWeek { get; set; }
//        /// <summary>
//        /// Year:可出现", - * /"四个字符，有效范围为1970-2099年
//        /// </summary>
//        public SortedSet<int> Year { get; set; }
//        public static bool Validation(string cronExpress)
//        {
//            string[] crs = cronExpress.Split(" ");
//            if (crs.Length != 6 || crs.Length != 7)
//            {
//                return false;
//            }
//        }
//        public static string Parse(string cronExpress)
//        {

//        }
//    }
//}
