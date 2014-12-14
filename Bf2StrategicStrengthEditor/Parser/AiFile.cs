using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Battlefield2
{
    public class AiFile
    {
        /// <summary>
        /// Relative file path from the Objects_server root folder
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// The ai file template type
        /// </summary>
        public AiFileType FileType { get; protected set; }

        /// <summary>
        /// A list of found ObjectTemplates in the AI file
        /// </summary>
        public Dictionary<string, ObjectTemplate> Objects;

        /// <summary>
        /// Creates a new instance of AiFile
        /// </summary>
        /// <param name="RelativePath"></param>
        /// <param name="Type"></param>
        public AiFile(string filePath, AiFileType fileType)
        {
            this.FilePath = filePath;
            this.FileType = fileType;
        }

        /// <summary>
        /// Returns all of this files objects into the Ai file format
        /// </summary>
        /// <returns></returns>
        public string GetParsedContents()
        {
            return AiFileParser.ToFileFormat(this);
        }

        /// <summary>
        /// Saves the Objects changes into the AiFile
        /// </summary>
        public void Save()
        {
            string fullPath = Path.Combine(Application.StartupPath, "Temp", FilePath);
            File.WriteAllText(fullPath, AiFileParser.ToFileFormat(this));
        }
    }

    public enum AiFileType
    {
        Kit,
        Vehicle, 
        Weapon
    }
}
