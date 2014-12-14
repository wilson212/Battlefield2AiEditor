﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.IO
{
    class DirectoryExt
    {
        /// <summary>
        /// Copies the contents of one folder, to another.
        /// </summary>
        /// <param name="sourceDirName">The source folder, being copied</param>
        /// <param name="destDirName">The destination dicrectoy, where all the copies will be stored. 
        /// The directory does NOT have to exist.</param>
        /// <param name="copySubDirs">Recursively copy sub folders?</param>
        public static void Copy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Source directory must exist!
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // Create the dir if it doesnt exist
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // Do we recursivly copy?
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    Copy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static void Delete(string path)
        {
            DirectoryInfo Dir = new DirectoryInfo(path);
            Delete(Dir);
        }

        public static void Delete(FileSystemInfo fileSystemInfo)
        {
            DirectoryInfo Dir = fileSystemInfo as DirectoryInfo;
            if (Dir != null && Dir.Exists)
            {
                foreach (FileSystemInfo Child in Dir.GetFileSystemInfos())
                {
                    Delete(Child);
                    System.Threading.Thread.Sleep(2);
                }
            }

            // Delete the root info
            if (fileSystemInfo.Exists)
            {
                fileSystemInfo.Attributes = FileAttributes.Normal;
                fileSystemInfo.Delete();
            }
        }
    }
}
