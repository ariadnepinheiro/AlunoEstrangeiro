using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
	public class AvaliacaoCurriculoMinimoListaViewModel : ViewModelPadrao
	{
		public AvaliacaoCurriculoMinimoListaViewModel(DocenteLogadoBindModel docenteLogadoModelo)
			: base(docenteLogadoModelo)
		{
		}

		public string TituloLista { get; set; }

		public SelecaoTurmasTurmaSelecionadaViewModel TurmaSelecionadaModelo { get; set; }

		//Dados para postback da pagina (Mudança de bimestre)
		public string CodigoCurso { get; set; }
		public string TipoCurso { get; set; }
		public string CodigoUnidadeEnsino { get; set; }
		public short Serie { get; set; }
		public string CodigoTurma { get; set; }
		public string CodigoDisciplina { get; set; }
		public string CodigoModalidade { get; set; }
		public short Ano { get; set; }
		public short Periodo { get; set; }
		public short BimestreSelecionado { get; set; }


		public List<DTOSubPeriodoLetivoAtivo> BimestresHabilitadosGeral { get; set; }
		public List<DTOSubPeriodoLetivoAtivo> BimestresHabilitadosCurriculoMinimo { get; set; }

		public bool BimestreAtivo(short codigoBimestre)
		{
			return BimestresHabilitadosCurriculoMinimo.Any(bim => bim.SubPeriodo == codigoBimestre);
		}

		public bool LancamentoPersistido { get; set; }
		public object MensagemLancamentoPersistido { get; set; }

		public DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa DadosAvaliacoesEJustificativas { get; set; }

		public string MensagemSumario { get; set; }
	}
}
