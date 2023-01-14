﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;

namespace TspCadPlugin
{
    public static class OrToolsTSP
    {

        /// <summary>
        ///   Print the solution.
        /// </summary>
        public static int[] GetSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution)
        {
            int[] tour = new int[routing.Size()+1];

            var index = routing.Start(0);
            int i = 0;

            while (routing.IsEnd(index) == false)
            {
                tour[i] = (manager.IndexToNode((int)index));
                index = solution.Value(routing.NextVar(index));
                i++;
            }
            tour[tour.Length - 1] = 0;
            return tour;
        }

        public static List<List<int>> GetRoutes(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution)
        {
            List<List<int>> routes = new List<List<int>>();
            for (int i=0; i<routing.Vehicles() ; i++)
            {
                List<int> route = new List<int>();
                long index = routing.Start(i);
                route.Add(manager.IndexToNode(index));
                while (!routing.IsEnd(index))
                {
                    index = solution.Value(routing.NextVar(index));
                    route.Add(manager.IndexToNode(index));
                }
                routes.Add(route);
            }
            return routes;
        }



        public static int[] Main(Double[,] distMatrix)
        {
            // Create Routing Index Manager
            RoutingIndexManager manager =
                new RoutingIndexManager(distMatrix.GetLength(0), 1, 0);
            RoutingModel routing = new RoutingModel(manager);

            int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
            {
                // Convert from routing variable Index to
                // distance matrix NodeIndex.
                var fromNode = manager.IndexToNode(fromIndex);
                var toNode = manager.IndexToNode(toIndex);
                return Convert.ToInt64(distMatrix[fromNode, toNode]);
            });


            // Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);


            // Setting first solution heuristic.
            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();

            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.Christofides;




            // Solve the problem.
            Assignment solution = routing.SolveWithParameters(searchParameters);
            int[] tour = GetSolution(routing, manager, solution);
            return tour;

        }
    }
}