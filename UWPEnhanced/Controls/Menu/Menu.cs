﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
    public sealed class Menu : Control, INotifyPropertyChanged
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Menu()
        {
            this.DefaultStyleKey = typeof(Menu);
			t();
        }

		private async Task t()
		{
			//await Task.Delay(1000);
			VisualStateManager.GoToState(this, "LeftOpen", false);
			await Task.Delay(3000);
			VisualStateManager.GoToState(this, "LeftClosed", false);
			await Task.Delay(3000);
			VisualStateManager.GoToState(this, "LeftOpen", false);
		}

		#endregion


		public event PropertyChangedEventHandler PropertyChanged;

		#region MenuPositionProperty Dependency Property

		/// <summary>
		/// Position of the menu
		/// </summary>
		public MenuPosition MenuPositionProperty
		{
			get => (MenuPosition)GetValue(MenuPositionPropertyProperty);
			set => SetValue(MenuPositionPropertyProperty, value);
		}

		/// <summary>
		/// Position of the menu
		/// </summary>
		public static readonly DependencyProperty MenuPositionPropertyProperty =
			DependencyProperty.Register(nameof(MenuPositionProperty), typeof(MenuPosition),
			typeof(Menu), new PropertyMetadata(MenuPosition.Left));

		#endregion

		#region IconsPanelLength Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public double IconsPanelLength
		{
			get => (double)GetValue(IconsPanelLengthProperty);
			set => SetValue(IconsPanelLengthProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IconsPanelLength"/>
		/// </summary>
		public static readonly DependencyProperty IconsPanelLengthProperty =
			DependencyProperty.Register(nameof(IconsPanelLength), typeof(double),
			typeof(Menu), new PropertyMetadata(100d));

		#endregion

		#region ContentLength Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public double ContentLength
		{
			get => (double)GetValue(ContentLengthProperty);
			set => SetValue(ContentLengthProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ContentLength"/>
		/// </summary>
		public static readonly DependencyProperty ContentLengthProperty =
			DependencyProperty.Register(nameof(ContentLength), typeof(double),
			typeof(Menu), new PropertyMetadata(100d, new PropertyChangedCallback((s,e)=>
			(s as Menu)?.PropertyChanged?.Invoke(s, new PropertyChangedEventArgs("ContentLength")))));

		#endregion


	}
}
