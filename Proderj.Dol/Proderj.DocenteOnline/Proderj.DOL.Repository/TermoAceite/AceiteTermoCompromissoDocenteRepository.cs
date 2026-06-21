using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class AceiteTermoCompromissoDocenteRepository : NHRepositoryBase<AceiteTermoCompromissoDocente>, IAceiteTermoCompromissoDocenteRepository
    {
        public AceiteTermoCompromissoDocente Insere(AceiteTermoCompromissoDocente aceite)
        {
            int resultado = 0;
            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                    @"INSERT  INTO [dbo].[TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE]
                                (
                                  ID_TERMO_DOCENTE,
                                  NUM_FUNC,
                                  IP,
                                  ANO,
                                  DT_ACEITE
                                )
                        VALUES  (
                                  :ID_TERMO_DOCENTE,
                                  :NUM_FUNC,
                                  :IP,
                                  :ANO,
                                  :DT_ACEITE
                                )");

                query.SetInt64("ID_TERMO_DOCENTE", aceite.TermoCompromissoDocente.Id);
                query.SetString("NUM_FUNC", aceite.Num_func);
                query.SetString("IP", aceite.IP);
                query.SetInt16("ANO", aceite.Ano);
                query.SetDateTime("DT_ACEITE", aceite.DataAceite);

                resultado = query.ExecuteUpdate();
                aceite.Id = resultado;
                return aceite;
            }
            catch (Exception)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }          
        }
    }
}
