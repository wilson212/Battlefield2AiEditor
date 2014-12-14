using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    public static class FileExtensions
    {
        /// <summary>
        /// Renames the file to the specified name
        /// </summary>
        /// <param name="newName">The new name of this file</param>
        public static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + newName);
        }
    }
}
