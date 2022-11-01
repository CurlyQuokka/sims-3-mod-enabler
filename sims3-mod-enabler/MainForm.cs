using System.Diagnostics;
using System.Windows.Forms;

namespace sims3_mod_enabler
{

    public partial class MainForm : Form
    {
        private DataLoader dataLoader;

        private NodeManager nodeManager;
        private FileManager fileManager;
        private LoadingForm loadingForm;
        private LoadingForm savingForm;

        public MainForm()
        {
            InitializeComponent();
            inputTree.AfterCheck += tree_AfterCheck;
            outputTree.AfterCheck += tree_AfterCheck;
            dataLoader = new DataLoader();
            dataLoader.DataLoaded += dataLoader_OnDataLoaded;
            dataLoader.TreeUpdated += dataLoader_OnTreeUpdated;
            loadingForm = new LoadingForm(dataLoader, "Loading data", "Loading packages, please wait...");
            inputTree.CheckBoxes = true;
            outputTree.CheckBoxes = true;
        }

        private void dataLoader_OnDataLoaded(object sender, EventArgs e)
        {
            nodeManager = dataLoader.GetNodeManager();
            fileManager = dataLoader.GetFileManager();
        }

        private void dataLoader_OnTreeUpdated(object sender, bool isOutput)
        {
            if (isOutput)
            {
                Invoke(() => {
                    CopyTree(dataLoader.OutputTree, outputTree);
                });
            } else
            {
                Invoke(() => {
                    CopyTree(dataLoader.InputTree, inputTree);
                });
            }
        }

        private void CopyTree(TreeView source, TreeView destination)
        {
            destination.BeginUpdate();
            destination.Nodes.Clear();
            CopyNodes(source.Nodes, destination.Nodes);
            if (destination.Nodes.Count > 0)
            {
                destination.Nodes[0].Expand();
            }
            destination.EndUpdate();
        }

        private void CopyNodes(TreeNodeCollection source, TreeNodeCollection destination)
        {
            foreach (TreeNode node in source)
            {
                TreeNode current = destination.Add(node.Name, node.Text);
                CopyNodes(node.Nodes, current.Nodes);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataLoader.LoadData();
            loadingForm.Show();
        }

        private void CheckChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;

                Node n = nodeManager.FindNode(node.Name);
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
                Node n = nodeManager.FindNode(e.Node.Name);
                if (n != null)
                {
                    n.isChecked = e.Node.Checked;
                }
            }
        }

        private void enableButton_Click(object sender, EventArgs e)
        {
            Task.Run(() => { dataLoader.UpdatePackagesState(true); } );
        }

        private void disableButton_Click(object sender, EventArgs e)
        {
            Task.Run(() => { dataLoader.UpdatePackagesState(false); }); ;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            savingForm = new LoadingForm(fileManager, "Applying configuration", "Applying configuration, please wait...");
            savingForm.Show();
            Task.Run(() => {
                List<EnableData> enabled = nodeManager.GetEnabled();
                fileManager.ApplyConfiguration(enabled); 
            });            
        }
    }
}