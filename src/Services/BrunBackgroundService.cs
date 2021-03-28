using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    public class BrunBackgroundService : IHostedService, IDisposable
    {
        readonly ILogger<BrunBackgroundService> _logger;
        private CancellationTokenSource _stoppingCts;
        private Task _executeTask;
        readonly IServiceProvider _serviceProvider;
        readonly IHostLifetime _hostLifetime;
        readonly IHostApplicationLifetime _hostApplicationLifetime;
        public BrunBackgroundService(ILogger<BrunBackgroundService> logger, IServiceProvider serviceProvider, IHostLifetime hostLifetime, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _hostLifetime = hostLifetime;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
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
        }

        private Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BrunBackgroundService startting...");
            WorkerServer.Instance.Start(_serviceProvider, stoppingToken);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //接收到进程停止信号
            _logger.LogInformation("BrunBackgroundService stopping...");
            Console.WriteLine("BrunBackgroundService stopping...");
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
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executeTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }
        }
        public void Dispose()
        {
            _logger.LogInformation("BrunBackgroundService disposing...");
            Console.WriteLine("BrunBackgroundService disposing...");
            _stoppingCts?.Cancel();
        }
    }
}
