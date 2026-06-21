using System;
namespace Techne.Lyceum.RN.DTOs
{
    [Serializable]
    public class DadosPossiveisRenovacoes
    {
        public int SerieSeguinte { get; set; }

        public string Turno { get; set; }

        public string TurnoNome { get; set; }

        public string UnidadeEnsino { get; set; }

        public string UnidadeEnsinoNome { get; set; }

        public string Modalidade { get; set; }

        public string ModalidadeDescricao { get; set; }

        public string Tipo { get; set; }

        public string TipoDescricao { get; set; }

        public string Curso { get; set; }

        public string CursoDescricao { get; set; }

        public string ModalidadeSegmentoCurso { get; set; }
    }
}
