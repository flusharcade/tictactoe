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
		public MvxCommand XButtonCommand
		{
			get { return new MvxCommand(xButtonCommandAction); }
		}

		private void xButtonCommandAction()
		{
			ShowViewModel<GameViewModel>(new DetailParameters () { 
				PlayerTurn = XOType.X,
				PlayerTitle = "X",
				ResumeGame = false,
			});
		}

		// command definition
		public MvxCommand OButtonCommand
		{
			get { return new MvxCommand(oButtonCommandAction); }
		}

		private void oButtonCommandAction()
		{
			ShowViewModel<GameViewModel>(new DetailParameters () { 
				PlayerTurn = XOType.O,
				PlayerTitle = "O",
				ResumeGame = false,
			});
		}

		private string oTitle = "Start as O";
		public string OTitle
		{ 
			get { return oTitle; }
			set { oTitle = value; RaisePropertyChanged(() => OTitle); }
		}

		// command definition
		public MvxCommand ResumeCommand
		{
			get { return new MvxCommand (resumeCommandAction); }
		}

		private void resumeCommandAction()
		{
			ShowViewModel<GameViewModel>(new DetailParameters () { 
				PlayerTurn = XOType.O,
				PlayerTitle = "O",
				ResumeGame = true,
			});
		}

		private string resumeTitle = "Resume";
		public string ResumeTitle
		{ 
			get { return resumeTitle; }
			set { resumeTitle = value; RaisePropertyChanged(() => resumeTitle); }
		}
    }
}
