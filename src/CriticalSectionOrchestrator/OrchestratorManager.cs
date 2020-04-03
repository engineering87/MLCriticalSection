// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CriticalSectionPrediction;
using CriticalSectionSimulation;
using CriticalSectionSimulation.Entity;

namespace CriticalSectionOrchestrator
{
    public class OrchestratorManager
    {
        /// <summary>
        /// The simulation data path
        /// </summary>
        private static string _resultFile;
        private static string _trainingFile;
        private static string _testFile;
        private static string _simulationFile;

        /// <summary>
        /// Simulation parameters steps
        /// </summary>
        private const int ThreadStep = 2;
        private const int CriticalSectionDimensionStep = 2;
        private const int CriticalSectionTimeStep = 100;

        /// <summary>
        /// Main method for simulation orchestration
        /// </summary>
        /// <param name="simulation"></param>
        public static void StartOrchestration(Simulation simulation)
        {
            // init the simulation data path
            SetSimulationDataPath();

            // check for the last simulation
            if (File.Exists(_simulationFile))
            {
                Console.WriteLine("Reset the last simulation? Y/N");
                var userResponse = Console.ReadLine();
                if (userResponse?.ToLower() == "y" || userResponse?.ToLower() == "yes")
                {
                    StartSimulation(simulation);
                }
            }
            else
            {
                StartSimulation(simulation);
            }

            // build training and test dataset
            BuildMachineLearningSet(simulation.TrainingSetPercentage);

            // apply ML
            PredictionManager.StartPrediction(_trainingFile, _testFile, _simulationFile, simulation.PredictionSimulationTime);
        }

        /// <summary>
        /// Iterate over parameters and simulate all the possible configurations
        /// </summary>
        /// <param name="simulation"></param>
        private static void StartSimulation(Simulation simulation)
        {
            ResetSimulationFile();

            var simulationDimension = 0;
            while (simulationDimension <= simulation.TotalSimulationDimension)
            {
                // iterate over thread configuration
                for (var t = simulation.MinThread;
                    t <= simulation.MaxThread;
                    t += ThreadStep)
                {
                    // iterate over critical section dimension
                    for (var c = simulation.MinCriticalSectionDimension;
                        c <= simulation.MaxCriticalSectionDimension;
                        c += CriticalSectionDimensionStep)
                    {
                        // iterate over time on critical section
                        for (var w = simulation.MinTimeOnSection;
                            w <= simulation.MaxTimeOnSection;
                            w += CriticalSectionTimeStep)
                        {
#if DEBUG
                            Console.WriteLine(
                                $"SimulateConfiguration " +
                                $"thread={t} " +
                                $"criticalSectionDimension={c} " +
                                $"criticalSectionTime={w}");
#endif
                            // iterate over the partial step
                            for (var p = 0;
                                p <= simulation.PartialSimulationDimension;
                                p++)
                            {
                                var currentSimulation = new CurrentSimulation()
                                {
                                    ThreadNumber = t,
                                    CriticalSectionDimension = c,
                                    TimeOnSection = w
                                };

                                SimulateConfiguration(currentSimulation);

                                simulationDimension++;
                            }
                        }
                    }
                }

                // simulation.TotalSimulationDimension < possible configurations
                break;
            }

            // stop all running threads
            SimulationManager.StopSimulation();
        }

        /// <summary>
        /// Simulate the current configuration
        /// </summary>
        /// <param name="currentSimulation"></param>
        private static void SimulateConfiguration(CurrentSimulation currentSimulation)
        {
            SimulationManager.SimulateConfiguration(currentSimulation, _resultFile);
        }

        /// <summary>
        /// Build the training and test dataset
        /// </summary>
        /// <param name="trainingSetPercentage"></param>
        private static void BuildMachineLearningSet(int trainingSetPercentage)
        {
            var resultCount = File.ReadLines(_resultFile).Count();
            var trainingSetCount = resultCount * (trainingSetPercentage / 100D);
            var trainingStep = (int)(resultCount / trainingSetCount);

            var trainingSet = new List<string>();
            var testingSet = new List<string>();

            using (var reader = new StreamReader(_resultFile))
            {
                var step = 0;
                while (!reader.EndOfStream)
                {
                    if (step % trainingStep == 0 && trainingSet.Count <= trainingSetCount)
                    {
                        trainingSet.Add(reader.ReadLine());
                    }
                    else
                    {
                        testingSet.Add(reader.ReadLine());
                    }

                    step++;
                }
            }

            WriteTrainingSet(trainingSet);
            WriteTestSet(testingSet);
        }

        private static void WriteTrainingSet(IEnumerable<string> trainingSet)
        {
            foreach (var line in trainingSet)
            {
                File.AppendAllText(_trainingFile, $"{line}{Environment.NewLine}");
            }
        }

        private static void WriteTestSet(IEnumerable<string> testingSet)
        {
            foreach (var line in testingSet)
            {
                File.AppendAllText(_testFile, $"{line}{Environment.NewLine}");
            }
        }

        /// <summary>
        /// Delete the last simulation files
        /// </summary>
        private static void ResetSimulationFile()
        {
            if (File.Exists(_resultFile))
                File.Delete(_resultFile);

            if (File.Exists(_trainingFile))
                File.Delete(_trainingFile);

            if (File.Exists(_testFile))
                File.Delete(_testFile);

            if (File.Exists(_simulationFile))
                File.Delete(_simulationFile);
        }

        /// <summary>
        /// Init the simulation data path
        /// </summary>
        private static void SetSimulationDataPath()
        {
            var requiredPath = TryGetSolutionDirectoryInfo();
            _resultFile = string.Concat(requiredPath, @"\Data\Result.csv");
            _trainingFile = string.Concat(requiredPath, @"\Data\Training.csv");
            _testFile = string.Concat(requiredPath, @"\Data\Test.csv");
            _simulationFile = string.Concat(requiredPath, @"\Data\Simulation.zip");
        }

        /// <summary>
        /// Get the main directory path
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        private static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
