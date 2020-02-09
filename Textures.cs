using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YetAnotherMinesweeperClone.Texture
{
	public static class Textures
	{
		public static BitmapSource[] Tiles = new BitmapSource[17];

		public static BitmapSource DisplayFrame;
		public static BitmapSource[] DisplayDigits = new BitmapSource[11];

		public static BitmapSource[] Smileys = new BitmapSource[5];

		public static BitmapSource[] FrameVertecies = new BitmapSource[6];
		public static BitmapSource[] FrameHorizontalBars = new BitmapSource[3];
		public static BitmapSource[] FrameVerticalBars = new BitmapSource[4];

		public static void MapSkin(BitmapImage skin)
		{
			//all tiles
			for (int i = 0; i <= 8; i++)
			{
				Tiles[i] = skin.QuickClone(16 * i, 0, 16, 16);
			}
			for (int i = 0; i <= 7; i++)
			{
				Tiles[i + 9] = skin.QuickClone(16 * i, 16, 16, 16);
			}

			//all display elements
			DisplayFrame = skin.QuickClone(28, 82, 41, 25);
			for (int i = 0; i <= 10; i++)
			{
				DisplayDigits[i] = skin.QuickClone(12 * i, 33, 11, 21);
			}

			//smileys
			for (int i = 0; i <= 4; i++)
			{
				Smileys[i] = skin.QuickClone(27 * i, 55, 26, 26);
			}

			//frame
			for (int i = 0; i < 3; i++)
			{
				FrameVertecies[2 * i] = skin.QuickClone(0, 82 + 14 * i, 12, 11);
				FrameVertecies[2 * i + 1] = skin.QuickClone(15, 82 + 14 * i, 12, 11);
			}

			for (int i = 0; i < 3; i++)
			{
				FrameHorizontalBars[i] = skin.QuickClone(13, 82 + 14 * i, 1, 11);
			}

			for (int i = 0; i < 2; i++)
			{
				FrameVerticalBars[i] = skin.QuickClone(i * 15, 94, 12, 1);
				FrameVerticalBars[i + 2] = skin.QuickClone(i * 15, 108, 12, 1);
			}
		}

		public static BitmapSource GetRandomTile()
		{
			var random = new Random();
			return Tiles[random.Next(0, 17)];

		}

		private static BitmapSource QuickClone(this BitmapImage source, int x, int y, int width, int height)
		{
			return new CroppedBitmap(source, new Int32Rect(x, y, width, height));
		}
	}
}
