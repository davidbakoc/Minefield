using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	public interface IConsoleWrapper
	{
		void Write(string message);
		void WriteLine(string message = "");
		ConsoleKeyInfo ReadKey();
	}

	public class ConsoleWrapper : IConsoleWrapper
	{
		public ConsoleKeyInfo ReadKey() =>
			Console.ReadKey();

		public void Write(string message) =>
			Console.Write(message);

		public void WriteLine(string message = "") =>
			Console.WriteLine(message);
	}
}
