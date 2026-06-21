namespace Seeduc.Infra.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.Helpers;
    using Seeduc.Infra.MapeamentoAtributos;

    internal class ReflectionCache
    {
        private readonly Dictionary<Type, IDictionary<string, PropertyInfo>> properties =
            new Dictionary<Type, IDictionary<string, PropertyInfo>>();

        private readonly object access = new object();

        private ReflectionCache()
        {
        }

        public static ReflectionCache Instance
        {
            get
            {
                return Nested.ReflectionCacheInstance;
            }
        }

        public IDictionary<string, PropertyInfo> GetProperties<T>()
            where T : class, IEntity, new()
        {
            Type entidade = typeof(T);

            IDictionary<string, PropertyInfo> camposEntidade = new Dictionary<string, PropertyInfo>();

            if (!this.properties.ContainsKey(entidade))
            {
                lock (this.access)
                {
                    camposEntidade = EnumerarNomeDosCamposPor(entidade);
                    this.properties.Add(entidade, camposEntidade);
                }
            }

            return properties[entidade];
        }

        private string RecuperaNomeDoCampoDefinidoNoAtributo(Type tipoEntidade, string nomePropriedade)
        {
            string nomeDoCampoDefinidoNoAtributo = string.Empty;

            IEnumerable<object> atributos =
                tipoEntidade.GetProperty(nomePropriedade).GetCustomAttributes(typeof(AtributoCampo), true).ToList();

            if (atributos != null && atributos.Count() > 0)
                nomeDoCampoDefinidoNoAtributo = ((AtributoCampo)atributos.First()).Nome;

            return nomeDoCampoDefinidoNoAtributo;
        }

        private IDictionary<string, PropertyInfo> EnumerarNomeDosCamposPor(Type tipoEntidade)
        {
            IDictionary<string, PropertyInfo> propiedadesDaEntidade = new Dictionary<string, PropertyInfo>();

            string nomePropriedade = string.Empty;
            string nomeDoCampoDefinidoNoAtributo = string.Empty;

            foreach (PropertyInfo informacaoDaPropriedade in tipoEntidade.GetProperties())
            {
                nomePropriedade = informacaoDaPropriedade.Name;
                nomeDoCampoDefinidoNoAtributo = RecuperaNomeDoCampoDefinidoNoAtributo(tipoEntidade, nomePropriedade);

                if (string.IsNullOrEmpty(nomeDoCampoDefinidoNoAtributo))
                    nomeDoCampoDefinidoNoAtributo = DbHelper.ConvertToColumnConvention(nomePropriedade);

                propiedadesDaEntidade.Add(nomeDoCampoDefinidoNoAtributo.ToUpper(), informacaoDaPropriedade);
            }

            return propiedadesDaEntidade;
        }

        private class Nested
        {
            internal static readonly ReflectionCache ReflectionCacheInstance = new ReflectionCache();

            static Nested()
            {
            }
        }
    }
}