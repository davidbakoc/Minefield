using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	/// <summary>
	/// Game default values.
	/// </summary>
	public static class Constants
	{
		public static readonly int MinBoardSize = 3;

		public static readonly int BoardSize = 8;
		public static readonly int NumberOfMines = 20;
		public static readonly int NumberOfLives = 3;

		public static readonly Dictionary<ConsoleKey, string> MovingDirections = new Dictionary<ConsoleKey, string>
		{
			{ ConsoleKey.D1, "1) Top -> Down" },
			{ ConsoleKey.D2, "2) Left -> Right" },
		};

		public static readonly Dictionary<ConsoleKey, string> GameInput = new Dictionary<ConsoleKey, string>
		{
			{ ConsoleKey.Escape, "ESC" },
			{ ConsoleKey.LeftArrow, "LEFT" },
			{ ConsoleKey.UpArrow, "UP" },
			{ ConsoleKey.RightArrow, "RIGHT" },
			{ ConsoleKey.DownArrow, "DOWN" },
		};
	}
}
