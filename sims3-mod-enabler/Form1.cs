using System.Diagnostics;
using System.Windows.Forms;

namespace sims3_mod_enabler
{

    public partial class Form1 : Form
    {
        private NodeManager tm;
        private FileManager fm;

        public Form1()
        {
            fm = new FileManager();
            tm = new NodeManager(fm.GetLibraryDir());
            List<string> currentlyEnbled = fm.ReadPackages();
            tm.EnablePackages(currentlyEnbled);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inputTree.AfterCheck += tree_AfterCheck;
            outputTree.AfterCheck += tree_AfterCheck;
            InitializeTree(inputTree, tm.Root, false);
            InitializeTree(outputTree, tm.Root, true);
        }
        
        private void InitializeTree(TreeView tree, Node sourceRoot, bool getEnabled)
        {
            tree.CheckBoxes = true;
            UpdateTree(tree, sourceRoot, getEnabled);
        }

        private void UpdateTree(TreeView tree, Node sourceRoot, bool getEnabled)
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();

            Node.LoadTree(tm.Root, tree.Nodes, getEnabled);
            tree.EndUpdate();
            tree.CollapseAll();

            bool removal = true;
            while (removal)
            {
                if (tree.Nodes.Count > 0)
                {
                    List<TreeNode> toRemove = new List<TreeNode>();

                    FindEmpty(tree.Nodes[0], tm.Root, toRemove);
                    if (toRemove.Count <= 0)
                    {
                        removal = false;
                    }
                    else
                    {
                        RemoveEmpty(tree, toRemove);
                    }
                } else
                {
                    break;
                }
            }
            if (tree.Nodes.Count > 0)
            {
                tree.Nodes[0].Expand();
            }
        }

        private void RemoveEmpty(TreeView tree, List<TreeNode> toRemove)
        {
            foreach (TreeNode tn in toRemove)
            {
                if (tn.Parent != null)
                {
                    tn.Parent.Nodes.Remove(tn);
                }
                else
                {
                    tree.Nodes.Remove(tn);
                }
            }
        }

        private void FindEmpty(TreeNode node, Node sourceRoot, List<TreeNode> toRemove)
        {
            if (node.Nodes.Count == 0 && !sourceRoot.FindByHash(node.Name).isFile)
            {
                toRemove.Add(node);
            } else
            {
                foreach (TreeNode n in node.Nodes)
                {
                    FindEmpty(n, sourceRoot, toRemove);
                }
            }
        }

        private void CheckChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;

                Node n = tm.Root.FindByHash(node.Name);
                if (n != null)
                {
                    n.isChecked = node.Checked;
                }

                if (node.Nodes.Count > 0)
                {
                    CheckChildNodes(node, nodeChecked);
                }
            }
        }

        private void CheckParentNodes(TreeNode treeNode)
        {
            if (treeNode.Parent != null)
            {
                bool isChecked = false;
                foreach (TreeNode node in treeNode.Parent.Nodes)
                {
                    if (node.Checked)
                    {
                        isChecked = true;
                        break;
                    }
                }
                treeNode.Parent.Checked = isChecked;
                CheckParentNodes(treeNode.Parent);
            }
        }

        private void tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    CheckChildNodes(e.Node, e.Node.Checked);
                }
                CheckParentNodes(e.Node);
                Node n = tm.Root.FindByHash(e.Node.Name);
                if (n != null)
                {
                    n.isChecked = e.Node.Checked;
                }
            }
        }

        private void enableButton_Click(object sender, EventArgs e)
        {
            doUpdate(true);
        }

        private void disableButton_Click(object sender, EventArgs e)
        {
            doUpdate(false);
        }

        private void doUpdate(bool enabled)
        {
            NodeManager.TogglePackages(tm.Root, enabled);
            UpdateTree(inputTree, tm.Root, false);
            UpdateTree(outputTree, tm.Root, true);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            List<EnableData> enabled = tm.GetEnabled();
            foreach (EnableData data in enabled)
            {
                Debug.WriteLine(data.path + " " + data.hash);
            }
            bool ok = fm.CreateLinks(enabled);
            if (ok)
            {
                MessageBox.Show("Package configuration applied", "Info");
            } else
            {
                MessageBox.Show("Failed to enable mods", "Error");
            }
            
        }
    }
}