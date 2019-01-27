using System;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.iOS.Extensions;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Menu
{
    public class CircleView : UIView
    {
        private readonly nfloat _radius;
        private readonly float _timingOffset;
        private readonly IMenuStyle _style;
        private ItemView _topItem;
        private ItemView _bottomItem;
        public nfloat Width => _radius + (2 * _padding);
        public CircleView(nfloat radius, float timingOffset, IMenuStyle style)
        {
            _radius = radius;
            _timingOffset = timingOffset;
            _style = style;
        }

        public void AddItemOnTop(ItemView item)
        {
            _topItem = item;
        }

        public void AddItemOnBottom(ItemView item)
        {
            _bottomItem = item;
        }

		private readonly int _padding = 20;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Frame = GetFrame();
            this.PulseToSize(5, 1.5 + _timingOffset, true);
            this.ExpandToSize(0.3f + _timingOffset);
            var view = new UIView
            {
                BackgroundColor = BackgroundColor,
                Alpha = Alpha,
                Frame = GetViewFrame()
            };
            view.Layer.CornerRadius = _radius / 2;
            Add(view);

            BackgroundColor = UIColor.Clear;
            Alpha = 1;

            if (WithCloseOption)
            {
                DrawCross(view);
            }
            if (_topItem != null)
            {
                var itemDelta = (_topItem.IconWidth / 2);
				_topItem.PreferedLocation = new CGPoint((_radius / 2) - itemDelta + _padding, _padding - itemDelta);
                Add(_topItem);
                _topItem.LayoutSubviews();
            }

            if (_bottomItem != null)
            {
                var itemDelta = (_bottomItem.IconWidth / 2);
                _bottomItem.PreferedLocation = new CGPoint((_radius / 2) - itemDelta + _padding, _radius - itemDelta + _padding);
                Add(_bottomItem);
                _bottomItem.LayoutSubviews();
            }
        }

        private CGRect GetViewFrame()
        {
            if (WithCloseOption)
            {
                return new CGRect(0, 0, _radius, _radius);
            }

            return new CGRect(_padding, _padding, _radius, _radius);
        }

        private CGRect GetFrame()
        {
            if (WithCloseOption)
            {
                var x = Center.X- (_radius / 2);
                var y = Center.Y- (_radius / 2);
                x -= Superview.Frame.X / 2;
                y -= Superview.Frame.Y / 2;
                var location = new CGPoint(x,y);
                return new CGRect(location, new CGSize(Width / 2, Width / 2));
            }
            else
            {
                var x = Center.X - (_radius / 2);
                var y = Center.Y - (_radius / 2);
                x -= Superview.Frame.X / 2;
                y -= Superview.Frame.Y / 2;
                var location = new CGPoint(x - _padding, y - _padding);
                return new CGRect(location, new CGSize(Width, Width));
            }

        }

        private void DrawCross(UIView parent)
        {
            var shapeLayer = new CAShapeLayer
            {
                Path = CrossPath().CGPath,
                CornerRadius = 5,
                StrokeColor = _style.CrossColor.ToCGColor(),
                FillColor = _style.CrossColor.ToCGColor(),
                LineWidth = 3
            };
            AddGestureRecognizer(new UITapGestureRecognizer(DismissAction));
            Layer.AddSublayer(shapeLayer);
        }

        private UIBezierPath CrossPath()
        {
            var height = 4;
            var width = 4;
            var centerX = Math.Ceiling(Frame.Width / 2);
            var centerY = Math.Ceiling(Frame.Height / 2);
            centerX += Math.Ceiling(centerX - (Frame.Width / 2));
            centerY += Math.Ceiling(centerY - (Frame.Height / 2));
            var x1 = centerX - width;
            var y1 = centerY - height;

            var x2 = centerX + width;
            var y2 = centerY + height;

            var path = new UIBezierPath();
            path.MoveTo(new CGPoint(x1, y1));
            path.AddLineTo(new CGPoint(x2, y2));

            path.MoveTo(new CGPoint(x2, y1));
            path.AddLineTo(new CGPoint(x1, y2));

            path.ClosePath();
            return path;
        }

        internal void Dismiss(Action onCompletedAction = null)
        {
            var action = new Action(async () =>
            {
                await Task.Delay(100);
                RemoveFromSuperview();
                onCompletedAction?.Invoke();
            });
            this.ShrinkToEmpty(0.3f + _timingOffset, action);
        }

        public bool WithCloseOption { get; set; }
        public Action DismissAction { get; set; }

        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            foreach (var subview in Subviews.Where(x => x is ItemView))
            {
                var subPoint = subview.ConvertPointFromView(point, this);
                var view = subview.HitTest(subPoint, uievent);
                if (view != null)
                {
                    return view;
                }
            }
            return base.HitTest(point, uievent);
        }
    }
}
