namespace Techne.Lyceum.RN.Cache
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using System.Security.Principal;
    using System.Data;

    public static class TabelaGeral
    {
        private const string Prefixo = "__CACHE_TABELA_GERAL_";
        private static bool UsarCache
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["UsarCache"] ?? "false");
            }
        }

        public static void Limpar()
        {
            var cache = HttpRuntime.Cache;
            var keys = cache
                .Cast<DictionaryEntry>()
                .Where(x => x.Key.ToString().StartsWith(Prefixo))
                .Select(x => x.Key.ToString())
                .ToList();

            foreach (var key in keys)
            {
                cache.Remove(key);
            }
        }

        public static object SelecionarItens(string tabela)
        {
            if (string.IsNullOrEmpty(tabela))
            {
                return null;
            }

            var cache = HttpRuntime.Cache;
            var key = string.Format("{0}{1}__", Prefixo, tabela);
            var result = cache[key];

            if (result == null || !UsarCache)
            {
                using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  ITEM,
                                  DESCR
                          FROM    dbo.ITEMTABELA
                          WHERE   TAB = @TAB
                          ORDER BY DESCR");

                    contextQuery.Parameters.Add("@TAB", tabela);

                    result = ctx.TryToBindEntities<TabelaItem>(contextQuery);

                    cache.Insert(key, result, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                }
            }

            return result;
        }
    }
}