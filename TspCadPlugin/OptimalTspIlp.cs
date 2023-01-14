using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.LinearSolver;

namespace TspCadPlugin
{
    public static class OptimalTspIlp
    {
        /// <summary>
        /// Function that solves TSP with ILP Formulation without the time variables.
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <returns>Optimal TSP tour</returns>
        public static int[] GetOptimalTspIlpWithTimeVars(Double[,] distMatrix)
        {
            return GetOptimalTspIlp(distMatrix, "withTimeVariables");
        }


        /// <summary>
        /// Function that solves TSP with ILP Formulation with time variables.
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <returns>Optimal TSP tour</returns>
        public static int[] GetOptimalTspIlpWithoutTimeVars(Double[,] distMatrix)
        {
            return GetOptimalTspIlp(distMatrix, "withoutTimeVariables");
        }


        /// <summary>
        /// Function that solves TSP with ILP, user can choose the type of formulation required.
        /// </summary>
        /// <param name="distMatrix">Distance Matrix</param>
        /// <param name="formulationType">ILP Formulation to avoid subtours, can be "withTimeVariables" or "withoutTimeVariables".</param>
        /// <returns>Optimal TSP tour</returns>
        public static int[] GetOptimalTspIlp(Double[,] distMatrix, String formulationType)
        {
           
            // Create the linear solver with SCIP backend
            Solver solver = Solver.CreateSolver("SCIP");
            if (solver is null)
            {
                return null;
            }

            // Create Variables: Edges
            int n = distMatrix.GetLength(0);
            Variable[,] x = new Variable[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    x[i, j] = solver.MakeIntVar(0, 1, $"{i}-{j}");
                }
            }
 




            // Defining constraints
            // Constraints (every node is connected only to 2 edges)
            for (int i = 0; i < n; i++)
            {
                Constraint constraint_1 = solver.MakeConstraint(1, 1, "");
                Constraint constraint_2 = solver.MakeConstraint(1, 1, "");
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        constraint_1.SetCoefficient(x[i, j], 1);
                        constraint_2.SetCoefficient(x[j, i], 1);

                    } 
                }
            }

            if (formulationType == "withTimeVariables")
            {
                // Create Variables: Time
                Variable[] u = new Variable[n];
                for (int i = 0; i < n; i++)
                {
                    u[i] = solver.MakeIntVar(0, n, $"u{i}");
                }

                // Timing constraints
                // u1 = 1
                Constraint constraintTimeU1 = solver.MakeConstraint(1, 1, "");
                constraintTimeU1.SetCoefficient(u[0], 1);

                for (int i = 1; i < n; i++)
                {
                    Constraint constraint = solver.MakeConstraint(2, n, "");
                    constraint.SetCoefficient(u[i], 1);
                }

                for (int i = 1; i < n; i++)
                {
                    for (int j = 1; j < n; j++)
                    {
                        if (i != j)
                        {
                            Constraint constraintTime = solver.MakeConstraint(Double.NegativeInfinity, n - 1, "");
                            constraintTime.SetCoefficient(u[i], 1);
                            constraintTime.SetCoefficient(u[j], -1);
                            constraintTime.SetCoefficient(x[i, j], n);
                        }
                    }
                }
            }
            else if (formulationType == "withoutTimeVariables")
            {
                List<string> subsets = GeneratorSubsets(n);

                for (int i = 0; i < subsets.Count(); i++)
                {
                    int subsetNodes = subsets[i].Count(num => (num == '1'));
                    if (subsetNodes > 1)
                    {
                        Constraint constraintSubtours = solver.MakeConstraint(0, subsetNodes - 1);
                        List<int[]> edgesList = GeneratorEdgesFromSubset(subsets[i]);
                        for (int j = 0; (j < edgesList.Count); j++)
                        {
                            constraintSubtours.SetCoefficient(x[edgesList[j][0], edgesList[j][1]], 1);
                        }
                    }

                }
            }


            // Defining cost function
            Objective objective = solver.Objective();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    objective.SetCoefficient(x[i, j], distMatrix[i, j]);
                }
            }

            objective.SetMinimization();
            Solver.ResultStatus resultStatus = solver.Solve();


            int[,] tspGraph = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (x[i, j].SolutionValue() == 1)
                    {
                        tspGraph[i, j] = 1;
                    }
                }

            }

            int[] tour = Hierholder.HierholderDirectedGraph(0, tspGraph).ToArray();
            //tour[n] = 0;

            return tour;

        }


        /// <summary>
        /// Function that generates all possible subsets for n elements using bitmasking,
        /// excluding "0", "1", "10" and "111...1".
        /// </summary>
        /// <param name="n">Number of elements</param>
        /// <returns>Returns a list of subsets, example (n=3): {011, 100, 101, 110}</returns>
        public static List<string> GeneratorSubsets(int n)
        {
            List<string> subsets = new List<string>();
            for (int i = 3; i < Math.Pow(2, n) - 1; i++)
            {
                string subset = Convert.ToString(i, 2).PadLeft(n, '0');
                subsets.Add(subset);
            }
            return subsets;
        }


        /// <summary>
        /// Given a subset in bitmask form returns all possible combinations of 2 elements.
        /// Example: "1011" will return { {0,1}, {0,3}, {1,3} }.
        /// </summary>
        /// <param name="subset">Subset in bitmask form, example: "1011".</param>
        /// <returns>List of arrays with all possible combinations of 2 elements in subset.</returns>
        public static List<int[]> GeneratorEdgesFromSubset(string subset)
        {
            int n = subset.Length;
            List<int[]> edgesList = new List<int[]>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (int.Parse(subset[i].ToString()) > 0 && int.Parse(subset[j].ToString()) > 0)
                    {
                        if (i != j)
                        {
                            int[] edge = new int[2];
                            edge[0] = i;
                            edge[1] = j;
                            edgesList.Add(edge);
                        }
                    }
                }
            }
            return edgesList;
        }



    }
}
