// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Newtonsoft.Json;

namespace CriticalSectionSimulation.Entity
{
    /// <summary>
    /// The simulation parameters mapping from JSON configuration
    /// </summary>
    public class Simulation
    {
        public int MinThread { get; set; }
        public int MaxThread { get; set; }
        public int ThreadRampUpPeriod { get; set; }
        public int MinTimeOnSection { get; set; }
        public int MaxTimeOnSection { get; set; }
        public int MinCriticalSectionDimension { get; set; }
        public int MaxCriticalSectionDimension { get; set; }
        public uint PredictionSimulationTime { get; set; }
        public int TotalSimulationDimension { get; set; }
        public int PartialSimulationDimension { get; set; }
        public int TrainingSetPercentage { get; set; }

        /// <summary>
        /// ToString override for JSON output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
