using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using YetAnotherMinesweeperClone.Texture;

namespace YetAnotherMinesweeperClone
{

	public class Game
	{
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public int MineCount { get; private set; }
		public GameState State { get; private set; }

		public delegate void TileChangedHandler(int x, int y, BitmapSource tile);
		public event TileChangedHandler TileChangedEvent;

		private Tile[,] Field { get; init; }
		private TileState[,] TileStates { get; init; }

		public Game(int columns, int rows, int mineCount)
		{
			Columns = columns;
			Rows = rows;
			MineCount = mineCount;

			var mines = GenerateMines(mineCount);
			Field = new Tile[columns, rows];
			TileStates = new TileState[columns, rows];

			foreach (var mine in mines)
				Field[mine.x, mine.y] = Tile.Bomb;

			for (int y = 0; y < rows; y++)
				for (int x = 0; x < columns; x++)
					if (Field[x, y] != Tile.Bomb)
						Field[x, y] = (Tile)CountAround(x, y);
		}

		private (int x, int y)[] GenerateMines(int count)
		{
			HashSet<(int x, int y)> mines = new(count);
			Random random = new();

			while (mines.Count < count)
				mines.Add((x: random.Next(Columns), y: random.Next(Rows)));

			(int x, int y)[] minesArray = new (int x, int y)[count];
			mines.CopyTo(minesArray);
			return minesArray;
		}

		private int CountAround(int x, int y)
		{
			int xStart = Math.Max(0, x - 1);
			int xEnd = Math.Min(Columns, x + 2);
			int yStart = Math.Max(0, y - 1);
			int yEnd = Math.Min(Rows, y + 2);

			int count = 0;

			for (int iy = yStart; iy < yEnd; iy++)
				for (int ix = xStart; ix < xEnd; ix++)
					if (Field[ix, iy] == Tile.Bomb)
						count++;

			return count;
		}

		public void UncoverTile(int x, int y)
		{
			if (TileStates[x, y] != TileState.Covered)
				return;

			TileStates[x, y] = TileState.Uncovered;
			Tile tile = Field[x, y];

			if (tile == Tile.Bomb) // bomb
			{
				State = GameState.Lost;
				TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)TileTexture.SteppedOnMine]);
				ExposeAllMines(x, y);
			}
			else if (tile == Tile.Blank) // 0
			{
				TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)TileTexture.UncoveredBlank]);
				UncoverAround(x, y);
			}
			else // 1..8
			{
				TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)tile]);
			}

			if (IsGameWon())
			{
				State = GameState.Won;
				FlagAllMines();
			}
		}

		private void UncoverAround(int x, int y)
		{
			int xStart = Math.Max(0, x - 1);
			int xEnd = Math.Min(Columns, x + 2);
			int yStart = Math.Max(0, y - 1);
			int yEnd = Math.Min(Rows, y + 2);

			for (int iy = yStart; iy < yEnd; iy++)
			{
				for (int ix = xStart; ix < xEnd; ix++)
				{
					if (ix == x && iy == y || TileStates[ix, iy] != TileState.Covered)
						continue;

					TileStates[ix, iy] = TileState.Uncovered;
					Tile tile = Field[ix, iy];

					if (tile == Tile.Blank) // 0
					{
						TileChangedEvent?.Invoke(ix, iy, Textures.Tiles[(int)TileTexture.UncoveredBlank]);
						UncoverAround(ix, iy);
					}
					else // 1..8
					{
						TileChangedEvent?.Invoke(ix, iy, Textures.Tiles[(int)tile]);
					}
				}
			}
		}

		public TileState GetTileState((int x, int y) position) => TileStates[position.x, position.y];

		// "right clicking" a tile
		public void CycleTileState(int x, int y)
		{
			TileState initialState = TileStates[x, y];

			if (initialState != TileState.Uncovered)
			{
				switch (initialState)
				{
					case TileState.Covered:
						TileStates[x, y] = TileState.Flagged;
						TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)TileTexture.Flag]);
						break;

					case TileState.Flagged:
						TileStates[x, y] = TileState.Covered;
						TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)TileTexture.Covered]);
						break;

					default:
						break;
				}
			}
		}

		private bool IsGameWon()
		{
			for (int y = 0; y < Rows; y++)
				for (int x = 0; x < Columns; x++)
					if (Field[x, y] != Tile.Bomb && TileStates[x, y] != TileState.Uncovered)
						return false;

			return true;
		}

		private void ExposeAllMines(int x, int y)
		{
			for (int iy = 0; iy < Rows; iy++)
			{
				for (int ix = 0; ix < Columns; ix++)
				{
					if (ix == x && iy == y)
						continue;

					// flagged, but not a bomb
					if (Field[ix, iy] != Tile.Bomb && TileStates[ix, iy] == TileState.Flagged)
						TileChangedEvent?.Invoke(ix, iy, Textures.Tiles[(int)TileTexture.IncorrectFlag]);
					// a bomb, but not flagged
					else if (Field[ix, iy] == Tile.Bomb && TileStates[ix, iy] != TileState.Flagged)
						TileChangedEvent?.Invoke(ix, iy, Textures.Tiles[(int)TileTexture.Mine]);
				}
			}
		}

		private void FlagAllMines()
		{
			for (int iy = 0; iy < Rows; iy++)
			{
				for (int ix = 0; ix < Columns; ix++)
				{
					// a bomb, but not flagged
					if (Field[ix, iy] == Tile.Bomb && TileStates[ix, iy] != TileState.Flagged)
						TileChangedEvent?.Invoke(ix, iy, Textures.Tiles[(int)TileTexture.Flag]);
				}
			}
		}
	}
}
