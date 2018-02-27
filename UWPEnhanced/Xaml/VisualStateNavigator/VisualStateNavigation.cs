using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigation : DependencyObject
	{
		#region TransferToState Dependency Property

		/// <summary>
		/// State to transfer to
		/// </summary>
		public string TransferToState
		{
			get => (string)GetValue(TransferToStateProperty);
			set => SetValue(TransferToStateProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TransferToState"/>
		/// </summary>
		public static readonly DependencyProperty TransferToStateProperty =
			DependencyProperty.Register(nameof(TransferToState), typeof(string),
			typeof(VisualStateNavigation), new PropertyMetadata(default(string)));

		#endregion	
	}
}
