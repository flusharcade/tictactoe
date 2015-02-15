using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TicTacToeLab;
using TicTacToeShared.Sqlite;
using TicTacToeLab.Interfaces;

using BodyshopWindows.Ioc;

namespace TicTacToeShared.State
{
	public class AppState : IAppState
	{
		private readonly IBlobRepository blobRepo;

		public event EventHandler OnPaused;
		public event EventHandler OnResumed;

		public AppState ()
		{
			blobRepo = Bootstrapper.Container.Resolve<IBlobRepository> ();
		}

		public async Task SaveAppState(List<XOItemModel> items, XOType playerTurn)
		{
			var settings = new ApplicationSettings ();

			// save current turn
			settings.PlayerTurn = (int)playerTurn;

			// save all cell states
			foreach (XOItemModel item in items) {
				settings.MarkedTypes [item.Index] = (int)item.Type;
				settings.MarkedItems [item.Index] = item.Marked;
			}

			await blobRepo.SaveAsync (settings);

			System.Diagnostics.Debug.WriteLine ("App state saved.");
		}

		public async Task<XOType> LoadAppState(List<XOItemModel> items)
		{
			var settings = await blobRepo.LoadAsync<ApplicationSettings> (BodyshopBlobKeys.ApplicationSettings.ToString ());
			settings = settings ?? new ApplicationSettings ();

			foreach (XOItemModel item in items)
			{
				item.Marked = settings.MarkedItems [item.Index];
				item.Type = (XOType)settings.MarkedTypes [item.Index];

				switch (item.Type) 
				{
					case XOType.O:
						item.LoadCommand.Execute (App.Storage.OImage);
						break;
					case XOType.X:
						item.LoadCommand.Execute (App.Storage.XImage);
						break;
				}
			}

			System.Diagnostics.Debug.WriteLine ("App state loaded.");

			return (XOType)settings.PlayerTurn;
		}

		public void NotifyOnPaused()
		{
			if (OnPaused != null)
				OnPaused (this, EventArgs.Empty);
		}

		public void NotifyOnResumed()
		{
			if (OnResumed != null)
				OnResumed (this, EventArgs.Empty);
		}
	}
}

