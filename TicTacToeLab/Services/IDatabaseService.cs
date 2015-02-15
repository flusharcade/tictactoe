using System;


namespace TicTacToeLab.Services
{
	public interface IDatabaseService
	{
        SQLiteConnection Conn { get; }
	}

}