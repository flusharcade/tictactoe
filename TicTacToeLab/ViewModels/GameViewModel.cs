using System.Collections.Generic;

using Cirrious.MvvmCross.ViewModels;

using System;
using System.Linq;
using System.Reactive.Linq;

namespace TicTacToeLab.ViewModels
{
	public enum GridPos
	{
		TopLeft,
		TopMiddle,
		TopRight,
		Left,
		Middle,
		Right,
		BottomLeft,
		BottomMiddle,
		BottomRight
	}

    public class GameViewModel : MvxViewModel
    {
		const int GRID_SIZE = 9;
		const string STR_EXTENSION = "{0}'s Turn";

		private readonly List<GridPos> POSITIONS = new List<GridPos>(){ GridPos.TopLeft, GridPos.TopMiddle, GridPos.TopRight, GridPos.Left, GridPos.Middle, GridPos.Right, GridPos.BottomLeft, GridPos.BottomMiddle, GridPos.BottomRight};
		private readonly IDictionary<GridPos, List<int>> checkDirections = new Dictionary<GridPos, List<int>> () {
			{GridPos.TopLeft, new List<int>() {1, 4, 3}},
			{GridPos.TopMiddle, new List<int>() {3}},
			{GridPos.TopRight, new List<int>() {-1, 2, 3}},
			{GridPos.Left, new List<int>() {1}},
			{GridPos.Middle, new List<int>() {}}, // we never use this check as we want to start check of rows from left to right
			{GridPos.Right, new List<int>() {-1}},
			{GridPos.BottomLeft, new List<int>() {-3, -2, 1}},
			{GridPos.BottomMiddle, new List<int>() {-3}},
			{GridPos.BottomRight, new List<int>() {-1, -4, -3}},
		};
			
		public event EventHandler LoadedImages;
		public event EventHandler<XOType> WinDetected;
		public event EventHandler<XOType> End;
		public event EventHandler GameLoaded;

		private bool Resume = false;
		private bool GameEnded = false;

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
			
		private IEnumerable<int> getChecks(XOItemModel item)
		{
			return checkDirections [item.Position]
				.Where (x => XOItems [x + item.Index].Marked);
		}

		private void checkForRows()
		{
			XOItems
				.Where(x => x.Marked && x.Index != 4)
				.Foreach (x =>
			{
					getChecks(x)
					.Foreach(d =>
					{
						if ((x.Type == XOItems [x.Index + d].Type))
							if (((x.Index + d) + d) >= 0 && ((x.Index + d) + d) < XOItems.Count)
								if (x.Type == XOItems [(x.Index + d) + d].Type)
										if (WinDetected != null)
											WinDetected(this, x.Type);

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

			if (Resume)
				loadGameState ();
		}

		private void NotifyGameEnd(XOType winner)
		{
			System.Diagnostics.Debug.WriteLine("End Result: " + winner + " wins.");

			GameEnded = true;

			// disable anymore touches
			XOItems.Foreach (x => x.Marked = true);

			if (End != null)
				End(this, winner);
		}

		public void SaveGameState()
		{
			if (!GameEnded)
				App.AppState.SaveAppState (XOItems, PlayerTurn);
		}

		private async void loadGameState()
		{
			PlayerTurn = await App.AppState.LoadAppState (XOItems);
			setTurn ();

			if (GameLoaded != null)
				GameLoaded(this, EventArgs.Empty);
		}
			
		public void Init(DetailParameters parameters)
		{
			Observable.FromEventPattern<XOType> (this, "WinDetected")
				.Throttle (TimeSpan.FromSeconds (0.1))
				.Subscribe (e => NotifyGameEnd(e.EventArgs));

			App.Storage.Loaded += (sender, e) => NotifyImagesLoaded ();
			App.AppState.OnPaused += (sender, e) => SaveGameState();

			// init
			PlayerTurn = parameters.PlayerTurn;
			Resume = parameters.ResumeGame;

			if (App.Storage.ImagesLoaded && Resume)
				loadGameState ();

			setTurn ();

			// create nine items
			for (var i = 0; i < GRID_SIZE; i++)
				XOItems.Add(new XOItemModel() {Index = i, Position = POSITIONS[i]});
		}
    }
}
