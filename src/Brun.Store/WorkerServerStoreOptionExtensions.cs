﻿using Brun.Services;
using Brun.Store.Commons;
using Brun.Store.Services;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Brun
{
    public static class WorkerServerStoreOptionExtensions
    {
        /// <summary>
        /// 配置SqlSugar支持的持久化模式
        /// </summary>
        /// <param name="workerServerOption"></param>
        /// <param name="connectionString"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        //TODO 是否再次封装，
        public static WorkerServerOption UseStore(this WorkerServerOption workerServerOption, string connectionString, DbType dbType)
        {
            //TODO 检测数据库，创建数据库等
            DatabaseHelper.ExistDatabase(connectionString, dbType);
            //TODO 初始化加载持久化内的程序集
            Brun.Commons.BrunTool.LoadFile("BrunTestHelper.dll");

            SqlSugar.ConnectionConfig connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = (SqlSugar.DbType)dbType,
                IsAutoCloseConnection = false,
            };
            workerServerOption.ServicesConfigure = services =>
            {
                //替换服务的方法,暂时用不上
                //var descriptor =new ServiceDescriptor(typeof(),typeof(),ServiceLifetime.Scoped);
                //services.Replace()

                services.AddTransient<SqlSugarClient>(m => new SqlSugarClient(connectionConfig));

                services.AddScoped<IOnceWorkerService, OnceWorkerStoreService>();
            };
            return workerServerOption;
        }
    }
}