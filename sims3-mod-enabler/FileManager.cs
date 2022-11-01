using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sims3_mod_enabler
{
    internal class FileManager : IReportProgress
    {
        const string gamePackagesDir = "Electronic Arts\\The Sims 3\\Mods\\Packages";
        const string packagesLibraryDir = "Electronic Arts\\The Sims 3\\PackagesLibrary";
        const string packageExtension = ".package";
        const string packagePattern = "*" + packageExtension;
        private string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private int progress;
        private bool finished = false;

        public int Progress { get => progress; set => progress = value; }
        public bool Finished { get => finished; set => finished = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetGamesPackagesDir()
        {
            return documentsPath + Path.DirectorySeparatorChar + gamePackagesDir;
        }

        public string GetLibraryDir()
        {
            return documentsPath + Path.DirectorySeparatorChar + packagesLibraryDir;
        }

        public List<string> ReadPackages()
        {
            string[] files = Directory.GetFiles(GetGamesPackagesDir(), packagePattern, SearchOption.AllDirectories);
            List<string> packages = new List<string>();

            foreach (string file in files)
            {
                string[] splitted = file.Split(Path.DirectorySeparatorChar);
                packages.Add(splitted[splitted.Length - 1].Replace(packageExtension, ""));
            }

            return packages;
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
              string lpFileName,
              string lpExistingFileName,
              IntPtr lpSecurityAttributes
          );

        public bool CreateLinks(List<EnableData> data)
        {
            DirectoryInfo di = new DirectoryInfo(GetGamesPackagesDir());

            FileInfo[] filesToDelete = di.GetFiles();
            int numOfOps = data.Count() + filesToDelete.Length;
            Progress = 0;
            int counter = 0;

            foreach (FileInfo file in filesToDelete)
            {
                file.Delete();
                UpdateProgress(ref counter, numOfOps);
            }

            foreach (EnableData d in data)
            {
                bool ok = CreateHardLink(GetGamesPackagesDir() + Path.DirectorySeparatorChar + 
                    d.hash + packageExtension, d.path, IntPtr.Zero);
                if (!ok)
                {
                    return false;
                }
                UpdateProgress(ref counter, numOfOps);
            }
            Finished = true;
            NotifyPropertyChanged("Finished");
            Finished = false;
            return true;
        }

        private void UpdateProgress(ref int counter, int numOfOps)
        {
            counter++;
            int newProgress = (int)((float)counter / (float) numOfOps * 100.0);
            if (newProgress != Progress)
            {
                Progress = newProgress;
                NotifyPropertyChanged("Progress");
            }
            
        }

        public void ApplyConfiguration(List<EnableData> enabled)
        {
            bool ok = CreateLinks(enabled);
            if (ok)
            {
                MessageBox.Show("Package configuration applied", "Info");
            }
            else
            {
                MessageBox.Show("Failed to enable mods", "Error");
            }
        }

        public int GetProgress()
        {
            return Progress;
        }
}
}
