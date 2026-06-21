using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosCurso
    {
        public string Curso { get; set; }

        public string Faculdade { get; set; }

        public string Depto { get; set; }        

        public string Mnemonico { get; set; }

        public string Nome { get; set; }

        public string Titulo { get; set; }

        public string Habilitacao { get; set; }

        public string Decreto { get; set; }

        public int? Vagas { get; set; }

        public DateTime? Dt_dou { get; set; }

        public string Tipo { get; set; }

        public string Modalidade { get; set; }

        public string Tipo_curso { get; set; }

        public string Ativo { get; set; }

        public string Tem_reclassificacao { get; set; }

        public string Formatura { get; set; }

        public string Concomitante { get; set; }

        public string ParticipaCalculoNovasTurmasTurnosVagas { get; set; }

        public string ParticipaFechamentoAutomatico { get; set; }

        public string PermiteTransferenciaTurmaTotal { get; set; }

        public string PermiteChoqueTurnoIntegralTurnosVagas { get; set; }

        public string Salaexterna { get; set; }

        public string OfertaEletiva { get; set; }

        public string FormacaoBasica { get; set; }

        public string ItinerarioFormativo { get; set; }

        public string NaoSeAplica { get; set; }

        public int? ItinerarioFormativoId { get; set; }

        public int? TrilhaAprendizagem { get; set; }

        public int? MaximoComponentes { get; set; }

        public List<int> UnidadesCurricular { get; set; }

        public List<int> AreaItinerarioFormativo { get; set; }

        public List<int> ComposicaoItinerarioFormativoIntegrado { get; set; }

        public List<int> TipoItinerarioFormacaoTecnicaProfissional { get; set; }

        public string UsuarioId { get; set; }
    }
}
