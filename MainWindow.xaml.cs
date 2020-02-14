using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using YetAnotherMinesweeperClone.Texture;

namespace YetAnotherMinesweeperClone
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public BindableValue<double> Scale = new BindableValue<double>(16);

		private Game game;
		private Binding scaleBinding;
		private Image[,] tileImages;

		public MainWindow()
		{
			InitializeComponent();

			//frame corners
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Image img = new Image { Source = Textures.FrameVertecies[2 * i + j] };
					Grid.SetColumn(img, 2 * j);
					Grid.SetRow(img, 2 * i);
					RootGrid.Children.Add(img);
				}
			}

			//frame horizontal bars
			for (int i = 0; i < 3; i++)
			{
				Image img = new Image { Source = Textures.FrameHorizontalBars[i], Stretch = Stretch.Fill };
				Grid.SetColumn(img, 1);
				Grid.SetRow(img, 2 * i);
				RootGrid.Children.Add(img);
			}

			//frame vertical bars
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Image img = new Image { Source = Textures.FrameVerticalBars[2 * i + j], Stretch = Stretch.Fill };
					Grid.SetColumn(img, 2 * j);
					Grid.SetRow(img, 2 * i + 1);
					RootGrid.Children.Add(img);
				}
			}

			game = new Game();
			game.NewGame(9, 9, 10);
			game.TileChnagedEvent += (int x, int y, Tile tile) => tileImages[x, y].Source = Textures.Tiles[(int)tile];

			scaleBinding = new Binding("Value")
			{
				Source = Scale
			};


			//initialize minefield grid
			FillMinefieldGrid();
		}

		private void FillMinefieldGrid()
		{
			tileImages = new Image[game.Columns, game.Rows];
			for (int x = 0; x < game.Columns; x++)
				Minefield.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

			for (int y = 0; y < game.Rows; y++)
			{
				Minefield.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				for (int x = 0; x < game.Columns; x++)
				{
					Image tile = new Image()
					{
						Source = Textures.Tiles[(int)Tile.Covered]
					};
					tile.SetBinding(WidthProperty, scaleBinding);
					tile.SetBinding(HeightProperty, scaleBinding);

					Grid.SetRow(tile, y);
					Grid.SetColumn(tile, x);

					Minefield.Children.Add(tile);
					tileImages[x, y] = tile;
				}
			}
		}

		private void ClearMinefieldGrid()
		{
			Minefield.Children.Clear();
			Minefield.ColumnDefinitions.Clear();
			Minefield.RowDefinitions.Clear();
		}

		private void ResetMinefield()
		{
			game.NewGame();
			foreach (var tileImage in tileImages)
			{
				tileImage.Source = Textures.Tiles[(int)Tile.Covered];
			}
		}

		private void ResetMinefield(int columns, int rows, int numberOfMines)
		{
			game.NewGame(columns, rows, numberOfMines);
			ClearMinefieldGrid();
			FillMinefieldGrid();
		}

		private void Minefield_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var pos = e.GetPosition(Minefield);
			(int x, int y) = ((int)(pos.X / Scale.Value), (int)(pos.Y / Scale.Value));

			game.UncoverTile(x, y);
		}

		private void NewGame(object sender, RoutedEventArgs e) => ResetMinefield();

		private void NewGameBeginner(object sender, RoutedEventArgs e) => ResetMinefield(9, 9, 10);
		private void NewGameIntermediate(object sender, RoutedEventArgs e) => ResetMinefield(16, 16, 40);
		private void NewGameExpert(object sender, RoutedEventArgs e) => ResetMinefield(30, 16, 99);
	}
}
