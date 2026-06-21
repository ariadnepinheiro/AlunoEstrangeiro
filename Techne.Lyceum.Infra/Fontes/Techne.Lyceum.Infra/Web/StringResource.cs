using System.Globalization;
using System.Resources;
using System.Threading;

namespace Techne.Web
{
    internal class StringResource
    {
        private readonly ResourceManager resources;

        private static StringResource loader;

        private static object s_InternalSyncObject;

        internal StringResource()
        {
            this.resources = new ResourceManager("Techne.Web.AssemblyResources", this.GetType().Assembly);
        }

        public static ResourceManager Resources
        {
            get
            {
                return GetLoader().resources;
            }
        }

        // Properties
        private static CultureInfo Culture
        {
            get
            {
                return null;
            }
        }

        private static object InternalSyncObject
        {
            get
            {
                if (s_InternalSyncObject == null)
                {
                    var obj2 = new object();
                    Interlocked.CompareExchange(ref s_InternalSyncObject, obj2, null);
                }

                return s_InternalSyncObject;
            }
        }

        public static string GetString(string name)
        {
            try
            {
                var loader = GetLoader();
                if (loader == null)
                {
                    return string.Empty;
                }

                return loader.resources.GetString(name, Culture);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetString(string name, params object[] args)
        {
            var loader = GetLoader();
            if (loader == null)
            {
                return null;
            }

            var format = loader.resources.GetString(name, Culture);
            if ((args == null) || (args.Length <= 0))
            {
                return format;
            }

            for (var i = 0; i < args.Length; i++)
            {
                var str2 = args[i] as string;
                if ((str2 != null) && (str2.Length > 0x400))
                {
                    args[i] = str2.Substring(0, 0x3fd) + "...";
                }
            }

            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        private static StringResource GetLoader()
        {
            if (loader == null)
            {
                lock (InternalSyncObject)
                {
                    if (loader == null)
                    {
                        loader = new StringResource();
                    }
                }
            }

            return loader;
        }
    }
}