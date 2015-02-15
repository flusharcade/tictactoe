using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.CrossCore.Converters;

using UIKit;

using TicTacToeLab.Interfaces;

using TicTacToeLabShared.Services;
using TicTacToeLab.iOS.Converters;

namespace TicTacToeLab.iOS
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
		{
		}

		protected override void FillValueConverters (IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters (registry);
			registry.AddOrOverwrite("ByteToUIImageConverter", new ByteToUIImageConverter());
		}

		protected override IMvxApplication CreateApp()
		{
			SetupService.Initialise ();
			
			return new TicTacToeLab.App();
		}
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
	}
}