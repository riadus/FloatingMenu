using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.FloatingMenu.Abstractions;
using CoreGraphics;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Menu
{
    public class FloatingMenuView : UIView
    {
		private readonly CircleViews _circleViews;
		public CGPoint FromPoint { get; set; }
		public FloatingMenuView(IEnumerable<MenuItemContext> items, IMenuStyle menuStyle)
        {
            _circleViews = new CircleViews(async () => await Dismiss(), items, menuStyle);
            FromPoint = new CGPoint(0, 0);
        }

        
		internal async Task Dismiss()
		{
            if(_circleViews == null)
            {
                return;
            }
			await AnimateAsync(0.3,() =>
			{
				_circleViews.Dismiss(RemoveFromSuperview);
				Alpha = 0;
			});
            
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			BuildView();
			AddGestureRecognizer(new UITapGestureRecognizer(async () => await Dismiss())
			{
                Delegate = new GestureDelegate(typeof(CircleView))
			});
		}

        private void BuildView()
		{
			var min = Math.Min(Frame.Width, Frame.Height);
			_circleViews.Frame = new CGRect(FromPoint, new CGSize(min, min));
			_circleViews.Center = FromPoint;
			var deltaX = FromPoint != CGPoint.Empty ? 0 : 10;
			var deltaY = FromPoint != CGPoint.Empty ? 0 : 20;
			_circleViews.Alpha = 0;
			BackgroundColor = UIColor.Black;
			Alpha = 0.8f;
			Add(_circleViews);
			Animate(0.5f, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
			{
				_circleViews.Alpha = 1;
				_circleViews.Center = new CGPoint(Center.X + deltaX, Center.Y + deltaY);
			}, () =>
			{
				Animate(0.5f, () => { _circleViews.Center = Center; });
			});
		}
	}
}
