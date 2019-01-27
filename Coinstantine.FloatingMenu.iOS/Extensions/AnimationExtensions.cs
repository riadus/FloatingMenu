using System;
using CoreAnimation;
using Foundation;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Extensions
{
    public static class AnimationExtensions
    {
        public static void PulseToSize(this UIView view, int pixels, double duration, bool repeat)
        {
            var scale = (float)((view.Frame.Width + pixels) / view.Frame.Width);
            CABasicAnimation pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.To = NSNumber.FromFloat(scale);
            pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = repeat == false ? 0 : float.MaxValue;
            pulseAnimation.AutoReverses = true;
            view.Layer.AddAnimation(pulseAnimation, "pulse");
        }

        public static void PulseToSize(this UIView view, float scaleFrom, float scaleTo, double duration, bool repeat)
        {
            CABasicAnimation pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.From = NSNumber.FromFloat(scaleFrom);
            pulseAnimation.To = NSNumber.FromFloat(scaleTo);
            pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = repeat == false ? 0 : float.MaxValue;
            pulseAnimation.AutoReverses = repeat;
            view.Layer.AddAnimation(pulseAnimation, "pulse");
        }

        public static void ExpandToSize(this UIView view, double duration)
        {
            var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.From = NSNumber.FromFloat(0);
            pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = 0;

            view.Layer.AddAnimation(pulseAnimation, "expandToSize");
        }

        public static void ShrinkToEmpty(this UIView view, double duration, Action onCompleted)
        {
            CATransaction.Begin();
            var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.To = NSNumber.FromFloat(0);
            pulseAnimation.From = NSNumber.FromFloat(1);
            pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = 0;
            CATransaction.CompletionBlock = onCompleted;
            view.Layer.AddAnimation(pulseAnimation, "shrinkToEmpty");
            CATransaction.Commit();
        }
    }
}
