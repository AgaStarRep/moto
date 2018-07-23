using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Motohusaria.Services.Utils.ScheduledTasks
{
    public class TaskScheduler<T> : IHostedService where T : IScheduledTask
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CancellationTokenSource _taskToken = new CancellationTokenSource();
        private Task<bool> serviceTask;
        private Task delayTask;

        public TaskScheduler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //jeżeli zostanie uruchomione asynchronicznie to zawiesi uruchamianie pozostałych IHostedService
            var task = ExecuteTask(cancellationToken);
            if (task.IsCompleted)
            {
                return task;
            }
            return Task.CompletedTask;
        }

        private async Task ExecuteTask(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var now = DateTime.Now;
                    DateTime nextDate = now.AddSeconds(1);
                    bool success = true;
                    int errorCount = 0;
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        int errorThreshold = 0;
                        try
                        {
                            var service = scope.ServiceProvider.GetService<T>();
                            nextDate = now.AddSeconds(service.IntervalBetweenRuns);
                            errorThreshold = service.ErrorThreshold;
                            var logger = scope.ServiceProvider.GetService<ILogger>();
                            logger.Information("TaskScheduler - uruchamianie zadania - " + typeof(T).Name, new { date = DateTime.Now, nextDate, errorCount });
                            serviceTask = service.RunAsync(_taskToken.Token);
                            success = await serviceTask;
                            nextDate = now.AddSeconds(service.IntervalBetweenRuns);//mógł zmienić wartośc po uruchomieniu taska
                            logger.Information("TaskScheduler - zakończono zadanie - " + typeof(T).Name, new { date = DateTime.Now, nextDate, errorCount, success });
                        }
                        catch (Exception e)
                        {
                            success = false;
                        }
                        if (!success)
                        {
                            errorCount++;
                            if (errorCount > errorThreshold)
                            {
                                return;
                            }
                        }
                        var timeSpan = nextDate - now;
                        var delay = timeSpan.TotalMilliseconds < 0 ? 0 : timeSpan.TotalMilliseconds;
                        delayTask = Task.Delay(delay > int.MaxValue ? int.MaxValue : (int)delay, cancellationToken);
                        await delayTask;
                    }
                }
            }
            catch (TaskCanceledException e) { LogCancelled(e); }
            catch (Exception e) { LogException("TaskScheduler - błąd wykonywania zadania, zadanie zostało zakończone", e, new { type = typeof(T).FullName }); }
        }

        private void LogException(string message, Exception ex, object data)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger>();
                logger.Error(message, ex, data);
            }
        }

        private void LogCancelled(TaskCanceledException ex)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger>();
                logger.Information("TaskScheduler - zadanie zostało przerwane", new { type = typeof(T).FullName, exception = ex });
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _taskToken.Cancel();
            if (serviceTask != null)
            {
                await Task.WhenAll(serviceTask, delayTask);
            }
        }
    }
}
