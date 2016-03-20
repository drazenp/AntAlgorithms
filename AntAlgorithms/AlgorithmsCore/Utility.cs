using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using log4net;

namespace AlgorithmsCore
{
    public static class Utility
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogDoubleMatrixAsTable(double[,] matrix)
        {
            var rowLength = matrix.GetLength(0);
            var colLength = matrix.GetLength(1);

            var line = new StringBuilder();
            for (var i = 0; i < rowLength; i++)
            {
                line.Append(Environment.NewLine);
                for (var j = 0; j < colLength; j++)
                {
                    line.Append(matrix[i, j].ToString(CultureInfo.InvariantCulture).PadLeft(32, ' '));
                }
                line.Append(Environment.NewLine);
            }
            Log.Debug(line);
        }

        public static void LogDecimalArrayAsList(decimal[] array)
        {
            var line = new StringBuilder(Environment.NewLine);
            foreach (decimal item in array)
            {
                line.Append(item.ToString(CultureInfo.InvariantCulture).PadLeft(32, ' '));
            }
            Log.Debug(line);
        }
    }
}
