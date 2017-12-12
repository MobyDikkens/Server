using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server.CloudConfigs
{
    static class WorkingDirectoryConfig
    {
        //Returns cloud working directory
        public static string GetWorkingDirectory()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\MobyDi\\Desktop\\Server\\Server\\Server\\Resources\\workdir.ini"))
                {
                    String workdir = sr.ReadToEnd();
                    return workdir;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        //Return a tree catalogs of chosen directory
        public static string[][] GetCatalogTree(string path)
        {
            string[][] catalogTree = null;

            try
            {
                string[] folders = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                string[] files = null;

                catalogTree = new string[folders.Length][];

                for(int i = 0;i<folders.Length;i++)
                {
                    files = Directory.GetFiles(folders[i]);

                    catalogTree[i] = new string[files.Length + 1];

                    catalogTree[i][0] = folders[i];

                    for(int j=1;j<files.Length + 1;j++)
                    {
                        catalogTree[i][j] = files[j - 1];
                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return catalogTree;

        }

        public static void CreateDirectory(string path,string name)
        {
            try
            {
                DirectoryInfo directory = Directory.CreateDirectory(path + name);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string[][] GetLastUpdate(string path,string workingDir)
        {
            //Gets the catalog tree for a client
            string[][] catalogTree = GetCatalogTree(path);

            //add an last upate to each one of file
            for(int i = 0;i<catalogTree.Length;i++)
            {
                for(int j=0;j<catalogTree[i].Length;j++)
                {
                    catalogTree[i][j] += "\\"+Directory.GetLastWriteTime(catalogTree[i][j]);

                    //first entry - to delete paths in comp
                    int firstEntry = catalogTree[i][j].IndexOf(workingDir);

                    catalogTree[i][j] = catalogTree[i][j].Substring(firstEntry, catalogTree[i][j].Length - firstEntry - 1);
                }
            }

            return catalogTree;

        }


    }
}
