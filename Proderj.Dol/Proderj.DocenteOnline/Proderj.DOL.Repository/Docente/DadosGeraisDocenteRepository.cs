using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Repository
{
    public class DadosGeraisDocenteRepository : NHRepositoryBase<DadosGeraisDocente>, IDadosGeraisDocenteRepository
    {
        public DadosGeraisDocente ObtemPor(string matricula)
        {
            return Sessao.QueryOver<DadosGeraisDocente>()
                .Where(d => d.Matricula == matricula)
                .SingleOrDefault();
        }

        public DadosGeraisDocente ObtemPorPessoa(string pessoa)
        {
            return Sessao.QueryOver<DadosGeraisDocente>()
                .Where(d => d.Num_func == pessoa)
                .SingleOrDefault();
        }


        public List<string> ObtemEmailsPor(string cpf, out string nome)
        {
            List<string> email = new List<string>();
            nome = string.Empty;

            IList<DadosGeraisDocente> dadosGeraisDocente = Sessao.QueryOver<DadosGeraisDocente>()
                .Where(d => d.CPF == cpf).List();

            if(dadosGeraisDocente.Count > 0)
                nome = dadosGeraisDocente.Select(x => x.Nome).First();

            foreach (DadosGeraisDocente item in dadosGeraisDocente)
            {
                if (!item.EmailGoogle.IsNullOrEmpty())
                    email.Add(item.EmailGoogle);

                if (!item.EmailInterno.IsNullOrEmpty())
                    email.Add(item.EmailInterno);
            }

            return email.Distinct().ToList();
        }
    }
}
