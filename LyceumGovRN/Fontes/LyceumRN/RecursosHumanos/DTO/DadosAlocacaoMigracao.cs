using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.RecursosHumanos.DTO
{
    public class DadosAlocacaoMigracao
    {
        public DadosAlocacaoMigracao()
        {
            Aulas = new List<DadosTurmaAlocacao>();
        }

        public int DocenteCandidatoId { get; set; }

        public int Pessoa { get; set; }

        public int NumFunc { get; set; }

        public string MatriculaDocente { get; set; }

        public string CategoriaAnterior { get; set; }

        public string Categoria { get; set; }

        public string Funcao { get; set; }

        public string UsuarioId { get; set; }

        public List<DadosTurmaAlocacao> Aulas { get; set; }

        public int Ano { get; set; }

        public DateTime? DataConvocacaoDO { get; set; }

        public string Observacao { get; set; }

    }
}
