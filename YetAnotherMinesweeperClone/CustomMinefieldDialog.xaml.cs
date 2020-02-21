using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YetAnotherMinesweeperClone
{
	public partial class CustomMinefieldDialog : Window
	{
		public int Columns { get; set; }
		public int Rows { get; set; }
		public int Mines { get; set; }

		public CustomMinefieldDialog()
		{
			InitializeComponent();
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			if (ValidateText(WidthInput.Text, 3, 1000, "Width")
				&& ValidateText(HeightInput.Text, 3, 1000, "Height")
				&& ValidateText(MinesInput.Text, 1, 200000, "Mines"))
			{
				Columns = int.Parse(WidthInput.Text);
				Rows = int.Parse(HeightInput.Text);
				Mines = int.Parse(MinesInput.Text);

				DialogResult = true;
			}
		}
		
		private bool ValidateText(string text, int min, int max, string field)
		{
			int result;

			if (!int.TryParse(text, out result))
			{
				MessageBox.Show("Input is not a number: " + field);
				return false;
			}

			if (result < min || result > max)
			{
				MessageBox.Show("Input out of range: " + field);
				return false;
			}

			return true;
		}
		
	}
}
