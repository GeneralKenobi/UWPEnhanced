using CSharpEnhanced.Helpers;
using System;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Helps with assigning <see cref="Type"/> properties in xaml
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(Type))]
	public class TypeResolver : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Name of the type to return as this <see cref="MarkupExtension"/>'s return value.
		/// </summary>
		public string TypeName { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns <see cref="Type"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => TypeHelpers.GetType(TypeName);

		#endregion
	}
}