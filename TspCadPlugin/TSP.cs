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
        };

        public static int[] ComputeTsp(string algorithmType)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (DocumentLock docLock = acDoc.LockDocument())
            using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForWrite) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                TSP.TSPData tspData = Utils.GetDistanceMatrix(tr);
                Double[,] distMatrix = tspData.distMatrix;
                List<TSP.Node> nodes = tspData.nodes;

                var watch = System.Diagnostics.Stopwatch.StartNew();
                Func<Double[,], int[]> tspAlgFunction = tspAlgorithmDict[algorithmType];
                int[] tour = tspAlgFunction(distMatrix);
                Double tourLength = Utils.PlotTour(tour, nodes, tr, acBlkTblRec);
                watch.Stop();
                Application.ShowAlertDialog("Execution time: " + (Convert.ToDouble(watch.ElapsedMilliseconds) / 1000).ToString() + " seconds \n" +
                    "Tour Length: " + tourLength.ToString("0.##") + " units");

                tr.Commit();

                return tour;


            }
        }


        public static void ComputeMst()
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (DocumentLock docLock = acDoc.LockDocument())
            using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForWrite) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                TSP.TSPData tspData = Utils.GetDistanceMatrix(tr);
                Double[,] distMatrix = tspData.distMatrix;
                List<Node> nodes = tspData.nodes;

                int[,] mst = Utils.GetMSTfromDistMatrix(distMatrix);
                Utils.PlotGraph(mst, nodes, tr, acBlkTblRec);
            }

        }
        

        public static void ComputeTspOrToolsMultipleVehicles(int vehicleNumber, TSP.Node startNode)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (DocumentLock docLock = acDoc.LockDocument())
            using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForWrite) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                
                TSP.TSPData tspData = Utils.GetDistanceMatrix(tr);
                Double[,] distMatrix = tspData.distMatrix;
                List<Node> nodes = tspData.nodes;

                // If Start Node is not selected means there is just one vehicle and vehicle routing is reduced to a TSP
                if (startNode.label is null)
                {
                    List<int> route = OrToolsTSP.Main(distMatrix, vehicleNumber, 0)[0];
                    Utils.PlotTour(route.ToArray(), nodes, tr, acBlkTblRec);
                } else
                {
                    int startNodeIdx = nodes.FindIndex(node => node.id == startNode.id);
                    List<List<int>> routes = OrToolsTSP.Main(distMatrix, vehicleNumber, startNodeIdx);
                    for (int i = 0; i < routes.Count; i++)
                    {
                        Utils.PlotTour(routes[i].ToArray(), nodes, tr, acBlkTblRec, i+1);
                    }
                }

                tr.Commit();

            }

        }





        //public static void ComputeTsp(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, String aglorithmType)
        //{

        //    TSP.TSPData tspData = Utils.GetDistanceMatrix(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec);
        //    Double[,] distMatrix = tspData.distMatrix;
        //    List<TSP.Node> nodes = tspData.nodes;

        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    Func<Double[,], int[]> tspAlgFunction = tspAlgorithmDict[aglorithmType];
        //    int[] tour =  tspAlgFunction(distMatrix);
        //    Double tourLength = Utils.PlotTour(tour, nodes, tr, acBlkTblRec);
        //    watch.Stop();
        //    Application.ShowAlertDialog("Execution time: " + (Convert.ToDouble(watch.ElapsedMilliseconds) / 1000).ToString() + " seconds \n" +
        //        "Tour Length: " + tourLength.ToString("0.##") + " units");
        //}


        //public static void ComputeTspOrToolsMultipleVehicles(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec)
        //{
        //    TSP.TSPData tspData = Utils.GetDistanceMatrix(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec);
        //    Double[,] distMatrix = tspData.distMatrix;
        //    List<TSP.Node> nodes = tspData.nodes;
        //}












    }
}
