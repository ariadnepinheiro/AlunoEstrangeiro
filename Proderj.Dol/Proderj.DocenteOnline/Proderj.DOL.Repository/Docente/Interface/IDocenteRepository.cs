using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface IDocenteRepository : IRepository<Docente>
    {
        Docente ObtemPor(string idfuncional);

        Docente ObtemPorPessoa(string pessoa);

		long ObtemNumFuncPor(string matricula);

        String ObtemEmailPor(string matricula);

        void RedefineSenha(string novasenha, string matricula);

        void AlteraSenha(string novaSenha, string matricula);

		Docente ObtemPorPessoaPor(string matricula);
    }
}
