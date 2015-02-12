using Cirrious.CrossCore.IoC;

using TicTacToeLab.ViewModels;

namespace TicTacToeLab
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
		public static IFileDownloader Downloader;
		public static ImgStorage Storage;

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<HomeViewModel>();
        }
    }
}