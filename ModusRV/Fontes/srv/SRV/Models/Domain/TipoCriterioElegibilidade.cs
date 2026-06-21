using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;
using System.ComponentModel;

namespace SRV.Models.Domain
{
	/// <summary>
	/// ID's  dos critérios usados nas regras de importação da avaliação externa
	/// </summary>
	public enum CriteriosAvaliacaoExternaEnum
	{
		[DescriptionAttribute("% DE PARTICIPAÇÃO DE UNIDADE ADMINISTRATIVA DIURNO POR TURMA")]
		ParticipacaoDiurnaIndividual = 31,
		[DescriptionAttribute("% DE PARTICIPAÇÃO DE UNIDADE ADMINISTRATIVA DIURNO P/ MÉDIA DAS TURMAS")]
		ParticipacaoDiurnaMedia = 32,
		[DescriptionAttribute("% DE PARTICIPAÇÃO DE UNIDADE ADMINISTRATIVA NOTURNO POR TURMA")]
		ParticipacaoNoturnaIndividual = 33,
		[DescriptionAttribute("% DE PARTICIPAÇÃO DE UNIDADE ADMINISTRATIVA NOTURNO P/ MÉDIA DAS TURMAS")]
		ParticipacaoNoturnaMedia = 34
	}
	
	public class TipoCriterioElegibilidade
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdTipoCriterioElegibilidade { get; set; }

        [PrimaryKey]
        [Display(Name = "Ano de Referência")]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        [Display(Name = "Tipo de Unidade Administrativa")]
        public TipoUnidadeAdministrativa TipoUnidadeAdministrativa { get; set; }
        
        [Display(Name = "Descrição do Critério")]
        [StringLength(200, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesTipoCriterioElegibilidade { get; set; }

        [Display(Name = "Valor do Critério")]
        public decimal? ValorCriterio { get; set; }
    }
}