using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Compose
				IConsoleWrapper consoleWrapper = new ConsoleWrapper();
				IRandomizer randomizer = new Randomizer();
				IBoard board = new Board(randomizer);
				Game game = new Game(consoleWrapper, board);

				game.Run();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Fatal error. Application will be terminated. Message: {ex.Message}");
			}
		}
	}
}
