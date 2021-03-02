using Brun.BaskRuns;
using Brun.Commons;
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
        public static WorkerBuilder Create<TBackRun>() where TBackRun : BackRun, new()
        {
            WorkerBuilder builder = new WorkerBuilder
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultOption
            };
            builder.option.BrunType = typeof(TBackRun);
            return builder;
        }
        public static WorkerBuilder CreateQueue<TQueueBackRun>() where TQueueBackRun : QueueBackRun, new()
        {
            WorkerBuilder builder = new WorkerBuilder
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultOption
            };
            builder.option.BrunType = typeof(TQueueBackRun);
            builder.option.WorkerType = typeof(QueueWorker);
            return builder;
        }
        public static WorkerBuilder CreateTime<TBackRun>()
        {
            WorkerBuilder builder = new WorkerBuilder()
            {
                //创建了新的对象
                config = WorkerServer.Instance.ServerConfig.DefaultConfig,
                option = WorkerServer.Instance.ServerConfig.DefaultOption
            };
            builder.option.WorkerType = typeof(TimeWorker);
            builder.option.BrunType = typeof(TBackRun);
            return builder;
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
                option.Name = option.BrunType.Name;
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
