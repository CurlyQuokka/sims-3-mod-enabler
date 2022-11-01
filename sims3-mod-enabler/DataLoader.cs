using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sims3_mod_enabler
{
    internal class DataLoader : IReportProgress
    {
        private NodeManager nodeManager;
        private FileManager fileManager;

        private TreeView inputTree;
        private TreeView outputTree;

        public event EventHandler DataLoaded;
        public event EventHandler ProgressChanged;
        public event EventHandler<bool> TreeUpdated;

        public event PropertyChangedEventHandler PropertyChanged;

        private float progress;
        private bool finished = false;

        public TreeView InputTree { get => inputTree; }
        public TreeView OutputTree { get => outputTree; }
        public float Progress { get => progress; set => progress = value; }
        public bool Finished { get => finished; set => finished = value; }

        public DataLoader()
        {
            inputTree = new TreeView();
            outputTree = new TreeView();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal virtual void OnDataLoaded(EventArgs e)
        {
            DataLoaded?.Invoke(this, e);
        }

        internal virtual void OnProgressChanged(EventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
        
        internal virtual void OnTreeUpdated(bool isOutput)
        {

            TreeUpdated?.Invoke(this, isOutput);
        }

        internal void LoadData()
        {
            Task.Run(() => {
                fileManager = new FileManager();
                nodeManager = new NodeManager(fileManager.GetLibraryDir());
                nodeManager.LoadPackages();
                List<string> currentlyEnabled = fileManager.ReadPackages();
                nodeManager.EnablePackages(currentlyEnabled);
                UpdateInputTree();
                UpdateOutputTree();
                Finished = true;
                NotifyPropertyChanged();
                Finished = false;
                OnDataLoaded(EventArgs.Empty);
            });

            Task.Run(() =>
            {
                while(true)
                {
                    Progress = GetProgress();
                    NotifyPropertyChanged("Progress");
                    if (Progress >= 100)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            });

        }

        internal NodeManager GetNodeManager()
        {
            return nodeManager;
        }

        internal FileManager GetFileManager()
        {
            return fileManager;
        }

        public int GetProgress()
        {
            if (nodeManager != null)
            {
                return nodeManager.GetProgress();
            }
            return 0;
        }

        internal void UpdateInputTree() {
            UpdateTree(inputTree, nodeManager.Root, false);
            OnTreeUpdated(false);
        }

        internal void UpdateOutputTree()
        {
            UpdateTree(outputTree, nodeManager.Root, true);
            OnTreeUpdated(true);
        }

        private void UpdateTree(TreeView tree, Node sourceRoot, bool getEnabled)
        {
            tree.Nodes.Clear();
            Node.LoadTree(nodeManager.Root, tree.Nodes, getEnabled);

            bool removal = true;
            while (removal)
            {
                if (tree.Nodes.Count > 0)
                {
                    List<TreeNode> toRemove = new List<TreeNode>();

                    FindEmpty(tree.Nodes[0], nodeManager.Root, toRemove);
                    if (toRemove.Count <= 0)
                    {
                        removal = false;
                    }
                    else
                    {
                        RemoveEmpty(tree, toRemove);
                    }
                }
                else
                {
                    break;
                }
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
            if (node.Nodes.Count == 0 && !nodeManager.FindNode(node.Name).isFile)
            {
                toRemove.Add(node);
            }
            else
            {
                foreach (TreeNode n in node.Nodes)
                {
                    FindEmpty(n, sourceRoot, toRemove);
                }
            }
        }

        internal void UpdatePackagesState(bool enabled)
        {
            NodeManager.TogglePackages(nodeManager.Root, enabled);
            UpdateInputTree();
            UpdateOutputTree();
        }
    }    
}
