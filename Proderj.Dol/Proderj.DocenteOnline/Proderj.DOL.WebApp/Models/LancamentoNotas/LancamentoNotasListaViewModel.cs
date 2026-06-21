using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework.Web.Seguranca;

namespace Proderj.DOL.WebApp.Models
{
	public class LancamentoNotasListaViewModel : ViewModelPadrao
	{
		public SelecaoTurmasTurmaSelecionadaViewModel TurmaSelecionadaModelo { get; set; }

		//Dados para postback da pagina
		public string CodigoCurso { get; set; }
		public string TipoCurso { get; set; }
		public string CodigoUnidadeEnsino { get; set; }
		public short Serie { get; set; }
		public string CodigoTurma { get; set; }
		public string CodigoDisciplina { get; set; }
		public string CodigoModalidade { get; set; }
		public int Ano { get; set; }
		public short Periodo { get; set; }		
		public string IdVinculo { get; set; }
		public string CodigoFilipeta { get; set; }
		public bool LancamentoPersistido { get; set; }
		public string MensagemLancamentoPersistido { get; set; }

		public bool SolicitadoReaberturaLancamento { get; set; }

		public string DescricaoUnidadeDeEnsino { get; set; }
		public string Turma { get; set; }
		public string DescricaoDisciplina { get; set; }
		public string MatriculaDocente { get; set; }
		public string NomeDocente { get; set; }		

		public short BimestreSelecionado { get; set; }
		public bool BimestreFechado { get; set; }
		public List<DTOSubPeriodoLetivoAtivo> BimestresHabilitados { get; set; }
		public bool ExisteBimestreAnteriorPendenteDeLancamento { get; set; }

		public DTORespostaCurriculoMinimo_StatusPreenchimentoPorTurma DadosPreenchimentoCurriculoMinimo { get; set; }
		public DTOFrequenciaTurma DadosFrequenciaTurma { get; set; }
		public DTOConfiguracaoNotaDisciplina DadosConfiguracaoNotaDisciplina { get ; set; }

		public string MensagemStatusCurriculoMinimo { get; set; }

		public bool HabilitaCurriculoMinimo { get; set;}
		public bool HabilitaEdicaoDeNotas { get; set; }
		public bool HabilitaEdicaoAulasPrevistasEDadas { get ; set; }
		public bool HabilitaEdicaoDeFaltas { get; set; }
		public bool HabilitaLancamentoNotas { get; set; }
		public bool HabilitaSolicitacaoDeLancamento { get; set; }
        public bool HabilitaLancamentoNotaRP { get; set; }

		public string MensagemSolicitacaoAlteracaoNotasExistente { get; set; }

		public string CodigoTurmaErro { get; set; }

		public IList<DTOItemJustificativa> ListaItemJustificativa { get; set; }

		private List<DTOItemLancamentoNotaFrequenciaAluno> listaItemLancamentoNotaFrequenciaAluno;
		public List<DTOItemLancamentoNotaFrequenciaAluno> ListaItemLancamentoNotaFrequenciaAluno 
		{
			get {
				if (listaItemLancamentoNotaFrequenciaAluno == null)
					listaItemLancamentoNotaFrequenciaAluno = new List<DTOItemLancamentoNotaFrequenciaAluno>();
				return listaItemLancamentoNotaFrequenciaAluno;
			}	
			set {
				listaItemLancamentoNotaFrequenciaAluno = value;
			}
		}

        //
        private List<DTOMaterialEstudo> listaMaterialEstudo;
        public List<DTOMaterialEstudo> ListaMaterialEstudo
        {
            get
            {
                if (listaMaterialEstudo == null)
                    listaMaterialEstudo = new List<DTOMaterialEstudo>();
                return listaMaterialEstudo;
            }
            set
            {
                listaMaterialEstudo = value;
            }
        }

        private List<DTOTurmaMaterialEstudo> listaTurmaMaterialEstudo;
        public List<DTOTurmaMaterialEstudo> ListaTurmaMaterialEstudo
        {
            get
            {
                if (listaTurmaMaterialEstudo == null)
                    listaTurmaMaterialEstudo = new List<DTOTurmaMaterialEstudo>();
                return listaTurmaMaterialEstudo;
            }
            set
            {
                listaTurmaMaterialEstudo = value;
            }
        }
        //

		public string QueryStringImpressaoFilipeta
		{
			get
			{
				return QueryStringDecode.CodificaQueryString(new Dictionary<string, string>
				                                             	{
				                                             		{"matricula", TurmaSelecionadaModelo.MatriculaDocente},
				                                             		{"disciplina", CodigoDisciplina},
				                                             		{"turma", CodigoTurma.Replace("&","##")},
				                                             		{"ano", Ano.ToString()},
				                                             		{"periodo", Periodo.ToString()},
				                                             		{"subperiodo", BimestreSelecionado.ToString()},
				                                             		{"nome", TurmaSelecionadaModelo.NomeDocente},
				                                             		{"escola", TurmaSelecionadaModelo.DescricaoUnidadeEnsino},
				                                             		{"semestre", Periodo.ToString()},
                                                                    {"idvinculo", TurmaSelecionadaModelo.IdFuncional.ToString()+"/"+ TurmaSelecionadaModelo.Vinculo.ToString()},
				                                             		{"nomedisciplina", TurmaSelecionadaModelo.DescricaoDisciplina},
				                                             		{"protocolo", CodigoFilipeta != null ? CodigoFilipeta.Replace("&","##") : "(sem protocolo)"}
				                                             	});

			}

		}

        public string MensagemFrequenciaNotaFalta { get; set; }

        public Domain.Disciplina DisciplinaFrequenciaNota { get; set; }

        public bool ExibeConsolidado { get; set; }

        public DTOLancamentoNotasConsolidado DadosConsolidados { get; set; }
	}
}
