using System;
using System.Collections;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;

namespace TicTacToeLab
{
	public enum XOType
	{
		X,
		O,
		None
	}

	public class XOItemModel : MvxViewModel
	{
		public int Index = 0;

		private XOType type = XOType.None;
		public XOType Type
		{ 
			get { return type; }
			set { type = value; RaisePropertyChanged(() => Type); }
		}
			
		private bool marked = false;
		public bool Marked
		{ 
			get { return marked; }
			set { marked = value; RaisePropertyChanged(() => Marked); }
		}

		private string imgUrl = "https://www.dropbox.com/sh/gau8ly51aw2yjtd/AAC3thnyl6Hhrh9a8-Dk3E14a/x-mark.png?raw=1&dl=0";
		public string ImgUrl
		{ 
			get { return imgUrl; }
			set { imgUrl = value; RaisePropertyChanged(() => ImgUrl); }
		}

		private byte[] imageData;
		public byte[] ImageData
		{ 
			get { return imageData; }
			set { imageData = value; RaisePropertyChanged(() => ImageData); }
		}

		// command definition
		public MvxCommand<byte[]> LoadCommand
		{
			get { return new MvxCommand<byte[]>(LoadCommandAction); }
		}

		public void LoadCommandAction(byte[] data)
		{
			ImageData = data;
		}
	}
}

