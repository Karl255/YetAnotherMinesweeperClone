using System.ComponentModel;

namespace YetAnotherMinesweeperClone
{
	public class BindableValue<T> : INotifyPropertyChanged
	{
		private T _value;
		public T Value
		{
			get => _value;
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public BindableValue() { }
		public BindableValue(T value) => _value = value;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
