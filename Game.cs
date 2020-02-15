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
		private bool[,] uncoveredTiles;

		public Game()
		{
			random = new Random(-9);
			Mines = new List<(int, int)>();
		}

		public void NewGame()
		{
			GenerateMines();
			State = GameState.Playing;
			uncoveredTiles = new bool[Columns, Rows];
		}

		public void NewGame(int columns, int rows, int numberOfMines)
		{
			Columns = columns;
			Rows = rows;
			NumberOfMines = numberOfMines;
			NumberOfMines = numberOfMines;
			uncoveredTiles = new bool[columns, rows];

			GenerateMines();
		}

		private void GenerateMines()
		{
			Mines.Clear();
			Mines.Capacity = NumberOfMines;

			for (int i = 0; i < NumberOfMines; i++)
			{
				var mine = (
					x: random.Next(Columns),
					y: random.Next(Rows));

				Mines.Add(mine);
			}
		}

		public void UncoverTile(int x, int y)
		{
			if (uncoveredTiles[x, y]) return;
			

			if (Mines.Contains((x, y)))
			{
				State = GameState.Lost;
				TileChnagedEvent?.Invoke(x, y, Tile.SteppedOnMine);
			}
			else
			{
				FloodUncoverTile(x, y);
			}
		}

		private void FloodUncoverAround(int x, int y)
		{
			for (int ix = x - 1; ix <= x + 1; ix++)
			{
				for (int iy = y - 1; iy <= y + 1; iy++)
				{
					if (IsOutOfBounds(ix, iy)) continue;
					FloodUncoverTile(ix, iy);
				}
			}
		}

		private void FloodUncoverTile(int x, int y)
		{
			if (!uncoveredTiles[x, y])
			{
				uncoveredTiles[x, y] = true;

				int n = GetMineAmountAt(x, y);
				TileChnagedEvent?.Invoke(x, y, (Tile)n);

				if (n == 0)
				{
					FloodUncoverAround(x, y);
				}
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

		private bool IsOutOfBounds(int x, int y) => !(0 <= x && x < Columns && 0 <= y && y < Rows);
	}
}
