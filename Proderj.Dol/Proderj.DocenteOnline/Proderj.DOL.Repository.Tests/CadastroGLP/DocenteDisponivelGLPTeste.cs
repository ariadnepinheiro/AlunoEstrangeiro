using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class DocenteDisponivelGLPTeste : TesteBaseRepositorio<DocenteDisponivelGLPRepository>
	{

		[TestMethod]
		public void EnumeraPor()
		{
			long numeroFuncionario = 43879L;

			var docentes = Repositorio.EnumeraPor(numeroFuncionario);

			Assert.IsTrue(docentes.Count() == 5);
		}

		[TestMethod]
		public void ExisteDisponibilidade()
		{
			var toExisteDisponibilidade = new TODocenteDisponivelGLPExisteDisponibilidade
			{
				CodigoMunicipio = "00006871",
				CodigoRegional = 1,
				DiaSemana = 2,
				HoraInicio = DateTime.Now,
				HoraFinal = DateTime.Now,
				NumeroFuncionario = 43879L
			};


			var docentes = Repositorio.ExisteDisponibilidade(toExisteDisponibilidade);

		}

		[TestMethod]
		public void ObtemDisponibilidadePor()
		{
			short diaSemana = 2;
			var codigoMunicipio = "00006871";
            var codigoRegional = 1;

			var docentes = Repositorio.EnumeraDisponibilidadePor(diaSemana, codigoMunicipio, codigoRegional);
		}

		[TestMethod]
		public void Insere()
		{
			var docenteDisponivel = new DocenteDisponivelGLP
			{
				Municipio = new Municipio {  Codigo = "00006871" },
                Regional = new Regional { Codigo = 1 },
				DiaSemana = 2,
				HoraInicio = DateTime.Now,
				HoraFinal = DateTime.Now,
				Docente = new Docente { NumeroFuncionario = 43879L },
				GrupoHabilitacao = new GrupoHabilitacao { Agrupamento = "207" }
			};
			
			Repositorio.InicializaTransacao();
			
			int docenteInserido = Repositorio.Insere(docenteDisponivel);
			Assert.IsTrue(docenteInserido == 1);

			Repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void RemovePor()
		{
			Repositorio.InicializaTransacao();

			int docenteInserido = Repositorio.RemovePor(7044);
			Assert.IsTrue(docenteInserido == 1);

			Repositorio.TransacaoRollback();
		}
	}
}
