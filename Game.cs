using System;
using System.Collections.Generic;

namespace YetAnotherMinesweeperClone
{

	class Game
	{
		public List<(int x, int y)> Mines { get; } = new List<(int, int)>();
		public int Columns { get; set; }
		public int Rows { get; set; }
		public int NumberOfMines { get; set; }

		private Random random = new Random();

		public void NewGame(int columns, int rows, int numberOfMines)
		{
			Columns = columns;
			Rows = rows;
			NumberOfMines = numberOfMines;
		}

		public void GenerateMines()
		{
			Mines.Clear();
			Mines.Capacity = NumberOfMines;

			for (int i = 0; i < NumberOfMines; i++)
			{
				Mines.Add((
					x: random.Next(Columns - 1),
					y: random.Next(Rows - 1)
				));
			}
		}

	}
}
