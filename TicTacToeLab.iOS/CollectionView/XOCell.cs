using System;

using Foundation;
using UIKit;

using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.CrossCore.Converters;
using System.Globalization;

namespace TicTacToeLab.iOS
{
	public partial class XOCell : MvxCollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("XOCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("XOCell");

		public XOCell (IntPtr handle) : base (handle)
		{
			BackgroundColor = UIColor.LightGray;

			this.DelayBind (() => {
				var set = this.CreateBindingSet<XOCell, XOItemModel>();
				set.Bind(Image).For(i => i.Image).To (xo => xo.ImageData).WithConversion("ByteToUIImageConverter");
				set.Apply ();
			});
		}

		public static XOCell Create ()
		{
			return (XOCell)Nib.Instantiate (null, null) [0];
		}
	}

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

