using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    public struct ObjectProperty
    {
        /// <summary>
        /// The name of this property tag
        /// </summary>
        public string Name;

        /// <summary>
        /// If there was a comment proceeding this object, its set here
        /// </summary>
        public RemComment Comment;

        /// <summary>
        /// The position in the AiFile this object is located
        /// </summary>
        public int Position;

        /// <summary>
        /// An array of values for this property
        /// </summary>
        public string[] Values;
    }
}
