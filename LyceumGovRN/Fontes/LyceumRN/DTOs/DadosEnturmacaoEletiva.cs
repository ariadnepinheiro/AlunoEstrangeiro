using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosEnturmacaoEletiva
    {
        public string Aluno { get; set; }

        public string Nome { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string TurmaReferencia { get; set; }

        public string TurmaEletiva1 { get; set; }

        public bool PossuiEletiva1 { get; set; }

        public string TurmaEletiva2 { get; set; }

        public bool PossuiEletiva2 { get; set; }

        public string TurmaEletiva3 { get; set; }

        public bool PossuiEletiva3 { get; set; }

        public string UsuarioResponsavel { get; set; }
    }
}
