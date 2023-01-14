using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;




namespace TspCadPlugin
{

    public class MinCostPerfectMatching
    {

        ///// <summary>
        ///// Function that takes as an argument an edge graph in string form (example: "1-3", means edge from node 1 to node 3)
        ///// and retuns the edge in array form (example: [1,3])
        ///// </summary>
        ///// <param name="name">Edge, in string form, example: "1-3"</param>
        ///// <param name="nodes">List of all graph nodes</param>
        ///// <returns>Edge in array form, example: [1,3]</returns>
        //public static int[] ParseEdgeName(String name)
        //{
        //    string vertexFromLabel = name.Split(new char[] { '-' }, 2).ElementAt(0);
        //    string vertexToLabel = name.Split(new char[] { '-' }, 2).ElementAt(1);
        //    int vertexFrom = int.Parse(vertexFromLabel);
        //    int vertexTo = int.Parse(vertexToLabel);
        //    // int vertexFrom = nodes.IndexOfKey(vertexFromLabel);
        //    // int vertexTo = nodes.IndexOfKey(vertexToLabel);

        //    int[] edge = new int[2];
        //    edge[1] = vertexFrom;
        //    edge[0] = vertexTo;

        //    return edge;

        //}



        public static int[,] GetMinCostPerfectMatching(Double[,] distMatrix, List<int> oddVerticesList)
        {
            // Create the linear solver with SCIP backend
            Solver solver = Solver.CreateSolver("SCIP");


            if (solver is null)
            {
                return null;
            }


            // Create array where variables will be stored
            int n = oddVerticesList.Count;


            Variable[,] x = new Variable[n, n];
            //List<Variable> Vars = new List<Variable>();

            // Creating variables
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    {
                        x[i, j] = solver.MakeIntVar(0, 1, $"{oddVerticesList[i]}-{oddVerticesList[j]}");
                    }
                    
                }
            }



            // Defining cost function
            Objective objective = solver.Objective();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    objective.SetCoefficient(x[i, j], distMatrix[oddVerticesList[i], oddVerticesList[j]]);
                }
            }




            // ADDING CONSTRAINTS
            for (int i = 0; i < n; i++)
            {
                Constraint constraintDegree1 = solver.MakeConstraint(1, 1, "");

                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        //Every vertex has to be 1 degree
                        constraintDegree1.SetCoefficient(x[i, j], 1);

                        // Edge symmetry (EA = AE) 
                        Constraint constraintEdgeSymmetry = solver.MakeConstraint(0, 0);
                        constraintEdgeSymmetry.SetCoefficient(x[i, j], 1);
                        constraintEdgeSymmetry.SetCoefficient(x[j, i], -1);


                    }

                }
            }



            objective.SetMinimization();
            Solver.ResultStatus resultStatus = solver.Solve();



            //List<int[]> oddEdgesAddToMST = new List<int[]>();

            int[,] mpmGraphOddVertices = new int[n,n];


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (x[i, j].SolutionValue() == 1)
                    {
                        //oddEdgesAddToMST.Add(ParseEdgeName(x[i, j].Name()));
                        mpmGraphOddVertices[i, j] = 1;
                    } else
                    {
                        mpmGraphOddVertices[i, j] = 0;
                    }
                }

            }


            return mpmGraphOddVertices;
        }
    }
}
