using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Almost exactly the same as <see cref="VisualSetup"/> except when TransitionIn is complete it will request
	/// a transition to the <see cref="NextSetup"/> <see cref="VisualSetup"/> from its parent <see cref="VisualSetupGroup"/>
	/// </summary>
	public class VisualSetupTemporary : VisualSetup
	{
		#region NextSetup Dependency Property

		/// <summary>
		/// Next setup to transition to
		/// </summary>
		public string NextSetup
		{
			get => (string)GetValue(NextSetupProperty);
			set => SetValue(NextSetupProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="NextSetup"/>
		/// </summary>
		public static readonly DependencyProperty NextSetupProperty =
			DependencyProperty.Register(nameof(NextSetup), typeof(string),
			typeof(VisualSetupTemporary), new PropertyMetadata(default(string)));

		#endregion

		#region Public Methods

		/// <summary>
		/// After TransitionIn task completes it is followed up with request to transition to NextSetup.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public override async Task TransitionIn(VisualTransitionType type, bool useTransitions = true)
		{
			await base.TransitionIn(type, useTransitions);
			Parent?.GoToSetup(NextSetup, useTransitions);
		}

		#endregion
	}
}
