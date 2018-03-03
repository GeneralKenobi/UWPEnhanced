using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigationPointerTrigger : VisualStateNavigationTrigger
	{

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualStateNavigationPointerTrigger()
		{
			TriggerTypes = new ObservableCollection<PointerTriggerEvent>();
			TriggerTypes.CollectionChanged += TriggerTypesCollectionChanged;
			InitializeRoutine();
		}

		private async Task InitializeRoutine()
		{
			await Task.Delay(10);
		}

		private void TriggerTypesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			while(TriggerTypes.Contains(0))
			{
				TriggerTypes.Remove(0);
			}

			// TODO: Make TriggerTypes unique

			if (TriggerFor == null)
			{
				return;
			}

			foreach(var item in e.NewItems)
			{
				switch (item)
				{
					case PointerTriggerEvent.PointerCanceled:
						{
							TriggerFor.PointerCanceled += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerCaptureLost:
						{
							TriggerFor.PointerCaptureLost += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerEntered:
						{
							TriggerFor.PointerEntered += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerExited:
						{
							TriggerFor.PointerExited += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerMoved:
						{
							TriggerFor.PointerMoved += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerPressed:
						{
							TriggerFor.PointerPressed += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerReleased:
						{
							TriggerFor.PointerReleased += FireNavigate;
						}
						break;

					case PointerTriggerEvent.PointerWheelChanged:
						{
							TriggerFor.PointerWheelChanged += FireNavigate;
						}
						break;
				}
			}
		}

		private void FireNavigate(object sender, PointerRoutedEventArgs e) => FireNavigate();


		#region TriggerFor Dependency Property

		/// <summary>
		/// Pointer events are captured for this <see cref="UIElement"/>
		/// </summary>
		public UIElement TriggerFor
		{
			get => (UIElement)GetValue(TriggerForProperty);
			set => SetValue(TriggerForProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TriggerFor"/>
		/// </summary>
		public static readonly DependencyProperty TriggerForProperty =
			DependencyProperty.Register(nameof(TriggerFor), typeof(UIElement),
			typeof(VisualStateNavigationPointerTrigger), new PropertyMetadata(default(UIElement),
				new PropertyChangedCallback(TriggerForChanged)));

		private static void TriggerForChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is VisualStateNavigationPointerTrigger t && e.NewValue is UIElement el)
			{
				t.TriggerTypes.Add(0);
			}
		}

		#endregion

		#region TriggerTypes Dependency Property

		/// <summary>
		/// PointerEvents for which to trigger
		/// </summary>
		public ObservableCollection<PointerTriggerEvent> TriggerTypes
		{
			get => (ObservableCollection<PointerTriggerEvent>)GetValue(TriggerTypesProperty);
			set => SetValue(TriggerTypesProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TriggerTypes"/>
		/// </summary>
		public static readonly DependencyProperty TriggerTypesProperty =
			DependencyProperty.Register(nameof(TriggerTypes), typeof(ObservableCollection<PointerTriggerEvent>),
			typeof(VisualStateNavigationPointerTrigger),
			new PropertyMetadata(null,
				new PropertyChangedCallback(TriggerTypesChanged)));

		private static void TriggerTypesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			
		}

		

		#endregion
	}

	public enum PointerTriggerEvent
	{
		PointerCanceled = 1,
		PointerCaptureLost = 2,
		PointerEntered = 3,
		PointerExited = 4,
		PointerMoved = 5,
		PointerPressed = 6,
		PointerReleased = 7,
		PointerWheelChanged = 8,
	}
}
