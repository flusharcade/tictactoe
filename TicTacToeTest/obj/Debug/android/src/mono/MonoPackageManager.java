package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	public static void LoadApplication (Context context, String runtimeDataDir, String[] apks)
	{
		synchronized (lock) {
			if (!initialized) {
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = context.getApplicationInfo ().dataDir + "/lib";
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						runtimeDataDir,
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				initialized = true;
			}
		}
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		"TicTacToeLab.Droid.dll",
		"Cirrious.CrossCore.Droid.dll",
		"Cirrious.MvvmCross.Binding.Droid.dll",
		"Cirrious.CrossCore.dll",
		"Cirrious.MvvmCross.Binding.dll",
		"Cirrious.MvvmCross.Localization.dll",
		"Cirrious.MvvmCross.Droid.dll",
		"Cirrious.MvvmCross.dll",
		"Cirrious.MvvmCross.Plugins.Accelerometer.dll",
		"Cirrious.MvvmCross.Plugins.Accelerometer.Droid.dll",
		"Cirrious.MvvmCross.Plugins.Bookmarks.dll",
		"Cirrious.MvvmCross.Plugins.Color.dll",
		"Cirrious.MvvmCross.Plugins.Color.Droid.dll",
		"Cirrious.MvvmCross.Plugins.DownloadCache.dll",
		"Cirrious.MvvmCross.Plugins.DownloadCache.Droid.dll",
		"Cirrious.MvvmCross.Plugins.Email.dll",
		"Cirrious.MvvmCross.Plugins.Email.Droid.dll",
		"Cirrious.MvvmCross.Plugins.File.dll",
		"Cirrious.MvvmCross.Plugins.File.Droid.dll",
		"Newtonsoft.Json.dll",
		"Cirrious.MvvmCross.Plugins.Json.dll",
		"Cirrious.MvvmCross.Plugins.JsonLocalisation.dll",
		"Cirrious.MvvmCross.Plugins.Location.dll",
		"Cirrious.MvvmCross.Plugins.Location.Droid.dll",
		"Cirrious.MvvmCross.Plugins.Messenger.dll",
		"Cirrious.MvvmCross.Plugins.Network.dll",
		"Cirrious.MvvmCross.Plugins.Network.Droid.dll",
		"Cirrious.MvvmCross.Plugins.PhoneCall.dll",
		"Cirrious.MvvmCross.Plugins.PhoneCall.Droid.dll",
		"Cirrious.MvvmCross.Plugins.PictureChooser.dll",
		"Cirrious.MvvmCross.Plugins.PictureChooser.Droid.dll",
		"Cirrious.MvvmCross.Plugins.ReflectionEx.dll",
		"Cirrious.MvvmCross.Plugins.ReflectionEx.Droid.dll",
		"Cirrious.MvvmCross.Plugins.ResourceLoader.dll",
		"Cirrious.MvvmCross.Plugins.ResourceLoader.Droid.dll",
		"Cirrious.MvvmCross.Plugins.Share.dll",
		"Cirrious.MvvmCross.Plugins.Share.Droid.dll",
		"Cirrious.MvvmCross.Plugins.SoundEffects.dll",
		"Cirrious.MvvmCross.Plugins.Sqlite.dll",
		"Cirrious.MvvmCross.Plugins.Sqlite.Droid.dll",
		"Cirrious.MvvmCross.Plugins.ThreadUtils.dll",
		"Cirrious.MvvmCross.Plugins.ThreadUtils.Droid.dll",
		"Cirrious.MvvmCross.Plugins.Visibility.dll",
		"Cirrious.MvvmCross.Plugins.Visibility.Droid.dll",
		"Cirrious.MvvmCross.Plugins.WebBrowser.dll",
		"Cirrious.MvvmCross.Plugins.WebBrowser.Droid.dll",
		"Cirrious.MvvmCross.Plugins.All.dll",
		"AndHUD.dll",
		"GooglePlayServicesLib.dll",
		"TicTacToeLab.dll",
		"System.Collections.Concurrent.dll",
		"System.Collections.dll",
		"System.ComponentModel.Annotations.dll",
		"System.ComponentModel.EventBasedAsync.dll",
		"System.ComponentModel.dll",
		"System.Diagnostics.Contracts.dll",
		"System.Diagnostics.Debug.dll",
		"System.Diagnostics.Tools.dll",
		"System.Dynamic.Runtime.dll",
		"System.Globalization.dll",
		"System.IO.dll",
		"System.Linq.Expressions.dll",
		"System.Linq.Parallel.dll",
		"System.Linq.Queryable.dll",
		"System.Linq.dll",
		"System.Net.NetworkInformation.dll",
		"System.Net.Primitives.dll",
		"System.Net.Requests.dll",
		"System.ObjectModel.dll",
		"System.Reflection.Emit.ILGeneration.dll",
		"System.Reflection.Emit.Lightweight.dll",
		"System.Reflection.Emit.dll",
		"System.Reflection.Extensions.dll",
		"System.Reflection.Primitives.dll",
		"System.Reflection.dll",
		"System.Resources.ResourceManager.dll",
		"System.Runtime.Extensions.dll",
		"System.Runtime.InteropServices.WindowsRuntime.dll",
		"System.Runtime.InteropServices.dll",
		"System.Runtime.Numerics.dll",
		"System.Runtime.Serialization.Json.dll",
		"System.Runtime.Serialization.Primitives.dll",
		"System.Runtime.Serialization.Xml.dll",
		"System.Runtime.dll",
		"System.Security.Principal.dll",
		"System.ServiceModel.Http.dll",
		"System.ServiceModel.Primitives.dll",
		"System.Text.Encoding.Extensions.dll",
		"System.Text.Encoding.dll",
		"System.Text.RegularExpressions.dll",
		"System.Threading.Tasks.Parallel.dll",
		"System.Threading.Tasks.dll",
		"System.Threading.dll",
		"System.Xml.ReaderWriter.dll",
		"System.Xml.XDocument.dll",
		"System.Xml.XmlSerializer.dll",
		"System.Reactive.Linq.dll",
		"System.Reactive.Core.dll",
		"System.Reactive.Interfaces.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = "Mono.Android.Platform.ApiLevel_21";
}
