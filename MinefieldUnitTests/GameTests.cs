using Minefield;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldUnitTests
{
	[TestFixture]
	public class GameTests
	{
		private IConsoleWrapper mockConsoleWrapper;
		private IBoard mockBoard;

		private Game classUnderTest;

		[SetUp]
		public void Setup()
		{
			mockConsoleWrapper = Substitute.For<IConsoleWrapper>();
			mockBoard = Substitute.For<IBoard>();

			classUnderTest = new Game(mockConsoleWrapper, mockBoard);
		}

		[Test]
		public void Run_ExitGame()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 4, 0);
		}

		[Test]
		public void Run_LostLives()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false));

			mockBoard.IsMineHit(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 4, 0);

			mockConsoleWrapper.Received(1).Write("Position: E1 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E2 | Lives: 2 | Moves: 1 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E3 | Lives: 1 | Moves: 2 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("You lost all lives! Game over!");
		}

		[Test]
		public void Run_MoveUpNotAllowed()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 4, 0);

			mockConsoleWrapper.Received(2).Write("Position: E1 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Cannot move outside of the board!");
		}

		[Test]
		public void Run_MoveDownNotAllowed()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

			mockBoard.IsMineHit(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 0, 4);

			mockConsoleWrapper.Received(1).Write("Position: A5 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: A6 | Lives: 3 | Moves: 1 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: A7 | Lives: 3 | Moves: 2 | Next move: ");
			mockConsoleWrapper.Received(2).Write("Position: A8 | Lives: 3 | Moves: 3 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Cannot move outside of the board!");
		}

		[Test]
		public void Run_MoveLeftNotAllowed()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 0, 4);

			mockConsoleWrapper.Received(2).Write("Position: A5 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Cannot move outside of the board!");
		}

		[Test]
		public void Run_MoveRightNotAllowed()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false));

			mockBoard.IsMineHit(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 4, 0);

			mockConsoleWrapper.Received(1).Write("Position: E1 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: F1 | Lives: 3 | Moves: 1 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: G1 | Lives: 3 | Moves: 2 | Next move: ");
			mockConsoleWrapper.Received(2).Write("Position: H1 | Lives: 3 | Moves: 3 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Cannot move outside of the board!");
		}

		[Test]
		public void Run_WinMovesDown()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false));

			mockBoard.IsMineHit(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 4, 0);

			mockConsoleWrapper.Received(1).Write("Position: E1 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E2 | Lives: 3 | Moves: 1 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E3 | Lives: 3 | Moves: 2 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E4 | Lives: 3 | Moves: 3 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E5 | Lives: 3 | Moves: 4 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E6 | Lives: 3 | Moves: 5 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E7 | Lives: 3 | Moves: 6 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Position: E8 | Lives: 3 | Moves: 7");
			mockConsoleWrapper.Received(1).WriteLine("Congratulations! You reached the other side.");
		}

		[Test]
		public void Run_WinMovesRight()
		{
			mockConsoleWrapper.ReadKey().Returns(
				new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false),
				new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false));

			mockBoard.IsMineHit(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			classUnderTest.Run();

			mockBoard.Received(1).Generate(8, 20, 0, 4);

			mockConsoleWrapper.Received(1).Write("Position: A5 | Lives: 3 | Moves: 0 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: B5 | Lives: 3 | Moves: 1 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: C5 | Lives: 3 | Moves: 2 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: D5 | Lives: 3 | Moves: 3 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: E5 | Lives: 3 | Moves: 4 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: F5 | Lives: 3 | Moves: 5 | Next move: ");
			mockConsoleWrapper.Received(1).Write("Position: G5 | Lives: 3 | Moves: 6 | Next move: ");
			mockConsoleWrapper.Received(1).WriteLine("Position: H5 | Lives: 3 | Moves: 7");
			mockConsoleWrapper.Received(1).WriteLine("Congratulations! You reached the other side.");
		}
	}
}
