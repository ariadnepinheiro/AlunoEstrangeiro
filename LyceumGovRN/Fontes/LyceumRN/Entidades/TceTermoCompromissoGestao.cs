using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceTermoCompromissoGestao : IEntity
    {
        public int IdTermoGestao { get; set; }

        public int Ano { get; set; }

        public string PadraoAcesso { get; set; }

        public Date DtInicio { get; set; }

        public Date DtFim { get; set; }

        public string Arquivo { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}
