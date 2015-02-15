using System;
using System.Globalization;

using Android.Graphics;

using Cirrious.CrossCore.Converters;

using TicTacToeLab.Droid.Static;

namespace TicTacToeShared.Converters
{
	public class ByteToBitmapConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			return BitmapFunctions.GetBitmapfromByteArray ((byte[])value);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return BitmapFunctions.ConvertBitmaptoByteArray((Bitmap)value);
		}
	}
}

