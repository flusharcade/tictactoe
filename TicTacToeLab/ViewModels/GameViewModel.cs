using System.Collections.Generic;

using Cirrious.MvvmCross.ViewModels;
using System;

namespace TicTacToeLab.ViewModels
{
    public class GameViewModel : MvxViewModel
    {
		const int GRID_SIZE = 9;

		public event EventHandler LoadedImages;

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
			
		// command definition
		public MvxCommand<XOItemModel> SelectionCommand
		{
			get { return new MvxCommand<XOItemModel>(SelectionCommandAction); }
		}

		public void SelectionCommandAction(XOItemModel item)
		{
			PlayerTurn = (PlayerTurn == XOType.O) ? XOType.X : XOType.O;

			if (item.LoadCommand != null)
				item.LoadCommand.Execute(App.Storage.OImage);
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
			PlayerType = parameters.PlayerType;
			TurnTitle = string.Format (TurnTitle, parameters.PlayerTitle);

			// create nine items
			for (var i = 0; i < GRID_SIZE; i++)
				XOItems.Add(new XOItemModel());
		}
    }
}
