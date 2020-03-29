// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriticalSectionPrediction.Utility
{
    public static class ConsoleHelper
    {
        public static void PrintRegressionPredictionVersusObserved(string predictionCount, string observedCount)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Predicted : {predictionCount}");
            Console.WriteLine($"Actual:     {observedCount}");
            Console.WriteLine("-------------------------------------------------");
        }

        public static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
        {
            Console.WriteLine("*************************************************");
            Console.WriteLine($"*       Metrics for {name} regression model      ");
            Console.WriteLine("*------------------------------------------------");
            Console.WriteLine($"*       LossFn:        {metrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
            Console.WriteLine("*************************************************");
        }

        public static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            var enumerable = values as double[] ?? values.ToArray();
            var average = enumerable.Average();
            var sumOfSquaresOfDifferences = enumerable.Select(val => (val - average) * (val - average)).Sum();
            var standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (enumerable.Count() - 1));
            return standardDeviation;
        }

        public static void ShowDataViewInConsole(MLContext mlContext, IDataView dataView, int numberOfRows = 4)
        {
            var msg = $"Show data in DataView: Showing {numberOfRows.ToString()} rows with the columns";
            ConsoleWriteHeader(msg);

            var preViewTransformedData = dataView.Preview(maxRows: numberOfRows);

            foreach (var row in preViewTransformedData.RowView)
            {
                var columnCollection = row.Values;
                var lineToPrint = columnCollection.Aggregate("Row--> ", (current, column) => current + $"| {column.Key}:{column.Value}");
                Console.WriteLine(lineToPrint + "\n");
            }
        }

        public static void ConsoleWriteHeader(params string[] lines)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            var maxLength = lines.Select(x => x.Length).Max();
            Console.WriteLine(new string('#', maxLength));
            Console.ForegroundColor = defaultColor;
        }
    }
}
