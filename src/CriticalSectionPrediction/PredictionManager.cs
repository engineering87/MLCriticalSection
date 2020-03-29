// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionPrediction.Model;
using CriticalSectionPrediction.Utility;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using System;

namespace CriticalSectionPrediction
{
    public class PredictionManager
    {
        /// <summary>
        /// Start the ML prediction
        /// </summary>
        /// <param name="trainDataPath"></param>
        /// <param name="testDataPath"></param>
        /// <param name="simulationPath"></param>
        /// <param name="simulationTime"></param>
        public static void StartPrediction(string trainDataPath, string testDataPath, string simulationPath, uint simulationTime)
        {
            //Create ML Context with seed for repeatable/deterministic results
            var mlContext = new MLContext(seed: 0);

            // Create, Train, Evaluate and Save a model
            BuildTrainEvaluateAndSaveModel(mlContext, testDataPath, trainDataPath, simulationPath, simulationTime);

            // Paint regression distribution chart for a number of elements read from a Test DataSet file
            PlotManager.PlotRegressionChart(mlContext, testDataPath, simulationPath);
        }

        /// <summary>
        /// Create, Train, Evaluate and Save the model
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="testDataPath"></param>
        /// <param name="trainDataPath"></param>
        /// <param name="simulationPath"></param>
        /// <param name="simulationTime"></param>
        /// <returns></returns>
        private static ITransformer BuildTrainEvaluateAndSaveModel(MLContext mlContext, string testDataPath, string trainDataPath, string simulationPath, uint simulationTime)
        {
            var trainingDataView = mlContext.Data.LoadFromTextFile<ExecutionModel>(trainDataPath, hasHeader: false, separatorChar: ',');
            var testDataView = mlContext.Data.LoadFromTextFile<ExecutionModel>(testDataPath, hasHeader: false, separatorChar: ',');

            ConsoleHelper.ShowDataViewInConsole(mlContext, trainingDataView);

            ConsoleHelper.ConsoleWriteHeader("=============== Training the model ===============");
            Console.WriteLine($"Running AutoML regression experiment for {simulationTime} seconds...");

            var experimentResult = mlContext.Auto()
                .CreateRegressionExperiment(simulationTime)
                .Execute(trainingDataView, "TotalTime");

            ConsoleHelper.ConsoleWriteHeader("===== Evaluating model's accuracy with test data =====");

            var best = experimentResult.BestRun;
            if (best == null)
            {
                ConsoleHelper.ConsoleWriteHeader("Cannot complete the regression, try with more data");
                throw new ApplicationException("Regression failed");
            }

            var trainedModel = best.Model;
            var predictions = trainedModel.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "TotalTime");

            ConsoleHelper.PrintRegressionMetrics(best.TrainerName, metrics);

            mlContext.Model.Save(trainedModel, trainingDataView.Schema, simulationPath);

            Console.WriteLine("The model is saved to {0}", simulationPath);

            return trainedModel;
        }
    }
}
