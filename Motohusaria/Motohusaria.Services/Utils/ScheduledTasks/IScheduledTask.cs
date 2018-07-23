using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Motohusaria.Services.Utils.ScheduledTasks
{
    public interface IScheduledTask
    {
        /// <summary>
        /// Co ile ma być uruchamiany dany task w sekundach
        /// </summary>
        int IntervalBetweenRuns { get; }

        /// <summary>
        /// Liczba błędych wykonań po której zadanie powinno zostać zakończone.
        /// </summary>
        int ErrorThreshold { get; }

        /// <summary>
        /// Metoda rozpoczynająca zadnie wywoływana co ustawiony przedział czasu. Zwraca czy zadanie zostało zrealizowane poprawnie.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> RunAsync(CancellationToken cancellationToken);
    }
}
