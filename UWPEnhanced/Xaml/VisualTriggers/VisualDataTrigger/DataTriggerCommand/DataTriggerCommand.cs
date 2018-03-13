using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class DataTriggerCommand : VisualDataTrigger
	{

		/// <summary>
		/// Default Constructor
		/// </summary>
		public DataTriggerCommand()
		{
			Triggered += (s, e) => Command.Execute(null);
		}

		#region Command Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Command"/>
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(nameof(Command), typeof(ICommand),
			typeof(DataTriggerCommand), new PropertyMetadata(default(ICommand)));

		#endregion

	}
}
