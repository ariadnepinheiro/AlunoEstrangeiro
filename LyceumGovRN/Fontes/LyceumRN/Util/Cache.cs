using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Util
{
    public class Cache
    {
        public const string Etnia = "ETNIA";
        public const string EstadoCivil = "ESTADO CIVIL";
        public const string Pais = "PAIS";
        public const string Nacionalidade = "NACIONALIDADE";
        public const string NecessidadeEspecial = "NECESSIDADE ESPECIAL";
        public const string OutroEnsino = "ESCOLARIZACAOEXTERNA";
        public const string Credo = "CREDO";
        public const string TipoSanguineo = "TIPOSANGUINEO";
        public const string RgTipoPessoa = "TIPO DOC";
        public const string RgEmissor = "ORGAO RG";
        public const string Uf = "UF";
        public const string TipoIngresso = "TIPOINGRESSO";
        public const string UfCartorio = "UFCARTORIO";
        public const string RedeEnsinoOrigem = "REDEENSINOORIGEM";
        public const string TransporteModal = "TRANSPORTEMODAL";
        public const string Modalidade = "MODALIDADE";
        public const string Nivel = "NIVEL";
        public const string UfNaturalidade = "UFNATURALIDADE";
        public const string ProximosAnosLetivos = "PROXIMOSANOSLETIVOS";
        public const string PeriodoLetivo = "PERIODOLETIVO";
        public const string AnoLetivo = "ANOLETIVO";
        public const string UnidadesEnsinoRenovacaoAutomatica = "UNIDADESENSINORENOVACAOAUTOMATICA";
        public const string ClasseFornecimentoEnergia = "CLASSEFORNECIMENTO";
        public const string TipoSuprimentoGas = "TIPOSUPRIMENTOGAS";
        public const string TipoInstituicao = "TIPOINSTITUICAO";
        public const string SituacaoCursoFormacao = "SITUACAOCURSOFORM";
        public const string FormacaoComplementar = "FORMACAOCOMPLEMENT";
        public const string TipoUA = "TIPOUA";
        public const string TipoDocCelebrado = "TIPODOCCELEBRADO";

        public static System.Object CarregaItemTabelaGeralPor(string tab)
        {
            try
            {
                System.Object lista = HttpRuntime.Cache[tab] as System.Object;
                TimeSpan timeSpan = new TimeSpan(24, 0, 0);

                if (lista == null)
                {
                    lista = Basico.ObtemListaTabelaGeralPor(tab);
                    AdicionaCache(tab, lista, timeSpan);
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static System.Object CarregaResultadoQueryPor(string chave, string query)
        {
            try
            {
                System.Object lista = HttpRuntime.Cache[chave] as System.Object;
                TimeSpan timeSpan = new TimeSpan(24, 0, 0);

                if (lista == null)
                {
                    lista = Basico.ObtemResultadoQueryPor(query);
                    AdicionaCache(chave, lista, timeSpan);
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AdicionaCache(string chave, Object entidade, TimeSpan timeSpan)
        {
            HttpRuntime.Cache.Add(chave, entidade, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                timeSpan,
                System.Web.Caching.CacheItemPriority.Default,
                null);
        }

        public static void RemoveCache(string chave)
        {
            HttpRuntime.Cache.Remove(chave);
        }
    }
}
