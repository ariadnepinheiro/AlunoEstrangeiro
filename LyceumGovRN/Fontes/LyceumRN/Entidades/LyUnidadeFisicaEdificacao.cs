using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyUnidadeFisicaEdificacao : IEntity
    {
        public string UnidadeFis { get; set; }

        public string Edificacao { get; set; }

        public string NomeEdificacao { get; set; }

        public string Pavimento { get; set; }

        public string NomePavimento { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
