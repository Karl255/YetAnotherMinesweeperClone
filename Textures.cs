using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YetAnotherMinesweeperClone
{
	public static class Textures
	{
		public static class Tiles
		{
			public static BitmapSource[] Uncovered = new BitmapSource[9];

			public static BitmapSource Covered;
			public static BitmapSource CoveredFlag;
			public static BitmapSource CoveredUnknown;

			public static BitmapSource Pushed;
			public static BitmapSource PushedUnknown;

			public static BitmapSource Mine;
			public static BitmapSource NoMine;
			public static BitmapSource SteppedOnMine;
		}

		public static class DigitDisplay
		{
			public static BitmapSource Display;
			public static BitmapSource Minus;

			public static BitmapSource[] Digits = new BitmapSource[10];
		}

		public static class SmileyButton
		{
			public static BitmapSource Normal;
			public static BitmapSource PushingTile;
			public static BitmapSource Lost;
			public static BitmapSource Won;
			public static BitmapSource Pushed;
		}

		public static class Frame
		{
			public static BitmapSource TopLeftCorner;
			public static BitmapSource TopRightCorner;
			public static BitmapSource MiddleLeftT;
			public static BitmapSource MiddleRightT;
			public static BitmapSource BottomLeftCorner;
			public static BitmapSource BottomRightCorner;

			public static BitmapSource TopHorizontal;
			public static BitmapSource TopMiddleVerticalLeft;
			public static BitmapSource TopMiddleVerticalRight;
			public static BitmapSource MiddleHorizontal;
			public static BitmapSource MiddleBottomVerticalLeft;
			public static BitmapSource MiddleBottomVerticalRight;
		}

		public static void MapSkin(BitmapImage skin)
		{
			for (int i = 0; i < 9; i++)
			{
				Tiles.Uncovered[i] = skin.QuickClone(16 * i, 0, 16, 16);
			}

			Tiles.Covered = skin.QuickClone(0, 16, 16, 16);
			Tiles.Pushed = skin.QuickClone(16, 16, 16, 16);
			Tiles.Mine = skin.QuickClone(32, 16, 16, 16);
			Tiles.CoveredFlag = skin.QuickClone(48, 16, 16, 16);
			Tiles.NoMine = skin.QuickClone(64, 16, 16, 16);
			Tiles.SteppedOnMine = skin.QuickClone(80, 16, 16, 16);
			Tiles.CoveredUnknown = skin.QuickClone(96, 16, 16, 16);
			Tiles.PushedUnknown = skin.QuickClone(112, 16, 16, 16);


			for (int i = 0; i < 10; i++)
			{
				DigitDisplay.Digits[i] = skin.QuickClone(12 * i, 33, 11, 21);
			}

			DigitDisplay.Minus = skin.QuickClone(120, 33, 11, 21);
			DigitDisplay.Display = skin.QuickClone(28, 82, 41, 25);


			SmileyButton.Normal = skin.QuickClone(0, 55, 26, 26);
			SmileyButton.PushingTile = skin.QuickClone(27, 55, 26, 26);
			SmileyButton.Lost = skin.QuickClone(54, 55, 26, 26);
			SmileyButton.Won = skin.QuickClone(81, 55, 26, 26);
			SmileyButton.Pushed = skin.QuickClone(108, 55, 26, 26);


			Frame.TopLeftCorner = skin.QuickClone(0, 82, 12, 11);
			Frame.TopRightCorner = skin.QuickClone(15, 82, 12, 11);
			Frame.MiddleLeftT = skin.QuickClone(0, 96, 12, 11);
			Frame.MiddleRightT = skin.QuickClone(15, 96, 12, 11);
			Frame.BottomLeftCorner = skin.QuickClone(0, 110, 12, 11);
			Frame.BottomRightCorner = skin.QuickClone(15, 110, 12, 11);

			Frame.TopHorizontal = skin.QuickClone(13, 82, 1, 11);
			Frame.MiddleHorizontal = skin.QuickClone(13, 96, 1, 11);
			Frame.TopHorizontal = skin.QuickClone(13, 110, 1, 11);

			Frame.TopMiddleVerticalLeft = skin.QuickClone(0, 94, 12, 1);
			Frame.TopMiddleVerticalRight = skin.QuickClone(15, 94, 12, 1);
			Frame.MiddleBottomVerticalLeft = skin.QuickClone(0, 108, 12, 1);
			Frame.MiddleBottomVerticalRight = skin.QuickClone(15, 108, 12, 1);
		}

		public static BitmapSource GetRandomTile()
		{
			BitmapSource[] tiles = new BitmapSource[17];

			for (int i = 0; i < 9; i++)
			{
				tiles[i] = Tiles.Uncovered[i];
			}

			tiles[9] = Tiles.Covered;
			tiles[10] = Tiles.CoveredFlag;
			tiles[11] = Tiles.CoveredUnknown;

			tiles[12] = Tiles.Pushed;
			tiles[13] = Tiles.PushedUnknown;

			tiles[14] = Tiles.Mine;
			tiles[15] = Tiles.NoMine;
			tiles[16] = Tiles.SteppedOnMine;

			var random = new Random();
			return tiles[random.Next(0, 17)];

		}

		private static BitmapSource QuickClone(this BitmapImage source, int x, int y, int width, int height)
		{
			return new CroppedBitmap(source, new Int32Rect(x, y, width, height));
		}
	}
}
