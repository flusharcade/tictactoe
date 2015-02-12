// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TicTacToeLab.iOS
{
	[Register ("XOCell")]
	partial class XOCell
	{
		[Outlet]
		UIKit.UIImageView Image { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}
		}
	}
}
