using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;
using System.Collections.Generic;

namespace Proderj.DOL.Repository
{
    public interface IDISPONIBILIDADEGLPRepository : IRepository<DISPONIBILIDADEGLP>
	{
        IQueryable<DISPONIBILIDADEGLP> ListaQueryable();
        IList<T> ListaPor<T>(long num_func, int ano);
        void InsereAuditada(DISPONIBILIDADEGLP entidade);
        void AlteraAuditada(DISPONIBILIDADEGLP entidade);
        void ExcluiAuditada(int disponibilidadeGlpId, string unidadeEnsino);
        bool EhDisciplinaHabilitadaPor(string agrupamento, double num_func);
        bool ExistePor(double num_func, int ano, string disciplina, string unidadeEscolar, string modalidade, int diaSemana, string turno);
	}
}
