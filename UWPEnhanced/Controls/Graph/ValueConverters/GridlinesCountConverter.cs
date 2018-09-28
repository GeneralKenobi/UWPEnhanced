using System;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Converts the number of labels to number of gridlines to procedurally generate. For values equal to or below 2 returns 1
	/// and for values above 2 returns value - 1 (One gridline is always created non-procedurally).
	/// </summary>
	public class GridlinesCountConverter : IValueConverter
	{
		#region Public methods

		/// <summary>
		/// Converts the number of labels to number of gridlines to procedurally generate. For values equal to or below 2 returns 1
		/// and for values above 2 returns value - 1 (One gridline is always created non-procedurally).
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is int i && i >= 2)
			{
				return i - 1;
			}

			return 1;
		}

		/// <summary>
		/// Not implemtented
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