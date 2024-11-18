using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ForgeCore.Utility.Serialization
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue>
    {
        [SerializeField] 
        private List<SerializedDictionaryItem<TKey, TValue>> dictionaryItems;

        public void Add(TKey key, TValue value) => dictionaryItems.Add(new SerializedDictionaryItem<TKey, TValue>(key, value));
        public void Remove(TKey key) => dictionaryItems.Remove(new SerializedDictionaryItem<TKey, TValue>(key, default(TValue)));
        public void Clear() => dictionaryItems.Clear();

        public Dictionary<TKey, TValue> ToDictionary() => dictionaryItems.ToDictionary(item => item.itemKey, item => item.itemValue);
        
        public SerializedDictionary()
        {
            dictionaryItems = new List<SerializedDictionaryItem<TKey, TValue>>();
        }

        public SerializedDictionary(SerializedDictionary<TKey, TValue> serializedDictionary)
        {
            dictionaryItems = new List<SerializedDictionaryItem<TKey, TValue>>(serializedDictionary.dictionaryItems);
        }
    }
    
    [Serializable]
    public class SerializedDictionaryItem<TKey, TValue>
    {
        public TKey itemKey;
        public TValue itemValue;

        public SerializedDictionaryItem(TKey key, TValue value)
        {
            itemKey = key;
            itemValue = value;
        }
    }
}