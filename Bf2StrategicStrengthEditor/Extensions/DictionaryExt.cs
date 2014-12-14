using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlefield2.Extensions
{
    static class DictionaryExt
    {
        /// <summary>
        /// Merges 2 dictionarys keys and values together
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="me"></param>
        /// <param name="merge"></param>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                me[item.Key] = item.Value;
            }
        }
    }
}
