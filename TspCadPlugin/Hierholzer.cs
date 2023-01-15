using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspCadPlugin
{
    public class Hierholzer
    {

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
