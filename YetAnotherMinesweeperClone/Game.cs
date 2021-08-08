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
		private bool[,] UncoveredTiles { get; init; }

		public Game(int columns, int rows, int mineCount)
		{
			Columns = columns;
			Rows = rows;
			MineCount = mineCount;

			var mines = GenerateMines(mineCount);
			Field = new Tile[columns, rows];
			UncoveredTiles = new bool[columns, rows];

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
			if (UncoveredTiles[x, y])
				return;

			UncoveredTiles[x, y] = true;
			Tile tile = Field[x, y];

			if (tile == Tile.Bomb) // bomb
			{
				State = GameState.Lost;
				TileChangedEvent?.Invoke(x, y, Textures.Tiles[(int)TileTexture.SteppedOnMine]);
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
					if (ix == x && iy == y || UncoveredTiles[ix, iy])
						continue;

					UncoveredTiles[ix, iy] = true;
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

		public bool IsTileUncovered((int x, int y) position) => UncoveredTiles[position.x, position.y];
	}
}
