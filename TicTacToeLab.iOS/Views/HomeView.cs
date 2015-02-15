using System.Drawing;

using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

using TicTacToeLab.ViewModels;

using GoogleAdMobAds;

namespace TicTacToeLab.iOS.Views
{
    [Register("FirstView")]
    public class HomeView : MvxViewController
    {
		private UIView BannerContainerView;
		private GADBannerView adView;
		private bool viewOnScreen = false;

		const string AdmobID = "ca-app-pub-4310988678591963/6547639332";

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

			var oButton = new UIButton () {
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			var resumeButton = new UIButton () {
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			xButton.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			oButton.SetTitleColor (UIColor.Blue, UIControlState.Normal);
			resumeButton.SetTitleColor (UIColor.Blue, UIControlState.Normal);

			BannerContainerView = new UIView () {
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Black
			};

			View.AddSubview (BannerContainerView);
			View.AddSubview (xButton);
			View.AddSubview (oButton);
			View.AddSubview (resumeButton);

			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("V:|-20-[BannerContainerView(==50)]-20-[xButton]-20-[oButton]-20-[resumeButton]", NSLayoutFormatOptions.AlignAllCenterX, null, new NSDictionary ("BannerContainerView", BannerContainerView, "xButton", xButton, "oButton", oButton, "resumeButton", resumeButton)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[BannerContainerView]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("BannerContainerView", BannerContainerView)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[xButton]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("xButton", xButton)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[oButton]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("oButton", oButton)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[resumeButton]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("resumeButton", resumeButton)));

			loadAdMob ();

			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind(xButton).For("Title").To(vm => vm.XTitle);
			set.Bind(xButton).To(vm => vm.XButtonCommand).WithConversion("CommandParameter", true);  
			set.Bind(oButton).For("Title").To(vm => vm.OTitle);
			set.Bind(oButton).To(vm => vm.OButtonCommand).WithConversion("CommandParameter", true); 
			set.Bind(resumeButton).For("Title").To(vm => vm.ResumeTitle);
			set.Bind(resumeButton).To(vm => vm.ResumeCommand).WithConversion("CommandParameter", true);  
            set.Apply();
        }


		private void loadAdMob()
		{
			adView = new GADBannerView (size: GADAdSizeCons.Banner, origin: new CGPoint (0, 0)) {
				AdUnitID = AdmobID,
				RootViewController = this
			};

			adView.DidReceiveAd += (sender, args) => {
				if (!viewOnScreen) BannerContainerView.AddSubview (adView);
				viewOnScreen = true;
			};
				
			GADRequest request = GADRequest.Request;
			request.TestDevices = new string[] { UIDevice.CurrentDevice.IdentifierForVendor.ToString () };
			adView.LoadRequest (request);
		}
    }
}