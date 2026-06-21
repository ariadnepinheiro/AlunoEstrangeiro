using System;
using System.Collections.Generic;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ContextoPrestacaoContas
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string DataPrestacao { get; set; }
        public decimal? IdPeriodoReferencia { get; set; }
        public string Coordenadoria { get; set; }
        public string UnidadeEnsino { get; set; }
    }

    public class SessaoUsuario
    {
        
        public SessaoUsuario()
        {
            Coordenadoria = string.Empty;
            Municipio = string.Empty;
            Escola = string.Empty;
            Ano = string.Empty;
            Periodo = string.Empty;
            Regional = string.Empty;
            PrestacaoContas = null;
        }

        public string Coordenadoria {get; set;}

        public string Municipio {get; set;}

        public string Escola {get; set;}

        public string Ano {get; set;}

        public string Periodo {get; set;}

        public string Regional {get; set;}

        //private ContextoPrestacaoContas _prestacaoContas = null;

        public ContextoPrestacaoContas PrestacaoContas {get; set;}

        public static SessaoUsuario GetSessaoUsuario()
        {
            SessaoUsuario sess = null;

            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
                return null;
            else
            {
                if (System.Web.HttpContext.Current.Session["_SESSAO_USUARIO_"] is SessaoUsuario)
                    sess = (SessaoUsuario)System.Web.HttpContext.Current.Session["_SESSAO_USUARIO_"];
                else
                {
                    sess = new SessaoUsuario();
                    System.Web.HttpContext.Current.Session["_SESSAO_USUARIO_"] = sess;
                }
            }

            return sess;
        }
    }
}
