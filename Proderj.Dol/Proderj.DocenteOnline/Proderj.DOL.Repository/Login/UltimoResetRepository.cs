using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class UltimoResetRepository : NHRepositoryBase<UltimoReset>, IUltimoResetRepository
    {
        #region IUltimoResetRepository Members

        public UltimoReset VerificaDadosUltimoReset(string matricula)
        {
            return Sessao.QueryOver<UltimoReset>()
                 .Where(ultimoReset => ultimoReset.Matricula == matricula)
                 .SingleOrDefault();
        }

        public void AtualizaUltimoResetPorMatricula(string matricula)
        {
            try
            {
                var reset = Sessao.QueryOver<UltimoReset>()
                   .Where(ultimoReset => ultimoReset.Matricula == matricula)
                       .SingleOrDefault();

                reset.DataUltimoReset = DateTime.Now;

                SessaoAuditada.Update(reset);
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        #endregion
    }
}
