using System;
using System.Collections.Generic;
using System.Linq;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.iOS.Extensions;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Menu
{
    public class CircleViews : UIView
    {
        private readonly UIColor _color;
        private readonly Action _dismissAction;
        private readonly IMenuStyle _menuStyle;
        private readonly List<MenuItemContext> _items;
        private readonly List<CircleView> _views;

        public CircleViews(Action dismissAction, IEnumerable<MenuItemContext> items, IMenuStyle menuStyle)
        {
            _color = menuStyle.CircleColor.ToUIColor();
            _dismissAction = dismissAction;
            _menuStyle = menuStyle;
            _items = items.ToList();
            _views = new List<CircleView>();
        }

		private int DetermineNumberOfCircles(int numberOfItems) => (numberOfItems / 2) + 1 + (numberOfItems % 2);

        private void BuildView()
        {
			var numberOfCircles = DetermineNumberOfCircles(_items.Count);
			var smalestRadius =  (Frame.Size.Width / numberOfCircles) / 2;
			var smalestAlpha = 1.0f / (numberOfCircles + 1);
			var iconWidth = 30;
			var loops = 1;
			var items = 0;
            var rightLimit = 0d;

			for (var i = numberOfCircles - 1; i >= 0; i--)
			{
				var factor = ((i + 1) * 2) - 1;

                var cercle = new CircleView(smalestRadius * factor, ((float)factor) / 10, _menuStyle)
                {
                    Center = Center,
                    BackgroundColor = _color,
                    Alpha = smalestAlpha * loops
                };
                if (loops == 1)
                {
                    rightLimit = cercle.Width;
                }
				if (i != 0)
				{
                    cercle.AddItemOnTop(new ItemView(iconWidth, _menuStyle.Fonts)
                    {
                        DataContext = _items[items],
                        RightLimit = rightLimit
					});
					items++;
					if (loops != 1 || _items.Count % 2 == 0)
					{
                        cercle.AddItemOnBottom(new ItemView(iconWidth, _menuStyle.Fonts)
                        {
                            DataContext = _items[items],
                            RightLimit = rightLimit
                        });
						items++;
					}
				}
				else
				{
					cercle.WithCloseOption = true;
					cercle.DismissAction = _dismissAction;
				}
				loops++;
				_views.Add(cercle);
			}
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            BackgroundColor = UIColor.Clear;
			BuildView();
            foreach (var view in _views)
            {
                Add(view);
                view.LayoutSubviews();
            }
        }
        
        internal void Dismiss(Action onCompletedAction)
        {
            foreach (var view in _views)
            {
                if (view == _views.Last())
                {
                    view.Dismiss(() =>
                    {
                        RemoveFromSuperview();
                        onCompletedAction?.Invoke();
                    });
                }
                else
                {
                    view.Dismiss();
                }
            }
            _views.Clear();
        }
    }
}
