using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class ContratoOperadora
    {
        public void Insere(DataContext contexto, RN.FiscalizacaoLink.Entidades.ContratoOperadora contratoOperadora)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO FiscalizacaoLink.CONTRATOOPERADORA
                                               (OPERADORAID
                                               ,CONTRATOID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@OPERADORAID, 
                                               @CONTRATOID, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@OPERADORAID", SqlDbType.Int, contratoOperadora.OperadoraId);
            contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contratoOperadora.ContratoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contratoOperadora.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);           

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiContratoOperadoraPor(DataContext contexto, int operadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.CONTRATOOPERADORA 
                                    where OPERADORAID = @OPERADORAID ";

            contextQuery.Parameters.Add("@OPERADORAID", operadoraId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Atualiza(DataContext contexto, RN.FiscalizacaoLink.Entidades.ContratoOperadora contratoOperadora)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FiscalizacaoLink.CONTRATOOPERADORA
                                           SET USUARIOID = @USUARIOID, 
                                              OPERADORAID = @OPERADORAID,
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE CONTRATOOPERADORAID = @CONTRATOOPERADORAID ";

            contextQuery.Parameters.Add("@CONTRATOOPERADORAID", SqlDbType.Int, contratoOperadora.ContratoOperadoraId);
            contextQuery.Parameters.Add("@OPERADORAID", SqlDbType.Int, contratoOperadora.OperadoraId);        
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contratoOperadora.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
