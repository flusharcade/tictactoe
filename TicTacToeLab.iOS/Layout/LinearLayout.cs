using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;

namespace TicTacToeLab.iOS
{
	public class TTTLayout : UICollectionViewFlowLayout
	{
		public TTTLayout ()
		{
			MinimumInteritemSpacing = 10;
			MinimumLineSpacing = 10;
		}

		public override bool ShouldInvalidateLayoutForBoundsChange (CGRect newBounds)
		{
			return true;
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect (CGRect rect)
		{
			var array = base.LayoutAttributesForElementsInRect (rect);

			ItemSize = new CGSize (CollectionView.Bounds.Width / 4, CollectionView.Bounds.Height / 4);

			return array;
		}
	}
}

