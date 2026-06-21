using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceMacroCampos : IEntity
    {
        public int IdMacroCampos { get; set; }

        public string Nome { get; set; }

        public bool Obrigatorio { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
