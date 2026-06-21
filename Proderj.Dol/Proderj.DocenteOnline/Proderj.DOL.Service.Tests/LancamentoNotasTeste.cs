using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.Foundation.Framework.Config;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service.Teste
{
    [TestClass]
	public class LancamentoNotasTeste : TesteBaseService<NinjectModuloServico, LancamentoNotasService>
    {
		private class Cenario {
			public long NumeroFuncionario { get; set; }
			public DTOLancamentoNotasSolicitacao DTOSolicitacao { get; set; }
		}
		private List<Cenario> listaDeCenarios = new List<Cenario>();

		public LancamentoNotasTeste()
		{
			//Cenario NOK - 1
			listaDeCenarios.Add(new Cenario
			{
				NumeroFuncionario = 26790,
				DTOSolicitacao = new DTOLancamentoNotasSolicitacao
				{
					CodigoUnidadeEnsino = "33045496",
					CodigoDisciplina = "159-CNR-3-4",
					CodigoTurma = "CN-3002-180040",
					Ano = 2012,
					Periodo = 0,
					CodigoCurso = "0003.31",
					CodigoModalidade = "NO9",
					TipoCurso = "3",
					Serie = 3

				}
			});

			//Cenario NOK - Turma/Disciplina Inválida para lancamento
			
			listaDeCenarios.Add(new Cenario
			{
				NumeroFuncionario = 19089,
				DTOSolicitacao = new DTOLancamentoNotasSolicitacao
				{
					CodigoUnidadeEnsino = "33142351",
					CodigoDisciplina = "29-EFR-5-20",
					CodigoTurma = "500-186964",
					Ano = 2012,
					Periodo = 0,
					CodigoCurso = "0001.11",
					CodigoModalidade = "RE1",
					TipoCurso = "1",
					Serie = 5
				}
			});


			//Cenario OK - 2
			//Dados de solicitação do professor matricula: 08376535 - Numero 43879
			listaDeCenarios.Add(new Cenario
			{
				NumeroFuncionario = 43879,
				DTOSolicitacao = new DTOLancamentoNotasSolicitacao
				{
					CodigoUnidadeEnsino = "33045496",
					CodigoDisciplina = "159-CNR-3-4",
					CodigoTurma = "CN-3002-180040",
					Ano = 2012,
					Periodo = 0,
					CodigoCurso = "0003.31",
					CodigoModalidade = "NO9",
					TipoCurso = "3",
					Serie = 3

				}
			});
		
		}

		[TestMethod]
		public void VerificaPermissaoParaLancarNota_NOK_AcessoNegado()
		{
			//Cenario NOK - 1
			//Matricula: 02819654
			//Dados de solicitação de uma turma do outro professor matricula: 08376535 - Numero 43879

			try
			{
				Servico.VerificaPermissaoParaLancarNota(listaDeCenarios[0].NumeroFuncionario, listaDeCenarios[0].DTOSolicitacao);
				Assert.Fail();
			}
			catch (LancamentoNotasException lnex)
			{
				Assert.IsTrue(lnex.Message == Proderj.DOL.Exception.LancamentoNotasResource.AcessoNegadoDocente_A_TurmaEDisciplina);
			}
		}

		[TestMethod]
		public void VerificaPermissaoParaLancarNota_NOK_InvalidoParaLancamento()
		{
			//Cenario NOK 2:  Turma/Disciplina Inválida para lancamento
			try
			{
				Servico.VerificaPermissaoParaLancarNota(listaDeCenarios[1].NumeroFuncionario, listaDeCenarios[1].DTOSolicitacao);
				Assert.Fail();
			}
			catch (LancamentoNotasException lnex)
			{
				Assert.IsTrue(lnex.Message == Proderj.DOL.Exception.LancamentoNotasResource.TurmaEDisciplinaInvalidaParaLancamento);
			}
		}

		[TestMethod]
		public void VerificaPermissaoParaLancarNota_OK()
		{
			//Cenario NOK - 1
			Servico.VerificaPermissaoParaLancarNota(listaDeCenarios[2].NumeroFuncionario, listaDeCenarios[2].DTOSolicitacao);

			//Nao ocorrendo erro, está tudo ok.
		}
	}
}
