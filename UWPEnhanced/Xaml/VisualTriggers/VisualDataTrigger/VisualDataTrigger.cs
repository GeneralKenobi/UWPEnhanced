using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualDataTrigger : VisualAttachment, IVisualTrigger
	{
		#region Triggered Event Handler

		/// <summary>
		/// Event fired when this triggered is triggered
		/// </summary>
		public EventHandler Triggered { get; set; }

		#endregion

		#region Data Dependency Property

		/// <summary>
		/// Data to compare
		/// </summary>
		public object Data
		{
			get => GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Data"/>
		/// </summary>
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(nameof(Data), typeof(object),
			typeof(VisualDataTrigger), new PropertyMetadata(default(object), new PropertyChangedCallback(CompareValues)));

		#endregion

		#region EqualTo Dependency Property

		/// <summary>
		/// Value to compare the data to
		/// </summary>
		public object CompareTo
		{
			get => GetValue(EqualToProperty);
			set => SetValue(EqualToProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="CompareTo"/>
		/// </summary>
		public static readonly DependencyProperty EqualToProperty =
			DependencyProperty.Register(nameof(CompareTo), typeof(object),
			typeof(VisualDataTrigger), new PropertyMetadata(default(object), new PropertyChangedCallback(CompareValues)));

		#endregion

		#region ComparisonType Dependency Property

		/// <summary>
		/// Type of comparison to employ
		/// </summary>
		public ComparisonType ComparisonType
		{
			get => (ComparisonType)GetValue(ComparisonTypeProperty);
			set => SetValue(ComparisonTypeProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ComparisonType"/>
		/// </summary>
		public static readonly DependencyProperty ComparisonTypeProperty =
			DependencyProperty.Register(nameof(ComparisonType), typeof(ComparisonType),
			typeof(VisualDataTrigger), new PropertyMetadata(ComparisonType.Equal, new PropertyChangedCallback(CompareValues)));

		#endregion

		#region IgnoreNulls Dependency Property

		/// <summary>
		/// If true it will ignore null values in comparisons even if they match the given comparison, for example:
		/// if <see cref="Data"/> == null, <see cref="CompareTo"/> == null and <see cref="ComparisonType"/> is
		/// <see cref="ComparisonType.Equal"/> then the trigger won't fire
		/// </summary>
		public bool IgnoreNulls
		{
			get => (bool)GetValue(IgnoreNullsProperty);
			set => SetValue(IgnoreNullsProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IgnoreNulls"/>
		/// </summary>
		public static readonly DependencyProperty IgnoreNullsProperty =
			DependencyProperty.Register(nameof(IgnoreNulls), typeof(bool),
			typeof(VisualDataTrigger), new PropertyMetadata(default(bool)));

		#endregion

		#region Private methods

		//private bool AssertNull()
		//{

		//}

		private static void CompareValues(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if(s is VisualDataTrigger trigger)
			{
				switch (trigger.ComparisonType)
				{
					case ComparisonType.Equal:
						{
							if (trigger.Data == null)
							{
								if (trigger.CompareTo == null)
								{
									trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
								}
							}
							else if (trigger.Data.Equals(trigger.CompareTo))
							{
								trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
							}


						}
						break;

					case ComparisonType.NotEqual:
						{
							if (trigger.Data == null)
							{
								if (trigger.CompareTo != null)
								{
									trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
								}
							}
							else if (!trigger.Data.Equals(trigger.CompareTo))
							{
								trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
							}

						}
						break;

					case ComparisonType.Greater:
					case ComparisonType.GreaterOrEqual:
					case ComparisonType.Smaller:
					case ComparisonType.SmallerOrEqual:
						{
							if(trigger.Data==null || trigger.CompareTo==null)
							{
								return;
							}

							if (trigger.Data is IComparable comparableData && trigger.CompareTo is IComparable comparableTo)
							{
								switch (trigger.ComparisonType)
								{
									case ComparisonType.Greater:
										{
											if (comparableData.CompareTo(comparableTo) > 0)
											{
												trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
											}
										}
										break;

									case ComparisonType.GreaterOrEqual:
										{
											if (comparableData.CompareTo(comparableTo) >= 0)
											{
												trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
											}
										}
										break;

									case ComparisonType.Smaller:
										{
											if (comparableData.CompareTo(comparableTo) < 0)
											{
												trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
											}
										}
										break;

									case ComparisonType.SmallerOrEqual:
										{
											if (comparableData.CompareTo(comparableTo) <= 0)
											{
												trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
											}
										}
										break;
								}
							}
							else
							{
								throw new ArgumentException(nameof(Data) + " and " + nameof(CompareTo) + " should be " +
									nameof(IComparable));
							}
						}
						break;
				}
			}
		}

		#endregion
	}
}
