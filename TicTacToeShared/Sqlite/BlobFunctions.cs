using Newtonsoft.Json;

using System.Text;

using TicTacToeLab.Interfaces;

namespace TicTacToeShared.Sqlite
{
	public static class BlobFunctions
	{
		public static Blob ToBlob (this IBlobStorable item)
		{
			var json = JsonConvert.SerializeObject (item);
			var result = new Blob {
				Key = item.Key,
				Data = json.UnicodeToBytes ()
			};
			return result;
		}

		public static T FromBlob<T> (this Blob blob)
		{
			var text = blob.Data.BytesToUnicode ();
			var result = JsonConvert.DeserializeObject<T> (text);
			return result;
		}

		public static byte[] UnicodeToBytes (this string s)
		{
			return Encoding.Unicode.GetBytes (s);
		}

		public static string BytesToUnicode (this byte[] ba)
		{
			return Encoding.Unicode.GetString (ba, 0, ba.Length);
		}
	}
}