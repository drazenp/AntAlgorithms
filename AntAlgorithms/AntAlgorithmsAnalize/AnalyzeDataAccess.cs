using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace AntAlgorithmsAnalize
{
    static class AnalyzeDataAccess
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AnalyzeConnectionString"].ConnectionString;

        public static AnalyzeData GetAnalyzeData()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                const string sqlAnalyzeData = @"select ad.ID, ad.GraphFilePath, ad.NumberOfPartitions, ad.Alfa, ad.Beta, ad.Ro, ad.Delta,
                                                ad.NumberOfIterations, count(ar.AnalyzeID) as NumberOfResults, ad.NumberOfEdges
                                                from AnalyzeData ad
                                                left join AnalyzeResults ar on ar.AnalyzeID=ad.ID
                                                group by ad.ID, ad.GraphFilePath, ad.NumberOfPartitions, ad.Alfa, ad.Beta, ad.Ro, ad.Delta,
                                                ad.NumberOfIterations, ad.TimesToRun, ad.NumberOfEdges
                                                having NumberOfResults<ad.TimesToRun";

                using (var cmd = new SQLiteCommand(sqlAnalyzeData, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        var analyzeData = new AnalyzeData
                        {
                            ID = reader.GetInt32(0),
                            GraphFilePath = reader.GetString(1),
                            NumberOfPartitions = reader.GetInt32(2),
                            Alfa = reader.GetDouble(3),
                            Beta = reader.GetDouble(4),
                            Ro = reader.GetDouble(5),
                            Delta = reader.GetDouble(6),
                            NumberOfIterations = reader.GetInt32(7),
                            NumberOfEdges = reader.GetInt32(9)
                        };

                        return analyzeData;
                    }
                }
            }
        }

        public static void SaveAnalyzeResult(AnalyzeResult analyzeResult)
        {
            const string sqlSaveAnalyzeResult = @"insert into AnalyzeResults(AnalyzeID, BestCost, StartDate, EndDate, Duration, BestCostIteration)
                                                      values(@AnalyzeID, @BestCost, @StartDate, @EndDate, @Duration, @BestCostIteration)";

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(sqlSaveAnalyzeResult, conn, tran))
                    {
                        cmd.Parameters.Add("AnalyzeID", DbType.Int32).Value = analyzeResult.AnalyzeID;
                        cmd.Parameters.Add("@BestCost", DbType.Int32).Value = analyzeResult.BestCost;
                        cmd.Parameters.Add("@StartDate", DbType.Int32).Value =
                            analyzeResult.GetIntegerFromDate(analyzeResult.StartDate);
                        cmd.Parameters.Add("@EndDate", DbType.Int32).Value =
                            analyzeResult.GetIntegerFromDate(analyzeResult.EndDate);
                        cmd.Parameters.Add("@Duration", DbType.Int32).Value = analyzeResult.Duration;
                        cmd.Parameters.Add("@BestCostIteration", DbType.Int32).Value = analyzeResult.BestCostIteration;
                        
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                }
            }
        }
    }
}
