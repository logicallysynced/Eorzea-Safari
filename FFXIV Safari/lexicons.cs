using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Lexicons
{
    public interface ILexicon<K,V> : ICollection<KeyValuePair<K,V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
    {
        // Properties

        int KeyCount { get; }
        ICollection<K> Keys { get; }
        ICollection<List<V>> ValueLists { get; }
        IEnumerable<V> Values { get; } 
        List<V> this[K key] { get; set; }

        // Methods
        
        void Add(K key, V value);
        void AddList(K key, List<V> valueList);
        bool ChangeValue(K key, V oldvalue, V newValue);
        bool Contains (K key, V value); 
        bool ContainsKey(K key);       
        int GetValueCount(K key);
        bool Remove(K key, V value);
        bool RemoveKey(K key);
        bool TryGetValueList(K key, out List<V> valueList);        
    }

   
    /* ........................................................................................... */
       

    
    [Serializable]
    [ComVisible(false)]
    public class Lexicon<K, V> : IEnumerable<KeyValuePair<K, V>>, ICollection<KeyValuePair<K, V>>, ILexicon<K,V>, IEnumerable, ICollection, ISerializable, IDeserializationCallback
    {
        // Fields

        private Dictionary<K,List<V>> dict;

        // Properties

        public IEqualityComparer<K> Comparer 
        { 
            get { return dict.Comparer; }                
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (List<V> list in dict.Values)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public int KeyCount
        {
           
            get { return dict.Keys.Count; }

        }
           
        public ICollection<K> Keys
        {
            get { return dict.Keys; }
        }

        public ICollection<List<V>> ValueLists
        {
            get { return dict.Values; }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (K key in dict.Keys)
                {
                    foreach (V value in dict[key])
                    {
                        yield return value;
                    }
                }
            }
        } 

        public List<V> this[K key]
        {
            get { return dict[key]; }
            set
            {  
               if (value == null || value.Count == 0) throw new ArgumentException("List cannot be null or empty");
               dict[key] = new List<V>(value); 
            }
        }
             

        public V this[K key, int index]
        {
            get
            {
                List<V> list = dict[key];
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentException("Index out of range for key");
                }
                return list[index];
            }

            set
            {
                if (dict.ContainsKey(key))
                {                                    
                    List<V> list = dict[key];
                    
                    if (index < 0 || index > list.Count)
                    {
                        throw new ArgumentException("Index out of range for key");
                    }                    
                    else if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                    }
                }
                else if (index == 0)
                {
                    List<V> list = new List<V>();
                    list.Add(value);
                    dict.Add(key, list);
                }
                else
                {
                    throw new ArgumentException("Index out of range for key");
                }
            }
        }
       
        // Constructors

        public Lexicon()
        {
            dict = new Dictionary<K, List<V>>();
        }

        public Lexicon(IDictionary<K, V> dictionary)
        {
            dict = new Dictionary<K, List<V>>();
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public Lexicon(ILexicon<K, V> lexicon)
        {
            dict = new Dictionary<K, List<V>>();
            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public Lexicon(IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
        }

        public Lexicon(int capacity)
        {
            dict = new Dictionary<K, List<V>>(capacity);
        }

        public Lexicon(IDictionary<K, V> dictionary, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public Lexicon(ILexicon<K, V> lexicon, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public Lexicon(int capacity, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(capacity, comparer);
        }

        protected Lexicon(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;
            dict = (Dictionary<K, List<V>>)info.GetValue("InternalDictionary", typeof(Dictionary<K, List<V>>));
        }

        // Methods

        public void Add(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                list.Add(value);
            }
            else
            {
                List<V> list = new List<V>();
                list.Add(value);
                dict.Add(key, list);
            }
        }

        public void Add(KeyValuePair<K, V> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddList(K key, List<V> valueList)
        {
            if(dict.ContainsKey(key))
            {
               if (valueList == null || valueList.Count == 0) return;
               List<V> list = dict[key];
               foreach(V val in valueList) list.Add(val);       
            }
            else
            { 
               if (valueList == null || valueList.Count == 0) throw new ArgumentException("List cannot be null or empty");   
               dict.Add(key, new List<V>(valueList));
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<K, V>> keyValuePairs)
        {
            foreach (KeyValuePair<K, V> kvp in keyValuePairs) this.Add(kvp.Key, kvp.Value);
        }

        public int AnalyzeKeys(out int multiple, out int single, out int empty)
        {
            multiple = single = empty = 0;
            int count;

            foreach (K key in dict.Keys)
            {
               count = dict[key].Count;

               switch(count)
               {
                  case 0 :
                    empty++;
                    break;

                  case 1 :
                    single++;
                    break;

                  default :
                    multiple++;
                    break; 
               }
            } 
            
            return multiple + single + empty;
        }

        public bool ChangeValue(K key, V oldValue, V newValue)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], oldValue))
                  {
                     list[i] = newValue;
                     return true;
                  }
               }
            }
            return false;
        }

        public bool ChangeValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index >= list.Count)
               {
                  return false;
               }                    
               else
               {
                  list[index] = newValue;
                  return true;
               }
           }
           return false;
        }  

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                foreach (V val in list)
                {
                    if (Object.Equals(val, value)) return true;
                }
            }
            return false;
        }

        public bool Contains(KeyValuePair<K, V> keyValuePair)
        {
            return this.Contains(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            K firstKey;
            return ContainsValue(value, out firstKey);
        }

        public bool ContainsValue(V value, out K firstKey)
        {
            foreach (K key in dict.Keys)
            {
                foreach (V val in dict[key])
                {
                    if (Object.Equals(val, value))
                    {
                        firstKey = key;       
                        return true;
                    }  
                }
            }
            firstKey = default(K); 
            return false;
        }
      
        public void CopyTo(KeyValuePair<K, V>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("Array cannot be null");
            if (index < 0) throw new ArgumentOutOfRangeException("Index cannot be negative");
            if (array.Length < index + this.Count) throw new ArgumentException("Index is invalid");
            int i = index;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                array[i++] = kvp;
            }
        }

        public IEnumerable<KeyValuePair<K, int>> FindKeyIndexPairs(V value)
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    if (Object.Equals(list[i], value))
                    {
                       yield return new KeyValuePair<K, int>(key, i);
                    }
                }
            }
        }
               
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                foreach (V value in list)
                {
                    yield return new KeyValuePair<K, V>(key, value);
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;  
            info.AddValue("InternalDictionary", dict);
        }

        public int GetValueCount(K key)
        {
            if (dict.ContainsKey(key)) return dict[key].Count;
            return -1;
        }

        public int IndexOfValue(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], value)) return i;
               }
            }
            return -1;
        }

        public bool InsertValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index > list.Count)
               {
                  return false;
               }                    
               else
               {
                  list.Insert(index, newValue);
                  return true; 
               }
           }
           return false;
        }

        public int MoveSingleKeys(IDictionary<K, V> destination)
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {
              List<V> list = dict[key];
              if (list.Count == 1 && !destination.ContainsKey(key))
              {
                 destination.Add(key, list[0]);
                 removals.Add(key);
              }
           }

           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count; 
        } 
      
        public virtual void OnDeserialization(object sender)
        {
            // nothing to do
        }

        public bool Remove(K key, V value)
        {
            int count = this.GetValueCount(key);
            if (count <= 0) return false;
            for (int i = 0; i < count; i++)
            {
                V val = dict[key][i];
                if (Object.Equals(val, value))
                {
                    if (count == 1)
                    {
                       dict.Remove(key);
                    }
                    else  
                    { 
                       dict[key].RemoveAt(i);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, V> keyValuePair)
        {
            return this.Remove(keyValuePair.Key, keyValuePair.Value);
        }

        public bool RemoveAt(K key, int index)
        {
            int count = this.GetValueCount(key);
            if (count <= 0 || index < 0 || index >= count) return false;
            if (count == 1)
            {
                dict.Remove(key);
            }
            else
            {
                List<V> list = dict[key];
                list.RemoveAt(index);               
            }
            return true;
        }

        public int RemoveEmptyKeys()
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {              
              if(dict[key].Count == 0) removals.Add(key);
           }
  
           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count;           
        }

        public bool RemoveKey(K key)
        {
            int count = this.GetValueCount(key);
            if (count >= 0)
            {
                dict.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryGetValueList(K key, out List<V> valueList)
        {
            return dict.TryGetValue(key, out valueList);
        }
               
        public bool TryGetValueAt(K key, int index, out V value)
        {
            if (dict.ContainsKey(key) && index >= 0 && index < dict[key].Count)
            {
                value = dict[key][index];
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        // Explicit Property Implementations

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly 
        { 
            get { return false; } 
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)dict).SyncRoot; }
        }

        // Explicit Method Implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((KeyValuePair<K, V>[]) array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    /* ........................................................................................... */


    [Serializable]
    [ComVisible(false)]
    public class OrderedLexicon<K, V> : IEnumerable<KeyValuePair<K, V>>, ICollection<KeyValuePair<K, V>>, ILexicon<K,V>, IEnumerable, ICollection
    {
        // Fields

        private SortedDictionary<K,List<V>> dict;

        // Properties

        public IComparer<K> Comparer 
        { 
            get { return dict.Comparer; }                
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (List<V> list in dict.Values)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public int KeyCount
        {
           
            get { return dict.Keys.Count; }

        }
           
        public ICollection<K> Keys
        {
            get { return dict.Keys; }
        }

        public ICollection<List<V>> ValueLists
        {
            get { return dict.Values; }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (K key in dict.Keys)
                {
                    foreach (V value in dict[key])
                    {
                        yield return value;
                    }
                }
            }
        } 

        public List<V> this[K key]
        {
            get { return dict[key]; }
            set
            {  
               if (value == null || value.Count == 0) throw new ArgumentException("List cannot be null or empty");
               dict[key] = new List<V>(value); 
            }
        }
             

        public V this[K key, int index]
        {
            get
            {
                List<V> list = dict[key];
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentException("Index out of range for key");
                }
                return list[index];
            }

            set
            {
                if (dict.ContainsKey(key))
                {                                    
                    List<V> list = dict[key];
                    
                    if (index < 0 || index > list.Count)
                    {
                        throw new ArgumentException("Index out of range for key");
                    }                    
                    else if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                    }
                }
                else if (index == 0)
                {
                    List<V> list = new List<V>();
                    list.Add(value);
                    dict.Add(key, list);
                }
                else
                {
                    throw new ArgumentException("Index out of range for key");
                }
            }
        }
       
        // Constructors

        public OrderedLexicon()
        {
            dict = new SortedDictionary<K, List<V>>();
        }

        public OrderedLexicon(IDictionary<K, V> dictionary)
        {
            dict = new SortedDictionary<K, List<V>>();
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public OrderedLexicon(ILexicon<K, V> lexicon)
        {
            dict = new SortedDictionary<K, List<V>>();
            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public OrderedLexicon(IComparer<K> comparer)
        {
            dict = new SortedDictionary<K, List<V>>(comparer);
        }

        public OrderedLexicon(IDictionary<K, V> dictionary, IComparer<K> comparer)
        {
            dict = new SortedDictionary<K, List<V>>(comparer);
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public OrderedLexicon(ILexicon<K, V> lexicon, IComparer<K> comparer)
        {
            dict = new SortedDictionary<K, List<V>>(comparer);
            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }
              
        // Methods

        public void Add(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                list.Add(value);
            }
            else
            {
                List<V> list = new List<V>();
                list.Add(value);
                dict.Add(key, list);
            }
        }

        public void Add(KeyValuePair<K, V> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddList(K key, List<V> valueList)
        {
            if(dict.ContainsKey(key))
            {
               if (valueList == null || valueList.Count == 0) return;
               List<V> list = dict[key];
               foreach(V val in valueList) list.Add(val);       
            }
            else
            { 
               if (valueList == null || valueList.Count == 0) throw new ArgumentException("List cannot be null or empty");   
               dict.Add(key, new List<V>(valueList));
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<K, V>> keyValuePairs)
        {
            foreach (KeyValuePair<K, V> kvp in keyValuePairs) this.Add(kvp.Key, kvp.Value);
        }

        public int AnalyzeKeys(out int multiple, out int single, out int empty)
        {
            multiple = single = empty = 0;
            int count;

            foreach (K key in dict.Keys)
            {
               count = dict[key].Count;

               switch(count)
               {
                  case 0 :
                    empty++;
                    break;

                  case 1 :
                    single++;
                    break;

                  default :
                    multiple++;
                    break; 
               }
            } 
            
            return multiple + single + empty;
        }

        public bool ChangeValue(K key, V oldValue, V newValue)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], oldValue))
                  {
                     list[i] = newValue;
                     return true;
                  }
               }
            }
            return false;
        }

        public bool ChangeValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index >= list.Count)
               {
                  return false;
               }                    
               else
               {
                  list[index] = newValue;
                  return true;
               }
           }
           return false;
        }  

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                foreach (V val in list)
                {
                    if (Object.Equals(val, value)) return true;
                }
            }
            return false;
        }

        public bool Contains(KeyValuePair<K, V> keyValuePair)
        {
            return this.Contains(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            K firstKey;
            return ContainsValue(value, out firstKey);
        }

        public bool ContainsValue(V value, out K firstKey)
        {
            foreach (K key in dict.Keys)
            {
                foreach (V val in dict[key])
                {
                    if (Object.Equals(val, value))
                    {
                        firstKey = key;       
                        return true;
                    }  
                }
            }
            firstKey = default(K); 
            return false;
        }
      
        public void CopyTo(KeyValuePair<K, V>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("Array cannot be null");
            if (index < 0) throw new ArgumentOutOfRangeException("Index cannot be negative");
            if (array.Length < index + this.Count) throw new ArgumentException("Index is invalid");
            int i = index;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                array[i++] = kvp;
            }
        }

        public IEnumerable<KeyValuePair<K, int>> FindKeyIndexPairs(V value)
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    if (Object.Equals(list[i], value))
                    {
                       yield return new KeyValuePair<K, int>(key, i);
                    }
                }
            }
        }
               
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                foreach (V value in list)
                {
                    yield return new KeyValuePair<K, V>(key, value);
                }
            }
        }

        public int GetValueCount(K key)
        {
            if (dict.ContainsKey(key)) return dict[key].Count;
            return -1;
        }

        public int IndexOfValue(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], value)) return i;
               }
            }
            return -1;
        }

        public bool InsertValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index > list.Count)
               {
                  return false;
               }                    
               else
               {
                  list.Insert(index, newValue);
                  return true; 
               }
           }
           return false;
        }

        public int MoveSingleKeys(IDictionary<K, V> destination)
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {
              List<V> list = dict[key];
              if (list.Count == 1 && !destination.ContainsKey(key))
              {
                 destination.Add(key, list[0]);
                 removals.Add(key);
              }
           }

           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count; 
        } 
      
        public bool Remove(K key, V value)
        {
            int count = this.GetValueCount(key);
            if (count <= 0) return false;
            for (int i = 0; i < count; i++)
            {
                V val = dict[key][i];
                if (Object.Equals(val, value))
                {
                    if (count == 1)
                    {
                       dict.Remove(key);
                    }
                    else  
                    { 
                       dict[key].RemoveAt(i);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, V> keyValuePair)
        {
            return this.Remove(keyValuePair.Key, keyValuePair.Value);
        }

        public bool RemoveAt(K key, int index)
        {
            int count = this.GetValueCount(key);
            if (count <= 0 || index < 0 || index >= count) return false;
            if (count == 1)
            {
                dict.Remove(key);
            }
            else
            {
                List<V> list = dict[key];
                list.RemoveAt(index);               
            }
            return true;
        }

        public int RemoveEmptyKeys()
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {              
              if(dict[key].Count == 0) removals.Add(key);
           }
  
           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count;           
        }

        public bool RemoveKey(K key)
        {
            int count = this.GetValueCount(key);
            if (count >= 0)
            {
                dict.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryGetValueList(K key, out List<V> valueList)
        {
            return dict.TryGetValue(key, out valueList);
        }
               
        public bool TryGetValueAt(K key, int index, out V value)
        {
            if (dict.ContainsKey(key) && index >= 0 && index < dict[key].Count)
            {
                value = dict[key][index];
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        // Explicit Property Implementations

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly 
        { 
            get { return false; } 
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)dict).SyncRoot; }
        }

        // Explicit Method Implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((KeyValuePair<K, V>[]) array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

  
    /* ........................................................................................... */


    [Serializable]
    [ComVisible(false)]
    public class SortedLexicon<K, V> : IEnumerable<KeyValuePair<K, V>>, ICollection<KeyValuePair<K, V>>, ILexicon<K,V>, IEnumerable, ICollection
    {
        // Fields

        private SortedDictionary<K,List<V>> dict;
        private IComparer<V> valueComparer;

        // Properties

        public IComparer<K> KeyComparer 
        { 
            get { return dict.Comparer; }                
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (List<V> list in dict.Values)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public int KeyCount
        {
           
            get { return dict.Keys.Count; }

        }
           
        public ICollection<K> Keys
        {
            get { return dict.Keys; }
        }

        public IComparer<V> ValueComparer
        {
            get { return this.valueComparer; }
        }

        public ICollection<List<V>> ValueLists
        {
            get { return dict.Values; }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (K key in dict.Keys)
                {
                    foreach (V value in dict[key])
                    {
                        yield return value;
                    }
                }
            }
        } 

        public List<V> this[K key]
        {
            get { return dict[key]; }
            set
            {  
               if (value == null || value.Count == 0) throw new ArgumentException("List cannot be null or empty");
               List<V> list = new List<V>(value);
               if (list.Count > 1) list.Sort(valueComparer); 
               dict[key] = list;
            }
        }
             

        public V this[K key, int index]
        {
            get
            {
                List<V> list = dict[key];
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentException("Index out of range for key");
                }
                return list[index];
            }

            set
            {
                if (dict.ContainsKey(key))
                {                                    
                    List<V> list = dict[key];
                    
                    if (index < 0 || index > list.Count)
                    {
                        throw new ArgumentException("Index out of range for key");
                    }                    
                    else if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                    }

                    if (list.Count > 1) list.Sort(valueComparer);
                }
                else if (index == 0)
                {
                    List<V> list = new List<V>();
                    list.Add(value);
                    dict.Add(key, list);
                }
                else
                {
                    throw new ArgumentException("Index out of range for key");
                }
            }
        }
       
        // Constructors

        public SortedLexicon()
        {
            dict = new SortedDictionary<K, List<V>>();
        }

        public SortedLexicon(IDictionary<K, V> dictionary)
        {
            dict = new SortedDictionary<K, List<V>>();
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public SortedLexicon(ILexicon<K, V> lexicon)
        {
            dict = new SortedDictionary<K, List<V>>();
            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                List<V> list = new List<V>(lexicon[key]);
                if (list.Count > 1) list.Sort(valueComparer);
                dict.Add(key, list);
            }
        }

        public SortedLexicon(IComparer<K> keyComparer, IComparer<V> valueComparer)
        {
            dict = new SortedDictionary<K, List<V>>(keyComparer);
            this.valueComparer = valueComparer;
        }

        public SortedLexicon(IDictionary<K, V> dictionary, IComparer<K> keyComparer, IComparer<V> valueComparer)
        {
            dict = new SortedDictionary<K, List<V>>(keyComparer);
            this.valueComparer = valueComparer;

            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public SortedLexicon(ILexicon<K, V> lexicon, IComparer<K> keyComparer, IComparer<V> valueComparer)
        {
            dict = new SortedDictionary<K, List<V>>(keyComparer);
            this.valueComparer = valueComparer;

            foreach (K key in lexicon.Keys)
            {
                if(lexicon[key].Count == 0) continue;
                List<V> list = new List<V>(lexicon[key]);
                if (list.Count > 1) list.Sort(valueComparer);
                dict.Add(key, list);
            }
        }
              
        // Methods

        public void Add(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                list.Add(value);
                if (list.Count > 1) list.Sort(valueComparer);
            }
            else
            {
                List<V> list = new List<V>();
                list.Add(value);
                dict.Add(key, list);
            }
        }

        public void Add(KeyValuePair<K, V> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddList(K key, List<V> valueList)
        {
            if(dict.ContainsKey(key))
            {
               if (valueList == null || valueList.Count == 0) return;
               List<V> list = dict[key];
               foreach(V val in valueList) list.Add(val);
               if (list.Count > 1) list.Sort(valueComparer);       
            }
            else
            { 
               if (valueList == null || valueList.Count == 0) throw new ArgumentException("List cannot be null or empty");   
               List<V> list = new List<V>(valueList);
               if (list.Count > 1) list.Sort(valueComparer);  
               dict.Add(key, list);
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<K, V>> keyValuePairs)
        {
            foreach (KeyValuePair<K, V> kvp in keyValuePairs) this.Add(kvp.Key, kvp.Value);
        }

        public int AnalyzeKeys(out int multiple, out int single, out int empty)
        {
            multiple = single = empty = 0;
            int count;

            foreach (K key in dict.Keys)
            {
               count = dict[key].Count;

               switch(count)
               {
                  case 0 :
                    empty++;
                    break;

                  case 1 :
                    single++;
                    break;

                  default :
                    multiple++;
                    break; 
               }
            } 
            
            return multiple + single + empty;
        }

        public bool ChangeValue(K key, V oldValue, V newValue)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], oldValue))
                  {
                     list[i] = newValue;
                     if (list.Count > 1) list.Sort(valueComparer);
                     return true;
                  }
               }
            }
            return false;
        }

        public bool ChangeValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index >= list.Count)
               {
                  return false;
               }                    
               else
               {
                  list[index] = newValue;
                  if (list.Count > 1) list.Sort(valueComparer);
                  return true;
               }
           }
           return false;
        }  

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                foreach (V val in list)
                {
                    if (Object.Equals(val, value)) return true;
                }
            }
            return false;
        }

        public bool Contains(KeyValuePair<K, V> keyValuePair)
        {
            return this.Contains(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            K firstKey;
            return ContainsValue(value, out firstKey);
        }

        public bool ContainsValue(V value, out K firstKey)
        {
            foreach (K key in dict.Keys)
            {
                foreach (V val in dict[key])
                {
                    if (Object.Equals(val, value))
                    {
                        firstKey = key;       
                        return true;
                    }  
                }
            }
            firstKey = default(K); 
            return false;
        }
      
        public void CopyTo(KeyValuePair<K, V>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("Array cannot be null");
            if (index < 0) throw new ArgumentOutOfRangeException("Index cannot be negative");
            if (array.Length < index + this.Count) throw new ArgumentException("Index is invalid");
            int i = index;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                array[i++] = kvp;
            }
        }

        public IEnumerable<KeyValuePair<K, int>> FindKeyIndexPairs(V value)
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    if (Object.Equals(list[i], value))
                    {
                       yield return new KeyValuePair<K, int>(key, i);
                    }
                }
            }
        }
               
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                foreach (V value in list)
                {
                    yield return new KeyValuePair<K, V>(key, value);
                }
            }
        }

        public int GetValueCount(K key)
        {
            if (dict.ContainsKey(key)) return dict[key].Count;
            return -1;
        }

        public int IndexOfValue(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], value)) return i;
               }
            }
            return -1;
        }

        public bool InsertValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index > list.Count)
               {
                  return false;
               }                    
               else
               {
                  list.Insert(index, newValue);
                  if (list.Count > 1) list.Sort(valueComparer);
                  return true; 
               }
           }
           return false;
        }

        public int MoveSingleKeys(IDictionary<K, V> destination)
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {
              List<V> list = dict[key];
              if (list.Count == 1 && !destination.ContainsKey(key))
              {
                 destination.Add(key, list[0]);
                 removals.Add(key);
              }
           }

           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count; 
        } 
      
        public bool Remove(K key, V value)
        {
            int count = this.GetValueCount(key);
            if (count <= 0) return false;
            for (int i = 0; i < count; i++)
            {
                V val = dict[key][i];
                if (Object.Equals(val, value))
                {
                    if (count == 1)
                    {
                       dict.Remove(key);
                    }
                    else  
                    { 
                       List<V> list = dict[key];
                       list.RemoveAt(i);
                       if (list.Count > 1) list.Sort(valueComparer);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, V> keyValuePair)
        {
            return this.Remove(keyValuePair.Key, keyValuePair.Value);
        }

        public bool RemoveAt(K key, int index)
        {
            int count = this.GetValueCount(key);
            if (count <= 0 || index < 0 || index >= count) return false;
            if (count == 1)
            {
                dict.Remove(key);
            }
            else
            {
                List<V> list = dict[key];
                list.RemoveAt(index);
                if (list.Count > 1) list.Sort(valueComparer);               
            }
            return true;
        }

        public int RemoveEmptyKeys()
        {
           List<K> removals = new List<K>();

           foreach(K key in dict.Keys)
           {              
              if(dict[key].Count == 0) removals.Add(key);
           }
  
           for(int i = 0; i < removals.Count; i++)
           {
              dict.Remove(removals[i]);
           }

           return removals.Count;           
        }

        public bool RemoveKey(K key)
        {
            int count = this.GetValueCount(key);
            if (count >= 0)
            {
                dict.Remove(key);
                return true;
            }
            return false;
        }

        public bool SortValueList(K key)
        {
            if (dict.ContainsKey(key))
            {                                           
               List<V> list = dict[key];
               if (list.Count > 1) list.Sort(valueComparer);
               return true; 
            }
            return false;         
        }

        public bool TryGetValueList(K key, out List<V> valueList)
        {
            return dict.TryGetValue(key, out valueList);
        }
               
        public bool TryGetValueAt(K key, int index, out V value)
        {
            if (dict.ContainsKey(key) && index >= 0 && index < dict[key].Count)
            {
                value = dict[key][index];
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        // Explicit Property Implementations

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly 
        { 
            get { return false; } 
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)dict).SyncRoot; }
        }

        // Explicit Method Implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((KeyValuePair<K, V>[]) array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    /* ........................................................................................... */


    public static class LexiconExtensions
    {

        public static Lexicon<K, S> ToLexicon<S, K>(this IEnumerable<S> source, Func<S, K> keySelector)
        {
           return source.ToLexicon<S, K, S>(keySelector, s => s, null);
        }
        
        public static Lexicon<K, S> ToLexicon<S, K>(this IEnumerable<S> source, Func<S, K> keySelector, IEqualityComparer<K> comparer)
        {
           return source.ToLexicon<S, K, S>(keySelector, s => s, comparer);
        }

        public static Lexicon<K, V> ToLexicon<S, K, V>(this IEnumerable<S> source, Func<S, K> keySelector, Func<S, V> valueSelector)
        {
           return source.ToLexicon<S, K, V>(keySelector, valueSelector, null);
        }

        public static Lexicon<K, V> ToLexicon<S, K, V>(this IEnumerable<S> source, Func<S, K> keySelector, Func<S, V> valueSelector, IEqualityComparer<K> comparer)
        {
           if (source == null)
           {
              throw new ArgumentException("Source cannot be null");
           }

           if (keySelector == null)
           {
              throw new ArgumentException("Key selector cannot be null");
           }

           if (valueSelector == null)
           {
              throw new ArgumentException("Value selector cannot be null");
           }

           Lexicon<K, V> lexicon = new Lexicon<K, V>(comparer);

           foreach (S s in source)
           {
              lexicon.Add(keySelector(s), valueSelector(s));
           }

           return lexicon;
        }

    }


    /* ........................................................................................... */

    class City
    {
       private string name;
       private string country;

       public string Name { get { return name; } }
       public string Country { get { return country; } }
  
       public City(string name, string country)
       {
          this.name = name;
          this.country = country;
       }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine ("Unordered\n");
            Lexicon<string, int> lex = new Lexicon<string, int>();
            lex.Add("Dave", 6);
            lex.Add("John", 1);
            lex.Add("Dave", 3);
            lex.Add("Stan", 4);
            lex.Add("Dave", 2);
            lex.Add("Fred", 5);

            foreach (var kvp in lex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine ("\nOrdered by key\n");

            OrderedLexicon<string, int> olex = new OrderedLexicon<string, int>(lex);
            foreach (var kvp in olex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine("\nOrdered by key, then by value\n");

            SortedLexicon<string, int> slex = new SortedLexicon<string, int>(lex);
            foreach (var kvp in slex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine();
           
            // create an empty key
            slex["John"].Clear();

            // mess up the order of another key's values
            slex["Dave"].Insert(0, 7);
        
            // analyze keys
            int m, s, e;
            int t = slex.AnalyzeKeys(out m, out s, out e);
            Console.WriteLine("\nMultiple {0}, Single {1}, Empty {2}\n", m, s, e); 
            
            // remove empty keys
            slex.RemoveEmptyKeys();

            // transfer single keys to a Dictionary
            Dictionary<string, int> dict = new Dictionary<string, int>();
            slex.MoveSingleKeys(dict);

            Console.WriteLine("\nOrdered by key, then by value, after some removals\n");

            foreach (var kvp in slex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            // reorder "Dave"
            slex.SortValueList("Dave");

            Console.WriteLine("\nOrdered by key, then by value, after some removals and resorting\n");

            foreach (var kvp in slex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            // create a list of City objects
            List<City> cities = new List<City>();
            cities.Add(new City("Delhi", "India"));
            cities.Add(new City("Madrid", "Spain"));
            cities.Add(new City("New York", "U.S.A"));
            cities.Add(new City("Mumbai", "India"));

            // create a new lexicon from the list of cities
            Lexicon<string, string> lex2 = cities.ToLexicon(c => c.Country, c => c.Name);

            Console.WriteLine("\nNew lexicon created from list of cities\n");

            foreach (var kvp in lex2)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            Console.ReadKey();
        }
    }
}
