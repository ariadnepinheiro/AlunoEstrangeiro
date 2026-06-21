using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class DadosFormacaoDocenteRepository : NHRepositoryBase<DadosFormacaoDocente>, IDadosFormacaoDocenteRepository
    {
        public IList<DadosFormacaoDocente> ListaFormacaoPor(string matricula, TipoFormacaoEnum tipoFormacao)
        {
            IList<DadosFormacaoDocente> lista = null;
            string[] formacoes = null;

            if (tipoFormacao == TipoFormacaoEnum.Graduacao)
                formacoes = new string[] { "Superior Tecnólogo", "Superior Licenciatura", "Superior", "Superior Bacharelado" }; 
            else
                formacoes = new string[] { "Pós-Graduação - Especialização", "Pós-Graduação - Doutorado", "Pós-Graduação - Mestrado" }; 

            lista = Sessao.QueryOver<DadosFormacaoDocente>()
                .Where(d => d.Matricula == matricula)
                .AndRestrictionOn(d => d.Escolaridade).IsIn(formacoes)
                .List();                        

            return lista;
        }
    }
}
