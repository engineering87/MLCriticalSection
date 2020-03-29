// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionSimulation.Entity;
using System;
using System.IO;

namespace CriticalSectionSimulation.Utility
{
    public class CsvWriter
    {
        /// <summary>
        /// Write the Csv file with result simulation
        /// </summary>
        /// <param name="currentSimulation"></param>
        /// <param name="totalTime"></param>
        /// <param name="dataPath"></param>
        public static void WriteCsv(CurrentSimulation currentSimulation, long totalTime, string dataPath)
        {
            try
            {
                var line = $"{currentSimulation.ThreadNumber},{currentSimulation.TimeOnSection},{currentSimulation.CriticalSectionDimension},{totalTime}";
                line += Environment.NewLine;

                File.AppendAllText(dataPath, line);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
