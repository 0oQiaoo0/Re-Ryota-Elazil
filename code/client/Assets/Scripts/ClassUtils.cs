using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class ClassUtils
    {

        public static V GetValue<K, V>(this Dictionary<K, V> dictionary, K key) where V : class
        {
            dictionary.TryGetValue(key, out V v);
            return v;
        }
    }
}