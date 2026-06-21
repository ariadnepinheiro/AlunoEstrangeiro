using System;
using System.Collections;
using System.Configuration;
using System.Xml;
using Techne.Data;

namespace Techne.Configuration
{
    public class DatabaseSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object context, XmlNode section)
        {
            return CreateStatic(parent, section, "key", "value");
        }

        internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
        {
            var dic = new ConnectionDictionary();
            var e = section.ChildNodes.GetEnumerator();

            try
            {
                while (e.MoveNext())
                {
                    var xnode = (XmlNode)e.Current;
                    if (xnode.Name == "add")
                    {
                        var ci = new ConnectionInfo();

                        var attrKey = xnode.Attributes["key"];
                        if (attrKey != null)
                        {
                            ci.Name = attrKey.Value;
                        }

                        var attrValue = xnode.Attributes["value"];
                        if (attrValue != null)
                        {
                            ci.ConnectionString = attrValue.Value;
                        }

                        var attrIsdefault = xnode.Attributes["isdefault"];
                        if (attrIsdefault != null)
                        {
                            ci.IsDefault = attrIsdefault.Value == "true";
                        }

                        if (ci.Name.Length > 0)
                        {
                            dic.Add(ci.Name, ci);
                        }
                    }
                }
            }
            finally
            {
                var disp = e as IDisposable;
                if (disp != null)
                {
                    disp.Dispose();
                }
            }

            return dic;
        }
    }
}