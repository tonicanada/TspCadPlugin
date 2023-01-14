using System;
using System.Collections.Generic;

namespace TspCadPlugin
{
    public static class OptimalTspDynProg
    {

        public static int[] GetOptimalTSP(Double[,] distMatrix)
        {
            int n = distMatrix.GetLength(0);
            double[,] memo = new double[n, (int)Math.Pow(2, n)];

            for (int i=0; i<memo.GetLength(0); i++)
            {
                for (int j=0; j<memo.GetLength(1); j++)
                {
                    memo[i, j] = double.PositiveInfinity;
                }
            }

            Setup(distMatrix, memo, 0, n);
            Solve(distMatrix, memo, 0, n);
            FindMinCost(memo, 0, n);
            int[] tour = FindOptimalTour(distMatrix, memo, 0, n);

            return tour;
        }


        /// <summary>
        /// Function that fills the memo table with the distance from the starting node to every other node.
        /// For example, if there are 4 nodes, and the 0 is the starting one, this function will fill the values in
        /// memo table for the following subsets {10, 100, 1000}
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <param name="memo">Memo table (n rows, 2^n columns)</param>
        /// <param name="startNode">Starting node</param>
        /// <param name="n">Number of nodes</param>
        public static void Setup(Double[,] distMatrix, Double[,] memo, int startNode, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (i == startNode)
                {
                    continue;
                }

                memo[i,  1 << i] = distMatrix[startNode, i];
            }
        }


        /// <summary>
        /// Function that solves the TSP problem using Dynamic Programming by a bottom-up approach.
        /// It builds a memo table (n rows, 2^n columns), where each column represents a subset of nodes,
        /// every row is the last node visited, and the table value is the minimum cost possible for
        /// a path from the starting node to the last one (row value). Example: row = 2, column = "10101", value = 25.4;
        /// This means that the optimal path from the starting node to the node 2, for the subset "10101" (bitmask notation) is 25.4.
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <param name="memo">Memo table (n rows, 2^n columns)</param>
        /// <param name="startNode">Starting node</param>
        /// <param name="n">Number of nodes</param>
        public static void Solve(Double[,] distMatrix, Double[,] memo, int startNode, int n)
        {
            for (int r = 3; r <= n; r++)
            {
                foreach (int subset in GenAllSubsets(r, n))
                {
                    if (NotIn(startNode, subset))
                    {
                        continue;
                    }
                    else
                    {
                        for (int next = 0; next < n; next++)
                        {
                            if (next == startNode || NotIn(next, subset)) continue;
                            
                            // We remove the next vertex and the starting node from the state
                            int state = subset ^ (1 << next);
                            state = state ^ (1 << startNode);
                            
                            Double minDist = Double.PositiveInfinity;

                            for (int e = 0; e < n; e++)
                            {
                                if (e == startNode || e == next || NotIn(e, subset)) continue;
                                Double newDistance = memo[e, state] + distMatrix[e, next];

                                if (newDistance < minDist)
                                {
                                    minDist = newDistance;
                                    memo[next, subset ^ (1<<startNode)] = minDist;
                                }
                            }
                        }
                    }
                }
            }

            
            Double currentMin = Double.PositiveInfinity;
            for (int i=0; i<n; i++)
            {
                if (i == startNode) continue;
                Double newDist = memo[i, GeneratePenultimateState(n, startNode)] + distMatrix[i, startNode];
                if (newDist < currentMin)
                {
                    currentMin = newDist;
                }
            }

            memo[startNode, (1<<n) - 1] = currentMin;


        }

        /// <summary>
        /// Function that returns the optimal cost for a solved TSP problem, given a filled memoization table.
        /// </summary>
        /// <param name="memo">Memo table (n rows, 2^n columns)</param>
        /// <param name="startNode">Starting node</param>
        /// <param name="n">Number of nodes</param>
        /// <returns>TSP optimal solution cost</returns>
        public static Double FindMinCost(Double[,] memo, int startNode, int n)
        {
            int endState = (1 << n) - 1;
            Double minCostTour = memo[startNode, endState];
            return minCostTour;
        }


        /// <summary>
        /// Function that returns the optimal tour for a solved TSP problem, given a filled memoization table.
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <param name="memo">Memo table (n rows, 2^n columns)</param>
        /// <param name="startNode">Starting node</param>
        /// <param name="n">Number of nodes</param>
        /// <returns>Array of integers that represents the optimal tour</returns>
        public static int[] FindOptimalTour(Double[,] distMatrix, Double[,] memo, int startNode, int n)
        {
            int[] tour = new int[n + 1];

            int lastIndex = startNode;
            int currentIndex = startNode;
            int currentSubset = ((1 << n) - 1) & ~(1 << startNode);

            double currentMin = double.PositiveInfinity;

            int step = n - 1;

            while (currentSubset > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    double d = memo[i, currentSubset] + distMatrix[i, lastIndex];
                    if (d < currentMin)
                    {
                        currentMin = d;
                        currentIndex = i;
                    }
                }

                tour[step] = currentIndex;
                step--;
                lastIndex = currentIndex;
                currentSubset = currentSubset & ~(1 << lastIndex);

            }

            tour[n] = tour[0] = startNode;
            return tour;
        }



        // Returns all sets of size n with r bits set to 1. Example: combinations(3,4) = [0111, 1011, 1101, 1110]



        /// <summary>
        /// Function that returns all sets of size n with r bits set to 1. Example: combinations(3,4) = {0111, 1011, 1101, 1110}
        /// </summary>
        /// <param name="r">Number of nodes</param>
        /// <param name="n">Total set size</param>
        /// <returns>Array of sets</returns>
        public static int[] GenAllSubsets(int r, int n)
        {
            List<int> subsets = new List<int>();
            Combinations(0, 0, r, n, subsets);
            return subsets.ToArray();
        }




        /// <summary>
        /// Recurrent function used by "GenAllSubsets" to generate all sets of size n with r bits set to 1.
        /// Example: combinations(3,4) = {0111, 1011, 1101, 1110}
        /// </summary>
        /// <param name="set">Current subset (in decimal form)</param>
        /// <param name="at">Starting index of iteration</param>
        /// <param name="r">Number of bits required to be 1</param>
        /// <param name="n">Total number of bits</param>
        /// <param name="subsets">List of sets</param>
        public static void Combinations(int set, int at, int r, int n, List<int> subsets)
        {
            if (r == 0)
            {
                subsets.Add(set);
                return;
            }

            for (int i = at; i < n; i++)
            {
                // Flip the ith element
                set = set | (1 << i);
                Combinations(set, i + 1, r - 1, n, subsets);
                set = set & ~(1 << i);
            }
        }


        
        /// <summary>
        /// Function that returns the penultimate state subset. For example, if starting node = 1 and n = 4,
        /// the penultimate state would be {1101}.
        /// </summary>
        /// <param name="n">Number of nodes</param>
        /// <param name="startNode">Starting node index</param>
        /// <returns>Penultimate state subset in decimal form {1101} would be 13.</returns>
        public static int GeneratePenultimateState(int n, int startNode)
        {
            return ((1 << n) - 1) & ~(1 << startNode);
        }
        
        
        /// <summary>
        /// Function that checks if an element exists in a given subset.
        /// </summary>
        /// <param name="i">Index node to check</param>
        /// <param name="subset">Subset in decimal form</param>
        /// <returns></returns>
        public static bool NotIn(int i, int subset)
        {
            return ((1 << i) & subset) == 0;
        }

    }
}
