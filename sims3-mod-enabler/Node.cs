using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sims3_mod_enabler
{
    internal class Node
    {
        private const string packageExtension = ".package";
        public string name;
        public string hash;
        public string parentHash;
        public string filepath;
        public bool isFile = false;
        public bool isChecked = false;
        public bool isEnabled = false;
        public List<Node>? dirs = null;
        public List<Node>? files = null;

        public Node() { }

        public Node(Dictionary<string, Node> nodeMap, string filepath, string parentHash = "")
        {
            //Debug.WriteLine(filepath);
            string[] pathSplit = filepath.Split(Path.DirectorySeparatorChar);
            this.parentHash = parentHash;
            this.filepath = filepath;
            name = pathSplit[pathSplit.Length - 1];
            
            isFile = IsFile(this.filepath);

            CalculateHash();

            if (!isFile)
            {
                dirs = InitializeList(Directory.GetDirectories, nodeMap);
                files = InitializeList(Directory.GetFiles, nodeMap);
            }
            //if (this.hash != null)
            //{
                nodeMap[this.hash] = this;
            //}

        }

        private void CalculateHash()
        {
            using (var md5 = MD5.Create())
            {
                byte[] fielpathBytes = Encoding.ASCII.GetBytes(filepath);
                hash = BitConverter.ToString(md5.ComputeHash(fielpathBytes)).Replace("-", "");
            }
        }

        //private void CalculateHash()
        //{
        //    using (var md5 = MD5.Create())
        //    {
        //        if (isFile)
        //        {
        //            long length = new FileInfo(filepath).Length;
        //            using (var stream = File.OpenRead(filepath))
        //            {
        //                int size = Convert.ToInt32(length);
        //                int bufferSize = 102400;
        //                if (size < bufferSize) {
        //                    bufferSize = size;
        //                }
        //                byte[] ar = new byte[bufferSize];
        //                stream.Read(ar, 0, bufferSize);
        //                hash = BitConverter.ToString(md5.ComputeHash(ar)).Replace("-", "");
        //            }
        //        } else
        //        {
        //            byte[] fielpathBytes = Encoding.ASCII.GetBytes(filepath);
        //            hash = BitConverter.ToString(md5.ComputeHash(fielpathBytes)).Replace("-", "");
        //        }
        //    }
        //}

        private List<Node> InitializeList(Func<string, string[]> getData, Dictionary<string, Node> nodeMap)
        {
            List<Node> worklist = new List<Node>();
            string[] dataListed = getData(filepath);
            foreach (string entry in dataListed)
            {
                bool isEntryAFile = IsFile(entry);
                if (isEntryAFile && (entry.EndsWith(packageExtension)) || !isEntryAFile)
                {
                    worklist.Add(new Node(nodeMap, entry, this.hash));
                }
            }
            return worklist;
        }

        private static bool IsFile(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory;
        }

        public static void LoadTree(Node node, TreeNodeCollection collection, bool getEnabled)
        {
            if (node.isFile && node.isEnabled == getEnabled || !node.isFile)
            {
                TreeNode current = collection.Add(node.hash, node.name);
                ProcessNodeList(node.dirs, current.Nodes, getEnabled);
                ProcessNodeList(node.files, current.Nodes, getEnabled);
            }
        }

        private static void ProcessNodeList(List<Node> nodelist, TreeNodeCollection current, bool getEnabled)
        {
            if (nodelist != null)
            {
                foreach (Node n in nodelist)
                {
                    LoadTree(n, current, getEnabled);
                }
            }
        }
    }
}
