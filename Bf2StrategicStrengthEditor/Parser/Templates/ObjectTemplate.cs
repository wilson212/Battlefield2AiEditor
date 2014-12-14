using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    public class ObjectTemplate
    {
        /// <summary>
        /// The Ai File that contains this Object
        /// </summary>
        public AiFile File;

        /// <summary>
        /// The name of this Object given in the create command
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of object (if specified in the create command) this is
        /// </summary>
        public string ObjectType;

        /// <summary>
        /// If there was a comment proceeding this object, its set here
        /// </summary>
        public RemComment Comment;

        /// <summary>
        /// The position in the AiFile this object is located
        /// </summary>
        public int Position;

        /// <summary>
        /// The object template name used to access this object in the AiFile
        /// </summary>
        public string TemplateTypeString;

        /// <summary>
        /// The Template type
        /// </summary>
        public TemplateType TemplateType;

        /// <summary>
        /// A list of properties this object contains
        /// </summary>
        public Dictionary<string, List<ObjectProperty>> Properties;

        public override string ToString()
        {
            return Name;
        }
    }
}
