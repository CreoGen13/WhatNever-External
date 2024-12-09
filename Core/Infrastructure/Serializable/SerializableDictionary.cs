using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Core.Infrastructure.Serializable
{
    // ---------------------------------------------------------------- //
    //? Serialized Dictionary Base
    // ---------------------------------------------------------------- //

    /// <summary>
    /// Serialized Dictionary Base class. 
    /// </summary>
    public abstract class SerializableDictionaryBase
    {
        protected class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
        {
            public Dictionary() { }
            public Dictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
            public Dictionary(SerializationInfo serialInfo, StreamingContext streamingContext) : base(serialInfo, streamingContext) { }
        }

        public abstract class Cache { }
    }


    // ---------------------------------------------------------------- //
    //? Serialized Dictionary Base <TKey, TValue, TValueCache>
    // ---------------------------------------------------------------- //

    /// <summary>
    /// Serialized Dictionary Base class. 
    /// </summary>
    [Serializable]
    public abstract class SerializableDictionaryBase<TKey, TValue, TValueCache> : SerializableDictionaryBase, IDictionary<TKey, TValue>, IDictionary, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
    {
        [SerializeField] protected TKey[] keys;
        [SerializeField] protected TValueCache[] values;
        
        private Dictionary<TKey, TValue> _dictionary;

        // ---------------------------------------------------------------- //

        protected SerializableDictionaryBase()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        protected SerializableDictionaryBase(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        // ---------------------------------------------------------------- //

        public void OnAfterDeserialize()
        {
            if (keys != null && values != null && keys.Length == values.Length)
            {
                _dictionary.Clear();
                int length = keys.Length;
                for (int i = 0; i < length; ++i)
                {
                    _dictionary[keys[i]] = GetValue(values, i);
                }

                keys = null;
                values = null;
            }
        }

        public void OnBeforeSerialize()
        {
            int count = _dictionary.Count;
            keys = new TKey[count];
            values = new TValueCache[count];

            int i = 0;
            foreach (var kvp in _dictionary)
            {
                keys[i] = kvp.Key;
                SetValue(values, i, kvp.Value);
                ++i;
            }
        }

        // ---------------------------------------------------------------- //

        protected abstract void SetValue(TValueCache[] cache, int i, TValue value);
        protected abstract TValue GetValue(TValueCache[] cache, int i);

        // ---------------------------------------------------------------- //

        /// <summary>
        /// Replaces the values in this dictionary with values from a different dictionary. 
        /// </summary>
        /// <param name="dictionary">Dictionary reference to copy from.</param>
        public void CopyFrom(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary.Clear();
            foreach (var kvp in dictionary)
            {
                _dictionary[kvp.Key] = kvp.Value;
            }
        }

        // ---------------------------------------------------------------- //
        #region IDictionary<TKey, TValue>
        // ---------------------------------------------------------------- //

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)_dictionary).Keys;
        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_dictionary).Values;
        public int Count => ((IDictionary<TKey, TValue>)_dictionary).Count;
        public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dictionary).IsReadOnly;

        public TValue this[TKey key]
        {
            get => ((IDictionary<TKey, TValue>)_dictionary)[key];
            set => ((IDictionary<TKey, TValue>)_dictionary)[key] = value;
        }

        // ---------------------------------------------------------------- //

        /// <summary>
        /// Adds a new Key Value Pair to this dictionary. 
        /// </summary>
        /// <param name="key">New key to add.</param>
        /// <param name="value">New value to add.</param>
        public void Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)_dictionary).Add(key, value);
        }

        /// <summary>
        /// Check to see if the dictionary contains the key. 
        /// </summary>
        /// <param name="key">Key reference to check.</param>
        /// <returns> <c>true</c> if the dictionary contains the key.</returns>
        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).ContainsKey(key);
        }

        /// <summary>
        /// Removes a Key Value Pair from the dictionary, by key reference. 
        /// </summary>
        /// <param name="key">Key reference to remove.</param>
        /// <returns> <c>true</c> if the key was removed successfully.</returns>
        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Remove(key);
        }

        /// <summary>
        /// Attempts to retrieve a Value from the dictionary, by key reference. 
        /// </summary>
        /// <param name="key">Key reference.</param>
        /// <param name="value">Value reference to write out to.</param>
        /// <returns> <c>true</c> if the value was retrieved successfully.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds a new Key Value Pair to this dictionary. 
        /// </summary>
        /// <param name="item">New key value pair to add.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_dictionary).Add(item);
        }

        /// <summary>
        /// Removes all Key Value Pairs in this dictionary. 
        /// </summary>
        public void Clear()
        {
            ((IDictionary<TKey, TValue>)_dictionary).Clear();
        }

        /// <summary>
        /// Check to see if the dictionary contains the key value pair. 
        /// </summary>
        /// <param name="item">Key value pair reference to check.</param>
        /// <returns> <c>true</c> if the dictionary contains the key value pair.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        /// <summary>
        /// Appends a range of key value pairs into an index in the dictionary. 
        /// </summary>
        /// <param name="array">Key value pair references to append.</param>
        /// <param name="arrayIndex">Array index to append at.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes a Key Value Pair from the dictionary. 
        /// </summary>
        /// <param name="item">Key value pair to remove.</param>
        /// <returns> <c>true</c> if the key value pair was removed successfully.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();
        }

        // ---------------------------------------------------------------- //
        #endregion IDictionary<TKey, TValue>
        // ---------------------------------------------------------------- //


        // ---------------------------------------------------------------- //
        #region IDictionary
        // ---------------------------------------------------------------- //

        public bool IsFixedSize => ((IDictionary)_dictionary).IsFixedSize;
        ICollection IDictionary.Keys => ((IDictionary)_dictionary).Keys;
        ICollection IDictionary.Values => ((IDictionary)_dictionary).Values;
        public bool IsSynchronized => ((IDictionary)_dictionary).IsSynchronized;
        public object SyncRoot => ((IDictionary)_dictionary).SyncRoot;

        public object this[object key]
        {
            get => ((IDictionary)_dictionary)[key];
            set => ((IDictionary)_dictionary)[key] = value;
        }

        // ---------------------------------------------------------------- //

        public void Add(object key, object value)
        {
            ((IDictionary)_dictionary).Add(key, value);
        }

        public bool Contains(object key)
        {
            return ((IDictionary)_dictionary).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_dictionary).GetEnumerator();
        }

        // ---------------------------------------------------------------- //

        /// <summary>
        /// Removes a Key Value Pair from the dictionary. 
        /// </summary>
        /// <param name="key">Key reference to remove.</param>
        /// <returns> <c>true</c> if the key value pair was removed successfully.</returns>
        public void Remove(object key)
        {
            ((IDictionary)_dictionary).Remove(key);
        }

        /// <summary>
        /// Appends a range of key value pairs into an index in the dictionary. 
        /// </summary>
        /// <param name="array">Key value pair references to append.</param>
        /// <param name="index">Array index to append at.</param>
        public void CopyTo(Array array, int index)
        {
            ((IDictionary)_dictionary).CopyTo(array, index);
        }

        // ---------------------------------------------------------------- //
        #endregion IDictionary
        // ---------------------------------------------------------------- //

        // ---------------------------------------------------------------- //
        #region IDeserializationCallback
        // ---------------------------------------------------------------- //

        public void OnDeserialization(object sender)
        {
            ((IDeserializationCallback)_dictionary).OnDeserialization(sender);
        }

        // ---------------------------------------------------------------- //
        #endregion IDeserializationCallback
        // ---------------------------------------------------------------- //

        // ---------------------------------------------------------------- //
        #region ISerializable
        // ---------------------------------------------------------------- //

        protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context)
        {
            _dictionary = new Dictionary<TKey, TValue>(info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_dictionary).GetObjectData(info, context);
        }

        // ---------------------------------------------------------------- //
        #endregion ISerializable
        // ---------------------------------------------------------------- //
    }

    // ---------------------------------------------------------------- //
    //? Serializable Dictionary
    // ---------------------------------------------------------------- //
    public static class SerializableDictionary
    {
        public class Cache<T> : SerializableDictionaryBase.Cache
        {
            public T data;
        }
    }

    // ---------------------------------------------------------------- //
    //? Serializable Dictionary <TKey, TValue>
    // ---------------------------------------------------------------- //

    /// <summary>
    /// Serialized Dictionary Class. Allows a dictionary-like data structure with key-value pairs that can be serialized and edited in the Unity editor. 
    /// It addresses the limitation of Unity's default Dictionary class, which cannot be directly serialized. 
    /// <see cref="ISerializationCallbackReciever"/> and <see cref="Dictionary"/>.
    ///
    /// <code>
    /// // Example usage in a MonoBehaviour script
    /// using UnityEngine;
    /// using Sherbert.Framework.Generic;
    ///
    /// public class DictionaryExample : MonoBehaviour
    /// {
    ///     [SerializeField] private SerializableDictionary<string, int> myDictionary = new();
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
    {
        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict) { }
        protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        // ---------------------------------------------------------------- //

        protected override TValue GetValue(TValue[] values, int i)
        {
            return values[i];
        }

        protected override void SetValue(TValue[] values, int i, TValue value)
        {
            values[i] = value;
        }

        // ---------------------------------------------------------------- //
    }

    // ---------------------------------------------------------------- //
    //? Serializable Dictionary <TKey, TValue, TValueCache>
    // ---------------------------------------------------------------- //
    
    /// <summary>
    /// Serialized Dictionary Class. Allows a dictionary-like data structure with key-value pairs that can be serialized and edited in the Unity editor. 
    /// It addresses the limitation of Unity's default Dictionary class, which cannot be directly serialized. 
    /// <see cref="ISerializationCallbackReciever"/> and <see cref="Dictionary"/>.
    ///
    /// <code>
    /// // Example usage in a MonoBehaviour script
    /// using UnityEngine;
    /// using Sherbert.Framework.Generic;
    ///
    /// public class DictionaryExample : MonoBehaviour
    /// {
    ///     [SerializeField] private SerializableDictionary<string, int> myDictionary = new();
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue, TValueCache> : SerializableDictionaryBase<TKey, TValue, TValueCache> where TValueCache : SerializableDictionary.Cache<TValue>, new()
    {
        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        protected SerializableDictionary(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        // ---------------------------------------------------------------- //

        protected override TValue GetValue(TValueCache[] cache, int i)
        {
            return cache[i].data;
        }

        protected override void SetValue(TValueCache[] cache, int i, TValue value)
        {
            cache[i] = new TValueCache
            {
                data = value
            };
        }

        // ---------------------------------------------------------------- //
    }
}