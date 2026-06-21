using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class ProtocoloRelHorasTrabalhadaGlp
    {
        public int GeraProtocolo(int ano, int mes, string faculdade, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            int protocolo = 0;
            try
            {
                //Cria Protocolo
                protocolo = this.Insere(contexto, usuarioId);

                //Cria vinculo com frequencias 
                this.InsereCargaHNaoTrabMes_ProtocoloRelHorasTrabalhadaGlp(contexto, protocolo, faculdade, ano, mes);

                return protocolo;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        private int Insere(DataContext contexto, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            //criação do protocolo
            contextQuery.Command = @" insert into RecursosHumanos.PROTOCOLORELHORASTRABALHADAGLP (DATAGERACAO, USUARIO)
                    values (@DATAGERACAO, @USUARIO)

                    SELECT IDENT_CURRENT('RecursosHumanos.PROTOCOLORELHORASTRABALHADAGLP')
                ";

            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAGERACAO", SqlDbType.DateTime, DateTime.Now);

            int protocolo = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            return protocolo;
        }

        private void InsereCargaHNaoTrabMes_ProtocoloRelHorasTrabalhadaGlp(DataContext contexto, int protocolo, string unidadeEnsino, int ano, int mes)
        {
            ContextQuery contextQuery = new ContextQuery();

            //criação do vinculo entre frequencia e protocolo
            contextQuery.Command = @"INSERT INTO RecursosHumanos.CARGAHNAOTRABMES_PROTOCOLORELHORASTRABALHADAGLP 
                                                (PROTOCOLO, 
                                                 ID_CARGAHNAOTRABMES) 
                                    SELECT @PROTOCOLO, 
                                           C.ID_CARGAHNAOTRABMES 
                                    FROM   RECURSOSHUMANOS.CARGAHNAOTRABMES C 
                                    WHERE  ANO = @ANO 
                                           AND MES = @MES 
                                           AND UNIDADE_ENS = @UNIDADE_ENS 
                ";

            contextQuery.Parameters.Add("@PROTOCOLO", SqlDbType.Int, protocolo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.VarChar, unidadeEnsino);


            contexto.ApplyModifications(contextQuery);
        }
    }
}
