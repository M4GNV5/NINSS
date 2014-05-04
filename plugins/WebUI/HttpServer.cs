using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace WebUI
{
	//modified code from http://www.codeproject.com/Articles/137979/Simple-HTTP-Server-in-C
	public class HttpProcessor
	{
		public TcpClient socket;        
		public HttpServer srv;
		
		private Stream inputStream;
		public StreamWriter outputStream;
		
		public string http_method;
		public string http_url;
		public string http_protocol_versionstring;
		
		public HttpProcessor(TcpClient s, HttpServer srv)
		{
			this.socket = s;
			this.srv = srv;                   
		}
		
		
		private string streamReadLine(Stream inputStream)
		{
			int next_char;
			string data = "";
			while (true)
			{
				next_char = inputStream.ReadByte();
				if (next_char == '\n')
					break;
				if (next_char == '\r')
					continue;
				if (next_char == -1)
				{
					Thread.Sleep(1);
					continue;
				}
				data += Convert.ToChar(next_char);
			}            
			return data;
		}
		public void process()
		{                        
			// we can't use a StreamReader for input, because it buffers up extra data on us inside it's
			// "processed" view of the world, and we want the data raw after the headers
			inputStream = new BufferedStream(socket.GetStream());
			
			// we probably shouldn't be using a streamwriter for all output from handlers either
			outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
			try
			{
				parseRequest();
				//readHeaders();
				if (http_method.Equals("GET"))
					srv.handleGETRequest(this);
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.ToString());
				writeFailure();
			}
			outputStream.Flush();
			inputStream = null;
			outputStream = null;
			socket.Close();             
		}
		
		public void parseRequest()
		{
			String request = streamReadLine(inputStream);
			string[] tokens = request.Split(' ');
			if (tokens.Length != 3)
				throw new Exception("Invalid http request line");
			http_method = tokens[0].ToUpper();
			http_url = tokens[1];
			http_protocol_versionstring = tokens[2];
		}
		
		public void writeSuccess(string content_type)
		{
			outputStream.WriteLine("HTTP/1.0 200 OK");            
			outputStream.WriteLine("Content-Type: " + content_type);
			outputStream.WriteLine("Connection: close");
			outputStream.WriteLine("");
		}
		
		public void writeFailure()
		{
			outputStream.WriteLine("HTTP/1.0 404 File not found");
			outputStream.WriteLine("Connection: close");
			outputStream.WriteLine("");
		}
	}
	
	public abstract class HttpServer
	{
		
		protected int port;
		protected IPAddress ip;
		public TcpListener listener;
		public bool is_active = true;

		public HttpServer()
		{}
		public HttpServer(string ip, int port)
		{
			this.port = port;
			IPAddress.TryParse(ip, out this.ip);
		}
		
		public void listen()
		{
			listener = new TcpListener(new IPEndPoint(ip, port));
			listener.Start();
			while (is_active)
			{                
				TcpClient s = listener.AcceptTcpClient();
				HttpProcessor processor = new HttpProcessor(s, this);
				Thread thread = new Thread(new ThreadStart(processor.process));
				thread.Start();
				Thread.Sleep(1);
			}
			listener.Stop();
		}
		
		public abstract void handleGETRequest(HttpProcessor p);
	}
}