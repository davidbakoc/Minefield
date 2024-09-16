using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	/// <summary>
	/// Type of field.
	/// </summary>
	public enum FieldType
	{
		Empty,
		Mine,
	}

	/// <summary>
	/// Game board interface.
	/// </summary>
	public interface IBoard
	{
		/// <summary>
		/// Generate randomly positioned mines. Desired starting position field is excluded when generating mines. It is a safe starting place.
		/// </summary>
		/// <param name="boardSize">Board size.</param>
		/// <param name="numberOfMines">Number of mines on the field.</param>
		/// <param name="startPosX">Desired starting position X.</param>
		/// <param name="startPosY">Desired starting position Y.</param>
		void Generate(int boardSize, int numberOfMines, int startPosX, int startPosY);

		/// <summary>
		/// Checks if there is a mine on provided field.
		/// </summary>
		/// <param name="posX">Position X.</param>
		/// <param name="posY">Position Y.</param>
		/// <returns>true if mine is present on the field, false otherwise.</returns>
		bool IsMineHit(int posX, int posY);
	}

	public class Board : IBoard
	{
		private readonly IRandomizer randomizer;

		private FieldType[,] fields;

		public Board(IRandomizer randomizer)
		{
			this.randomizer = randomizer;
		}

		public void Generate(int boardSize, int numberOfMines, int startPosX, int startPosY)
		{
			ValidateInput(boardSize, numberOfMines, startPosX, startPosY);

			int indexStartPosition = startPosY * boardSize + startPosX;
			fields = new FieldType[boardSize, boardSize];

			// Generate an array of ordered numbers matching entire board (index 0 is [0,0], index 1*boardSize+3 is [3,1], and so on...)
			// Remove index of starting position since mine cannot be at that position. Order indexes by randomly assigned number and take
			// first n elements matching number of mines. Resulting array will be only indexes where mines are placed. Board is initially
			// with no mines so go through mine indexes and set mines to fields
			int[] indexMines = Enumerable.Range(0, boardSize * boardSize)
				.Except(new int[] { indexStartPosition })
				.OrderBy(x => randomizer.Next())
				.Take(numberOfMines)
				.ToArray();

			for (int i = 0; i < indexMines.Length; i++)
			{
				fields[indexMines[i] % boardSize, indexMines[i] / boardSize] = FieldType.Mine;
			}
		}

		public bool IsMineHit(int posX, int posY)
		{
			if (fields == null)
			{
				throw new InvalidOperationException($"{nameof(fields)} not initialized");
			}

			ValidatePosition(fields.GetLength(0), posX, "Input position 'X'");
			ValidatePosition(fields.GetLength(0), posY, "Input position 'Y'");
			return fields[posX, posY] == FieldType.Mine;
		}

		private void ValidateInput(int boardSize, int numberOfMines, int startPosX, int startPosY)
		{
			if (boardSize < Constants.MinBoardSize)
			{
				throw new ArgumentException($"Board size must be at least {Constants.MinBoardSize} or larger");
			}

			if (numberOfMines < 0 || numberOfMines > boardSize * boardSize - 1)
			{
				throw new ArgumentException($"Number of mines must be a number in range [0, {boardSize * boardSize - 1}]");
			}

			ValidatePosition(boardSize, startPosX, "Start position 'X'");
			ValidatePosition(boardSize, startPosY, "Start position 'Y'");
		}

		private void ValidatePosition(int max, int position, string positionName)
		{
			if (position < 0 || position >= max)
			{
				throw new ArgumentException($"{positionName} must be a number in range [0, {max - 1}]");
			}
		}
	}
}
