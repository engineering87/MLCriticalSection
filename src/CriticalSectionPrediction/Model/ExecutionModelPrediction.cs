// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Microsoft.ML.Data;

namespace CriticalSectionPrediction.Model
{
    public class ExecutionModelPrediction
    {
        [ColumnName("Score")]
        public float TotalTime;
    }
}
