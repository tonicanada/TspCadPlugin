using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;

namespace TspCadPlugin
{
    public static class OrToolsTSP
    {

        public static Dictionary<string, FirstSolutionStrategy.Types.Value> firstSolutionStrategy = new Dictionary<string, FirstSolutionStrategy.Types.Value>()
        {
            {"AUTOMATIC", FirstSolutionStrategy.Types.Value.Automatic},
            {"PATH_CHEAPEST_ARC", FirstSolutionStrategy.Types.Value.PathCheapestArc},
            {"PATH_MOST_CONSTRAINED_ARC", FirstSolutionStrategy.Types.Value.PathMostConstrainedArc},
            {"EVALUATOR_STRATEGY", FirstSolutionStrategy.Types.Value.Savings},
            {"SAVINGS", FirstSolutionStrategy.Types.Value.Savings},
            {"SWEEP", FirstSolutionStrategy.Types.Value.Sweep},
            {"CHRISTOFIDES", FirstSolutionStrategy.Types.Value.Christofides},
            {"ALL_UNPERFORMED", FirstSolutionStrategy.Types.Value.AllUnperformed},
            {"BEST_INSERTION", FirstSolutionStrategy.Types.Value.BestInsertion},
            {"PARALLEL_CHEAPEST_INSERTION", FirstSolutionStrategy.Types.Value.ParallelCheapestInsertion},
            {"LOCAL_CHEAPEST_INSERTION", FirstSolutionStrategy.Types.Value.LocalCheapestArc},
            {"GLOBAL_CHEAPEST_ARC", FirstSolutionStrategy.Types.Value.GlobalCheapestArc},
            {"LOCAL_CHEAPEST_ARC", FirstSolutionStrategy.Types.Value.LocalCheapestArc},
            {"FIRST_UNBOUND_MIN_VALUE", FirstSolutionStrategy.Types.Value.FirstUnboundMinValue}
        };


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


        
        public static List<List<int>> Main(Double[,] distMatrix, string firstSolStrategy, int vehicleNumber = 3, int startNode = 0)
        {
            // Create Routing Index Manager
            RoutingIndexManager manager =
                new RoutingIndexManager(distMatrix.GetLength(0), vehicleNumber, startNode);


            // Create Routing Model.
            RoutingModel routing = new RoutingModel(manager);

            // Create and register a transit callback.
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

            if (vehicleNumber > 1)
            {
                // Add Distance constraint.
                routing.AddDimension(transitCallbackIndex, 0, 3000,
                                     true, // start cumul to zero
                                     "Distance");
                RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
                distanceDimension.SetGlobalSpanCostCoefficient(100);
            }

            
            // Setting first solution heuristic.
            RoutingSearchParameters searchParameters =
                operations_research_constraint_solver.DefaultRoutingSearchParameters();


            searchParameters.FirstSolutionStrategy = firstSolutionStrategy[firstSolStrategy];


            // Solve the problem.
            Assignment solution = routing.SolveWithParameters(searchParameters);

            if (solution != null)
            {
                List<List<int>> routes = GetRoutes(routing, manager, solution);
                return routes;
            } else
            {
                throw new Exception("No solution found");
            }
            

        }
    }
}
