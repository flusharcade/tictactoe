using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using TicTacToeLab.ViewModels;
using System.Drawing;

namespace TicTacToeLab.iOS.Views
{
    [Register("FirstView")]
    public class HomeView : MvxViewController
    {
        public override void ViewDidLoad()
        {
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.BackgroundColor = UIColor.White;

            base.ViewDidLoad();

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
               EdgesForExtendedLayout = UIRectEdge.None;
            }
			   
			var xButton = new UIButton () {
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			xButton.SetTitleColor (UIColor.Blue, UIControlState.Normal);

			var oButton = new UIButton () {
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			oButton.SetTitleColor (UIColor.Blue, UIControlState.Normal);

			View.AddSubview (xButton);
			View.AddSubview (oButton);

			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("V:|-20-[xButton]-20-[oButton]", NSLayoutFormatOptions.AlignAllCenterX, null, new NSDictionary ("xButton", xButton, "oButton", oButton)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[xButton]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("xButton", xButton)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[oButton]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("oButton", oButton)));

			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind(xButton).For("Title").To(vm => vm.XTitle);
			set.Bind(xButton).To(vm => vm.xButtonCommand).WithConversion("CommandParameter", true);  
			set.Bind(oButton).For("Title").To(vm => vm.OTitle);
			set.Bind(oButton).To(vm => vm.oButtonCommand).WithConversion("CommandParameter", true);  
		
            set.Apply();
        }

		protected void HandleTouchUpInsideX (object sender, System.EventArgs e)
		{
			var button = sender as UIButton;
			new UIAlertView (button.Title(UIControlState.Normal) + " click"
				, "TouchUpInside Handled"
				, null
				, "OK"
				, null).Show();
		}
    }
}