﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YetAnotherMinesweeperClone
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public BindableValue<double> Scale = new BindableValue<double>(20);

		public MainWindow()
		{
			InitializeComponent();

			Binding scaleBinding = new Binding("Value")
			{
				Source = Scale
			};

			for (int i = 0; i < 9; i++)
			{
				Minefield.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
			}

			for (int i = 0; i < 9; i++)
			{
				Minefield.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
				for (int j = 0; j < 9; j++)
				{
					Image tile = new Image()
					{
						Source = Textures.GetRandomTile()
					};
					tile.SetBinding(WidthProperty, scaleBinding);
					tile.SetBinding(HeightProperty, scaleBinding);
					Grid.SetRow(tile, i);
					Grid.SetColumn(tile, j);
					Minefield.Children.Add(tile);
				}
			}
		}

		private void Minefield_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var pos = e.GetPosition(Minefield);
			pos.X = (int)(pos.X / Scale.Value);
			pos.Y = (int)(pos.Y / Scale.Value);

			//var element = Minefield.Children.Cast<UIElement>().First(e => Grid.GetColumn(e) == pos.X && Grid.GetRow(e) == pos.Y) as Image;
			
		}

		private void SmileButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
