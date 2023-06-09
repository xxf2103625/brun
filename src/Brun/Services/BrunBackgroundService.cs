﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Services
{
    public class BrunBackgroundService : IHostedService
    {
        readonly ILogger<BrunBackgroundService> _logger;
        private CancellationTokenSource _stoppingCts;
        private Task _executeTask;
        readonly IServiceProvider _serviceProvider;
        ILoggerFactory loggerFactory;
        WorkerServer workerServer;
        public BrunBackgroundService(ILogger<BrunBackgroundService> logger, IServiceProvider serviceProvider, WorkerServer workerServer, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            this.workerServer = workerServer;
            this.loggerFactory = loggerFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BrunBackgroundService startting...");
            // Create linked token to allow cancelling executing task from provided token
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executeTask = ExecuteAsync(_stoppingCts.Token);
            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executeTask.IsCompleted)
            {
                return _executeTask;
            }
            return Task.CompletedTask;
            //新开了线程不会在这卡住
        }

        private Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("BrunBackgroundService startting...");
            workerServer.Init(_serviceProvider);
            //Start之前的Server配置
            if (workerServer.Option.ConfigreWorkerServer != null)
            {
                workerServer.Option.ConfigreWorkerServer.Invoke(workerServer);
            }
            workerServer.Start(stoppingToken);
            _logger.LogInformation("BrunBackgroundService is started");
            //Start之后配置初始化BackRun
            if (workerServer.Option.InitWorkers != null)
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    workerServer.Option.InitWorkers.Invoke(scope.ServiceProvider.GetRequiredService<IWorkerService>());
                }
            }
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //接收到进程停止信号
            _logger.LogInformation("BrunBackgroundService stopping...");
            // Stop called without start
            if (_executeTask == null)
            {
                return;
            }
            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _logger.LogInformation("BrunBackgroundService finally stopping...");
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executeTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
                _logger.LogInformation("BrunBackgroundService is stopped.");
            }
        }
    }
}
