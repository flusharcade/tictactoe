using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Locations;
using Android.Gms.Ads;

using Cirrious.MvvmCross.Droid.Views;

using TicTacToeLab.ViewModels;
using Android.Views;
using TicTacToeLab.Droid.Listeners;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace TicTacToeLab.Droid
{
	[Activity(Label = "GameView", ScreenOrientation = ScreenOrientation.Portrait)]
	public class GameView : MvxActivity
	{
		AdView adView;

		public new GameViewModel ViewModel
		{
			get { return (GameViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.GameView);

			LoadAd();
		}

		protected override void OnPause ()
		{
			App.AppState.NotifyOnPaused ();

			base.OnPause ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		private void LoadAd()
		{
			adView = FindViewById<AdView>(Resource.Id.adView);

			var customAdListener = new CustomAdListener();
			adView.AdListener = customAdListener;
			adView.SetLayerType(LayerType.Software, null); // required due to an android graphics bug

			var adRequest = new AdRequest.Builder();
			adRequest.AddTestDevice("47CBFBCF6326163AE1714420FC4C4610");
			adRequest.AddKeyword("travel");
			adView.LoadAd(adRequest.Build());
		}

		public void OnProviderDisabled (string provider) {}
		public void OnProviderEnabled (string provider) {}
		public void OnStatusChanged (string provider, Availability status, Bundle extras) {}
	}
}