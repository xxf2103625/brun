using Brun.Enums;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Brun.Commons
{
    public class BrunTool
    {
        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args: args);
        }
        public static TType CreateInstance<TType>(params object[] args)
        {
            return (TType)Activator.CreateInstance(typeof(TType), args);
        }
        public static Type GetTypeByFullName(string typeFullName)
        {
            Type type = null;
            Array.ForEach(AppDomain.CurrentDomain.GetAssemblies(), m =>
            {
                if (m.DefinedTypes.Any(t => t.FullName == typeFullName))
                {
                    type = m.GetType(typeFullName);
                }
            });
            return type;
        }
        /// <summary>
        /// 获取当前程序域里的程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetReferanceAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static (int, Assembly, string) LoadFile(string fileName)
        {
            //重复判断 //TODO 文件名可能和程序集名不一致
            if (fileName.EndsWith(".dll"))
            {
                string assName = fileName.Substring(0, fileName.Length - 4);
                if (AppDomain.CurrentDomain.GetAssemblies().Any(m => m.GetName().Name == assName))
                {
                    return (-1, null, "该程序集已加载");
                }
            }

            try
            {
                //TODO 卸载程序集需要创建新的AppDomain
                var ass = Assembly.LoadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                return (1, ass, "加载成功");
            }
            catch (Exception ex)
            {
                return (0, null, ex.Message);
            }
        }
        /// <summary>
        /// 获取Worker类型
        /// </summary>
        /// <param name="workerType"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.BrunException"></exception>
        public static Type GetWorkerType(WorkerType workerType)
        {
            switch (workerType)
            {
                case WorkerType.OnceWorker:
                    return typeof(OnceWorker);
                case WorkerType.TimeWorker:
                    return typeof(TimeWorker);
                case WorkerType.QueueWorker:
                    return typeof(QueueWorker);
                case WorkerType.PlanWorker:
                    return typeof(PlanWorker);
                default:
                    throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type error");
            }
        }
        /// <summary>
        /// 获取Worker类型枚举
        /// </summary>
        /// <param name="workerType"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.BrunException"></exception>
        public static WorkerType GetWorkerType(Type workerType)
        {
            if (workerType == null)
            {
                throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type is null");
            }
            if (workerType == typeof(OnceWorker))
                return WorkerType.OnceWorker;
            if (workerType == typeof(TimeWorker))
                return WorkerType.TimeWorker;
            if (workerType == typeof(QueueWorker))
                return WorkerType.QueueWorker;
            if (workerType == typeof(PlanWorker))
                return WorkerType.PlanWorker;
            throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type error");
        }
        /// <summary>
        /// 类型字符串获取Worker类型枚举 OnceWorker/TimeWorker
        /// </summary>
        /// <param name="workerTypeName"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.BrunException"></exception>
        public static WorkerType GetWorkerType(string workerTypeName)
        {
            if (string.IsNullOrEmpty(workerTypeName))
            {
                throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type is null");
            }
            switch (workerTypeName)
            {
                case nameof(OnceWorker):
                    return WorkerType.OnceWorker;
                case nameof(TimeWorker):
                    return WorkerType.TimeWorker;
                case nameof(QueueWorker):
                    return WorkerType.QueueWorker;
                case nameof(PlanWorker):
                    return WorkerType.PlanWorker;
                default:
                    throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type name error:" + workerTypeName);
            }
        }
    }
}
