using System;

using Cirrious.CrossCore.IoC;

using TicTacToeLab.ViewModels;
using TicTacToeLab.Interfaces;

namespace TicTacToeLab
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
		public static IFileDownloader Downloader;
		public static ImgStorage Storage;
		public static IAppState AppState;
		public static IBlobRepository Datebase;

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