using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	public sealed class MultiDropDownList : Control, INotifyPropertyChanged
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public MultiDropDownList()
		{
			this.DefaultStyleKey = typeof(MultiDropDownList);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Private members

		/// <summary>
		/// Index selected in the list with input parts
		/// </summary>
		private int mInputListSelectedIndex = -1;

		/// <summary>
		/// Index selected in the list with simple logic parts
		/// </summary>
		private int mSimpleLogicListSelectedIndex = -1;

		/// <summary>
		/// Index selected in the list with complex logic parts
		/// </summary>
		private int mComplexLogicListSelectedIndex = -1;

		#endregion

		#region Public properties

		/// <summary>
		/// Index selected in the list with input parts
		/// </summary>
		public int InputListSelectedIndex
		{
			get => mInputListSelectedIndex;
			set
			{
				// Set the value
				mInputListSelectedIndex = value;

				// Uncheck the other lists
				mSimpleLogicListSelectedIndex = -1;
				mComplexLogicListSelectedIndex = -1;

				// Call Property Changed Event
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SimpleLogicListSelectedIndex)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComplexLogicListSelectedIndex)));
			}
		}

		/// <summary>
		/// Index selected in the list with simple logic parts
		/// </summary>
		public int SimpleLogicListSelectedIndex
		{
			get => mSimpleLogicListSelectedIndex;
			set
			{
				// Set the value
				mSimpleLogicListSelectedIndex = value;

				// Uncheck the other lists
				mInputListSelectedIndex = -1;
				mComplexLogicListSelectedIndex = -1;

				// Call Property Changed Event
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputListSelectedIndex)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ComplexLogicListSelectedIndex)));
			}
		}

		/// <summary>
		/// Index selected in the list with complex logic parts
		/// </summary>
		public int ComplexLogicListSelectedIndex
		{
			get => mComplexLogicListSelectedIndex;
			set
			{
				// Set the value
				mComplexLogicListSelectedIndex = value;

				// Uncheck the other lists
				mInputListSelectedIndex = -1;
				mSimpleLogicListSelectedIndex = -1;

				// Call Property Changed Event
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputListSelectedIndex)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SimpleLogicListSelectedIndex)));
			}
		}


		#endregion

		#region Private methods

		/// <summary>
		/// If the clicked item was selected deselects it. Normally it has to be done by ctrl+click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void PartAddingListViewItemClicked(object sender, ItemClickEventArgs e)
		{
			// If the sender is a listview and the clicked item is the selected item
			if (sender is ListView listView && e.ClickedItem == listView.SelectedItem)
			{
				// Wait for a bit because otherwise the item will be reselected
				await Task.Delay(25);

				// Remove the reference to the selected item
				listView.SelectedItem = null;
			}
		}

		#endregion
	}
}