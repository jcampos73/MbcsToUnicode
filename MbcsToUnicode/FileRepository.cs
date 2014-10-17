using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MbcsToUnicode
{
    public class FileRepository
    {
        public List<string> FileList;

        public FileRepository()
        {
            FileList = new List<string>();
        }

        public void DoProcessDirectories(string path, string pattern)
        { 
            if(path.LastIndexOf(@"\") != path.Length-1){
                path += @"\";
            }

            DoProcessFiles(path, pattern);

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(path);

            // Get a reference to each directory in that directory.
            DirectoryInfo[] diArr = di.GetDirectories();

            // Display the names of the directories. 
            foreach (DirectoryInfo dri in diArr)
                DoProcessFiles(dri.FullName, pattern);
        }

        public void DoProcessFiles(string path, string pattern)
        {
            if (path.LastIndexOf(@"\") != path.Length - 1)
            {
                path += @"\";
            }

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.EnumerateFiles(pattern))
            {
                FileList.Add(fi.FullName);
            }
        }   
    }
}
