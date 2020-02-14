using System;
using System.Collections.Generic;
using YetAnotherMinesweeperClone.Texture;

namespace YetAnotherMinesweeperClone
{
	public delegate void TileChangedHandler(int x, int y, Tile tile);

	public class Game
	{
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public int NumberOfMines { get; private set; }
		public GameState State { get; private set; }

		public List<(int x, int y)> Mines { get; private set; }

		public event TileChangedHandler TileChnagedEvent;

		private Random random;

		public Game()
		{
			random = new Random();
			Mines = new List<(int, int)>();
		}

		public void NewGame()
		{
			GenerateMines();
			State = GameState.Playing;
		}

		public void NewGame(int columns, int rows, int numberOfMines)
		{
			Columns = columns;
			Rows = rows;
			NumberOfMines = numberOfMines;
			NumberOfMines = numberOfMines;

			GenerateMines();
		}

		private void GenerateMines()
		{
			Mines.Clear();
			Mines.Capacity = NumberOfMines;

			for (int i = 0; i < NumberOfMines; i++)
			{
				var mine = (
					x: random.Next(Columns - 1),
					y: random.Next(Rows - 1));

				Mines.Add(mine);
			}
		}

		public void UncoverTile(int x, int y)
		{
			if (Mines.Contains((x, y)))
			{
				State = GameState.Lost;
				TileChnagedEvent?.Invoke(x, y, Tile.SteppedOnMine);
			}
			else
			{
				TileChnagedEvent?.Invoke(x, y, (Tile)GetMineAmountAt(x, y));
			}
		}

		private int GetMineAmountAt(int x, int y)
		{
			int total = 0;

			for (int ix = x - 1; ix <= x + 1; ix++)
			{
				for (int iy = y - 1; iy <= y + 1; iy++)
				{
					if (Mines.Contains((ix, iy)))
					{
						total++;
					}
				}
			}

			return total;
		}
	}
}
