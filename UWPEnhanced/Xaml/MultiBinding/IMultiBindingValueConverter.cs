namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Converter combining 3 values into one for <see cref="MultiBinding"/>
	/// </summary>
	public interface IMultiBindingValueConverter
	{
		/// <summary>
		/// Converts three objects into one
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="value3"></param>
		/// <returns></returns>
		object Convert(object value1, object value2, object value3, object parameter);
	}
}