using System;

using UIKit;
using Foundation;

namespace TicTacToeLab.iOS
{
	public class ImageFunctions
	{
		public static UIImage GetImagefromByteArray (byte[] imageBuffer)
		{
			NSData imageData = NSData.FromArray(imageBuffer);
			return UIImage.LoadFromData(imageData);
		}

		public static byte[] ConvertUIImagetoByteArray (UIImage image)
		{
			using (NSData imageData = image.AsPNG()) {
				byte[] data = new byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, data, 0, Convert.ToInt32(imageData.Length));
				return data;
			}
		}
	}
}

