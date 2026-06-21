using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class LogCompetenciaHabilidadeDocenteTeste : TesteBaseRepositorio<LogCompetenciaHabilidadeDocenteRepository>
	{
		[TestMethod]
		public void InserePorCompetenciaHabilidadeItemPor()
		{
			string matricula = "08376535";
			string turma = "CN-4001-180040";
			string disciplina = "159-CNR-4-2";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			Repositorio.InicializaTransacao();
			
			var docente = new CompetenciaHabilidadeDocenteTeste().InsereCompetenciaHabilidadeDocente(matricula, turma,disciplina,ano,periodo,subperiodo);
			var retorno = Repositorio.InserePorCompetenciaHabilidadeItemPor(matricula, turma, disciplina, ano, periodo, subperiodo);
			
			Repositorio.TransacaoRollback();

		}

		//[TestMethod]
		//public void ObtemPorChavePrimaria()
		//{
		//    var resultado = Repositorio.ObtemPorChavePrimaria(611617);
		//}
	}
}
