using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Command which is triggered by a single <see cref="VisualPointerTrigger"/>
	/// </summary>
	public class PointerTriggerCommand : VisualPointerTrigger
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public PointerTriggerCommand()
		{
			this.Triggered += (s,e) => Command?.Execute(CommandParameter);
		}

		#endregion

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
			typeof(PointerTriggerCommand), new PropertyMetadata(default(ICommand)));

		#endregion

		#region CommandParameter Dependency Property

		/// <summary>
		/// Parameter to pass to the command
		/// </summary>
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="CommandParameter"/>
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register(nameof(CommandParameter), typeof(object),
			typeof(PointerTriggerCommand), new PropertyMetadata(default(object)));

		#endregion
	}
}
