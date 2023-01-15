using System.Windows.Forms;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

using Autodesk.AutoCAD.EditorInput;

namespace TspCadPlugin
{
    public class MyCommands

    {
        /// <summary>
        /// Main command to show program form interface.
        /// </summary>
        [CommandMethod("tsp")]
        public static void ShortestPathMatrixCommand()
        {
            FormInterface form = new FormInterface();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(form);
        }


        /// <summary>
        /// Function that prompts the user to select graph the nodes (cities), where TSP will be performed.
        /// Nodes are block objects in AutoCAD named as "node", it doesn't matter if more objects like points, circles,
        /// blocks with different names... are selected, selection is filtered afterwards by "GetAdjacencyMatrix"
        /// function in TSP.cs.
        /// </summary>
        /// <returns>ObjectID array.</returns>
        [CommandMethod("select_nodes")]
        public static ObjectId[] SelectNodes(string promptMsg = "Please select nodes where TSP will be performed")
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            ObjectId[] selectionObjectsIdArray;


            TypedValue[] filterlist = new TypedValue[2];
            filterlist[0] = new TypedValue(0, "INSERT");
            filterlist[1] = new TypedValue(2, "node");
            SelectionFilter filter = new SelectionFilter(filterlist);

            selectionObjectsIdArray = selectCadObjects(acDoc, promptMsg, filter);
            return selectionObjectsIdArray;
        }




        /// <summary>
        /// Function that prompts the user to select CAD objects in the model.
        /// </summary>
        /// <param name="acDoc">CAD Document.</param>
        /// <param name="selectionPromptMessage">Message that will be displayed to the user</param>
        /// <returns>ObjectID array.</returns>
        public static ObjectId[] selectCadObjects(Document acDoc, string selectionPromptMessage, SelectionFilter selectionFilter = null)
        {
            PromptSelectionResult acSPrompt;
            PromptSelectionOptions PtSelOpts = new PromptSelectionOptions();
            PtSelOpts.MessageForAdding = $"\n{selectionPromptMessage}";

            if (selectionFilter == null)
            {
                acSPrompt = acDoc.Editor.GetSelection(PtSelOpts);
            } else
            {
                acSPrompt = acDoc.Editor.GetSelection(PtSelOpts, selectionFilter);
            }
            

            SelectionSet acSSetBlocks;

            if (acSPrompt.Status == PromptStatus.OK)
            {
                acSSetBlocks = acSPrompt.Value;
                ObjectId[] objIdArrayTotal = acSSetBlocks.GetObjectIds();
                return objIdArrayTotal;

            }
            else
            {
                return null;
            }
        }

    }
}
