using Android.Content;

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Converters;

using TicTacToeLabShared.Services;
using TicTacToeShared.Converters;

namespace TicTacToeLab.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

		protected override void FillValueConverters (IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters (registry);
			registry.AddOrOverwrite("ByteToUIImageConverter", new ByteToBitmapConverter());
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