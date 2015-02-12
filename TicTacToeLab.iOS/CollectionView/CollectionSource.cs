using UIKit;
using Foundation;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ViewModels;

namespace TicTacToeLab.iOS
{
	public class CollectionSource : MvxCollectionViewSource
	{
		public MvxCommand<XOItemModel> SelectionCommand { get; set; }

		public CollectionSource(UICollectionView collectionView, NSString Identifier): base(collectionView, Identifier)
		{
		}
			
		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = (XOItemModel)GetItemAt(indexPath);

			if (SelectionCommand != null)
				SelectionCommand.Execute(item);
		}
	}
}

