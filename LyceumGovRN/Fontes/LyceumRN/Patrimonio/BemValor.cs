using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class BemValor
    { 
        public Entidades.BemValor ObtemBemValorVigentePor(DataContext contexto, int bemId, DateTime dataPesquisa)
        {
            Entidades.BemValor bemValor = new Entidades.BemValor();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                    FROM PATRIMONIO.BEMVALOR (NOLOCK)
                                    WHERE  BEMID = @BEMID
	                                    AND DATAINICIO  <= @DATA
	                                    AND (DATAFIM IS NULL OR DATAFIM >= @DATA) ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, dataPesquisa.Date);

            bemValor = contexto.TryToBindEntity<Entidades.BemValor>(contextQuery);

            return bemValor;
        }

        public bool PossuiMoedaPor(DataContext ctx, int moedaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PATRIMONIO.BEMVALOR (NOLOCK)
                                    WHERE MOEDAID = @MOEDAID ";

            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, moedaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiEstadoConservacaoPor(DataContext ctx, int estadoConservacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Patrimonio.BEMVALOR (NOLOCK)
                                    WHERE ESTADOCONSERVACAOID = @ESTADOCONSERVACAOID ";

            contextQuery.Parameters.Add("@ESTADOCONSERVACAOID", SqlDbType.Int, estadoConservacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DataContext contexto, Entidades.BemValor bemValor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Patrimonio.BEMVALOR
                                                (BEMID, 
                                                 MOEDAID, 
                                                 ESTADOCONSERVACAOID, 
                                                 VALOR, 
                                                 VIDAUTIL, 
                                                 DATAINICIO,                                                  
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@BEMID, 
                                                 @MOEDAID, 
                                                 @ESTADOCONSERVACAOID, 
                                                 @VALOR, 
                                                 @VIDAUTIL, 
                                                 @DATAINICIO,                                                 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) 

                        SELECT IDENT_CURRENT('Patrimonio.BEMVALOR') ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemValor.BemId);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, bemValor.MoedaId);
            contextQuery.Parameters.Add("@ESTADOCONSERVACAOID", SqlDbType.Int, bemValor.EstadoconservacaoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, bemValor.Valor);
            contextQuery.Parameters.Add("@VIDAUTIL", SqlDbType.Int, bemValor.VidaUtil);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, bemValor.DataInicio.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, bemValor.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            bemValor.BemValorId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Altera(DataContext contexto, Entidades.BemValor bemValor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Patrimonio.BEMVALOR
                                    SET ESTADOCONSERVACAOID = @ESTADOCONSERVACAOID, 
                                        VALOR = @VALOR, 
                                        VIDAUTIL = @VIDAUTIL,
                                        DATAINICIO = @DATAINICIO,    
                                        USUARIOID = @USUARIOID, 
                                        DATAALTERACAO = @DATAALTERACAO 
                                    WHERE  BEMID = @BEMID ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemValor.BemId);
            contextQuery.Parameters.Add("@MOEDAID", SqlDbType.Int, bemValor.MoedaId);
            contextQuery.Parameters.Add("@ESTADOCONSERVACAOID", SqlDbType.Int, bemValor.EstadoconservacaoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, bemValor.Valor);
            contextQuery.Parameters.Add("@VIDAUTIL", SqlDbType.Int, bemValor.VidaUtil);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, bemValor.DataInicio.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, bemValor.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void FinalizaBemValorAtivo(DataContext contexto, int bemId, DateTime dataFim, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PATRIMONIO.BEMVALOR
                                       SET DATAFIM = @DATAFIM,
                                          USUARIOID = @USUARIOID,
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE BEMID = @BEMID
	                                    AND (DATAFIM IS NULL OR DATAFIM >= GETDATE())
                                     ";

            contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public int ObtemBemValorAtivoPor(DataContext contexto, int bemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT TOP 1 BEMVALORID
									FROM  PATRIMONIO.BEMVALOR (NOLOCK)
                                    WHERE BEMID = @BEMID
	                                    AND (DATAFIM IS NULL OR DATAFIM >= GETDATE()) 
                                    ORDER BY DATACADASTRO DESC ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["BEMVALORID"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public DateTime ObtemInicioValorAtivoPor(DataContext contexto, int bemId)
        {
            DateTime data = DateTime.MinValue;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DATAINICIO
                                            FROM  PATRIMONIO.BEMVALOR (NOLOCK) 
                                            WHERE  BEMID = @BEMID 
	                                            AND DATAFIM IS NULL ";

                contextQuery.Parameters.Add("@BEMID", SqlDbType.Int, bemId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    data = Convert.ToDateTime(reader["DATAINICIO"]);
                }

                return data;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
