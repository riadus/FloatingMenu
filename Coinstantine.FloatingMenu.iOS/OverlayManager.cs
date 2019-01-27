using System;
using System.Threading.Tasks;
using Coinstantine.FloatingMenu.iOS.Extensions;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Coinstantine.FloatingMenu
{
    internal abstract class OverlayManager<T> : NSObject where T : UIView
    {
        protected OverlayManager()
        {
            Alpha = 0.95f;
            TapToDismiss = false;
        }
        protected virtual CGRect ViewFrame { get; set; } = CGRect.Null;
        protected abstract bool UseDefaultOverlay { get; }
        protected UIView _overlay;
        protected T View { get; set; }
        protected virtual Task RemoveView(bool animated = false)
        {
            BeginInvokeOnMainThread(() =>
            {
                if (View?.GestureRecognizers != null)
                {
                    foreach (var gesture in View.GestureRecognizers)
                    {
                        View.RemoveGestureRecognizer(gesture);
                    }
                }
                if (animated && View != null)
                {
                    UIView.AnimateNotify(0.5f, 0, 0.3f, 0, UIViewAnimationOptions.TransitionNone, () =>
                    {
                        View.Alpha = 0;
                        View.Transform = CGAffineTransform.MakeScale(0.2f, 0.2f);
                    }, successfulyFinished =>
                    {
                        if (successfulyFinished)
                        {
                            View?.RemoveFromSuperview();
                            View = null;
                        }
                    });
                }
                else
                {
                    View?.RemoveFromSuperview();
                    View = null;
                }
                if (_overlay?.GestureRecognizers != null)
                {
                    foreach (var gesture in _overlay?.GestureRecognizers)
                    {
                        _overlay?.RemoveGestureRecognizer(gesture);
                    }
                }
                _overlay?.RemoveFromSuperview();
                _overlay = null;
            });
            return Task.FromResult(0);
        }

        protected Task ShowView(bool animated = false)
        {
            var bounds = UIApplication.SharedApplication.KeyWindow.RootViewController.View.Frame;
            View.Frame = bounds;
            if (UseDefaultOverlay)
            {
                _overlay = new UIView
                {
                    BackgroundColor = UIColor.Black,
                    Alpha = Alpha,
                    Frame = bounds
                };
            }
            else
            {
                View.Alpha = Alpha;
            }

            if (TapToDismiss)
            {
                var tapGesture = new UITapGestureRecognizer(async () => await RemoveView());
                var overlayTapGesture = new UITapGestureRecognizer(async () => await RemoveView());
                if (UseDefaultOverlay)
                {
                    _overlay.AddGestureRecognizer(overlayTapGesture);
                }
                View.AddGestureRecognizer(tapGesture);
            }
            RemoveAllViewLikeT();
            if (UseDefaultOverlay)
            {
                UIApplication.SharedApplication.KeyWindow.RootViewController.Add(_overlay);
            }
            UIApplication.SharedApplication.KeyWindow.RootViewController.Add(View);
            if (animated)
            {
                View.PulseToSize(0.3f, 1.1f, 0.3f, false);
            }
            return Task.FromResult(0);
        }

        private void RemoveAllViewLikeT()
        {
            _overlay?.RemoveFromSuperview();
            foreach (var view in UIApplication.SharedApplication.KeyWindow.RootViewController.View.Subviews)
            {
                if (view.GetType() == typeof(T))
                {
                    view.RemoveFromSuperview();
                }
            }
        }

        protected nfloat Alpha { get; set; }
        protected bool TapToDismiss { get; set; }
    }
}