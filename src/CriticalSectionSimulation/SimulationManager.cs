// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionSimulation.Entity;
using CriticalSectionSimulation.Simulation;

namespace CriticalSectionSimulation
{
    public class SimulationManager
    {
        /// <summary>
        /// Simulate the current configuration
        /// </summary>
        /// <param name="currentSimulation"></param>
        /// <param name="dataPath"></param>
        /// <param name="rampUpPeriod"></param>
        public static void SimulateConfiguration(CurrentSimulation currentSimulation, string dataPath, int rampUpPeriod = 100)
        {
            ThreadGenerator.StopThreads();

            CriticalSection.InitCriticalSection(currentSimulation.CriticalSectionDimension, dataPath);

            ThreadGenerator.InitThreads(currentSimulation);

            ThreadGenerator.StartThreads(rampUpPeriod);
        }

        /// <summary>
        /// Stop the simulation
        /// </summary>
        public static void StopSimulation()
        {
            ThreadGenerator.StopThreads();
        }
    }
}
