using System;
using SQLite;

namespace TicTacToeLab.Dependancies
{
    public interface ISQLiteFactory
    {
        SQLiteConnection CreateConnection(string dbName);
    }
}

