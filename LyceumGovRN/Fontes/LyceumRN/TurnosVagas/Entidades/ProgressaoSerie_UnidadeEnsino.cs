using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class ProgressaoSerie_UnidadeEnsino
    {

        public int ProgressaoSerie_UnidadeEnsino_Id { get; set; }

        public int Serie { get; set; }
        
        public int Preferencial { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public string CursoId { get; set; }

        public string ProximoCursoId { get; set; }

        public int ProximaSerie { get; set; }

        public string UnidadeEnsinoId { get; set; }

    }
}
