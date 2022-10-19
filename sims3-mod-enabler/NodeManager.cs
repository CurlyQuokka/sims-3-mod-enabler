using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sims3_mod_enabler
{
    internal class EnableData
    {
        public string path;
        public string hash;

        public EnableData(string path, string hash)
        {
            this.path = path;
            this.hash = hash;
        }
    }
    internal class NodeManager
    {
        private Node root;

        public NodeManager(string inputPath)
        {
            root = new Node(inputPath);
        }

        internal Node Root { get => root; set => root = value; }

        public static void TogglePackages(Node node, bool desiredEnableState = true)
        {
            if (node.isFile && node.isChecked && node.isEnabled != desiredEnableState)
            {
                node.isEnabled = desiredEnableState;
            }
            else
            {
                ToggleList(node.dirs, desiredEnableState);
                ToggleList(node.files, desiredEnableState);
            }
            node.isChecked = false;
        }

        private static void ToggleList(List<Node> listToToggle, bool desiredEnableState)
        {
            if (listToToggle != null)
            {
                foreach (Node n in listToToggle)
                {
                    TogglePackages(n, desiredEnableState);
                }
            }
        }

        public void EnablePackages(List<string> packages)
        {
            foreach (string package in packages)
            {
                this.root.FindByHash(package).isEnabled = true;
            }
        }

        public List<EnableData> GetEnabled()
        {
            return GetEnabled(Root);
        }

        private static List<EnableData> GetEnabled(Node node)
        {
            List<EnableData> enabled = new List<EnableData>();
            if (node.isEnabled && node.isFile)
            {
                enabled.Add(new EnableData(node.filepath, node.hash));
            }

            enabled.AddRange(GetEnabledFromList(node.dirs));
            enabled.AddRange(GetEnabledFromList(node.files));

            return enabled;
        }

        private static List<EnableData> GetEnabledFromList(List<Node> entries)
        {
            List<EnableData> enabled = new List<EnableData>();
            if (entries != null) {
                foreach (Node entry in entries)
                {
                    enabled.AddRange(GetEnabled(entry));
                }
            }
            return enabled;
        }
    }
}

