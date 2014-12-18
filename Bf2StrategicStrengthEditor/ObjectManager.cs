using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    /// <summary>
    /// The object manager class is used to keep a list of all loaded
    /// ObjectTemplates, and provide methods to fetch them globally
    /// </summary>
    class ObjectManager
    {
        /// <summary>
        /// Contains a list of all objects registered in the global namespace
        /// </summary>
        protected static Dictionary<string, AiFile> Globals = new Dictionary<string, AiFile>();

        /// <summary>
        /// Indicates the number of objects loaded into the Global Namespace
        /// </summary>
        public static int ObjectsCount
        {
            get { return Globals.Count; }
        }

        /// <summary>
        /// Clears all objects loaded from memory
        /// </summary>
        public static void ReleaseAll()
        {
            Globals.Clear();
        }

        /// <summary>
        /// Loads all of the AiFiles objects into the Global Namespace
        /// </summary>
        /// <param name="ConFile">The AI file to parse</param>
        /// <returns>Returns whether or not the file could be fully parsed</returns>
        public static bool RegisterFileObjects(AiFile ConFile)
        {
            // Parsing this file will add all objects to the Globals
            try
            {
                AiFileParser.Parse(ConFile);
                return true;
            }
            catch (Exception)
            {
                // Remove all objects in the AiFile
                foreach (var item in Globals.Where(x => x.Value.FilePath == ConFile.FilePath).ToList())
                {
                    Globals.Remove(item.Key);
                }

                return false;
            }
        }

        /// <summary>
        /// USED BY PARSER - Registers a new object into the namespace
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        public static void RegisterObject(string objectName, AiFile file)
        {
            Globals.Add(objectName, file);
        }

        /// <summary>
        /// Returns whether the specified object name is registered
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ContainsObject(string name)
        {
            return Globals.ContainsKey(name);
        }

        /// <summary>
        /// Fetches the loaded object by th specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectTemplate GetObjectByName(string name)
        {
            return Globals[name].Objects[name];
        }

        /// <summary>
        /// Returns the AiFile that the specified object is located in
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AiFile GetObjectsFileByName(string name)
        {
            return Globals[name];
        }
    }
}
