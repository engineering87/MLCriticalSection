// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CriticalSectionPrediction.Model;

namespace CriticalSectionPrediction.Utility
{
    public class CsvReader
    {
        /// <summary>
        /// Extract all data from the CSV
        /// </summary>
        /// <param name="dataLocation"></param>
        /// <returns></returns>
        public IEnumerable<ExecutionModel> GetDataFromCsv(string dataLocation)
        {
            try
            {
                var records =
                    File.ReadAllLines(dataLocation)
                        .Skip(1)
                        .Select(x => x.Split(','))
                        .Select(x => new ExecutionModel()
                        {
                            Thread = float.Parse(x[0], CultureInfo.InvariantCulture),
                            ExecutionTime = float.Parse(x[1], CultureInfo.InvariantCulture),
                            CriticalSectionDimension = float.Parse(x[2], CultureInfo.InvariantCulture),
                            TotalTime = float.Parse(x[3], CultureInfo.InvariantCulture)
                        }).ToList();

                return records;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return default;
            }
        }

        /// <summary>
        /// Extract data from the CSV
        /// </summary>
        /// <param name="dataLocation"></param>
        /// <param name="numMaxRecords"></param>
        /// <returns></returns>
        public IEnumerable<ExecutionModel> GetDataFromCsv(string dataLocation, int numMaxRecords)
        {
            return GetDataFromCsv(dataLocation)?.Take(numMaxRecords);
        }
    }
}
