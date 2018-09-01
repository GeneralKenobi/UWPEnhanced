using System.ComponentModel;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Allows to bind to actual width/height on elements and update binding on SizeChanged
	/// </summary>
	public class SizeBindingHelper : VisualAttachment, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public properties
		
		/// <summary>
		/// Actual width of the attached to element
		/// </summary>
		public double Width => AttachedTo == null ? 0 : (AttachedTo as FrameworkElement).ActualWidth;

		/// <summary>
		/// Actual height of the attached to element
		/// </summary>
		public double Height => AttachedTo == null ? 0 : (AttachedTo as FrameworkElement).ActualHeight;

		#endregion

		#region Private methods

		/// <summary>
		/// Invokes property changed event for <see cref="Width"/> and <see cref="Height"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AttachedToSizeChangedCallback(object sender, SizeChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Attatches to the object and if it's a <see cref="FrameworkElement"/> subscribes to its SizeChanged event
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			base.Attach(obj);

			if(obj is FrameworkElement element)
			{
				element.SizeChanged += AttachedToSizeChangedCallback;
			}
		}

		/// <summary>
		/// Unsubscribes from SizeChanged event on <see cref="AttachedTo"/> and detatches
		/// </summary>
		/// <param name="obj"></param>
		public override void Detach()
		{
			if (AttachedTo is FrameworkElement element)
			{
				element.SizeChanged -= AttachedToSizeChangedCallback;
			}

			base.Detach();
		}

		#endregion
	}
}