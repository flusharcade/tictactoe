using System;
using Android.Gms.Ads;

namespace TicTacToeLab.Droid.Listeners
{
    class CustomAdListener : AdListener
    {
        // Declare the delegate (if using non-generic pattern). 
        public delegate void AdLoadedEvent();
        public delegate void AdClosedEvent();
        public delegate void AdOpenedEvent();
        public delegate void AdFailedToLoadEvent();

        // Declare the event. 
        public event AdLoadedEvent AdLoaded;
        public event AdClosedEvent AdClosed;
        public event AdOpenedEvent AdOpened;
        public event AdFailedToLoadEvent AdFailedToLoad;

        public override void OnAdLoaded()
        {
            if (AdLoaded != null)
                this.AdLoaded();
            base.OnAdLoaded();
        }

        public override void OnAdClosed()
        {
            if (AdClosed != null)
                this.AdClosed();
            base.OnAdClosed();
        }

        public override void OnAdOpened()
        {
            if (AdOpened != null)
                this.AdOpened();
            base.OnAdOpened();
        }

        public override void OnAdFailedToLoad(int p0)
        {
            if (AdFailedToLoad != null)
                this.AdFailedToLoad();
            base.OnAdFailedToLoad(p0);
        }
    }
}