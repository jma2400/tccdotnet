using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using tccdotnet.Debug;

namespace tccdotnet
{
    /// <summary>
    /// this class helps populate the treeview given a parsetree
    /// </summary>
    public sealed class ParseTreeViewer
    {
        private ParseTreeViewer()
        {
        }

        public static void Populate(TreeView treeview, IParseTree parsetree)
        {
            treeview.Visible = false;
            treeview.SuspendLayout();
            treeview.Nodes.Clear();
            treeview.Tag = parsetree;

            IParseNode start = parsetree.INodes[0];
            TreeNode node = new TreeNode(start.Text);
            node.Tag = start;
            node.ForeColor = Color.SteelBlue;
            treeview.Nodes.Add(node);

            PopulateNode(node, start);
            treeview.ExpandAll();
            treeview.ResumeLayout();
            treeview.Visible = true;
        }
            
        private static void PopulateNode(TreeNode node, IParseNode start)
        {
            foreach (IParseNode ipn in start.INodes)
            {
                TreeNode tn = new TreeNode(ipn.Text);
                tn.Tag = ipn;
                node.Nodes.Add(tn);
                PopulateNode(tn, ipn);
            }
        }

    }
}
