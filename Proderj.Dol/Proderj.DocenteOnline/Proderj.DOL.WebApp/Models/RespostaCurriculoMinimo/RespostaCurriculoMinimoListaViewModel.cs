using System;
using System.Collections.Generic;
using System.Linq;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework.Web.Seguranca;
using Resources;

namespace Proderj.DOL.WebApp.Models
{
	public class RespostaCurriculoMinimoListaViewModel : ViewModelPadrao
	{
		public RespostaCurriculoMinimoListaViewModel(DocenteLogadoBindModel docenteLogadoModelo) 
			: base(docenteLogadoModelo)
		{ }

		public string TituloLancamentoResposta { get; set; }
		
		public SelecaoTurmasTurmaSelecionadaViewModel TurmaSelecionadaModelo { get; set; }

		//Dados para postback da pagina
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
		
		public bool HabilitarAvaliacao { get; set; }

		public bool BimestreAtivo(short codigoBimestre)
		{
			return BimestresHabilitadosCurriculoMinimo.Any(bim => bim.SubPeriodo == codigoBimestre);
		}

		public List<DTORespostaCurriculoMinimo_RespostasPorGrupo> ListaRespostasPorGrupo { get; set; }

		public List<DTORespostaCurriculoMinimo_GrupoSimples> ListaGruposDistintosOrdenados
		{
			get
			{
				if (ListaRespostasPorGrupo != null)
				{
					return ListaRespostasPorGrupo
						.OrderBy(resp => resp.OrdemGrupo)
						.Select(resp => new {resp.CodigoGrupo, resp.DescricaoGrupo})
						.Distinct()
						.ToList()
						.ConvertAll(grupo => new DTORespostaCurriculoMinimo_GrupoSimples
						                     	{
						                     		Codigo = grupo.CodigoGrupo,
						                     		Descricao = grupo.DescricaoGrupo
						                     	});
				}
				else
				{
					return null;
				}
			}

		}

		public List<DTORespostaCurriculoMinimo_RespostasPorGrupo> ListaRespostasOrdenadasPor(DTORespostaCurriculoMinimo_GrupoSimples grupoSimples)
		{
			return
				ListaRespostasPorGrupo
					.Where(resp => resp.CodigoGrupo == grupoSimples.Codigo)
					.OrderBy(resp => resp.OrdemResposta)
					.ToList();
		}

		public bool LancamentoPersistido { get; set; }

		public object MensagemLancamentoPersistido { get; set; }

		public string QueryStringImpressao { 
			get
			{
				return QueryStringDecode.CodificaQueryString(new Dictionary<string, string>
				                                             	{
				                                             		{"matricula", TurmaSelecionadaModelo.MatriculaDocente},
				                                             		{"disciplina", CodigoDisciplina},
				                                             		{"turma", CodigoTurma},
				                                             		{"ano", Ano.ToString()},
				                                             		{"periodo", Periodo.ToString()},
				                                             		{"subperiodo", BimestreSelecionado.ToString()},
				                                             		{"curso", CodigoCurso},
																	{"modalidade", CodigoModalidade},
																	{"tipoCurso", TipoCurso},
																	{"serie", Serie.ToString()},
																	{"nome", TurmaSelecionadaModelo.NomeDocente},
				                                             		{"escola", TurmaSelecionadaModelo.DescricaoUnidadeEnsino},
				                                             		{"nomedisciplina", TurmaSelecionadaModelo.DescricaoDisciplina}
				                                             	});
			} 
		}

		public string MensagemSumario { get; set; }
	}
}