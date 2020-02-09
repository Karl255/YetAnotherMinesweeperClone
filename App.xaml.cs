using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YetAnotherMinesweeperClone
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			BitmapImage skin = new BitmapImage(new Uri("Assets/FullSkin.png", UriKind.Relative));
			Textures.MapSkin(skin);
		}
	}
}
