using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspCadPlugin
{
    public static class AproxChristofides
    {
        public static int[] GetAproxTSPChristofides(Double[,] distMatrix)
        {

            int[,] mst = Utils.GetMSTfromDistMatrix(distMatrix);
            int n = mst.GetLength(0);

            // We make a set of the MST odd vertices (vertices with odd degree)
            List<int> oddVerticesList = new List<int>();
            for (int i = 0; i < n; i++)
            {
                int degree = 0;
                for (int j = 0; j < n; j++)
                {
                    if (mst[i, j] > 0)
                    {
                        degree++;
                    }
                }
                if (degree % 2 != 0)
                {
                    oddVerticesList.Add(i);
                }
            }

            int numOddVertices = oddVerticesList.Count;

            int[,] mstSubgraph = new int[numOddVertices, numOddVertices];
            for (int i = 0; i < numOddVertices; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (mst[oddVerticesList.ElementAt(i), oddVerticesList.ElementAt(j)] == 1)
                    {
                        mstSubgraph[i, j] = 1;
                        mstSubgraph[j, i] = 1;
                    }

                }
            }

            int[,] mpmGraphOddVertices = MinCostPerfectMatching.GetMinCostPerfectMatching(distMatrix, oddVerticesList);


            // Adding edges found in Min Cost Perfect Matching
            for (int i=0; i< numOddVertices; i++)
            {
                for (int j=0; j< numOddVertices; j++) 
                {
                    if (mpmGraphOddVertices[i, j] == 1)
                    {
                        mst[oddVerticesList[i], oddVerticesList[j]] += 1;
                    }
                }
            }


            List<int> path = new List<int>();
            path = Hierholder.HierholderUndirectedGraph(0, mst);

            // Remove duplicates from path
            path = Utils.RemoveVisitedVerticesFromPath(distMatrix.GetLength(0), path);
            path.Add(0);
            return path.ToArray();

        }
    }
}
