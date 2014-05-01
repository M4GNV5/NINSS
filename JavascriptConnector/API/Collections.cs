using System;
namespace JavascriptConnector
{
	namespace API
	{
		public class Array
		{
			private object[] array;
			public object this[int index]
			{
				get
				{
					return array[index];
				}
				set
				{
					array[index] = value;
				}
			}
			public int length
			{
				get
				{
					return array.Length;
				}
				set {}
			}
			public Array()
			{}
			public Array (int count)
			{
				array = new string[count];
			}
			public Array getNew(int length)
			{
				return new Array(length);
			}
			public object get(int index)
			{
				return array[index];
			}
			public void set(int index, object value)
			{
				array[index] = value;
			}
		}
		public class List
		{
			private System.Collections.Generic.List<object> list;
			public object this[int index]
			{
				get
				{
					return list[index];
				}
				set
				{
					list[index] = value;
				}
			}
			public int Count
			{
				get
				{
					return list.Count;
				}
				set
				{}
			}
			public List()
			{
				list = new System.Collections.Generic.List<object>();
			}
			public List getNew()
			{
				return new List();
			}
			public object get(int index)
			{
				return list[index];
			}
			public void set(int index, object value)
			{
				list[index] = value;
			}
			public void Add(string item)
			{
				list.Add(item);
			}
			public void Remove(string item)
			{
				list.Remove(item);
			}
			public void RemoveAt(int i)
			{
				list.RemoveAt(i);
			}
			public void Clear()
			{
				list.Clear();
			}
		}
		public class Dictionary
		{
			private System.Collections.Generic.Dictionary<object, object> dictionary;
			public object this[int index]
			{
				get
				{
					return dictionary[index];
				}
				set
				{
					dictionary[index] = value;
				}
			}
			public int Count
			{
				get
				{
					return dictionary.Count;
				}
				set
				{}
			}
			public Dictionary()
			{
				dictionary = new System.Collections.Generic.Dictionary<object, object>();
			}
			public Dictionary getNew()
			{
				return new Dictionary();
			}
			public object get(object key)
			{
				return dictionary[key];
			}
			public void set(object key, object value)
			{
				dictionary[key] = value;
			}
			public void Add(object key, object value)
			{
				dictionary.Add(key, value);
			}
			public void Remove(object key)
			{
				dictionary.Remove(key);
			}
			public void Clear()
			{
				dictionary.Clear();
			}
		}
	}
}

