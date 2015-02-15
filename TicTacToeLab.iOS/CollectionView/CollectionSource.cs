using System;
using System.Drawing;
using System.Security.Cryptography;

using System.Collections.Generic;

using UIKit;
using Foundation;
using CoreAnimation;

using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ViewModels;

namespace TicTacToeLab.iOS
{
	public class CollectionSource : MvxCollectionViewSource
	{
		public MvxCommand<XOItemModel> SelectionCommand { get; set; }
		public List<XOCell> animatingCells;

		public CollectionSource(UICollectionView collectionView, NSString Identifier): base(collectionView, Identifier)
		{
			animatingCells = new List<XOCell>();
		}
			
		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = (XOItemModel)GetItemAt(indexPath);

			if (item.Marked)
				return;

			if (SelectionCommand != null)
				SelectionCommand.Execute(item);

			var cell = (XOCell)collectionView.CellForItem(indexPath);
			animatingCells.Add (cell);
			startQuivering(cell);
		}

		private void startQuivering(XOCell cell)
		{
			CABasicAnimation quiverAnim = CABasicAnimation.FromKeyPath ("transform.rotation");

			float startAngle = ((-2f) * (float)Math.PI / 180.0f);
			float stopAngle = -startAngle; 

			quiverAnim.From = NSNumber.FromFloat (startAngle);
			quiverAnim.To = NSNumber.FromFloat (3 * stopAngle);
			quiverAnim.AutoReverses = true;
			quiverAnim.Duration = 0.2f;
			quiverAnim.RepeatCount = 1000;

			float timeOffset = ((arc4random() % 100f) / 100f - 0.50f);
			quiverAnim.TimeOffset = timeOffset;

			cell.Layer.AddAnimation (quiverAnim, null);
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (XOCell) base.GetCell (collectionView, indexPath);
			var item = (XOItemModel)GetItemAt (indexPath);

			if (item.Marked) {
				animatingCells.Add (cell);
				startQuivering (cell);
			}

			return cell;
		}

		public void StopAllQuivering()
		{
			foreach (UICollectionViewCell cell in animatingCells)
				cell.Layer.RemoveAllAnimations ();
		}

		protected static int arc4random()
		{
			var rngCsp = new RNGCryptoServiceProvider();
			var randomNumber = new byte[4];
			rngCsp.GetBytes(randomNumber); //this fills randomNumber 

			return BitConverter.ToInt32(randomNumber, 0);
		}
	}
}

