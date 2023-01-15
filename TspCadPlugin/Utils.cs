using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

namespace TspCadPlugin
{
    public static class Utils
    {


        public static Double PlotTour(int[] tour, List<TSP.Node> nodes, Transaction tr, BlockTableRecord acBlkTblRec, int colorIndex = 0)
        {
            Double tourLength = 0;
            for (int i = 0; i < tour.Length - 1; i++)
            {
                double x_i = nodes[tour[i]].x;
                double y_i = nodes[tour[i]].y;
                double x_j = nodes[tour[i+1]].x;
                double y_j = nodes[tour[i+1]].y;

                Line acLine = new Line(new Point3d(x_i, y_i, 0),
                                 new Point3d(x_j, y_j, 0));
                acLine.SetDatabaseDefaults();
                acLine.ColorIndex = colorIndex;
                acBlkTblRec.AppendEntity(acLine);
                tr.AddNewlyCreatedDBObject(acLine, true);
                tourLength += acLine.Length;
            }

            //tr.Commit();
            return tourLength;
        }


        public static Double PlotGraph(int[,] adjMatrix, List<TSP.Node> nodes, Transaction tr, BlockTableRecord acBlkTblRec, int colorIndex = 0)
        {
            Double tourLength = 0;
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (adjMatrix[i, j] > 0)
                    {
                        double x_i = nodes[i].x;
                        double y_i = nodes[i].y;
                        double x_j = nodes[j].x;
                        double y_j = nodes[j].y;

                        Line acLine = new Line(new Point3d(x_i, y_i, 0),
                             new Point3d(x_j, y_j, 0));
                        acLine.SetDatabaseDefaults();
                        acBlkTblRec.AppendEntity(acLine);
                        tr.AddNewlyCreatedDBObject(acLine, true);
                        tourLength += acLine.Length;
                    }
                }
            }

            tr.Commit();
            return tourLength;
        }

        public static int[,] GetAdjMatrixCompleteGraph(int n)
        {
            int[,] adjMatrix = new int[n, n];
            for (int i = 0; i<n; i++)
            {
                for (int j=0; j<n; j++)
                {
                    if (i != j)
                    {
                        adjMatrix[i, j] = 1;
                    }
                }
            }
            return adjMatrix;
        }

        public static List<int> RemoveVisitedVerticesFromPath(int nodesLength, List<int> path)
        {
            List<int> newPath = new List<int>();
            bool[] visited = new bool[nodesLength];
            for (int i = 0; i < path.Count; i++)
            {
                if (!visited[path[i]])
                {
                    newPath.Add(path[i]);
                    visited[path[i]] = true;
                }
            }
            return newPath;

        }

        public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }

            #endregion
        }


        
        public static TSP.Node GetStartNode(Transaction tr)
        {
            ObjectId[] selectedObjectsIdArray = MyCommands.SelectNodes("Please select the starting node.");
            if (selectedObjectsIdArray is null)
            {
                MessageBox.Show("Please select a starting node.");
                throw new System.Exception("Null selection");
            } else if (selectedObjectsIdArray.Length > 1)
            {
                MessageBox.Show("Please select just one starting node.");
                throw new System.Exception("More than 1 element selected");
            }
            TSP.Node node = new TSP.Node();

            if (selectedObjectsIdArray[0].ObjectClass.Name == "AcDbBlockReference")
            {
                BlockReference currentBlock = (BlockReference)tr.GetObject(selectedObjectsIdArray[0], OpenMode.ForRead);
                if (currentBlock.Name == "node")
                {
                    
                    node.id = currentBlock.ObjectId;
                    node.x = currentBlock.Position.X;
                    node.y = currentBlock.Position.Y;

                    foreach (ObjectId att_id in currentBlock.AttributeCollection)
                    {
                        AttributeReference att = (AttributeReference)tr.GetObject(att_id, OpenMode.ForRead);
                        node.label = att.TextString;
                    }
                    
                }
            }
            return node;

        }
        
        
        public static TSP.TSPData GetDistanceMatrix(Transaction tr)
        {

            ObjectId[] selectedObjectsIdArray = MyCommands.SelectNodes();
            List<TSP.Node> nodes = new List<TSP.Node>();

            if (selectedObjectsIdArray is null)
            {
                throw new System.Exception("Please select a graph.");
            }

            foreach (ObjectId blocknode_id in selectedObjectsIdArray)
            {
                if (blocknode_id.ObjectClass.Name == "AcDbBlockReference")
                {
                    BlockReference currentBlock = (BlockReference)tr.GetObject(blocknode_id, OpenMode.ForRead);
                    if (currentBlock.Name == "node")
                    {
                        TSP.Node node = new TSP.Node();
                        node.id = currentBlock.ObjectId;
                        node.x = currentBlock.Position.X;
                        node.y = currentBlock.Position.Y;

                        foreach (ObjectId att_id in currentBlock.AttributeCollection)
                        {
                            AttributeReference att = (AttributeReference)tr.GetObject(att_id, OpenMode.ForRead);
                            node.label = att.TextString;
                        }

                        nodes.Add(node);
                    }
                }
            }

            Double[,] distMatrix = new Double[nodes.Count, nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double x_i = nodes[i].x;
                    double y_i = nodes[i].y;
                    double x_j = nodes[j].x;
                    double y_j = nodes[j].y;
                    distMatrix[i, j] = Math.Sqrt(Math.Pow(x_i - x_j, 2) + Math.Pow(y_i - y_j, 2));
                    distMatrix[j, i] = distMatrix[i, j];
                }
            }
            TSP.TSPData graph = new TSP.TSPData();
            graph.distMatrix = distMatrix;
            graph.nodes = nodes;
            return graph;
        }


        public static List<List<int>> GetAdjListFromAdjMatrix(int[,] adjMatrix)
        {
            List<List<int>> adjList = new List<List<int>>();
            for (int i=0; i<adjMatrix.GetLength(0); i++)
            {
                adjList.Add(new List<int>());
                for (int j=0; j< adjMatrix.GetLength(1); j++)
                {
                    if (adjMatrix[i, j] > 0)
                    {
                        adjList[i].Add(j);
                    }
                }
            }
            return adjList;
        }


        public static int[] DfsTraversal(int[,] adjMatrix, int startVertex)
        {
            int n = adjMatrix.GetLength(0);
            bool[] visitedNodes = new bool[n];
            List<int> nodesSequence = new List<int>();
            visitedNodes[startVertex] = true;
            nodesSequence.Add(startVertex);
            dfsRecursive(visitedNodes, nodesSequence, adjMatrix, startVertex);
            nodesSequence.Add(startVertex);
            return nodesSequence.ToArray();

        }

        //public static void dfsRecursive(bool[] visitedNodes, List<int> nodesSequence, int[,] adjMatrix, int startVertex)
        //{

        //    int n = adjMatrix.GetLength(0);
        //    for (int i=0; i<n; i++)
        //    {
        //        if (adjMatrix[startVertex, i] > 0 && !visitedNodes[i])
        //        {

        //            nodesSequence.Add(i);
        //            visitedNodes[i] = true;
        //            dfsRecursive(visitedNodes, nodesSequence, adjMatrix, i);


        //        }
        //    }
        //}

        public static void dfsRecursive(bool[] visitedNodes, List<int> nodesSequence, int[,] adjMatrix, int startVertex)
        {

            int n = adjMatrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                if (adjMatrix[startVertex, i] > 0)
                {

                    nodesSequence.Add(i);
                    visitedNodes[i] = true;
                    dfsRecursive(visitedNodes, nodesSequence, adjMatrix, i);


                }
            }
        }


        public class UnionFind
        {
            private int[] id;
            private int[] sizes;

            public UnionFind(int size)
            {
                this.id = new int[size];
                this.sizes = new int[size];

                for (int i = 0; i < size; i++)
                {
                    this.id[i] = i;
                    this.sizes[i] = 1;
                }
            }

            public int Find(int p)
            {
                int root = p;
                while (root != this.id[root])
                {
                    root = this.id[root];
                }

                // Now we apply path compression
                int el = p;
                while (el != root)
                {
                    int temp = this.id[el];
                    this.id[el] = root;
                    el = temp;
                }

                return root;
            }


            public bool Connected(int p, int q)
            {
                return this.Find(p) == this.Find(q);
            }

            public bool Union(int p, int q)
            {
                int root1 = this.Find(p);
                int root2 = this.Find(q);
                if (root1 == root2)
                {
                    return false;
                }
                else
                {
                    if (this.sizes[root1] > this.sizes[root2])
                    {
                        this.id[root2] = root1;
                        this.sizes[root1] += this.sizes[root2];
                    }
                    else
                    {
                        this.id[root1] = root2;
                        this.sizes[root2] += this.sizes[root1];
                    }
                    return true;
                }

            }
        }


        public struct Node
        {
            public String label;
            public Double x;
            public Double y;
        }


        public static int[,] GetMSTfromDistMatrix(Double[,] distMatrix)
        {
            SortedList<double, TSP.Edge> sortedEdges = new SortedList<double, TSP.Edge>(new Utils.DuplicateKeyComparer<double>());

            int n = distMatrix.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    TSP.Edge currentEdge = new TSP.Edge();
                    currentEdge.vertexFrom = i;
                    currentEdge.vertexTo = j;
                    currentEdge.weight = distMatrix[i, j];
                    sortedEdges.Add(distMatrix[i, j], currentEdge);
                }
            }


            int[,] mst = new int[n, n];
            UnionFind uF = new UnionFind(sortedEdges.Count);

            for (int i = 0; i < sortedEdges.Count; i++)
            {
                int vertexFrom = sortedEdges.Values[i].vertexFrom;
                int vertexTo = sortedEdges.Values[i].vertexTo;
                {
                    if (!uF.Connected(vertexFrom, vertexTo))
                    {
                        uF.Union(vertexFrom, vertexTo);
                        mst[vertexFrom, vertexTo] = 1;
                        mst[vertexTo, vertexFrom] = 1;
                    }
                }
            }

            return mst;
        }







        /// <summary>
        /// Method that generates the entities to build a node block with the shape of a circle with a letter inside.
        /// </summary>
        /// <param name="db">AutoCAD database</param>
        /// <param name="basePoint">Nodeblock basepoint</param>
        /// <returns>Entities list to build the block</returns>
        public static void CreateNodeBlockCADDatabase(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, Point3d basePoint, String blockName)
        {
            List<Entity> blockEntities = new List<Entity>();

            Circle circ = new Circle();
            circ.Center = basePoint;
            circ.Radius = 2.5;

            AttributeDefinition attr = new AttributeDefinition();
            attr.Tag = "Node name";
            attr.Prompt = "Node name:";
            //attr.TextString = "A";
            attr.Verifiable = true;
            attr.LockPositionInBlock = true;
            attr.TextStyleId = acCurDb.Textstyle;
            attr.Height = 1.75;
            attr.Justify = AttachmentPoint.MiddleCenter;

            blockEntities.Add(circ);
            blockEntities.Add(attr);

            Editor ed = acDoc.Editor;
            if (acBlkTbl.Has(blockName))
            {
                ed.WriteMessage($"Block {blockName} already exists");
                return;
            }
            BlockTableRecord btr = new BlockTableRecord();
            acBlkTbl.Add(btr);
            btr.Name = blockName;
            Point3d base_point = new Point3d(0, 0, 0);
            btr.Origin = base_point;
            btr.BlockScaling = BlockScaling.Uniform;
            btr.Explodable = true;

            foreach (Entity ent in blockEntities)
            {
                btr.AppendEntity(ent);
                tr.AddNewlyCreatedDBObject(ent, true);
            }

            tr.AddNewlyCreatedDBObject(btr, true);

        }



        /// <summary>
        ///  Function that reads CSV file stored in Resources folder, this file contains the information regarding the graph's nodes. 
        ///  CSV has 3 columns (nodeLabel, x coordinate, y coordinate)
        /// </summary>
        /// <param name="listSeparator">List separator char in csv file (examples: "," or ";")</param>
        /// <param name="fileName">CSV file name inlcuding extension stored in Resources folder (example: "nodes.csv")</param>
        /// <returns>Dictionary of `Polyline` CAD objects, corresponding to graph's edges</returns>
        public static List<Node> GetNodesFromCsv(String listSeparator, String fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = "TspCadPlugin.Resources." + fileName;

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                List<Node> result = new List<Node>();
                while (!reader.EndOfStream)
                {
                    string str;
                    string[] strArray;

                    str = reader.ReadLine();
                    strArray = str.Split(char.Parse(listSeparator));

                    Node currentNode = new Node();
                    currentNode.label = strArray[0];
                    currentNode.x = double.Parse(strArray[1]);
                    currentNode.y = double.Parse(strArray[2]);

                    result.Add(currentNode);

                }
                return result;
            }
        }

        /// <summary>
        /// Function that inserts a block into the model.
        /// </summary>
        /// <param name="bt">AutoCAD BlockTable</param>
        /// <param name="btr">AutoCAD BlockTableRecord</param>
        /// <param name="blockName">Node block's name</param>
        /// <param name="nodeLabel">Node Label</param>
        /// <param name="origin">Node Position</param>
        public static void DrawBlockNodeToModel(BlockTable bt, BlockTableRecord btr, String blockName, String nodeLabel, Point3d origin)
        {
            BlockTableRecord btrNode = bt[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

            using (BlockReference blockRef = new BlockReference(origin, btrNode.ObjectId))
            {
                btr.AppendEntity(blockRef);
                foreach (ObjectId id in btrNode)
                //Iterate block definition to find all non-constant
                // AttributeDefinitions
                {
                    DBObject obj = id.GetObject(OpenMode.ForRead);
                    AttributeDefinition attDef = obj as AttributeDefinition;
                    if ((attDef != null) && (!attDef.Constant))
                    {
                        //This is a non-constant AttributeDefinition
                        //Create a new AttributeReference
                        using (AttributeReference attRef = new AttributeReference())
                        {
                            attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);
                            attRef.TextString = nodeLabel;
                            //Add the AttributeReference to the BlockReference
                            blockRef.AttributeCollection.AppendAttribute(attRef);
                        }
                    }
                }
            }
        }







        public static void ClearModel(BlockTableRecord acBlkTblRec)
        {
                foreach (var id in acBlkTblRec)
            {
                id.GetObject(OpenMode.ForWrite).Erase(true);
            }
        }




        public static void PlotNodesFromCsv(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, String graphNodesNum)
        {
            Utils.ClearModel(acBlkTblRec);
            List<Node> nodes;
            nodes = GetNodesFromCsv(";", "samplegraph_" + graphNodesNum + "nodes.csv");
            CreateNodeBlockCADDatabase(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec, new Point3d(0, 0, 0), "node");


            for (var i = 0; i < nodes.Count; i++)
            {
                DrawBlockNodeToModel(acBlkTbl, acBlkTblRec, "node", nodes[i].label, new Point3d(nodes[i].x, nodes[i].y, 0));
            }

            tr.Commit();

            //using InvokeMember to support .NET 3.5
            Object acadObject = Autodesk.AutoCAD.ApplicationServices.Application.AcadApplication;
            acadObject.GetType().InvokeMember("ZoomExtents",
                        BindingFlags.InvokeMethod, null, acadObject, null);

        }


        public static int ParseTxtToInt(string input)
        {
            try
            {
                return Convert.ToInt32(input);
            } catch
            {
                return -1;
            }
        }
    }
}
