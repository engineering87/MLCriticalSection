// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using CriticalSectionPrediction.Model;
using CriticalSectionPrediction.Utility;
using Microsoft.ML;
using PLplot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CriticalSectionPrediction
{
    public class PlotManager
    {
        public static void PlotRegressionChart(MLContext mlContext,
                                        string testDataSetPath,
                                        string simulationPath)
        {
            ITransformer trainedModel;
            using (var stream = new FileStream(simulationPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                trainedModel = mlContext.Model.Load(stream, out var modelInputSchema);
            }

            // Create prediction engine related to the loaded trained model
            var predFunction = mlContext.Model.CreatePredictionEngine<ExecutionModel, ExecutionModelPrediction>(trainedModel);

            string chartFileName;
            using (var pl = new PLStream())
            {
                pl.sdev("pngcairo");
                chartFileName = "RegressionDistribution.png";
                pl.sfnam(chartFileName);

                // use white background with black foreground
                pl.spal0("cmap0_alternate.pal");

                // Initialize plplot
                pl.init();

                //var totalNumber = numberOfRecordsToRead;
                var testData = new CsvReader().GetDataFromCsv(testDataSetPath).ToList();
                var totalNumber = testData.Count;

                var (xMinLimit, xMaxLimit) = GetMinMax(testData);

                // set axis limits
                pl.env(xMinLimit, xMaxLimit, xMinLimit, xMaxLimit, AxesScale.Independent, AxisBox.BoxTicksLabelsAxes);

                // Set scaling for mail title text 125% size of default
                pl.schr(0, 1.25);

                // The main title
                pl.lab("Measured", "Predicted", "Distribution of TotalTime Prediction");

                // plot using different colors
                // see http://plplot.sourceforge.net/examples.php?demo=02 for palette indices
                pl.col0(1);

                //This code is the symbol to paint
                const char code = (char)9;

                // plot using other color
                //pl.col0(9); //Light Green
                //pl.col0(4); //Red
                pl.col0(2); //Blue

                double yTotal = 0;
                double xTotal = 0;
                double xyMultiTotal = 0;
                double xSquareTotal = 0;

                foreach (var t in testData)
                {
                    var x = new double[1];
                    var y = new double[1];

                    //Make Prediction
                    var prediction = predFunction.Predict(t);

                    x[0] = t.TotalTime;
                    y[0] = prediction.TotalTime;

                    //Paint a dot
                    pl.poin(x, y, code);

                    xTotal += x[0];
                    yTotal += y[0];

                    var multi = x[0] * y[0];
                    xyMultiTotal += multi;

                    var xSquare = x[0] * x[0];
                    xSquareTotal += xSquare;

                    //Console.WriteLine($"-------------------------------------------------");
                    //Console.WriteLine($"Predicted : {prediction.TotalTime}");
                    //Console.WriteLine($"Actual:    {testData[i].TotalTime}");
                    //Console.WriteLine($"-------------------------------------------------");
                }

                var minY = yTotal / totalNumber;
                var minX = xTotal / totalNumber;
                var minXy = xyMultiTotal / totalNumber;
                var minXsquare = xSquareTotal / totalNumber;

                var m = ((minX * minY) - minXy) / ((minX * minX) - minXsquare);

                var b = minY - (m * minX);

                //Generic function for Y for the regression line
                // y = (m * x) + b;

                const int x1 = 1;
                var y1 = (m * x1) + b;

                const int x2 = 1000;
                //Function for Y2 in the line
                var y2 = (m * x2) + b;

                var xArray = new double[2];
                var yArray = new double[2];
                xArray[0] = x1;
                yArray[0] = y1;
                xArray[1] = x2;
                yArray[1] = y2;

                pl.col0(4);
                pl.line(xArray, yArray);

                // end page (writes output to disk)
                pl.eop();

                // output version of PLplot
                pl.gver(out var verText);
                //Console.WriteLine("PLplot version " + verText);

            } // the pl object is disposed here

            // Open Chart File
            Console.WriteLine("Showing chart...");
            var p = new Process();
            var chartFileNamePath = @".\" + chartFileName;
            p.StartInfo = new ProcessStartInfo(chartFileNamePath)
            {
                UseShellExecute = true
            };
            p.Start();
        }

        /// <summary>
        /// Get min and max of TotalTime
        /// </summary>
        /// <param name="testData"></param>
        /// <returns></returns>
        private static Tuple<float, float> GetMinMax(IEnumerable<ExecutionModel> testData)
        {
            var executionModels = testData.ToList();
            var max = executionModels[0].TotalTime;
            var min = executionModels[0].TotalTime;

            foreach (var executionModel in executionModels)
            {
                if (executionModel.TotalTime > max)
                    max = executionModel.TotalTime;

                if (executionModel.TotalTime < min)
                    min = executionModel.TotalTime;
            }

            return new Tuple<float, float>(min, max);
        }
    }
}
