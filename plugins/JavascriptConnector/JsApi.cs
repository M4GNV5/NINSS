using System;

namespace JavascriptConnector.API
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
}

