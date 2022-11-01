using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Principal;

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
        private Dictionary<string, Node> nodeMap;
        private string inputPath;
        public int numberOfFiles;

        private string libraryFile = @"PackageLibrary.json";
        private string nodeMapFile = @"PackageMap.json";

        public NodeManager(string inputPath)
        {
            this.inputPath = inputPath;
            nodeMap = new Dictionary<string, Node>();
            numberOfFiles = Directory.GetFiles(inputPath, "*.package", SearchOption.AllDirectories).Length;
            numberOfFiles += Directory.GetDirectories(inputPath, "*", SearchOption.AllDirectories).Length + 1;
        }

        internal void LoadPackages()
        {
            if (File.Exists(libraryFile))
            {
                root = LoadLibrary(libraryFile);
                if (root != null) {
                    CreateNodeMap(root);
                }
            }

            if (root == null)
            {
                root = new Node(nodeMap, this.inputPath);
            }

            SaveLibrary(root);
        }

        internal void SaveLibrary(Node root)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(libraryFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, root);
            }

            using (StreamWriter sw = new StreamWriter(nodeMapFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, nodeMap);
            }
        }

        internal Node LoadLibrary(string libraryFile)
        {
            string libraryJSON = File.ReadAllText(libraryFile);
            return JsonConvert.DeserializeObject<Node>(libraryJSON);
        }

        internal Dictionary<String, Node> LoadNodeMap(string nodeMapFile)
        {
            string nodeMapJSON = File.ReadAllText(nodeMapFile);
            return JsonConvert.DeserializeObject<Dictionary<String,Node>>(nodeMapJSON);
        }

        internal void CreateNodeMap(Node current)
        {
            nodeMap[current.hash] = current;
            CreateNodeMapForList(current.dirs);
            CreateNodeMapForList(current.files);
        }

        private void CreateNodeMapForList(List<Node> data)
        {
            if (data != null)
            {
                foreach (Node node in data)
                {
                    CreateNodeMap(node);
                }
            }
        }

        internal int GetProgress() {
            int value = (int) Math.Floor(((float) nodeMap.Count / (float) numberOfFiles)*100.0);
            return value;
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
                FindNode(package).isEnabled = true;
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

        public Node FindNode(string hash)
        {
            return nodeMap[hash];
        }
    }
}

