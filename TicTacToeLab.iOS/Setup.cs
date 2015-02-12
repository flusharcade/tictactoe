using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.CrossCore.Converters;

namespace TicTacToeLab.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override void FillValueConverters (IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters (registry);
			registry.AddOrOverwrite("ByteToUIImageConverter", new ByteToUIImageConverter());
		}

		protected override IMvxApplication CreateApp()
		{
			App.Downloader = new FileDownloader ();
			App.Storage = new ImgStorage ();
			App.Storage.LoadImgs ();
			
			return new TicTacToeLab.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
	}
}