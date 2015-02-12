using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;

namespace TicTacToeLab.Droid
{
	public class FileDownloader : IFileDownloader
	{
		public async Task<byte[]> GetFile(string url)
		{
			HttpWebRequest request = new HttpWebRequest(new Uri(url));

			request.Method = "GET";

			byte[] data = null;

			try
			{
				HttpWebResponse httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
				using (Stream responseStream = httpResponse.GetResponseStream())
				{
					using (MemoryStream ms = new MemoryStream())
					{
						responseStream.CopyTo(ms);
						data = ms.ToArray();
					}

					System.Diagnostics.Debug.WriteLine("Downloaded Image: " + data.Length);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine ("Error with downloading file - " + ex);
			}

			return data;
		}
	}
}

