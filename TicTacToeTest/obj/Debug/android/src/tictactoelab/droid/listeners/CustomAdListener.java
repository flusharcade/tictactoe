package tictactoelab.droid.listeners;


public class CustomAdListener
	extends com.google.android.gms.ads.AdListener
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdLoaded:()V:GetOnAdLoadedHandler\n" +
			"n_onAdClosed:()V:GetOnAdClosedHandler\n" +
			"n_onAdOpened:()V:GetOnAdOpenedHandler\n" +
			"n_onAdFailedToLoad:(I)V:GetOnAdFailedToLoad_IHandler\n" +
			"";
		mono.android.Runtime.register ("TicTacToeLab.Droid.Listeners.CustomAdListener, TicTacToeLab.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomAdListener.class, __md_methods);
	}


	public CustomAdListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CustomAdListener.class)
			mono.android.TypeManager.Activate ("TicTacToeLab.Droid.Listeners.CustomAdListener, TicTacToeLab.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAdLoaded ()
	{
		n_onAdLoaded ();
	}

	private native void n_onAdLoaded ();


	public void onAdClosed ()
	{
		n_onAdClosed ();
	}

	private native void n_onAdClosed ();


	public void onAdOpened ()
	{
		n_onAdOpened ();
	}

	private native void n_onAdOpened ();


	public void onAdFailedToLoad (int p0)
	{
		n_onAdFailedToLoad (p0);
	}

	private native void n_onAdFailedToLoad (int p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
