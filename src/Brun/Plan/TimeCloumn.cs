using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 时间列/表达式的域 6列 yyyy-MM-dd HH:mm:ss 
    /// </summary>
    public class TimeCloumn
    {
        private readonly TimeCloumnType cloumnType;
        private string plan;
        private int min;
        private int max;
        /// <summary>
        /// 时间列
        /// </summary>
        /// <param name="cloumnType"></param>
        public TimeCloumn(TimeCloumnType cloumnType)
        {
            this.cloumnType = cloumnType;
        }
        private TimeStrategy timeStrategy;
        /// <summary>
        /// 时间列 6列 yyyy-MM-dd HH:mm:ss 
        /// </summary>
        /// <param name="timeCloumnType"></param>
        /// <param name="planStr"></param>
        public TimeCloumn(TimeCloumnType timeCloumnType, string planStr)
        {
            cloumnType = timeCloumnType;
            plan = planStr;
            Init();
        }
        private void Init()
        {
            switch (cloumnType)
            {
                case TimeCloumnType.Second:
                    min = 0; max = 59;
                    break;
                case TimeCloumnType.Minute:
                    min = 0; max = 59;
                    break;
                case TimeCloumnType.Hour:
                    min = 0; max = 23;
                    break;
                case TimeCloumnType.Day:
                    min = 1; max = 31;
                    break;
                case TimeCloumnType.Month:
                    min = 1; max = 12;
                    break;
                case TimeCloumnType.Week:
                    min = 1; max = 7;
                    break;
                case TimeCloumnType.Year:
                    min = 1970; max = 2099;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 设置策略类型
        /// </summary>
        /// <param name="timeStrategy"></param>
        public void SetStrategy(TimeStrategy timeStrategy)
        {
            this.timeStrategy = timeStrategy;
        }
        /// <summary>
        /// 原始字符串
        /// </summary>
        public string Plan => plan;
        /// <summary>
        /// Cloumn类型，分/秒/天...
        /// </summary>
        public TimeCloumnType CloumnType => cloumnType;
        /// <summary>
        /// 策略类型
        /// </summary>
        public TimeStrategy TimeStrategy => this.timeStrategy;
        /// <summary>
        /// 是否已解析完成
        /// </summary>
        public bool IsFinish => cloumnType != TimeCloumnType.None && timeStrategy != TimeStrategy.None;
        /// <summary>
        /// 允许的最小值
        /// </summary>
        public int Min => min;
        /// <summary>
        /// 允许的最大值
        /// </summary>
        public int Max => max;
    }

}
