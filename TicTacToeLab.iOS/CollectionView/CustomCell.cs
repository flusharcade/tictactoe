using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace TicTacToeLab.iOS
{
	public class CustomCell : UICollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CustomCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CustomCell");

		public UIImageView imageView;

		[Export ("initWithFrame:")]
		public CustomCell (System.Drawing.RectangleF frame) : base (frame)
		{
			BackgroundView = new UIView{BackgroundColor = UIColor.Orange};

			SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Green};

			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			ContentView.BackgroundColor = UIColor.White;
			ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);

			imageView = new UIImageView ();
			imageView.Center = ContentView.Center;
			imageView.Transform = CGAffineTransform.MakeScale (0.7f, 0.7f);

			ContentView.AddSubview (imageView);
		}

		public UIImage Image {
			set {
				imageView.Image = value;
			}
		}
	}
}

