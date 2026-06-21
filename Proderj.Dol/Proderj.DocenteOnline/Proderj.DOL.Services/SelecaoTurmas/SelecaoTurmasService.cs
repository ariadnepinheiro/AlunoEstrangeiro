using Proderj.DOL.Domain;
using System.Collections.Generic;
using System.Linq;
using Proderj.DOL.Repository;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Exception;
using AutoMapper;

namespace Proderj.DOL.Service
{
    public class SelecaoTurmasService : ISelecaoTurmasService
    {
		private ISelecaoTurmasRepository repositorioSelecao;
		private IDocenteRepository repositorioDocente;

		public SelecaoTurmasService(ISelecaoTurmasRepository repositorioSelecao, IDocenteRepository repositorioDocente)
        {
            this.repositorioDocente = repositorioDocente;
			this.repositorioSelecao = repositorioSelecao;
        }

        #region ISelecaoTurmasService Members

		public IEnumerable<DTOSelecaoTurmas> EnumeraSelecaoTurmasPor(long numeroFuncionarioDocente)
		{
			if (numeroFuncionarioDocente == default(long))
			{
				throw new SelecaoTurmasException(SelecaoTurmasException.TipoEnum.NumeroFuncionario);
			}

			IEnumerable<SelecaoTurmas> turmas = repositorioSelecao.EnumeraTurmasPor(numeroFuncionarioDocente);

			//Mapper.CreateMap<SelecaoTurmas, DTOSelecaoTurmas>();
            
			foreach (SelecaoTurmas turma in turmas)
			{
				yield return Mapper.Map<SelecaoTurmas, DTOSelecaoTurmas>(turma);
			}
		}

        public IEnumerable<DTOSelecaoTurmas> EnumeraSelecaoTurmasPor(string matricula)
        {
			matricula = (matricula != null) ? matricula.Trim() : null;

			if (matricula.IsNullOrEmpty())
			{
				throw new SelecaoTurmasException(SelecaoTurmasException.TipoEnum.MatriculaInvalida);
			}
			
			long numeroFuncionario = repositorioDocente.ObtemNumFuncPor(matricula);

			return EnumeraSelecaoTurmasPor(numeroFuncionario);
        }

        #endregion
    }
}
