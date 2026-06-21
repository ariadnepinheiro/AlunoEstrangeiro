using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
   public  class DadosAvaliacaoNapes
    {
        public int AvaliacaoNapesId { get; set; }

        public string AlunoId { get; set; }

        public int TipoRecursoNecessidadeEspecialId { get; set; }

        public bool NecessitaRecurso { get; set; }

        public string Justificativa { get; set; }

        public bool Transitorio { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string UsuarioId { get; set; }

        public string NomeUsuario { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
