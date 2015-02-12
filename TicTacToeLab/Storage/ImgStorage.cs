using System;

namespace TicTacToeLab
{
	public class ImgStorage
	{
		public byte[] XImage;
		public byte[] OImage;

		public async void LoadImgs ()
		{
			XImage = await App.Downloader.GetFile ("https://www.dropbox.com/sh/gau8ly51aw2yjtd/AAC3thnyl6Hhrh9a8-Dk3E14a/x-mark.png?raw=1&dl=1");
			OImage = await App.Downloader.GetFile ("https://www.dropbox.com/sh/gau8ly51aw2yjtd/AABY6e5OF5kqxnnatqaEdx8za/o-mark.png?raw=1&dl=0");
		}
	}
}

