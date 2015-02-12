using System.Collections;
using System.Collections.Generic;

using Cirrious.MvvmCross.ViewModels;

namespace TicTacToeLab.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
		private string xTitle = "Start as X";
        public string XTitle
		{ 
			get { return xTitle; }
			set { xTitle = value; RaisePropertyChanged(() => XTitle); }
		}

		// command definition
		public MvxCommand<bool> xButtonCommand
		{
			get { return new MvxCommand<bool>(xButtonCommandAction); }
		}

		public void xButtonCommandAction(bool show)
		{
			ShowViewModel<GameViewModel>(new DetailParameters () { 
				PlayerType = XOType.X,
				PlayerTitle = "X",
			});
		}

		// command definition
		public MvxCommand<bool> oButtonCommand
		{
			get { return new MvxCommand<bool>(oButtonCommandAction); }
		}

		public void oButtonCommandAction(bool show)
		{
			ShowViewModel<GameViewModel>(new DetailParameters () { 
				PlayerType = XOType.O,
				PlayerTitle = "O",
			});
		}

		private string oTitle = "Start as O";
		public string OTitle
		{ 
			get { return oTitle; }
			set { oTitle = value; RaisePropertyChanged(() => OTitle); }
		}
    }
}
