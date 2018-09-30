using System;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Helps with assigning <see cref="Type"/> properties in xaml. Works mostly with default types and not user-defined types.
	/// In such cases <see cref="TypeResolver"/> should be used.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(Type))]
	public class TypeSetter : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Type to return as this <see cref="MarkupExtension"/>'s return value.
		/// </summary>
		public Type Type { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns <see cref="Type"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => Type;

		#endregion
	}
}