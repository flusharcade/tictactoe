using TicTacToeLab.Interfaces;
using TicTacToeLab;

namespace TicTacToeLabShared.Services
{
	using TicTacToeShared.Modules;

	using BodyshopWindows.Ioc;

	public class SetupService
	{
		public static void Initialise ()
		{
			var bootstrapper = Bootstrapper.Instance;
			bootstrapper.Modules.Clear ();
			bootstrapper.Modules.Add (new SharedFrameworkModule ());
			bootstrapper.Start ();

			App.Downloader = Bootstrapper.Container.Resolve<IFileDownloader> ();
			App.AppState = Bootstrapper.Container.Resolve<IAppState> ();
			App.Datebase = Bootstrapper.Container.Resolve<IBlobRepository> ();
			App.Datebase.SetupDatabase ();

			App.Storage = new ImgStorage ();
			App.Storage.LoadImgs ();
		}
	}
}