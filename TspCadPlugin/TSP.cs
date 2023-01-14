using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;



namespace TspCadPlugin
{
    public static class TSP
    {
        
        public struct Node
        {
            public ObjectId id;
            public Double x;
            public Double y;
            public string label;
        }

        public struct Edge
        {
            public int vertexFrom;
            public int vertexTo;
            public double weight;
        }

        public struct TSPData
        {
            public Double[,] distMatrix;
            public List<Node> nodes;
        }

        public static Dictionary<string, Func<double[,], int[]>> tspAlgorithmDict = new Dictionary<string, Func<double[,], int[]>>()
        {
            {"optimalDynamicProgramming", OptimalTspDynProg.GetOptimalTSP},
            {"optimalIlpWithTimeVars",  OptimalTspIlp.GetOptimalTspIlpWithTimeVars},
            {"optimalIlpWithoutTimeVars", OptimalTspIlp.GetOptimalTspIlpWithoutTimeVars},
            {"approxChristofides", AproxChristofides.GetAproxTSPChristofides},
            {"approx2Tree",  AproxDoubleTree.GetAproxDoubleTree},
            {"ORTools",  OrToolsTSP.Main}

        };


        public static void ComputeTsp(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, String aglorithmType)
        {

            TSP.TSPData tspData = Utils.GetDistanceMatrix(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec);
            Double[,] distMatrix = tspData.distMatrix;
            List<TSP.Node> nodes = tspData.nodes;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Func<Double[,], int[]> tspAlgFunction = tspAlgorithmDict[aglorithmType];
            int[] tour =  tspAlgFunction(distMatrix);
            Double tourLength = Utils.PlotTour(tour, nodes, tr, acBlkTblRec);
            watch.Stop();
            Application.ShowAlertDialog("Execution time: " + (Convert.ToDouble(watch.ElapsedMilliseconds) / 1000).ToString() + " seconds \n" +
                "Tour Length: " + tourLength.ToString("0.##") + " units");
        }


        public static void ComputeTspOrToolsMultipleVehicles(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec)
        {
            TSP.TSPData tspData = Utils.GetDistanceMatrix(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec);
            Double[,] distMatrix = tspData.distMatrix;
            List<TSP.Node> nodes = tspData.nodes;
        }




        public static void ComputeMST(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, String aglorithmType)
        {
            TSP.TSPData tspData = Utils.GetDistanceMatrix(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec);
            Double[,] distMatrix = tspData.distMatrix;
            List<Node> nodes = tspData.nodes;

            int[,] mst = Utils.GetMSTfromDistMatrix(distMatrix);
            //int[,] mstChris = AproxChristofides.GetAproxTSPChristofides(distMatrix);
            Utils.PlotGraph(mst, nodes, tr, acBlkTblRec);

        }







    }
}
