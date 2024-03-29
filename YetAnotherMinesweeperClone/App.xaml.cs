﻿using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using YetAnotherMinesweeperClone.Texture;

namespace YetAnotherMinesweeperClone
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			BitmapImage skin = new();
			skin.BeginInit();
			skin.StreamSource = assembly.GetManifestResourceStream("YetAnotherMinesweeperClone.Assets.FullSkin.png");
			skin.EndInit();
			Textures.MapSkin(skin);
		}
	}
}
