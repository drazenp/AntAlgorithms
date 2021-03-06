﻿using System;
using System.Diagnostics;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace Basic
{
    public class Aspg : AspgBase
    {
        public Aspg(BaseOptions options, IGraph graph, Random rnd)
            : base(options, graph, rnd) { }
        
        public override ResultData GetQuality()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = new Result(double.MinValue);
            var bestCostIteration = 0;

            while (Options.NumberOfIterations > 0)
            {
                Log.Debug("Iteration: " + Options.NumberOfIterations);
                
                var antSystemFragment = new WeightedAntSystemFragment(Rnd, Options, Graph);
                var antSystem = new AntSystemBasic(antSystemFragment, Options, Graph);

                for (var vertexIndex = Options.NumberOfRegions; vertexIndex < Graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    decimal[] probability = antSystem.CalculateProbability(nextColony);
                    var chosenVertexIndex = Roulette(probability);
                    var chosenVertex = Graph.VerticesWeights[chosenVertexIndex];
                    antSystem.AddFreeVertexToTreil(nextColony, chosenVertex);
                }

                var bestFragment = antSystem.UpdatePhermone();

                var newQuality = bestFragment.SumOfOptimalityCriterion;
                Console.WriteLine($"New quality: {newQuality}");
                //Log.Debug($"New quality: {newQuality}");

                // Save the best results.
                if (result.Quality < newQuality)
                {
                    result = new Result(newQuality, bestFragment.Treil);
                    bestCostIteration = Options.NumberOfIterations;
                }

                Options.NumberOfIterations--;
            }
            stopwatch.Stop();

            //Log.Debug($"Best result: {result.Quality}");

            var qualityResult = new ResultData((int)result.Quality, bestCostIteration, stopwatch.ElapsedMilliseconds);

            return qualityResult;
        }
    }
}
