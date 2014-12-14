using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2
{
    /// <summary>
    /// Indicats an aiTemplate object within the Objects.ai
    /// </summary>
    public class AiTemplate : ObjectTemplate
    {
        /// <summary>
        /// An array of each Template plugin associated with this AiTemplate
        /// </summary>
        public Dictionary<AiTemplatePluginType, ObjectTemplate> Plugins = new Dictionary<AiTemplatePluginType, ObjectTemplate>();
    }
}
