using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Framework.Config;
using Proderj.DOL.WebApp.Controllers;
using System.Web.Mvc;
using Proderj.DOL.WebApp.Models;
using Proderj.DOL.Service;
using Proderj.Foundation.Common;
using System.Web;

namespace Proderj.DOL.WebApp.Teste
{
	[TestClass]
	public class LancamentoNotasTeste
	{
		public LancamentoNotasController Controller { get; set; }

		[TestInitialize]
		public void Start()
		{
			AppDomain.CurrentDomain.SetData("ExecutingDir", System.IO.Directory.GetCurrentDirectory());
			ConfiguracaoDeAplicacao.Instance.Config(ContextClassEnum.ThreadStatic);
			FluentHelper.Instance.LoadSession();

			var fabricaDeController = new NinjectControllerFactory();

			Controller = (LancamentoNotasController)fabricaDeController.CriaController(typeof(LancamentoNotasController));
		}

		[TestMethod]
		public void SalvaNotaTurmaControllerTeste()
		{
			var ninject = new NinjectFactoryBase<NinjectModuloServico>();

			var loginService = ninject.Obtem<LoginService>();
			var lancamentoDeNotasServico = ninject.Obtem<LancamentoNotasService>();
			var selecaoTurmasServico = ninject.Obtem<SelecaoTurmasService>();
			var turmaServico = ninject.Obtem<TurmaService>();

            HttpContext.Current.Session["captcha"] = "123456";
            string captchaGerado = "123456";
			string matricula = "08376535";
			string senha = "123456";
            string idfuncional = "";
            string vinculo = "";
			//var codigoDisciplina = "159-CNR-3-4";

            DTODocenteLogado dtoDocenteLogado = loginService.VerificaLogin(matricula, senha, captchaGerado, idfuncional, vinculo);
			var dtoDocente = new DTODocenteLogadoPrincipal(dtoDocenteLogado);

			// Caso 0: não tem frequencia
			// Caso 1: tem frequencia
			DTOSelecaoTurmas turma = selecaoTurmasServico.EnumeraSelecaoTurmasPor(dtoDocenteLogado.NumeroFuncionario).ToArray()[0];

			DocenteLogadoBindModel docenteLogado = new DocenteLogadoBindModel(dtoDocente);
			LancamentoNotasSalvaRequestModel lancaNotasModelo = new LancamentoNotasSalvaRequestModel
			{
				Ano = 2012,
				CodigoCurso = turma.Curso,
				CodigoDisciplina = turma.Disciplina,
				CodigoModalidade = turma.Modalidade,
				CodigoTurma = turma.Turma,
				CodigoUnidadeEnsino = turma.UnidadeEnsino,
				Periodo = turma.Semestre,
				Serie = turma.Serie,
				Subperiodo = 3,
				TipoCurso = turma.Tipo,
			};

			lancaNotasModelo.ListaItemLancamentoNotaFrequenciaAluno = lancamentoDeNotasServico.ListaLancamentoNotaFrequenciaAlunoPor
				(lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo, lancaNotasModelo.Subperiodo)
				.Where(x => x.Situacao == "Matriculado")
				.ToList()
				.ConvertAll(itemLancamento => new DTOItemSalvaNotaFrequenciaAluno
				                            {
				                                Codigo = itemLancamento.Codigo,
												CodigoJustificativa = itemLancamento.CodigoJustificativa,
												Faltas = itemLancamento.Faltas,
												Nota = itemLancamento.Nota,
												RecuperacaoParalela = itemLancamento.RecuperacaoParalela,
												SemAvaliacao = itemLancamento.SemAvaliacao
				                            });

			lancaNotasModelo.ListaItemLancamentoNotaFrequenciaAluno.ExecuteForEach
			(x =>
				{
					x.Nota = 1;
					x.Faltas = 0;
				}
			);

			lancaNotasModelo.DadosFrequenciaTurma = turmaServico.ObtemFrequenciaDaTurmaPor
				(lancaNotasModelo.CodigoDisciplina, lancaNotasModelo.CodigoTurma, lancaNotasModelo.Ano, lancaNotasModelo.Periodo, lancaNotasModelo.Subperiodo);

			Controller.Salva(docenteLogado, lancaNotasModelo);

		}
	}
}
