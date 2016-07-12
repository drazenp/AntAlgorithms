using System;
using AlgorithmsCore;
using AlgorithmsCore.Options;
using BasicUnweighted;

namespace AntAlgorithmsAnalize
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random(Environment.TickCount);

            Func<AnalyzeData, DimacsGraph> graphFunc = data =>
            {
                var dataLoader = new FileLoader(data.GraphFilePath);
                var graph = new DimacsGraph(dataLoader);
                graph.InitializeGraph();

                return graph;
            };

            var analyzeData = AnalyzeDataAccess.GetAnalyzeData();

            while (analyzeData != null)
            {
                var startDate = DateTime.UtcNow;
                var inputGraph = graphFunc(analyzeData);

                //var fileWriter = new FileWriter("graph.json");
                //var exportGraph = new GephiFileExport(fileWriter);

                var graphOptions = new BaseOptions(analyzeData.NumberOfIterations, analyzeData.NumberOfPartitions, analyzeData.Alfa,
                                                analyzeData.Beta, analyzeData.Ro, analyzeData.Delta);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(graphOptions);

                var aspg = new AspgUnweighted(graphOptions, inputGraph, rnd);
                var resultBasic = aspg.GetQuality();

                var analyzeResult = new AnalyzeResult
                {
                    AnalyzeID = analyzeData.ID,
                    BestCost = resultBasic.BestCost,
                    BestCostIteration = resultBasic.BestCostIteration,
                    Duration = resultBasic.ElapsedMilliseconds,
                    StartDate = startDate,
                    EndDate = DateTime.UtcNow
                };

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(analyzeResult);

                AnalyzeDataAccess.SaveAnalyzeResult(analyzeResult);

                analyzeData = AnalyzeDataAccess.GetAnalyzeData();
            }

            //var options = new BaseOptions(numberOfIterations: 10, numberOfRegions: 2, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            //var dataLoader = new FileLoader("C.txt");
            //var graph = new DimacsGraph(dataLoader);
            //graph.InitializeGraph();
            //var aspg = new AspgUnweighted(options, graph, rnd);
            //var resultBasic = aspg.GetQuality();
            //var globlaCost = resultBasic.BestCost;
        }
    }
}
