using System;
using System.Collections.Generic;
using Coinstantine.FloatingMenu.Abstractions;

namespace Coinstantine.FloatingMenu
{
    /// <summary>
    /// Cross platform FloatingMenu implemenations
    /// </summary>
    public static class CrossFloatingMenu
    {
        static Lazy<IFloatingMenu> Implementation =
            new Lazy<IFloatingMenu>(CreateFloatingMenu, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IFloatingMenu Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IFloatingMenu CreateFloatingMenu()
        {
#if NETSTANDARD
            return null;
#else
            return new FloatingMenuImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
                "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
        private static Dictionary<string, string> _fontsDictionnary;
        public static void AddFont(string key, string fontFamily)
        {
            _fontsDictionnary = _fontsDictionnary ?? new Dictionary<string, string>();
            _fontsDictionnary.Add(key, fontFamily);
        }

        public static string GetFontFamily(string key)
        {
            if(!_fontsDictionnary?.ContainsKey(key) ?? true)
            {
                throw new InvalidOperationException("The mapping dictionnary of fonts needs to be initialised. Please use CrossFloatingMenu.AddFont(key, fontFamily)");
            }
            return _fontsDictionnary[key];
        }
    }
}
