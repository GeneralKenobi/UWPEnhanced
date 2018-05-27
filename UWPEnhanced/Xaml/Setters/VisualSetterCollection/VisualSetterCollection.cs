using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// This class's purpose is to be able to create such collection in xaml because generic classes cannot be created in xaml
	/// </summary>
	public class VisualSetterCollection : DependencyObjectCollectionOfT<IVisualSetter>
	{

	}
}
