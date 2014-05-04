namespace JavascriptConnector
{
	namespace API
	{
		/// <summary>
		/// Console class to interact with the Console
		/// </summary>
		public class Console
		{
			public Console ()
			{}
			/// <summary>
			/// Writes a line in the console
			/// </summary>
			/// <param name="message">Message</param>
			public static void WriteLine(string message)
			{
				System.Console.WriteLine(message);
			}
			/// <summary>
			/// Writes in the console
			/// </summary>
			/// <param name="message">Message</param>
			public static void Write(string message)
			{
				System.Console.Write(message);
			}
			/// <summary>
			/// Clears the console
			/// </summary>
			public static void Clear()
			{
				System.Console.Clear();
			}
			/// <summary>
			/// Plays a Beep sound
			/// </summary>
			public static void Beep()
			{
				System.Console.Beep();
			}
		}
		/// <summary>
		/// Var class can be used to save local variables
		/// </summary>
		public class Var
		{
			/// <summary>
			/// list of all variables
			/// </summary>
			private static System.Collections.Generic.Dictionary<string, object> vars;
			public object this[string key]
			{
				get
				{
					if(vars.ContainsKey(key))
						return vars[key];
					else
						return null;
				}
				set
				{
					if(!vars.ContainsKey(key))
						vars.Add(key, value);
					else
						vars[key] = value;
				}
			}
			public Var()
			{
				if(vars == null)
					vars = new System.Collections.Generic.Dictionary<string, object>();
			}
			/// <summary>
			/// gets the variable with specific key
			/// </summary>
			/// <param name="key">Key.</param>
			public object get(string key)
			{
				if(vars.ContainsKey(key))
					return vars[key];
				else
					return null;
			}
			/// <summary>
			/// sets the variable with specific key to specific value
			/// </summary>
			/// <param name="key">Key</param>
			/// <param name="value">Value</param>
			public void set(string key, object value)
			{
				if(!vars.ContainsKey(key))
					vars.Add(key, value);
				else
					vars[key] = value;
			}
		}
	}
}


