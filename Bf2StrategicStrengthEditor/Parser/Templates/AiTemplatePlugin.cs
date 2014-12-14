using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    /// <summary>
    /// Tells the AI bots how to operate vehicles, and what they're worth
    /// </summary>
    public struct AiTemplatePlugin
    {
        public string Name;

        public AiTemplatePluginType Type;

        public Dictionary<string, List<string[]>> Entries;
    }
}
