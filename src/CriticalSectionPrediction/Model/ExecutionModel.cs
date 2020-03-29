// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.ML.Data;

namespace CriticalSectionPrediction.Model
{
    public class ExecutionModel
    {
        [LoadColumn(0), ColumnName("Thread")]
        public float Thread;

        [LoadColumn(1), ColumnName("ExecutionTime")]
        public float ExecutionTime;

        [LoadColumn(2), ColumnName("CriticalSectionDimension")]
        public float CriticalSectionDimension;

        [LoadColumn(3), ColumnName("TotalTime")]
        public float TotalTime;
    }
}
