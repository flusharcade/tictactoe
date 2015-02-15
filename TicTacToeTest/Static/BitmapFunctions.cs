using System;
using System.IO;

using Android.Graphics;

namespace TicTacToeLab.Droid.Static
{
	public class BitmapFunctions
	{
		public static Bitmap GetBitmapfromByteArray (byte[] imageBuffer)
		{
			return BitmapFactory.DecodeByteArray(imageBuffer, 0, imageBuffer.Length);;
		}

		public static byte[] ConvertBitmaptoByteArray (Bitmap image)
		{
			byte[] bitmapData;

			using (var stream = new MemoryStream())
			{
				image.Compress(Bitmap.CompressFormat.Png, 0, stream);
				bitmapData = stream.ToArray();
			}

			return bitmapData;
		}
	}
}

