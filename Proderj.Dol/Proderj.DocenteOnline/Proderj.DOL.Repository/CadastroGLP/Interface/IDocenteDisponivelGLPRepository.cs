using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IDocenteDisponivelGLPRepository : IRepository<DocenteDisponivelGLP>
	{
		IEnumerable<DocenteDisponivelGLP> EnumeraPor(long numeroFuncionario);

		bool ExisteDisponibilidade(TODocenteDisponivelGLPExisteDisponibilidade toExisteDisponibilidade);

		IEnumerable<DocenteDisponivelGLP> EnumeraDisponibilidadePor(short diaSemana, string codigoMunicipio, int codigoRegional);

		int Insere(DocenteDisponivelGLP docenteDisponivel);

		int RemovePor(int identificador);

		bool ConfereItemEhDoDocente(int docenteDisponivelId, long numeroFuncionario);
	}
}
