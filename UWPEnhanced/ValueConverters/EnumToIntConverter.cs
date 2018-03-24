using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
    public class EnumToIntConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is Enum e)
			{
				return (int)value;
			}
			else
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (parameter is string typeName)
			{
				Type candidateType = Type.GetType(typeName);

				if(candidateType.IsEnum)
				{
					try
					{
						return Enum.ToObject(candidateType, value);
					}
					catch (Exception) { }
				}
			}
			
			return null;			
		}
	}
}
