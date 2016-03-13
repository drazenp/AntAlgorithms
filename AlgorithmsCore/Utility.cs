using System;
using System.Reflection;
using log4net;

namespace AlgorithmsCore
{
    public static class Utility
    {
        public static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogDoubleMatrixAsTable(double[,] array)
        {
            throw  new NotImplementedException();
        }
    }
}
