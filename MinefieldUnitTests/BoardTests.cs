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
	public class BoardTests
	{
		private IRandomizer mockRandomizer;

		private Board classUnderTest;

		[SetUp]
		public void Setup()
		{
			mockRandomizer = Substitute.For<IRandomizer>();

			classUnderTest = new Board(mockRandomizer);
		}

		[Test]
		public void Generate_InvalidBoardSize()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(2, 0, 0, 0), "Invalid board size 2 is not detected");
		}

		[Test]
		public void Generate_InvalidNumberOfMines_Negative()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, -1, 0, 0), "Invalid number of mines -1 is not detected");
		}

		[Test]
		public void Generate_InvalidNumberOfMines_AboveLimit()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, 9, 0, 0), "Invalid number of mines 9 (max is 8 in this test scenario) is not detected");
		}

		[Test]
		public void Generate_InvalidStartPositionX_Negative()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, 8, -1, 0), "Invalid starting position 'X' -1 is not detected");
		}

		[Test]
		public void Generate_InvalidStartPositionX_AboveLimit()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, 8, 3, 0), "Invalid starting position 'X' 3 (max is 2 in this test scenario) is not detected");
		}

		[Test]
		public void Generate_InvalidStartPositionY_Negative()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, 8, 0, -1), "Invalid starting position 'Y' -1 is not detected");
		}

		[Test]
		public void Generate_InvalidStartPositionY_AboveLimit()
		{
			Assert.Throws<ArgumentException>(() => classUnderTest.Generate(3, 8, 2, 3), "Invalid starting position 'Y' 3 (max is 2 in this test scenario) is not detected");
		}

		[Test]
		public void Generate_OneMine()
		{
			mockRandomizer.Next().Returns(
				//    [0, 0] - Starting position
				1, // [1, 0] - No mine
				1, // [2, 0] - No mine
				0, // [0, 1] - Mine exists
				1, // [1, 1] - No mine
				1, // [2, 1] - No mine
				1, // [0, 2] - No mine
				1, // [1, 2] - No mine
				1  // [2, 2] - No mine
			);

			classUnderTest.Generate(3, 1, 0, 0);

			Assert.False(classUnderTest.IsMineHit(0, 0), "Mine should not exist at position [0, 0]");
			Assert.True(classUnderTest.IsMineHit(0, 1), "Mine should exist at position [0, 1]");
			Assert.False(classUnderTest.IsMineHit(0, 2), "Mine should not exist at position [0, 2]");
			Assert.False(classUnderTest.IsMineHit(1, 0), "Mine should not exist at position [1, 0]");
			Assert.False(classUnderTest.IsMineHit(1, 1), "Mine should not exist at position [1, 1]");
			Assert.False(classUnderTest.IsMineHit(1, 2), "Mine should not exist at position [1, 2]");
			Assert.False(classUnderTest.IsMineHit(2, 0), "Mine should not exist at position [2, 0]");
			Assert.False(classUnderTest.IsMineHit(2, 1), "Mine should not exist at position [2, 1]");
			Assert.False(classUnderTest.IsMineHit(2, 2), "Mine should not exist at position [2, 2]");
		}

		[Test]
		public void Generate_ThreeRandomMines()
		{
			mockRandomizer.Next().Returns(
				0, // [0, 0] - Mine exists
				1, // [1, 0] - No mine
				1, // [2, 0] - No mine
				   // [0, 1] - Starting position
				1, // [1, 1] - No mine
				1, // [2, 1] - No mine
				0, // [0, 2] - Mine exists
				0, // [1, 2] - Mine exists
				1  // [2, 2] - No mine
			);

			classUnderTest.Generate(3, 3, 0, 1);

			Assert.True(classUnderTest.IsMineHit(0, 0), "Mine should exist at position [0, 0]");
			Assert.False(classUnderTest.IsMineHit(0, 1), "Mine should not exist at position [0, 1]");
			Assert.True(classUnderTest.IsMineHit(0, 2), "Mine should exist at position [0, 2]");
			Assert.False(classUnderTest.IsMineHit(1, 0), "Mine should not exist at position [1, 0]");
			Assert.False(classUnderTest.IsMineHit(1, 1), "Mine should not exist at position [1, 1]");
			Assert.True(classUnderTest.IsMineHit(1, 2), "Mine should exist at position [1, 2]");
			Assert.False(classUnderTest.IsMineHit(2, 0), "Mine should not exist at position [2, 0]");
			Assert.False(classUnderTest.IsMineHit(2, 1), "Mine should not exist at position [2, 1]");
			Assert.False(classUnderTest.IsMineHit(2, 2), "Mine should not exist at position [2, 2]");
		}

		[Test]
		public void Generate_AllMines()
		{
			mockRandomizer.Next().Returns(
				0, // [0, 0] - Mine exists
				0, // [1, 0] - Mine exists
				   // [2, 0] - Starting position
				0, // [0, 1] - Mine exists
				0, // [1, 1] - Mine exists
				0, // [2, 1] - Mine exists
				0, // [0, 2] - Mine exists
				0, // [1, 2] - Mine exists
				0  // [2, 2] - Mine exists
			);

			classUnderTest.Generate(3, 8, 2, 0);

			Assert.True(classUnderTest.IsMineHit(0, 0), "Mine should exist at position [0, 0]");
			Assert.True(classUnderTest.IsMineHit(0, 1), "Mine should exist at position [0, 1]");
			Assert.True(classUnderTest.IsMineHit(0, 2), "Mine should exist at position [0, 2]");
			Assert.True(classUnderTest.IsMineHit(1, 0), "Mine should exist at position [1, 0]");
			Assert.True(classUnderTest.IsMineHit(1, 1), "Mine should exist at position [1, 1]");
			Assert.True(classUnderTest.IsMineHit(1, 2), "Mine should exist at position [1, 2]");
			Assert.False(classUnderTest.IsMineHit(2, 0), "Mine should not exist at position [2, 0]");
			Assert.True(classUnderTest.IsMineHit(2, 1), "Mine should exist at position [2, 1]");
			Assert.True(classUnderTest.IsMineHit(2, 2), "Mine should exist at position [2, 2]");
		}
	}
}
