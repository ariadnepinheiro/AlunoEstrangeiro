using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.WebApp.Controllers;
using Proderj.Foundation.Framework.Config;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Proderj.DOL.Repository;
using System.Web;

namespace Proderj.DOL.WebApp.Teste.Controllers
{
	/// <summary>
	/// Summary description for AvaliacaoCurriculoMinimoTeste
	/// </summary>
	[TestClass]
	public class AvaliacaoCurriculoMinimoTeste : TesteBaseController<NinjectModuloController, NinjectModuloServico, AvaliacaoCurriculoMinimoController>
	{
		[TestMethod]
		public void LancaAvaliacoesTeste()
		{
			var loginService = ObtemServico<LoginService>();
			var lancamentoDeNotasServico = ObtemServico<LancamentoNotasService>();
			var selecaoTurmasServico = ObtemServico<SelecaoTurmasService>();
			var turmaServico = ObtemServico<TurmaService>();

            HttpContext.Current.Session["captcha"] = "123456";
            string captchaGerado = "123456";
			string matricula = "08376535";
			string senha = "123456";
            string vinculo = "1";
            string idfuncional = "123456";

            DTODocenteLogado dtoDocenteLogado = loginService.VerificaLogin(matricula, senha, captchaGerado, idfuncional, vinculo);
			var dtoDocente = new DTODocenteLogadoPrincipal(dtoDocenteLogado);

			DTOSelecaoTurmas turma = selecaoTurmasServico.EnumeraSelecaoTurmasPor(dtoDocenteLogado.NumeroFuncionario).ToArray()[1];

			DocenteLogadoBindModel docenteLogado = new DocenteLogadoBindModel(dtoDocente);

			AvaliacaoCurriculoMinimoListaRequestModel solicitacaoModelo = new AvaliacaoCurriculoMinimoListaRequestModel
			{
				Ano = 2012,
				Periodo = turma.Semestre,
				Subperiodo = 1,
				CodigoTurma = turma.Disciplina
			};

			Controller.Lista(docenteLogado, solicitacaoModelo);
		}

		[TestMethod]
		public void SalvaNotaTurmaControllerTeste()
		{
			var ninject = new NinjectFactoryBase<NinjectModuloServico>();

			var loginService = ninject.Obtem<LoginService>();
			var lancamentoDeNotasServico = ninject.Obtem<LancamentoNotasService>();

			var selecaoTurmasServico = ninject.Obtem<SelecaoTurmasService>();
			var avaliacaoCurriculoMinimoServico = ninject.Obtem<AvaliacaoCurriculoMinimoService>();

            HttpContext.Current.Session["captcha"] = "123456";
            string captchaGerado = "123456";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;
			string matricula = "08376535";
			string senha = "123456";
            string idfuncional = "";
            string vinculo = "";

            DTODocenteLogado dtoDocenteLogado = loginService.VerificaLogin(matricula, senha, captchaGerado, idfuncional,vinculo);
			var dtoDocente = new DTODocenteLogadoPrincipal(dtoDocenteLogado);

			DTOSelecaoTurmas turma = selecaoTurmasServico.EnumeraSelecaoTurmasPor(dtoDocenteLogado.NumeroFuncionario).ToArray()[1];

			DocenteLogadoBindModel docenteLogado = new DocenteLogadoBindModel(dtoDocente);

			var retorno = avaliacaoCurriculoMinimoServico.ObtemAvaliacoesEJustificativaPor(ano, periodo, subperiodo, matricula);

			//ALTERANDO OS VALORES:
			retorno.ListaAvaliacaoCurriculoMinimo.ExecuteForEach(x => x.EhAvaliadoPositivamente = true);


			var solicitacaoSalvarModelo = new AvaliacaoCurriculoMinimoSalvaRequestModel
			{
				Ano = ano,
				Periodo = periodo,
				Subperiodo = subperiodo,
				ListaAvaliacao = retorno.ListaAvaliacaoCurriculoMinimo.ConvertAll(avaliacao => new AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao
																								{
																									Codigo = avaliacao.IdAvaliacaoCurriculoMinimo,
																									Resposta = TrataRequestBoolEmEnum(avaliacao.EhAvaliadoPositivamente)
																								}),
				DescricaoJustificativa = retorno.DescricaoJustificativa
			};

			var repositorioBase = new AvaliacaoCurriculoMinimoDocenteRepository();

			repositorioBase.InicializaTransacao();

			Controller.Salva(docenteLogado, solicitacaoSalvarModelo);

			repositorioBase.TransacaoRollback();
		}

		public AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum? TrataRequestBoolEmEnum(bool? entrada)
		{
			if (!entrada.HasValue)
				return null;

			if (entrada.Value)
				return AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Sim;

			return AvaliacaoCurriculoMinimoSalvaRequestModel.RespostaAvaliacao.RespostaEnum.Nao;
		}
	}
}
