﻿using Brun.BaskRuns;
using Brun.Commons;
using Brun.Options;
using Brun.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brun
{
    /// <summary>
    /// Woker建造器
    /// </summary>
    public sealed class WorkerBuilder
    {
        //private ILogger<WorkerBuilder> logger => WorkerServer.Instance.ServiceProvider.GetService<ILogger<WorkerBuilder>>();
        private const string defaultTag = "Default";
        private WorkerOption option;
        private WorkerConfig config;
        /// <summary>
        /// 设置单独的配置
        /// </summary>
        /// <param name="workerConfig"></param>
        public static void SetConfig(WorkerConfig workerConfig)
        {
            //TODO 覆盖部分默认的Config
        }
        public ILogger<WorkerBuilder> log => WorkerServer.Instance?.ServiceProvider?.GetService<ILogger<WorkerBuilder>>();
        /// <summary>
        /// 创建默认的OnceWorker
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <returns></returns>
        public static WorkerBuilder Create<TBackRun>() where TBackRun : IBackRun, new()
        {
            WorkerBuilder builder = new WorkerBuilder
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultOption
            };
            builder.option.BrunTypes = new List<Type>() { typeof(TBackRun) };
            return builder;
        }
        /// <summary>
        /// 配置其他的BackRun在同一个Worker中运行，自定义数据会共享
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <returns></returns>
        public WorkerBuilder Add<TBackRun>() where TBackRun : IBackRun, new()
        {
            Type bType = typeof(TBackRun);
            if (option.BrunTypes.Any(m => m == bType))
            {
                log?.LogWarning("同一个Worker中不能配置2个相同类型的BackRun,type:{0}", typeof(TBackRun).FullName);
                return this;
            }
            option.BrunTypes.Add(bType);
            return this;
        }
        /// <summary>
        /// 创建队列任务
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <returns></returns>
        public static WorkerBuilder CreateQueue<TQueueBackRun>() where TQueueBackRun : QueueBackRun, new()
        {
            WorkerBuilder builder = new WorkerBuilder
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultOption
            };
            builder.option.BrunTypes = new List<Type>() { typeof(TQueueBackRun) };
            builder.option.WorkerType = typeof(QueueWorker);
            return builder;
        }
        /// <summary>
        /// 配置其他的QueueBackRun在同一个QueueBackRun中运行，
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <returns></returns>
        public WorkerBuilder AddQueue<TQueueBackRun>() where TQueueBackRun : QueueBackRun, new()
        {
            this.option.BrunTypes.Add(typeof(TQueueBackRun));
            return this;
        }
        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <returns></returns>
        public static WorkerBuilder CreateTime<TBackRun>() where TBackRun : IBackRun, new()
        {
            WorkerBuilder builder = new WorkerBuilder()
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultTimeWorkerOption
            };
            builder.option.BrunTypes = new List<Type>() { typeof(TBackRun) };
            builder.option.WorkerType = typeof(TimeWorker);
            return builder;
        }
        /// <summary>
        /// 设置TimeWorker的定时执行周期
        /// </summary>
        /// <param name="cycle">运行周期</param>
        /// <param name="runWithStart">程序运行/重启时是否立即执行一次</param>
        /// <returns></returns>
        public WorkerBuilder SetCycle(TimeSpan cycle, bool runWithStart = false)
        {
            if (option is TimeWorkerOption option1)
            {
                option1.RunWithStart = runWithStart;
                option1.Cycle = cycle;
            }
            else
            {
                throw new Exception("only TimeWorker can SetCycle");
            }

            return this;
        }
        /// <summary>
        /// 可以不指定BackRun类型而指定Worker实例Key，内部调用OnceWorker的run
        /// </summary>
        /// <returns></returns>
        public static WorkerBuilder CreateTime()
        {
            throw new NotImplementedException();
            //WorkerBuilder builder = new WorkerBuilder()
            //{
            //    //创建了新的对象
            //    config = WorkerServer.Instance.ServerConfig.DefaultConfig,
            //    option = WorkerServer.Instance.ServerConfig.DefaultOption
            //};
            //builder.option.WorkerType = typeof(TimeWorker);
            //builder.option.BrunType = typeof(TBackRun);
            //return builder;
        }
        public WorkerBuilder SetConfig(Action<WorkerConfig> configure)
        {
            configure(config);
            return this;
        }
        public WorkerBuilder SetOption(Action<WorkerOption> configure)
        {
            configure(option);
            return this;
        }
        /// <summary>
        /// 设置worker类型，默认OnceWorker
        /// </summary>
        /// <param name="workerType"></param>
        /// <returns></returns>
        public WorkerBuilder SetWorkerType(Type workerType)
        {
            if (!workerType.IsSubclassOf(typeof(AbstractWorker)))
            {
                throw new NotSupportedException($"not support workerType:{workerType.FullName},must inherited from AbstractWorker");
            }
            option.WorkerType = workerType;
            return this;
        }
        public WorkerBuilder SetWorker<TWorker>() where TWorker : AbstractWorker
        {
            option.WorkerType = typeof(TWorker);
            return this;
        }

        /// <summary>
        /// 自己保证唯一，为空时默认newGuid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public WorkerBuilder SetKey(string key)
        {
            option.Key = key;
            return this;
        }
        /// <summary>
        /// 可以重复，为空默认使用类名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WorkerBuilder SetName(string name)
        {
            option.Name = name;
            return this;
        }
        public WorkerBuilder SetTag(string tag)
        {
            option.Tag = tag;
            return this;
        }
        public WorkerBuilder SetNameTagKey(string name, string tag, string key = null)
        {
            return SetKey(key).SetName(name).SetTag(tag);
        }
        public WorkerBuilder SetData(ConcurrentDictionary<string, string> data)
        {
            option.Data = data;
            return this;
        }
        public IWorker Build()
        {
            if (string.IsNullOrEmpty(option.Key))
                option.Key = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(option.Name))
                option.Name = option.DefaultBrunType.Name;
            if (string.IsNullOrEmpty(option.Tag))
                option.Tag = defaultTag;


            if (option.WorkerType == null)
                option.WorkerType = typeof(OnceWorker);
            else
            {
                if (!option.WorkerType.IsSubclassOf(typeof(AbstractWorker)))
                {
                    throw new NotSupportedException($"not allow this workertype:{option.WorkerType.FullName}");
                }
            }

            AbstractWorker worker = (AbstractWorker)BrunTool.CreateInstance(option.WorkerType, args: new object[] { option, config });
            //worker.SetTaskFactory(WorkerServer.Instance.TaskFactory);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
        public IOnceWorker BuildOnceWorker()
        {
            IWorker woker = Build();
            return (IOnceWorker)woker;
        }
    }
}
