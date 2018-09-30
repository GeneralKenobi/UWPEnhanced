using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Lazy <see cref="ITemplateTypeRule"/> that is fulfilled only if data's <see cref="Type"/> is assignable to Type stored in public
	/// property.
	/// </summary>
	public class LazyTemplateTypeRule : BaseTemplateTypeRule, ITemplateTypeRule
	{
		#region Public methods

		/// <summary>
		/// Returns true if <paramref name="type"/> is assignable from <see cref="Type"/> stored in public property
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public bool Compatible(Type type) => type == null ? throw new ArgumentNullException(nameof(type)) : Type.IsAssignableFrom(type);

		#endregion
	}
}