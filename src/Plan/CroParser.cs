using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// Cron的简化版，合并可能冲突的日期和星期域
    ///     1      2     3          4          5        6
    ///  {秒数} {分钟} {小时} {日期}/{星期} {月份} {年份(可为空)}
    /// </summary>
    public class CroParser : IPlanTimeParser
    {
        private ParseResult result;
        private string[] _cros;
        /// <summary>
        /// 解析Cro表达式
        /// </summary>
        /// <param name="croExpression"></param>
        public ParseResult Parse(string croExpression)
        {
            result = new ParseResult();
            _cros = croExpression.Split(" ");
            if (_cros.Length != 5 && _cros.Length != 6)
            {
                AddError(0, "croExpress length must be 5 or 6.");
                return result;
            }
            for (int i = 1; i <= _cros.Length; i++)
            {
                TimeCloumn r;
                if (i == 6)//为了后期兼容Cron cron这里是week域
                {
                    r = CoumnParse(TimeCloumnType.Year, _cros[i - 1]);
                }
                else
                {
                    //TODO week 以X开头
                    if (i == 4 && _cros[i - 1].StartsWith("X"))
                    {
                        //day切换到week 移除X
                        _cros[3] = _cros[3].Substring(1);
                        r = CoumnParse(TimeCloumnType.Week, _cros[3]);
                    }
                    else
                    {
                        r = CoumnParse((TimeCloumnType)i, _cros[i - 1]);
                    }

                }
                if (result.IsError)
                {
                    break;
                }
                result.AddTimeCloumn(r);
            }
            return result;
        }
        /// <summary>
        /// 解析单个域
        /// </summary>
        /// <param name="cloumnType"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        private TimeCloumn CoumnParse(TimeCloumnType cloumnType, string plan)
        {
            if (string.IsNullOrEmpty(plan))
            {
                AddError(cloumnType, "the string is empty");
            }
            TimeCloumn cloumn = new TimeCloumn(cloumnType, plan);
            List<Action<TimeCloumn>> funcs = new List<Action<TimeCloumn>>()
            {
                //解析*
                new Action<TimeCloumn>(ParseAny),
                //解析,
                new Action<TimeCloumn>(ParseAnd),
                //解析-
                new Action<TimeCloumn>(ParseTo),
                //解析/
                new Action<TimeCloumn>(ParseStep),
                //解析纯数字
                new Action<TimeCloumn>(ParseNb),
                //解析L 
                new Action<TimeCloumn>(ParseLast),
            };
            for (int i = 0; i < funcs.Count; i++)
            {
                if (!cloumn.IsFinish && !result.IsError)
                    funcs[i].Invoke(cloumn);
            }
            if (!cloumn.IsFinish)
            {
                AddError(cloumnType, "not find any TimeStrategy for this plan");
            }
            return cloumn;
        }
        /// <summary>
        /// 解析, TimeStrategy.And
        /// </summary>
        /// <returns></returns>
        private void ParseAnd(TimeCloumn cloumn)
        {
            if (cloumn.Plan.IndexOf(",") > -1)
            {
                //TODO 可能需要先按大小排序
                string[] nbs = cloumn.Plan.Split(",");
                for (int i = 0; i < nbs.Length; i++)
                {
                    if (int.TryParse(nbs[i], out int nb))
                    {
                        if (nb < cloumn.Min || nb > cloumn.Max)
                        {
                            AddError(cloumn.CloumnType, $"the number {nb} is out of range");
                        }
                    }
                    else
                    {
                        AddError(cloumn.CloumnType, $"can not parse to int:{nbs[i]}");
                    }
                }
                if (!this.IsError)
                {

                    cloumn.SetStrategy(TimeStrategy.And);
                }
            }
        }
        /// <summary>
        /// 解析-  TimeStrategy.To  特殊： /步进第一个参数可以包含-
        /// </summary>
        /// <param name="cloumn"></param>
        /// <returns></returns>
        private void ParseTo(TimeCloumn cloumn)
        {
            if (cloumn.Plan.IndexOf("-") > -1 && cloumn.Plan.IndexOf("/") == -1)
            {
                string[] nbs = cloumn.Plan.Split("-");
                if (nbs.Length != 2)
                {
                    AddError(cloumn.CloumnType, "must be 2 length of int");
                }
                else
                {
                    //TODO 比大小，可能出现 20-10
                    for (int i = 0; i < nbs.Length; i++)
                    {
                        if (int.TryParse(nbs[i], out int nb))
                        {
                            if (nb < cloumn.Min || nb > cloumn.Max)
                            {
                                AddError(cloumn.CloumnType, $"the number {nb} is out of range");
                            }
                        }
                        else
                        {
                            AddError(cloumn.CloumnType, $"can not parse to int:{nbs[i]}");
                        }
                    }
                }
                if (!this.IsError)
                {
                    cloumn.SetStrategy(TimeStrategy.To);
                }
            }
        }
        /// <summary>
        /// 解析/ TimeStrategy.Step 特殊： /步进第一个参数可以包含-，*
        /// </summary>
        /// <param name="cloumn"></param>
        /// <returns></returns>
        private void ParseStep(TimeCloumn cloumn)
        {
            if (cloumn.Plan.IndexOf("/") > -1)
            {
                string[] nbs = cloumn.Plan.Split("/");
                if (nbs.Length != 2)
                {
                    AddError(cloumn.CloumnType, "out of range for / length");
                }
                else
                {
                    if (int.TryParse(nbs[1], out int nb))
                    {
                        if (nb < cloumn.Min || nb > cloumn.Max)
                        {
                            AddError(cloumn.CloumnType, $"the number {nb} is out of range");
                        }
                    }
                    else
                    {
                        AddError(cloumn.CloumnType, $"cont parse to int:{nbs[1]}");
                    }
                    if (int.TryParse(nbs[0], out int first))
                    {
                        if (first < cloumn.Min || first > cloumn.Max)
                        {
                            AddError(cloumn.CloumnType, $"the number {first} is out of range");
                        }
                    }
                    else
                    {
                        // */10
                        if (nbs[0] != "*")
                        {
                            // 10-20/5
                            if (nbs[0].IndexOf("-") > -1)
                            {
                                string[] tos = nbs[0].Split("-");
                                if (tos.Length != 2)
                                {
                                    AddError(cloumn.CloumnType, $"can not parse {nbs[0]}");
                                }
                                else
                                {
                                    for (int i = 0; i < tos.Length; i++)
                                    {
                                        //TODO 比大小，可能出现 20-10/5
                                        if (int.TryParse(tos[i], out int start))
                                        {
                                            if (start < cloumn.Min || start > cloumn.Max)
                                            {
                                                AddError(cloumn.CloumnType, $"the number {start} is out of range");
                                            }
                                        }
                                        else
                                        {
                                            AddError(cloumn.CloumnType, $"can not parse {tos[i]}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                AddError(cloumn.CloumnType, $"can not parse {nbs[0]}");
                            }
                        }
                    }
                }
                if (!IsError)
                {
                    cloumn.SetStrategy(TimeStrategy.Step);
                }
            }
        }
        private void ParseAny(TimeCloumn cloumn)
        {
            if (cloumn.Plan == "*")
            {
                cloumn.SetStrategy(TimeStrategy.Any);
            }
        }
        /// <summary>
        /// 解析纯数字
        /// </summary>
        /// <param name="cloumn"></param>
        /// <returns></returns>
        private void ParseNb(TimeCloumn cloumn)
        {
            if (int.TryParse(cloumn.Plan, out int nb))
            {
                if (nb < cloumn.Min || nb > cloumn.Max)
                {
                    AddError(cloumn.CloumnType, $"the number {nb} is out of range");
                }
                if (!IsError)
                    cloumn.SetStrategy(TimeStrategy.Number);
            }
        }
        private void ParseLast(TimeCloumn cloumn)
        {
            if (cloumn.Plan.IndexOf("L") > -1)
            {
                if (cloumn.Plan != "L")//5L
                {
                    string nbStr = cloumn.Plan.Substring(0, cloumn.Plan.Length - 1);
                    if (int.TryParse(nbStr, out int nb))
                    {
                        if (nb < 0 || nb > cloumn.Max - 1)
                        {
                            AddError(cloumn.CloumnType, $"the number {nb} is out of range");
                        }
                    }
                    else
                    {
                        AddError(cloumn.CloumnType, $"can not parse {nbStr} to int");
                    }
                }
                if (!IsError)
                {
                    cloumn.SetStrategy(TimeStrategy.Last);
                }
            }
        }
        private void AddError(int n, string error)
        {
            this.result.AddError(n, error);
        }
        private void AddError(TimeCloumnType type, string error)
        {
            this.result.AddError((int)type, error);
        }
        private bool IsError => this.result.IsError;
    }
}
