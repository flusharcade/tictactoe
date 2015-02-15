
using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Locations;
using Android.Gms.Ads;

using Cirrious.MvvmCross.Droid.Views;

using TicTacToeLab.ViewModels;
using Android.Views;
using TicTacToeLab.Droid.Listeners;

namespace TicTacToeLab.Droid
{
	[Activity(Label = "HomeView", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class FirstView : MvxActivity
	{
		AdView adView;

		public new HomeViewModel ViewModel
		{
			get { return (HomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.HomeView);

			LoadAd();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		private void LoadAd()
		{
			adView = FindViewById<AdView>(Resource.Id.adView);
			if (adView == null)
				return;

			var customAdListener = new CustomAdListener();
			customAdListener.AdLoaded += () => {
				System.Console.WriteLine("Ad Loaded!");
			};
			customAdListener.AdFailedToLoad += () => {
				System.Console.WriteLine("Ad Failed to Load!");
			};

			adView.AdListener = customAdListener;
			adView.SetLayerType(LayerType.Software, null); // required due to an android graphics bug

			var adRequest = new AdRequest.Builder();
			adRequest.AddTestDevice("47CBFBCF6326163AE1714420FC4C4610");
			adRequest.AddKeyword("travel");
			adView.LoadAd(adRequest.Build());
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		public void OnProviderDisabled (string provider) {}
		public void OnProviderEnabled (string provider) {}
		public void OnStatusChanged (string provider, Availability status, Bundle extras) {}
	}
}