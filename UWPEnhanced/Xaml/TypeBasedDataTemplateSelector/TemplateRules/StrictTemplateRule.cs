using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Strict <see cref="ITemplateTypeRule"/> that is fulfilled only if data's <see cref="Type"/> exactly matches Type stored in public
	/// property.
	/// </summary>
	public class StrictTemplateTypeRule : BaseTemplateTypeRule, ITemplateTypeRule
	{
		#region Public methods

		/// <summary>
		/// Returns true if <paramref name="type"/> equals the <see cref="Type"/> stored in public property
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public bool Compatible(Type type) => type == null ? throw new ArgumentNullException(nameof(type)) : type == Type;

		#endregion
	}
}