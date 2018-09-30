using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for classes implementing <see cref="ITemplateTypeRule"/>
	/// </summary>
	public abstract class BaseTemplateTypeRule
	{
		#region Public properties

		/// <summary>
		/// The template to use when this rule is compatible with data's type
		/// </summary>
		public DataTemplate Template { get; set; }

		/// <summary>
		/// The type that is used to determine compatibility with data's type
		/// </summary>
		public Type Type { get; set; }

		#endregion
	}
}