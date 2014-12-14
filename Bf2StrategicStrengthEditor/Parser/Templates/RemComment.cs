using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    public struct RemComment
    {
        /// <summary>
        /// The position in the AiFile this rem comment is located
        /// </summary>
        public int Position;

        /// <summary>
        /// The comment text
        /// </summary>
        public string Value;

        public override string ToString()
        {
            return Value;
        }
    }
}
