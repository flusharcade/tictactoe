using System.Collections.Generic;

using Cirrious.MvvmCross.ViewModels;

using System;
using System.Reactive;
using System.Linq;

namespace TicTacToeLab.ViewModels
{
    public class GameViewModel : MvxViewModel
    {
		const int GRID_SIZE = 9;
		const string STR_EXTENSION = "{0}'s Turn";

		public event EventHandler LoadedImages;
		public event EventHandler<XOType> EndResult;

		private XOType playerTurn;
		public XOType PlayerTurn
		{ 
			get { return playerTurn; }
			set { playerTurn = value; RaisePropertyChanged(() => PlayerTurn); }
		}

		private XOType playerType;
		public XOType PlayerType
		{ 
			get { return playerType; }
			set { playerType = value; RaisePropertyChanged(() => PlayerType); }
		}

		private string turnTitle = "{0}'s Turn";
		public string TurnTitle
		{ 
			get { return turnTitle; }
			set { turnTitle = value; RaisePropertyChanged(() => TurnTitle); }
		}

		private List<XOItemModel> xOItems = new List<XOItemModel>();
		public List<XOItemModel> XOItems
		{
			get { return xOItems; }
			set { xOItems = value; RaisePropertyChanged(() => xOItems); }
		}

		private byte[] turnImage;
			
		// command definition
		public MvxCommand<XOItemModel> SelectionCommand
		{
			get { return new MvxCommand<XOItemModel>(SelectionCommandAction); }
		}

		public void SelectionCommandAction(XOItemModel item)
		{
			if (item.Marked)
				return;
				
			item.Marked = true;
			item.Type = PlayerTurn;

			checkForRows ();

			PlayerTurn = (PlayerTurn == XOType.O) ? XOType.X : XOType.O;

			setTurn ();

			if (item.LoadCommand != null)
				item.LoadCommand.Execute(turnImage);
		}
			
		private IEnumerable<int> checkDirections(int index)
		{
			IList<int> checks = new List<int> ();

			// check all directions, if cell marked, we can check
			checks.Add (((index - 3) < XOItems.Count && (index - 3) >= 0) ? index - 3 : -1);
			checks.Add (((index + 3) < XOItems.Count && (index + 3) >= 0) ? index + 3 : -1);
			checks.Add (((index - 1) < XOItems.Count && (index - 1) >= 0) ? index - 1 : -1);
			checks.Add (((index + 1) < XOItems.Count && (index + 1) >= 0) ? index + 1 : -1);
			checks.Add (((index - 4) < XOItems.Count && (index - 4) >= 0) ? index - 4 : -1);
			checks.Add (((index + 4) < XOItems.Count && (index + 4) >= 0) ? index + 4 : -1);
			checks.Add (((index - 2) < XOItems.Count && (index - 2) >= 0) ? index - 2 : -1);
			checks.Add (((index + 2) < XOItems.Count && (index + 2) >= 0) ? index + 2 : -1);

			return checks;
		}

		private void checkForRows()
		{
			XOItems.Foreach(x => 
			{
				int matches = 0;

				checkDirections(x.Index)
					.Where(i => (i != -1))
					.Foreach(d =>
					{
						if ((x.Type == XOItems [d].Type) && (XOItems [d].Type != XOType.None))
						{
							matches++;
							
							if (matches == 2)
								if (EndResult != null)
									EndResult(this, x.Type);
						}
						else
							matches--;
					});
			});
		}

		private void setTurn()
		{
			TurnTitle = string.Format (STR_EXTENSION, PlayerTurn.ToString ());
			turnImage = (PlayerTurn == XOType.X) ? App.Storage.OImage : App.Storage.XImage;
		}

		private void NotifyImagesLoaded()
		{
			if (LoadedImages != null)
				LoadedImages(this, EventArgs.Empty);
		}

		public void Init(DetailParameters parameters)
		{
			App.Storage.Loaded += (sender, e) => NotifyImagesLoaded ();

			// init
			PlayerTurn = parameters.PlayerTurn;
			setTurn ();

			// create nine items
			for (var i = 0; i < GRID_SIZE; i++)
				XOItems.Add(new XOItemModel() {Index = i});
		}
    }
}
