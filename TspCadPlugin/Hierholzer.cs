using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspCadPlugin
{
    public class Hierholzer
    {
        //public static List<int> FindCycleFromNodeUsingDFS(int current, List<int> path, int[,] adjMatrix, int startNode)
        //{

        //    for (int i = 0; i < adjMatrix.GetLength(0); i++)
        //    {
        //        if (adjMatrix[current, i] > 0)
        //        {
        //            adjMatrix[current, i] -= 1;
        //            adjMatrix[i, current] -= 1;
        //            path.Add(current);
                    
        //            if (i == startNode)
        //            {
        //                return path;
        //            }
        //            return FindCycleFromNodeUsingDFS(i, path, adjMatrix, startNode);
        //        }
        //    }
        //    return path;
        //}

        //public static int RemainingEdges(int[,] adjMatrix)
        //{
        //    int count = 0;
        //    for (int i=0; i<adjMatrix.GetLength(0); i++)
        //    {
        //        for (int j=0; j<adjMatrix.GetLength(0); j++)
        //        {
        //            if (adjMatrix[i, j] > 0)
        //            {
        //                count++;
        //            }
        //        }
        //    }
        //    return count / 2;
        //}


        //public static void InsertPathIntoPath(List<int> path, List<int> newPath, int index)
        //{
        //    for (int i=0; i< newPath.Count; i++)
        //    {
        //        path.Insert(index+i, newPath[i]);
        //    }
        //}

        //public static List<int> FindEulerianCircuit(int start, int[,] adjMatrix)
        //{
        //    List<int> emptyPath = new List<int>();
        //    List<int> path = FindCycleFromNodeUsingDFS(start, emptyPath, adjMatrix, start);
        //    List<int> currentPath = new List<int>();
        //    while (RemainingEdges(adjMatrix) > 0)
        //    {
        //        for (int i=0; i<path.Count; i++)
        //        {
        //            List<int> emptyPath2 = new List<int>();
        //            currentPath = FindCycleFromNodeUsingDFS(path.ElementAt(i), emptyPath2, adjMatrix, path.ElementAt(i));
        //            if (currentPath.Count > 0)
        //            {
        //                InsertPathIntoPath(path, currentPath, i);
        //            }
        //        }
        //    }
        //    path.Add(start);
        //    return path;
        //}

        public static List<List<TSP.Edge>> GetAdjListFromAdjMatrix(int[,] adjMatrix)
        {
            int n = adjMatrix.GetLength(0);
            List<List<TSP.Edge>> adjList = new List<List<TSP.Edge>>();

            for (int i=0; i<n; i++)
            {
                List<TSP.Edge> edgeList = new List<TSP.Edge>();
                for (int j=0; j<n; j++)
                {
                    if (adjMatrix[i,j] >0)
                    {

                        TSP.Edge edge = new TSP.Edge();
                        edge.vertexFrom = i;
                        edge.vertexTo = j;
                        edge.weight = 0;
                        edgeList.Add(edge);
                    }
                 
                }
                adjList.Add(edgeList);
            }
            return adjList;

        }


        public static void countInOutDegrees(List<List<TSP.Edge>> adjList, int[] in_, int[] out_) {
            foreach (List<TSP.Edge> edges in adjList)
            {
                foreach (TSP.Edge edge in edges)
                {
                    out_[edge.vertexFrom]++;
                    in_[edge.vertexTo]++;
                }
            }
        }

        public static void DfsDirectedGraph(List<List<TSP.Edge>> adjList, int at, int[] out_, List<int> eulerianTour)
        {
            while (out_[at] != 0)
            {
                TSP.Edge nextEdge = adjList[at].ElementAt(--out_[at]);
                DfsDirectedGraph(adjList, nextEdge.vertexTo, out_, eulerianTour);
            }
            eulerianTour.Insert(0, at);
            
        }


        public static void DfsUndirectedGraph(List<List<TSP.Edge>> adjList, int at, int[] out_, List<int> eulerianTour)
        {
            while (out_[at] != 0)
            {
                TSP.Edge nextEdge = adjList[at].ElementAt(0);
                adjList[at].Remove(nextEdge);
                out_[at]--;
                TSP.Edge edgeToRemove = adjList[nextEdge.vertexTo].First(edge => (edge.vertexTo == at));
                adjList[nextEdge.vertexTo].Remove(edgeToRemove);
                out_[nextEdge.vertexTo]--;

                DfsUndirectedGraph(adjList, nextEdge.vertexTo, out_, eulerianTour);
            }
            eulerianTour.Insert(0, at);

        }



        public static List<int> HierholderDirectedGraph(int startNode, int[,] adjMatrix)
        {
            int n = adjMatrix.GetLength(0);
            List<List<TSP.Edge>> g = GetAdjListFromAdjMatrix(adjMatrix);
            int[] in_ = new int[n];
            int[] out_ = new int[n];
            List<int> eulerianTour = new List<int>();
            countInOutDegrees(g, in_, out_);
            DfsDirectedGraph(g, 0, out_, eulerianTour);
            return eulerianTour;

        }


        public static List<int> HierholderUndirectedGraph(int startNode, int[,] adjMatrix)
        {
            int n = adjMatrix.GetLength(0);
            List<List<TSP.Edge>> g = GetAdjListFromAdjMatrix(adjMatrix);
            int[] in_ = new int[n];
            int[] out_ = new int[n];
            List<int> eulerianTour = new List<int>();
            countInOutDegrees(g, in_, out_);
            DfsUndirectedGraph(g, 0, out_, eulerianTour);
            return eulerianTour;

        }



    }
}
