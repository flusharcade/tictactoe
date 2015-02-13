using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

using TicTacToeLab.ViewModels;
using MBProgressHUD;

namespace TicTacToeLab.iOS.Views
{
	[Register("GameView")]
    public class GameView : MvxViewController
    {
		private UICollectionView PlayView;
		private UILabel turnLabel;

		GameViewModel model;

        public override void ViewDidLoad()
        {
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.BackgroundColor = UIColor.White;

			base.ViewDidLoad();

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
               EdgesForExtendedLayout = UIRectEdge.None;
				
			PlayView = new UICollectionView(new CGRect(), new TTTLayout()){
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Gray
			};

			turnLabel = new UILabel () {
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center
			};

			var hud = new MTMBProgressHUD (View) {
				LabelText = "Downloading Images...",
				RemoveFromSuperViewOnHide = true
			};
					
			View.AddSubview (PlayView);
			View.AddSubview (turnLabel);
			View.AddSubview (hud);

			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("V:|[PlayView]-20-[turnLabel(==50)]|", NSLayoutFormatOptions.AlignAllCenterX, null, new NSDictionary ("PlayView", PlayView, "turnLabel", turnLabel)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[PlayView]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("PlayView", PlayView)));
			View.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|[turnLabel]|", NSLayoutFormatOptions.AlignAllCenterY, null, new NSDictionary ("turnLabel", turnLabel)));

			var source = new CollectionSource (PlayView, XOCell.Key);
			PlayView.RegisterNibForCell(XOCell.Nib, XOCell.Key);
			PlayView.Source = source;
			PlayView.ReloadData();

			var set = this.CreateBindingSet<GameView, GameViewModel>();
			set.Bind(turnLabel).For("Text").To(vm => vm.TurnTitle);
			set.Bind(source).To(vm => vm.XOItems);
			set.Bind(source).For(s => s.SelectionCommand).To (vm => vm.SelectionCommand); 
            set.Apply();

			model = (GameViewModel)this.DataContext;
			model.LoadedImages += (sender, e) => hud.Hide (animated: true, delay: 0);
			model.End += HandleGameEnd;

			if (!App.Storage.ImagesLoaded)
				hud.Show (animated: true);
        }

		protected void HandleGameEnd (object sender, XOType e)
		{
			InvokeOnMainThread (() => new UIAlertView ("Tic Tac Toe Lab", e.ToString () + " Wins", null, "OK", null).Show ());
		}
    }
}