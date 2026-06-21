using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface ITurmaMaterialEstudoRepository : IRepository<TurmaMaterialEstudo>
    {
        IList<TurmaMaterialEstudo> ObtemPor(string identificador);
        int Insere(TurmaMaterialEstudo docenteDisponivel);
        int Remove(int identificador, string turma, int ano, int semestre, string disciplina,  int subperiodo);

        IList<TurmaMaterialEstudo> ObtemTurmaMatEstPor(string turma, int ano, int semestre, string disciplina,  int subperiodo);
    }
}
