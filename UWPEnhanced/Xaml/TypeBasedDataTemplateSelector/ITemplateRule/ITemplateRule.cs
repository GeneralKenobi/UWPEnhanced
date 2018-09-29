﻿using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for classes that are used in <see cref="TypeBasedDataTemplateSelector"/> to determine matching templates
	/// </summary>
	public interface ITemplateRule
	{
		#region Properties

		/// <summary>
		/// Template to use whan a <see cref="Type"/> is compatible with this <see cref="ITemplateRule"/>
		/// </summary>
		DataTemplate Template { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Returns true if <paramref name="type"/> fulfills this <see cref="ITemplateRule"/>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		bool Compatible(Type type);

		#endregion
	}
}