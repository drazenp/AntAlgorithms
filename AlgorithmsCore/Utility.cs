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

        public static void LogDoubleMatrixAsTable(double[,] array)
        {
            var rowLength = array.GetLength(0);
            var colLength = array.GetLength(1);

            var line = new StringBuilder();
            for (var i = 0; i < rowLength; i++)
            {
                line.Append(Environment.NewLine);
                for (var j = 0; j < colLength; j++)
                {
                    line.Append(array[i, j].ToString(CultureInfo.InvariantCulture).PadLeft(10, ' '));
                }
                line.Append(Environment.NewLine);
            }
            Log.Debug(line);
        }
    }
}
