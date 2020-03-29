// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionSimulation.Simulation;
using System.Threading;

namespace CriticalSectionSimulation.Entity
{
    public class ThreadSimulation
    {
        public Thread Thread { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public ThreadSimulation(CurrentSimulation currentSimulation, int threadId)
        {
            CancellationTokenSource = new CancellationTokenSource();

            Thread = new Thread(delegate()
            {
                CriticalSection.UseCriticalSection(currentSimulation, threadId, CancellationTokenSource.Token);
            });
        }

        /// <summary>
        /// Start the thread
        /// </summary>
        public void Start()
        {
            Thread.Start();
        }

        /// <summary>
        /// Stop the thread
        /// </summary>
        public void Stop()
        {
            CancellationTokenSource.Cancel(false);
        }
    }
}
