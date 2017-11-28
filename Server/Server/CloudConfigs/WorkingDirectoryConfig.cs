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
    }
}
