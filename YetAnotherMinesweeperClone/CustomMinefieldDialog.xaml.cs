using System.Windows;

namespace YetAnotherMinesweeperClone
{
	public partial class CustomMinefieldDialog : Window
	{
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public int MineCount { get; private set; }

		public CustomMinefieldDialog(int columns, int rows, int mineCount)
		{
			InitializeComponent();

			WidthInput.Text = columns.ToString();
			HeightInput.Text = rows.ToString();
			MinesInput.Text = mineCount.ToString();
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			if (ValidateText(WidthInput.Text, 3, 1000, "Width")
				&& ValidateText(HeightInput.Text, 3, 1000, "Height")
				&& ValidateText(MinesInput.Text, 1, 200000, "Mines"))
			{
				Columns = int.Parse(WidthInput.Text);
				Rows = int.Parse(HeightInput.Text);
				MineCount = int.Parse(MinesInput.Text);

				DialogResult = true;
			}
		}

		private bool ValidateText(string text, int min, int max, string field)
		{
			if (!int.TryParse(text, out int result))
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
