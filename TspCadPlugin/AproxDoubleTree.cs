using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspCadPlugin
{
    public static class AproxDoubleTree
    {

        public static int[] GetAproxDoubleTree(Double[,] distMatrix)
        {
            int[,] mst = Utils.GetMSTfromDistMatrix(distMatrix);
            for (int i = 0; i < mst.GetLength(0); i++)
            {
                for (int j = 0; j < mst.GetLength(1); j++)
                {
                    if (mst[i, j] > 0)
                    {
                        mst[i, j] = 2;
                    }
                }
            }

            List<int> path = new List<int>();
            path = Hierholder.HierholderDirectedGraph(0, mst);

            // Remove duplicates from path
            path = Utils.RemoveVisitedVerticesFromPath(distMatrix.GetLength(0), path);
            path.Add(0);
            return path.ToArray();

        }
    }
}
