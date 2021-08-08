using System.Windows;

namespace YetAnotherMinesweeperClone
{
	public partial class SetScaleDialog : Window
	{
		public double Scale { get; private set; }

		public SetScaleDialog(double scale)
		{
			InitializeComponent();

			ScaleInput.Text = scale.ToString();
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			if (ValidateText(ScaleInput.Text, 1, 100, "Scale"))
			{
				Scale = double.Parse(ScaleInput.Text);

				DialogResult = true;
			}
		}

		private static bool ValidateText(string text, int min, int max, string field)
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
