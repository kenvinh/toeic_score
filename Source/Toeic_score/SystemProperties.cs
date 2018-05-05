using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Toeic_score
{
    public static class SystemProperties
    {
        public static string UserAppPath = Application.StartupPath;
        
        public static string UserFolderName = Application.StartupPath + @"\users";

        public static string TestDataFolderName = Application.StartupPath + @"\test_repo";

        public static string FileDateTimeFormat = "yyyy-mm-dd HH:mm:ss";

        /// <summary>
        /// Get all files inside the folder path and return files' name only
        /// </summary>
        /// <param name="path">Path to the folder</param>
        /// <returns>List of file name</returns>
        public static List<string> GetFolderFilesName(string path)
        {
            if (Directory.Exists(path))
            {
                List<string> output = new List<string>();
                DirectoryInfo  dir_info = new DirectoryInfo(UserFolderName);
                FileInfo[] flist = dir_info.GetFiles();

                foreach(FileInfo item in flist)
                {
                    output.Add(item.Name);
                }
                return output;
            }
            else
            {
                throw new ArgumentException("Folder not exist");
            }
        }

        /// <summary>
        /// Get all files inside the folder path and return file directory (path name)
        /// </summary>
        /// <param name="path">Path to the folder</param>
        /// <returns>List of file directory</returns>
        public static List<string> GetFolderFilesDir(string path)
        {
            if (Directory.Exists(path))
            {
                List<string> output = new List<string>();
                DirectoryInfo dir_info = new DirectoryInfo(UserFolderName);
                FileInfo[] flist = dir_info.GetFiles();

                foreach (FileInfo item in flist)
                {
                    output.Add(item.FullName);
                }
                return output;
            }
            else
            {
                throw new ArgumentException("Folder not exist");
            }
        }

        /// <summary>
        /// Get file name from full path
        /// </summary>
        /// <param name="path">full path to file</param>
        /// <returns>File name</returns>
        public static string GetFileNameFromPath(string path)
        {
            string filename = Path.GetFileNameWithoutExtension(path);

            return filename;
        }

        public static long DurationInSecondFromTimeSpan(DateTime dt_start, DateTime dt_stop)
        {
            TimeSpan tspan = dt_stop - dt_start;

            return (long)tspan.TotalSeconds;
        }

        public static long DurationInSecondFromTimeSpan(double dt_start, double dt_stop)
        {
            DateTime dt_start_original = DateTime.FromOADate(dt_start);
            DateTime dt_stop_original = DateTime.FromOADate(dt_stop);

            TimeSpan tspan = dt_stop_original - dt_start_original;

            return (long)tspan.TotalSeconds;
        }
    }
}
