using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class GenericVisualDataTrigger<T> : VisualAttachment, IVisualTrigger
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
		public T Data
		{
			get => (T)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Data"/>
		/// </summary>
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(nameof(Data), typeof(T),
			typeof(GenericVisualDataTrigger<T>), new PropertyMetadata(default(T), new PropertyChangedCallback(CompareValues)));

		#endregion

		#region CompareTo Dependency Property

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
			typeof(GenericVisualDataTrigger<T>), new PropertyMetadata(default(object), new PropertyChangedCallback(CompareValues)));

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
			typeof(GenericVisualDataTrigger<T>), new PropertyMetadata(ComparisonType.Equal, new PropertyChangedCallback(CompareValues)));

		#endregion

		#region IgnoreNull Dependency Property

		/// <summary>
		/// If true it will ignore null <see cref="Data"/> values in comparisons even if they match the given comparison, for example:
		/// if <see cref="Data"/> == null, <see cref="CompareTo"/> != null and <see cref="ComparisonType"/> is
		/// <see cref="ComparisonType.NotEqual"/> then the trigger won't fire
		/// </summary>
		public bool IgnoreNull
		{
			get => (bool)GetValue(IgnoreNullProperty);
			set => SetValue(IgnoreNullProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IgnoreNull"/>
		/// </summary>
		public static readonly DependencyProperty IgnoreNullProperty =
			DependencyProperty.Register(nameof(IgnoreNull), typeof(bool),
			typeof(GenericVisualDataTrigger<T>), new PropertyMetadata(default(bool)));

		#endregion

		#region Protected Methods

		/// <summary>
		/// Checks if the compared value is null, and if so, carries out the comparison including <see cref="IgnoreNull"/>
		/// Returns true if it determined the result of the comparison and taken appropriate action, false otherwise.
		/// </summary>
		/// <returns></returns>
		protected bool AssertNull()
		{
			if(Data == null)
			{
				if(!IgnoreNull && ((ComparisonType == ComparisonType.Equal && CompareTo==null) ||
					(ComparisonType == ComparisonType.NotEqual && CompareTo != null)))
				{
					Triggered?.Invoke(this, EventArgs.Empty);
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Compares <see cref="Data"/> and <see cref="CompareTo"/> and triggers if they match the <see cref="ComparisonType"/>
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		protected static void CompareValues(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if(s is GenericVisualDataTrigger<T> trigger)
			{
				// Comparison for nulls
				if(trigger.AssertNull())
				{
					return;
				}

				switch (trigger.ComparisonType)
				{
					// Equality is a standard comparison
					case ComparisonType.Equal:
						{
							if(trigger.Data.Equals(trigger.CompareTo))
							{
								trigger.Triggered?.Invoke(trigger, EventArgs.Empty);
							}
						}
						break;

					// Inequality is a standard comparison
					case ComparisonType.NotEqual:
						{
							if (!trigger.Data.Equals(trigger.CompareTo))
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
							// Order comparisons require IComparable type
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
							else if(trigger.CompareTo!=null)
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
