using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class TipoSolicitacaoService: SingletonBase<TipoSolicitacaoService>
    {
        private static readonly TipoSolicitacaoQuery tipoSolicitacaoQuery = TipoSolicitacaoQuery.Instancia;

        TipoSolicitacaoService() { }

        public List<TipoSolicitacao> ListaTodos()
        {
            return tipoSolicitacaoQuery.ListaTodos<TipoSolicitacao>();
        }
    }
}
