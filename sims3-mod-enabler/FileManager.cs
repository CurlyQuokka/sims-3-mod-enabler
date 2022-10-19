using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sims3_mod_enabler
{
    internal class FileManager
    {
        const string gamePackagesDir = "Electronic Arts\\The Sims 3\\Mods\\Packages";
        const string packagesLibraryDir = "Electronic Arts\\The Sims 3\\PackagesLibrary";
        const string packagePattern = "*.package";
        private string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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
                packages.Add(splitted[splitted.Length - 1].Replace(".package",""));
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

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (EnableData d in data)
            {
                bool ok = CreateHardLink(GetGamesPackagesDir() + Path.DirectorySeparatorChar + d.hash + ".package", d.path, IntPtr.Zero);
                if (!ok)
                {
                    return false;
                }
            }
            return true;
        }
}
}
