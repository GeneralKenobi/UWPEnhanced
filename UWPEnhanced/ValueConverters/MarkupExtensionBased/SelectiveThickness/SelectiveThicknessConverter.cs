using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class SelectiveThicknessMEC : MarkupExtension
	{
		/// <summary> 
		/// If value is a <see cref="Thickness"/>, returns a new thickness with all entries marked in <see cref="SelectedSides"/> equal to
		/// their respective entries in value and all entries not marked in <see cref="SelectedSides"/> equal to 0.
		/// If value is not a Thickenss, returns thickness with all entries equal to 0.
		/// </summary>
		private class SelectiveThicknessConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public SelectiveThicknessConverter(Side selectedSides)
			{
				_SelectedSides = selectedSides;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Sides that whose values are returned (all others are set to 0)
			/// </summary>
			private Side _SelectedSides { get; }

			#endregion

			#region Public methods

			/// <summary> 
			/// If value is a <see cref="Thickness"/>, returns a new thickness with all entries marked in <see cref="SelectedSides"/> equal to
			/// their respective entries in value and all entries not marked in <see cref="SelectedSides"/> equal to 0.
			/// If value is not a Thickenss, returns thickness with all entries equal to 0.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (value is Thickness thickness)
				{
					return new Thickness(
						_SelectedSides.HasFlag(Side.Left) ? thickness.Left : 0,
						_SelectedSides.HasFlag(Side.Top) ? thickness.Top : 0,
						_SelectedSides.HasFlag(Side.Right) ? thickness.Right : 0,
						_SelectedSides.HasFlag(Side.Bottom) ? thickness.Bottom : 0);
				}

				return new Thickness();
			}

			/// <summary>
			/// Not implemented
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object ConvertBack(object value, Type targetType, object parameter, string language)
			{
				throw new NotImplementedException();
			}

			#endregion
		}
	}
}