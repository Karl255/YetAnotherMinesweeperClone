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

		private Game game = new Game();
		private Binding scaleBinding;
		private Image[,] gridTiles;

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

			scaleBinding = new Binding("Value")
			{
				Source = Scale
			};

			game.NewGame(9, 9, 10);
			gridTiles = new Image[game.Columns, game.Rows];

			for (int x = 0; x < game.Columns; x++)
				Minefield.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

			for (int y = 0; y < game.Rows; y++)
			{
				Minefield.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
				for (int x = 0; x < game.Columns; x++)
				{
					Image tile = new Image()
					{
						Source = Textures.GetRandomTile()
					};
					tile.SetBinding(WidthProperty, scaleBinding);
					tile.SetBinding(HeightProperty, scaleBinding);
					Grid.SetRow(tile, y);
					Grid.SetColumn(tile, x);
					Minefield.Children.Add(tile);
					gridTiles[x, y] = tile;
				}
			}
		}

		private void Minefield_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var pos = e.GetPosition(Minefield);
			(int x, int y) = ((int)(pos.X / Scale.Value), (int)(pos.Y / Scale.Value));

			//var element = Minefield.Children.Cast<UIElement>().First(e => Grid.GetColumn(e) == pos.X && Grid.GetRow(e) == pos.Y) as Image;
			gridTiles[x, y].Source = Textures.GetRandomTile();
		}

		private void SmileButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
