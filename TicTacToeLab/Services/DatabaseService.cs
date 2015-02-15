using System;
using SQLite;

using TicTacToeLab.Dependancies;

namespace TicTacToeLab.Services
{
    public class DatabaseService : IDatabaseService 
    {
        SQLiteConnection _connection;

        public DatabaseService() :this(Xamarin.Forms.DependencyService.Get<ISQLiteFactory>())
        {
        }

        public DatabaseService(ISQLiteFactory factory)
        {
            _connection = factory.CreateConnection("quote.db");
            Setup ();
        }

        void Setup()
        {
            _connection.CreateTable<XOItem> ();
        }

        public SQLiteConnection Conn
        {
            get { return _connection; }
        }
    }
}

