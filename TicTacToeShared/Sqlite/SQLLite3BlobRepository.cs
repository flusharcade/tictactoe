using System;
using System.IO;
using System.Threading.Tasks;

using BodyshopWindows.Ioc;

using TicTacToeLab;
using TicTacToeLab.Interfaces;

namespace TicTacToeShared.Sqlite
{
	public class SqlLite3BlobRepository : IBlobRepository
	{
		private SQLiteAsyncConnection dbAsync;
		private SQLiteConnection db;
		string path;

		public SqlLite3BlobRepository ()
		{
			this.path = Bootstrapper.Container.ResolveNamed<string> (RegisteredNames.DatabasePath.ToString ());
			dbAsync = new SQLiteAsyncConnection (path);
		}

		public async Task SetupDatabase ()
		{
			await dbAsync.CreateTableAsync<Blob> ();
		}

		public async Task SaveAsync<T> (T item) where T : IBlobStorable, new()
		{
			var blob = item.ToBlob ();
			await dbAsync.InsertOrReplaceAsync (blob);
		}

		public async Task<T> LoadAsync<T> (BodyshopBlobKeys key) where T : IBlobStorable, new()
		{
			return await LoadAsync<T> (key.ToString ());
		}

		public async Task<T> LoadAsync<T> (string key) where T : IBlobStorable, new()
		{
			var item = await dbAsync.FindAsync<Blob> (key);
			if (item == default(Blob)) {
				return default(T);
			}
			var result = item.FromBlob<T> ();
			return result;
		}
    }
}