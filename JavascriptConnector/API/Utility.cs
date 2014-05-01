namespace JavascriptConnector
{
	namespace API
	{
		public class Console
		{
			public Console ()
			{}
			public static void WriteLine(string message)
			{
				System.Console.WriteLine(message);
			}
			public static void Write(string message)
			{
				System.Console.Write(message);
			}
			public static void Clear()
			{
				System.Console.Clear();
			}
			public static void Beep()
			{
				System.Console.Beep();
			}
		}
		public class Var
		{
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
			public object get(string key)
			{
				if(vars.ContainsKey(key))
					return vars[key];
				else
					return null;
			}
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

