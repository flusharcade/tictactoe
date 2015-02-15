using System;
using System.Globalization;

using UIKit;

using Cirrious.CrossCore.Converters;

using TicTacToeLab.iOS;

namespace TicTacToeLab.iOS.Converters
{
	public class ByteToUIImageConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			return ImageFunctions.GetImagefromByteArray ((byte[])value);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ImageFunctions.ConvertUIImagetoByteArray((UIImage)value);
		}
	}
}

