using System;

using System.Collections.Generic;
using System.Threading.Tasks;

using TicTacToeLab;

namespace TicTacToeLab.Interfaces
{
	public interface IAppState
	{
		event EventHandler OnPaused;
		event EventHandler OnResumed;

		Task SaveAppState(List<XOItemModel> items, XOType playerTurn);
		Task<XOType> LoadAppState (List<XOItemModel> items);

		void NotifyOnPaused ();
		void NotifyOnResumed();
	}
}