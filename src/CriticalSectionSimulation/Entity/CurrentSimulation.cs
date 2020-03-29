// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)

using Newtonsoft.Json;

namespace CriticalSectionSimulation.Entity
{
    public class CurrentSimulation
    {
        public int ThreadNumber { get; set; }
        public int CriticalSectionDimension { get; set; }
        public int TimeOnSection { get; set; }


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
