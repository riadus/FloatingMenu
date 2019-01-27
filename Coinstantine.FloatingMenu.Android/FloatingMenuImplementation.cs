using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.Android.Views;
using Plugin.CurrentActivity;

namespace Coinstantine.FloatingMenu
{
    internal class FloatingMenuImplementation : IFloatingMenu
    {
        public Task ShowMenuFrom(IEnumerable<MenuItemContext> items, TouchLocation touchLocation)
        {
            return Task.FromResult(0);
        }

        private const string Tag = "MenuManager";
        readonly object _locker = new object();
        private Activity _currentContext;
        private IMenuStyle _menuStyle;
        public Task HideMenu()
        {
            return HideFragment();
        }

        public Task ShowMenu(IEnumerable<MenuItemContext> items)
        {
            _currentContext = CrossCurrentActivity.Current.Activity;
            _currentContext.RunOnUiThread(() =>
            {
                lock (_locker)
                {
                    var fragment = new FloatingMenuFragment(_currentContext, items, _menuStyle, HideFragment);
                    fragment.Show(CrossCurrentActivity.Current.Activity.FragmentManager, Tag);
                }
            });

            return Task.FromResult(0);
        }

        private Task HideFragment()
        {
            _currentContext = _currentContext ?? CrossCurrentActivity.Current.Activity;
            _currentContext.RunOnUiThread(() =>
            {
                lock (_locker)
                {
                    if (_currentContext != null)
                    {
                        using (var fragmentTransaction = _currentContext.FragmentManager.BeginTransaction())
                        {
                            _currentContext.FragmentManager.ExecutePendingTransactions();
                            var dialogFragment = (DialogFragment)_currentContext.FragmentManager.FindFragmentByTag(Tag);
                            if (dialogFragment != null)
                            {
                                dialogFragment.DismissAllowingStateLoss();
                                fragmentTransaction.Remove(dialogFragment);
                            }
                            _currentContext = null;
                        }
                    }
                }
            });
            return Task.FromResult(0);
        }

        public void SetStyle(IMenuStyle style)
        {
            _menuStyle = style;
        }

    }
}