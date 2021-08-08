using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using YetAnotherMinesweeperClone.Texture;

namespace YetAnotherMinesweeperClone
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private BindableValue<double> Scale = new(16);

		private Game Game;
		private (int x, int y) PreviousPosition = (-1, -1);
		private Binding ScaleBinding { get; init; }
		private Image[,] TileImages;
		private DispatcherTimer Timer { get; init; }

		private int _timePassed = 0;
		public int TimePassed
		{
			get => _timePassed;
			set
			{
				_timePassed = value;
				TimeCounter.Text = TimePassed.ToString("000");
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			//frame corners
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Image img = new () { Source = Textures.FrameVertecies[2 * i + j] };
					Grid.SetColumn(img, 2 * j);
					Grid.SetRow(img, 2 * i);
					RootGrid.Children.Add(img);
				}
			}

			//frame horizontal bars
			for (int i = 0; i < 3; i++)
			{
				Image img = new () { Source = Textures.FrameHorizontalBars[i], Stretch = Stretch.Fill };
				Grid.SetColumn(img, 1);
				Grid.SetRow(img, 2 * i);
				RootGrid.Children.Add(img);
			}

			//frame vertical bars
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Image img = new () { Source = Textures.FrameVerticalBars[2 * i + j], Stretch = Stretch.Fill };
					Grid.SetColumn(img, 2 * j);
					Grid.SetRow(img, 2 * i + 1);
					RootGrid.Children.Add(img);
				}
			}

			Timer = new(TimeSpan.FromSeconds(1), DispatcherPriority.Send, (_, _) => TimePassed++, Dispatcher);
			ScaleBinding = new Binding("Value")
			{
				Source = Scale
			};

			ResetGame(9, 9, 10);
		}

		private void FillMinefieldGrid()
		{
			TileImages = new Image[Game.Columns, Game.Rows];

			for (int x = 0; x < Game.Columns; x++)
				Minefield.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

			for (int y = 0; y < Game.Rows; y++)
			{
				Minefield.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

				for (int x = 0; x < Game.Columns; x++)
				{
					Image tile = new()
					{
						Source = Textures.Tiles[(int)TileTexture.Covered]
					};

					tile.SetBinding(WidthProperty, ScaleBinding);
					tile.SetBinding(HeightProperty, ScaleBinding);

					Grid.SetRow(tile, y);
					Grid.SetColumn(tile, x);

					Minefield.Children.Add(tile);
					TileImages[x, y] = tile;
				}
			}
		}

		private void ClearMinefieldGrid()
		{
			Minefield.Children.Clear();
			Minefield.ColumnDefinitions.Clear();
			Minefield.RowDefinitions.Clear();
		}

		private void ResetGame(int columns, int rows, int mineCount)
		{
			Timer.Stop();
			TimePassed = 0;

			Game = new(columns, rows, mineCount);
			Game.TileChangedEvent += (x, y, tile) => TileImages[x, y].Source = tile;

			ClearMinefieldGrid();
			FillMinefieldGrid();
			SmileyButton.Content = ":)";
			MineCounter.Text = Game.MinesLeft.ToString("000");
		}

		private (int x, int y) GetGridPosition(Point point) => ((int)(point.X / Scale.Value), (int)(point.Y / Scale.Value));

		// handles the "pushed down" state of covered tiles before they are actually uncovered
		private void Minefield_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (Game.State != GameState.Playing)
				return;

			// nothing to do if left click is released, everything is handled by the left button up handler
			if (e.LeftButton == MouseButtonState.Released)
				return;

			var position = GetGridPosition(e.GetPosition(Minefield));

			// didn't move, do nothing
			if (position == PreviousPosition)
				return;

			if (PreviousPosition != (-1, -1)
				&& Game.GetTileState(PreviousPosition) == TileState.Covered)
				// "unpushes" the previously pushed tile
				TileImages[PreviousPosition.x, PreviousPosition.y].Source = Textures.Tiles[(int)TileTexture.Covered];

			if (Game.GetTileState(position) == TileState.Covered)
				// pushes the tile at the mouse position
				TileImages[position.x, position.y].Source = Textures.Tiles[(int)TileTexture.CoveredPushed];

			PreviousPosition = position;
		}

		// in case the mouse leaves the grid while left click is pressed
		private void Minefield_OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (Game.State != GameState.Playing)
				return;

			// "unpush" pushed covered tile
			if (PreviousPosition != (-1, -1)
				&& Game.GetTileState(PreviousPosition) == TileState.Covered)
				TileImages[PreviousPosition.x, PreviousPosition.y].Source = Textures.Tiles[(int)TileTexture.Covered];
		}

		private void Minefield_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (Game.State != GameState.Playing)
				return;

			SmileyButton.Content = ":O";
		}

		private void Minefield_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Game.State != GameState.Playing)
				return;

			Timer.Start();
			(int x, int y) = GetGridPosition(e.GetPosition(Minefield));
			Game.UncoverTile(x, y);

			if (Game.State == GameState.Won)
			{
				Timer.Stop();
				SmileyButton.Content = "B)";
			}
			else if (Game.State == GameState.Lost)
			{
				Timer.Stop();
				SmileyButton.Content = "x|";
			}
			else
				SmileyButton.Content = ":)";
		}

		private void Minefield_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Game.State != GameState.Playing)
				return;

			Timer.Start();
			(int x, int y) = GetGridPosition(e.GetPosition(Minefield));
			Game.CycleTileState(x, y);

			MineCounter.Text = Game.MinesLeft >= 0
				? Game.MinesLeft.ToString("000")
				: Game.MinesLeft.ToString("00");
		}

		private void NewGame(object sender, RoutedEventArgs e) => ResetGame(Game.Columns, Game.Rows, Game.MineCount);
		private void NewGameBeginner(object sender, RoutedEventArgs e) => ResetGame(9, 9, 10);
		private void NewGameIntermediate(object sender, RoutedEventArgs e) => ResetGame(16, 16, 40);
		private void NewGameExpert(object sender, RoutedEventArgs e) => ResetGame(30, 16, 99);

		private void NewGameCustom(object sender, RoutedEventArgs e)
		{
			var dialog = new CustomMinefieldDialog(Game.Columns, Game.Rows, Game.MineCount)
			{
				Owner = this
			};

			dialog.ShowDialog();

			// mmm, nullable bools
			if (dialog.DialogResult is true)
				ResetGame(dialog.Columns, dialog.Rows, dialog.MineCount);
		}

		private void SetScale(object sender, RoutedEventArgs e)
		{
			var dialog = new SetScaleDialog(Scale.Value)
			{
				Owner = this
			};

			dialog.ShowDialog();

			// mmm, nullable bools
			if (dialog.DialogResult is true)
				Scale.Value = dialog.Scale;
		}
	}
}
