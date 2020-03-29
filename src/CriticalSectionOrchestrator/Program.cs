// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using CriticalSectionSimulation.Entity;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CriticalSectionOrchestrator
{
    internal class Program
    {
        private const string Section = "Simulation";
        private const string Config = "appsettings.json";

        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Config)
                .Build();

            // read the simulation parameters and build the Simulation entity
            var simulation = new Simulation()
            {
                MinThread = int.Parse(builder[$"{Section}:MinThread"]),
                MaxThread = int.Parse(builder[$"{Section}:MaxThread"]),
                ThreadRampUpPeriod = int.Parse(builder[$"{Section}:ThreadRampUpPeriod"]),
                MinTimeOnSection = int.Parse(builder[$"{Section}:MinTimeOnSection"]),
                MaxTimeOnSection = int.Parse(builder[$"{Section}:MaxTimeOnSection"]),
                MinCriticalSectionDimension = int.Parse(builder[$"{Section}:MinCriticalSectionDimension"]),
                MaxCriticalSectionDimension = int.Parse(builder[$"{Section}:MaxCriticalSectionDimension"]),
                PredictionSimulationTime = uint.Parse(builder[$"{Section}:PredictionSimulationTime"]),
                TotalSimulationDimension = int.Parse(builder[$"{Section}:TotalSimulationDimension"]),
                PartialSimulationDimension = int.Parse(builder[$"{Section}:PartialSimulationDimension"]),
                TrainingSetPercentage = int.Parse(builder[$"{Section}:TrainingSetPercentage"])
            };

            // preview parameters
            Console.WriteLine("Simulation parameters:");
            Console.WriteLine(simulation.ToString());

            Console.WriteLine("Press any key to start the simulation..");
            Console.ReadLine();

            // start the simulation
            Console.WriteLine("Orchestration starting...");
            OrchestratorManager.StartOrchestration(simulation);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
