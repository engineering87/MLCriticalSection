// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionSimulation.Entity;
using System.Collections.Generic;
using System.Threading;

namespace CriticalSectionSimulation.Simulation
{
    public class ThreadGenerator
    {
        /// <summary>
        /// The thread list for the simulation
        /// </summary>
        private static readonly List<ThreadSimulation> ThreadList = new List<ThreadSimulation>();

        /// <summary>
        /// All thread running flag
        /// </summary>
        public static bool StartCompleted { get; private set; }

        /// <summary>
        /// Init the thread list
        /// </summary>
        /// <param name="currentSimulation"></param>
        public static void InitThreads(CurrentSimulation currentSimulation)
        {
            ThreadList.Clear();

            for (var i = 0; i < currentSimulation.ThreadNumber; i++)
            {
                var thread = new ThreadSimulation(currentSimulation, i);

                ThreadList.Add(thread);
            }
        }

        /// <summary>
        /// Stop all the thread
        /// </summary>
        public static void StopThreads()
        {
            foreach (var thread in ThreadList)
            {
                thread.Stop();
            }
        }

        /// <summary>
        /// Start all the thread for the simulation
        /// </summary>
        /// <param name="rampUpPeriod"></param>
        public static void StartThreads(int rampUpPeriod)
        {
            foreach (var thread in ThreadList)
            {
                thread.Start();

                Thread.Sleep(rampUpPeriod);
            }

            StartCompleted = true;
        }
    }
}
