using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Converts <see cref="IEnumerable{T}"/> data points (<see cref="KeyValuePair{TKey, TValue}"/> of two doubles) to
	/// <see cref="PointCollection"/>
	/// </summary>
	public class DataPointsToPolylinePointsConverter : IValueConverter
	{
		#region Public methods

		/// <summary>
		/// Converts <see cref="IEnumerable{T}"/> data points (<see cref="KeyValuePair{TKey, TValue}"/> of two doubles) to
		/// <see cref="PointCollection"/>, Y value is negated.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var result = new PointCollection();

			if (value is IEnumerable<KeyValuePair<double, double>> points)
			{
				foreach(var point in points)
				{
					result.Add(new Point(point.Key, -point.Value));
				}
			}

			return result;
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