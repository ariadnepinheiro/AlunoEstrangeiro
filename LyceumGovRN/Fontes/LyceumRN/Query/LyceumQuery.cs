using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data;
using Techne.Web;

namespace Techne.Lyceum.RN.Query
{
    public abstract class LyceumQuery : TSearchSettings
    {
        public LyceumQuery()
        {
        this.QueryTypeName = this.GetType().FullName;
        this.QueryMethod = "GetData";
        }

        protected Techne.Data.TConnection CreateConnection()
        {
          return Techne.Lyceum.Config.CreateConnection();
        }
        public abstract DataView GetData(IDictionary<string, object> pars, object key, int maxRows);

        protected string ContainsExpression(string parValue)
        {
            if (parValue == null || parValue.Trim().Length == 0)
                return "";
            parValue = parValue.Replace("'", "\"").Replace("%", "*");

            List<string> expressions = new List<string>();
            int nExpr=0;
            int pos = 0;
            while(parValue.IndexOf("\"",pos)>-1)
            {
                int start = parValue.IndexOf('\"');
                int end = parValue.IndexOf('\"',start+1);
                if (end == -1)
                {
                    parValue+="\"";
                    end = parValue.Length-1;
                }
                expressions.Add(parValue.Substring(start,end-start+1));
                parValue=parValue.Remove(start, end - start + 1);
                parValue=parValue.Insert(start, " \"" + nExpr.ToString() + "\" ");
                pos = start + 4 + nExpr.ToString().Length;
                nExpr++;
            }

            StringBuilder strContains=new StringBuilder();
            string[] words = parValue.Split(' ');
            
            foreach (string w in words)
            {
                    // Retira strings com 1 caracter ou vazias
                    if (string.IsNullOrEmpty(w))
                        continue;
                    string word = w;
                    if (word.StartsWith("\""))
                    {
                        word = word.Replace("\"", "");
                        int i = -1;
                        try
                        {
                            i = int.Parse(word);
                            word = expressions[i];
                        }
                        catch { }
                    }
                    else
                    {
                        word = word.TrimStart('*').Replace("&", "").Replace("|", "").Replace("~", "");
                        word = "\"" + word + "*\"";
                    }
                    if (string.IsNullOrEmpty(word) || word.Replace("\"", "").Trim().Length == 0)
                        continue;
                    if (strContains.Length > 0)
                        strContains.Append(" and ");
                    strContains.Append(word);
            }

            return strContains.ToString();
        }

        protected string LikeExpression(string parValue)
        {
            if (parValue == null || parValue.Trim().Length == 0)
                return "%";
            if (!parValue.EndsWith("%"))
                parValue += "%";
            parValue=parValue.Replace("*", "%");
            return parValue;
        }

        protected bool HasValue(IDictionary<string, object> pars, string key)
        {
            return pars.ContainsKey(key)
                   && pars[key] != null
                   && pars[key].ToString().Trim().Length > 0;
        }
    }
}
