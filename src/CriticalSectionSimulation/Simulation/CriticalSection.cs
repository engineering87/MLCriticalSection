// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionSimulation.Entity;
using CriticalSectionSimulation.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CriticalSectionSimulation.Simulation
{
    public class CriticalSection
    {
        /// <summary>
        /// We represent the critical section as a list of objects
        /// </summary>
        public static List<object> CriticalSectionList = new List<object>();

        /// <summary>
        /// The CSV result file
        /// </summary>
        private static string _dataPath;

        /// <summary>
        /// Initialize the critical section
        /// </summary>
        /// <param name="dimension">The dimension of the critical section</param>
        /// <param name="dataPath"></param>
        public static void InitCriticalSection(int dimension, string dataPath)
        {
            _dataPath = dataPath;

            CriticalSectionList.Clear();

            for (var i = 0; i < dimension; i++)
            {
                CriticalSectionList.Add(new object());
            }
        }

        /// <summary>
        /// Simulate thread execution
        /// </summary>
        /// <param name="currentSimulation"></param>
        /// <param name="threadId"></param>
        /// <param name="cancellationToken"></param>
        public static void UseCriticalSection(CurrentSimulation currentSimulation, int threadId, 
            CancellationToken cancellationToken)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                while (true)
                {
                    // check for thread cancellation request
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    // get the first critical section available slot
                    for (var i = 0; i < CriticalSectionList.Count; i++)
                    {
                        var obj = CriticalSectionList[i];
                        if (Monitor.TryEnter(obj))
                        {
                            // simulate work
                            Thread.Sleep(currentSimulation.TimeOnSection);

                            stopwatch.Stop();
                            var elapsedMs = stopwatch.ElapsedMilliseconds;
                            Console.WriteLine($"Total time {elapsedMs} ms");

                            // just the first thread writes the simulation file
                            if (threadId == 0 && ThreadGenerator.StartCompleted)
                                CsvWriter.WriteCsv(currentSimulation, elapsedMs, _dataPath);

                            // reset the stopwatch for the current thread
                            stopwatch = Stopwatch.StartNew();

                            // exit
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
