using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	public class Game
	{
		private readonly IConsoleWrapper consoleWrapper;
		private readonly IBoard board;

		private int boardSize = Constants.BoardSize;
		private int posX = 0;
		private int posY = 0;
		private int numberOfLives = Constants.NumberOfLives;
		private int numberOfMoves = 0;
		private int winPosX = -1;
		private int winPosY = -1;

		public Game(IConsoleWrapper consoleWrapper, IBoard board)
		{
			this.consoleWrapper = consoleWrapper;
			this.board = board;
		}

		public void Run()
		{
			consoleWrapper.WriteLine("Welcome to Minefield game!");

			ConsoleKey keyDirection = GetKeyInput("Enter moving direction", Constants.MovingDirections, true);
			SetStartingPosition(keyDirection);
			SetWinningPositions(keyDirection);

			board.Generate(boardSize, Constants.NumberOfMines, posX, posY);

			consoleWrapper.WriteLine($"Use keyboard arrows ({string.Join(", ", Constants.GameInput.Where(x => x.Key != ConsoleKey.Escape).Select(x => x.Value))}) to move through the board or ESC to exit game. Good luck!");

			while (true)
			{
				ConsoleKey gameInput = GetKeyInput($"Position: {GetPrintablePosition()} | Lives: {numberOfLives} | Moves: {numberOfMoves} | Next move", Constants.GameInput, false);

				if (gameInput == ConsoleKey.Escape)
				{
					break;
				}

				ChangePosition(gameInput);

				if (numberOfLives == 0)
				{
					consoleWrapper.WriteLine("You lost all lives! Game over!");
					break;
				}

				if (IsEndReached())
				{
					consoleWrapper.WriteLine($"Position: {GetPrintablePosition()} | Lives: {numberOfLives} | Moves: {numberOfMoves}");
					consoleWrapper.WriteLine("Congratulations! You reached the other side.");
					break;
				}
			}

			consoleWrapper.WriteLine("Thank you for playing Minefield game. Goodbye!");
		}

		private ConsoleKey GetKeyInput(string message, Dictionary<ConsoleKey, string> availableInput, bool shouldWriteAvailableInput)
		{
			string availableInputStr = " " + string.Join(", ", availableInput.OrderBy(x => x.Key).Select(x => x.Value));
			consoleWrapper.Write($"{message}{(shouldWriteAvailableInput ? availableInputStr : string.Empty)}: ");

			while (true)
			{
				ConsoleKeyInfo keyInfo = consoleWrapper.ReadKey();
				consoleWrapper.WriteLine();

				if (availableInput.ContainsKey(keyInfo.Key))
				{
					return keyInfo.Key;
				}

				consoleWrapper.Write($"Invalid input. Choose between{availableInputStr}: ");
			}
		}

		private void SetStartingPosition(ConsoleKey keyDirection)
		{
			// Set starting position depending on board moving direction
			posX = keyDirection == ConsoleKey.D1 ? boardSize / 2 : 0;
			posY = keyDirection == ConsoleKey.D2 ? boardSize / 2 : 0;
		}

		private void SetWinningPositions(ConsoleKey keyDirection)
		{
			// Set winning axis value, depending on board moving direction. Any field in this axis is a winning field
			winPosY = keyDirection == ConsoleKey.D1 ? boardSize - 1 : -1;
			winPosX = keyDirection == ConsoleKey.D2 ? boardSize - 1 : -1;
		}

		private void ChangePosition(ConsoleKey key)
		{
			if (posX == 0 && key == ConsoleKey.LeftArrow
				|| posX == boardSize - 1 && key == ConsoleKey.RightArrow
				|| posY == 0 && key == ConsoleKey.UpArrow
				|| posY == boardSize - 1 && key == ConsoleKey.DownArrow)
			{
				consoleWrapper.WriteLine("Cannot move outside of the board!");
				return;
			}

			switch (key)
			{
				case ConsoleKey.LeftArrow: posX--; break;
				case ConsoleKey.RightArrow: posX++; break;
				case ConsoleKey.UpArrow: posY--; break;
				case ConsoleKey.DownArrow: posY++; break;
			}

			if (board.IsMineHit(posX, posY))
			{
				numberOfLives--;
			}

			numberOfMoves++;
		}

		private string GetPrintablePosition() =>
			// Game is 8x8 board so A - H, 1 - 8 characters will be used for position naming
			$"{(char)('A' + posX)}{posY + 1}";

		private bool IsEndReached() =>
			posX == winPosX || posY == winPosY;
	}
}
