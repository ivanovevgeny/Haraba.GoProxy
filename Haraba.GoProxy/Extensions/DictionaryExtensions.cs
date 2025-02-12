﻿using System.Collections.Generic;

namespace Haraba.GoProxy.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }
            
            dictionary.Add(key, value);
        }
    }
}