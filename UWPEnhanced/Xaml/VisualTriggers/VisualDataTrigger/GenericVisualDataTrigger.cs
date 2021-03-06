﻿using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Generic implementation of the <see cref="IVisualTrigger"/>. If the implicit type conversion does not work, a more
	/// specific class may be created simply by declaring public class Foo : GenericVisualDataTrigger of MyType.
	/// This is a simpler <see cref="IValueConverter"/>.
	/// It will also allow xaml to work with proper input type, for example in case of enums.
	/// This is sort of a workaround because UWP xaml does not support generic classes but a class inheriting
	/// the generic one with specific type is fine.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericVisualDataTrigger<T> : VisualAttachment, IVisualTrigger
	{
		#region Triggered Event Handler

		/// <summary>
		/// Event fired when this triggered is triggered. Argument contains the value that caused the trigger
		/// </summary>
		public EventHandler<object> Triggered { get; set; }

		#endregion

		#region Data Dependency Property

		/// <summary>
		/// Data to compare
		/// </summary>
		public virtual T Data
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
		public T CompareTo
		{
			get => (T)GetValue(EqualToProperty);
			set => SetValue(EqualToProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="CompareTo"/>
		/// </summary>
		public static readonly DependencyProperty EqualToProperty =
			DependencyProperty.Register(nameof(CompareTo), typeof(T),
			typeof(GenericVisualDataTrigger<T>), new PropertyMetadata(default(T), new PropertyChangedCallback(CompareValues)));

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
					Triggered?.Invoke(this, new VisualDataTriggerEventArgs<T>(Data));
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
								trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
							}
						}
						break;

					// Inequality is a standard comparison
					case ComparisonType.NotEqual:
						{
							if (!trigger.Data.Equals(trigger.CompareTo))
							{
								trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
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
												trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
											}
										}
										break;

									case ComparisonType.GreaterOrEqual:
										{
											if (comparableData.CompareTo(comparableTo) >= 0)
											{
												trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
											}
										}
										break;

									case ComparisonType.Smaller:
										{
											if (comparableData.CompareTo(comparableTo) < 0)
											{
												trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
											}
										}
										break;

									case ComparisonType.SmallerOrEqual:
										{
											if (comparableData.CompareTo(comparableTo) <= 0)
											{
												trigger.Triggered?.Invoke(trigger, new VisualDataTriggerEventArgs<T>(trigger.Data));
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