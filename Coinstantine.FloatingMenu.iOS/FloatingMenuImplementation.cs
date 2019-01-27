using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.iOS.Menu;
using CoreGraphics;
using UIKit;

namespace Coinstantine.FloatingMenu
{
    internal class FloatingMenuImplementation : OverlayManager<FloatingMenuView>, IFloatingMenu
    {
        private UIImpactFeedbackGenerator _impact;
        private IMenuStyle _menuStyle;
        protected override bool UseDefaultOverlay => false;

        public FloatingMenuImplementation()
        {
            TapToDismiss = false;
            Alpha = 0.8f;
            BeginInvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    _impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
                    _impact.Prepare();
                }
            });
        }

        public Task HideMenu()
        {
            return RemoveView();
        }

        protected override Task RemoveView(bool animated = false)
        {
            if (View == null)
            {
                return Task.FromResult(0);
            }
            return View.Dismiss();
        }

        public Task ShowMenu(IEnumerable<MenuItemContext> items)
        {
            if (View?.Superview != null)
            {
                return Task.FromResult(0);
            }
            CreateViewIfNeeded(items);
            View.FromPoint = CGPoint.Empty;

            return ShowView();
        }

        protected virtual void CreateViewIfNeeded(IEnumerable<MenuItemContext> items)
        {
            if (View == null)
            {
                View = new FloatingMenuView(items, _menuStyle)
                {
                    FromPoint = new CGPoint(0, 0)
                };
            }
        }

        public Task ShowMenuFrom(IEnumerable<MenuItemContext> items, TouchLocation touchLocation)
        {
            if (View?.Superview != null)
            {
                return Task.FromResult(0);
            }
            View = new FloatingMenuView(items, _menuStyle)
            {
                FromPoint = new CGPoint(touchLocation.X, touchLocation.Y)
            };

            _impact?.ImpactOccurred();

            return ShowView();
        }

        public void SetStyle(IMenuStyle style)
        {
            _menuStyle = style;
        }
    }
}