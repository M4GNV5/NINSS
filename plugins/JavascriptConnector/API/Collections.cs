using System;
namespace JavascriptConnector
{
	namespace API
	{
		/// <summary>
		/// Array class that can be used to store data in arrays
		/// </summary>
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
			/// <summary>
			/// Get the length of the array
			/// </summary>
			/// <value>The length of the array</value>
            public int Length
			{
				get
				{
					return array.Length;
				}
				set {}
			}
			public Array()
			{}
			/// <summary>
			/// Creates new array with specific length
			/// </summary>
			/// <param name="length">Length of the Array</param>
			public Array (int count)
			{
				array = new string[count];
			}
			/// <summary>
			/// Creates new array with specific length
			/// </summary>
			/// <returns>The new Array</returns>
			/// <param name="length">Length of the Array</param>
            public Array GetNew(int length)
			{
				return new Array(length);
			}
			/// <summary>
			/// Get the value at the specific index
			/// </summary>
			/// <param name="index">Index</param>
            public object Get(int index)
			{
				return array[index];
			}
			/// <summary>
			/// Sets the value at the specific index
			/// </summary>
			/// <param name="index">Index</param>
			/// <param name="value">Value</param>
			public void set(int index, object value)
			{
				array[index] = value;
			}
		}
		/// <summary>
		/// List class that can be used to store data in lists
		/// </summary>
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
			/// <summary>
			/// Get the count of the list
			/// </summary>
			/// <value>The count of the list</value>
			public int Count
			{
				get
				{
					return list.Count;
				}
				set
				{}
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="JavascriptConnector.API.List"/> class.
			/// </summary>
			public List()
			{
				list = new System.Collections.Generic.List<object>();
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="JavascriptConnector.API.List"/> class.
			/// </summary>
			/// <returns>The new List</returns>
            public List GetNew()
			{
				return new List();
			}
			/// <summary>
			/// Get the value at the specific index
			/// </summary>
			/// <param name="index">Index</param>
            public object Get(int index)
			{
				return list[index];
			}
			/// <summary>
			/// Set the value at the specific index
			/// </summary>
			/// <param name="index">Index</param>
			/// <param name="value">Value</param>
            public void Set(int index, object value)
			{
				list[index] = value;
			}
			/// <summary>
			/// Add a new value
			/// </summary>
			/// <param name="item">Item</param>
			public void Add(object item)
			{
				list.Add(item);
			}
			/// <summary>
			/// Remove an Item
			/// </summary>
			/// <param name="item">Item</param>
			public void Remove(object item)
			{
				list.Remove(item);
			}
			/// <summary>
			/// Remove an Item at index
			/// </summary>
			/// <param name="i">Index</param>
			public void RemoveAt(int i)
			{
				list.RemoveAt(i);
			}
			/// <summary>
			/// Clears the List
			/// </summary>
			public void Clear()
			{
				list.Clear();
			}
		}
		/// <summary>
		/// Dictionary class that can be used to store data in dictionaries
		/// </summary>
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
			/// <summary>
			/// Get the count of the dictionary
			/// </summary>
			/// <value>The count of the dictionary</value>
			public int Count
			{
				get
				{
					return dictionary.Count;
				}
				set
				{}
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="JavascriptConnector.API.Dictionary"/> class.
			/// </summary>
			public Dictionary()
			{
				dictionary = new System.Collections.Generic.Dictionary<object, object>();
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="JavascriptConnector.API.Dictionary"/> class.
			/// </summary>
            public Dictionary GetNew()
			{
				return new Dictionary();
			}
			/// <summary>
			/// Get the value with specific key
			/// </summary>
			/// <param name="key">Key</param>
            public object Get(object key)
			{
				return dictionary[key];
			}
			/// <summary>
			/// Set the value at the specific key
			/// </summary>
			/// <param name="key">Key</param>
			/// <param name="value">Value</param>
            public void Set(object key, object value)
			{
				dictionary[key] = value;
			}
			/// <summary>
			/// Adds a new value with specific key
			/// </summary>
			/// <param name="key">Key</param>
			/// <param name="value">Value</param>
			public void Add(object key, object value)
			{
				dictionary.Add(key, value);
			}
			/// <summary>
			/// Removes a value with specific key
			/// </summary>
			/// <param name="key">Key</param>
			public void Remove(object key)
			{
				dictionary.Remove(key);
			}
			/// <summary>
			/// Clears the dictionary
			/// </summary>
			public void Clear()
			{
				dictionary.Clear();
			}
		}
	}
}



