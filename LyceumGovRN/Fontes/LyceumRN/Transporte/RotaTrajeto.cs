using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Transporte
{
    public class RotaTrajeto
    {
        public bool PossuiTipoContratacaoPor(DataContext contexto, int tipoContratacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTATRAJETO (NOLOCK)
                                    WHERE TIPOCONTRATACAOID = @TIPOCONTRATACAOID ";

            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, tipoContratacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Desativa(DataContext contexto, int rotaId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RT
                                        SET    ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        FROM   Transporte.ROTA r (NOLOCK) 
											   INNER JOIN TRANSPORTE.ROTATRAJETO RT (NOLOCK) 
													ON R.ROTAID = RT.ROTAID
                                        WHERE   R.ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public int ObtemVeiculoPor(DataContext contexto, int rotaTrajetoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT VEICULOID
                                          FROM TRANSPORTE.ROTATRAJETO (NOLOCK)
                                                WHERE ROTATRAJETOID = @ROTATRAJETOID ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["VEICULOID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["VEICULOID"]);
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

        public bool ObtemTipoTrajetoPor(DataContext contexto, int rotaTrajetoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT IDA
                                          FROM TRANSPORTE.ROTATRAJETO (NOLOCK)
                                                WHERE ROTATRAJETOID = @ROTATRAJETOID ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToBoolean(reader["IDA"]);
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

        public bool PossuiRotaAtivaPor(DataContext contexto, int prestadorId, int condutorId, int veiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTATRAJETO (NOLOCK)
                                    WHERE PRESTADORID = @PRESTADORID
										  AND CONDUTORID = @CONDUTORID
										  AND VEICULOID = @VEICULOID
										  AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DataContext contexto, Entidades.RotaTrajeto rotaTrajeto)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.ROTATRAJETO 
                                                    (ROTAID, 
                                                     TIPOCONTRATACAOID, 
                                                     VALORROTA, 
                                                     IDA, 
                                                     QUANTIDADEKM, 
                                                     TEMPO,
                                                     ATIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      ( @ROTAID, 
                                                      @TIPOCONTRATACAOID, 
                                                      @VALORROTA, 
                                                      @IDA, 
                                                      @QUANTIDADEKM, 
                                                      @TEMPO,
                                                      @ATIVO, 
                                                      @USUARIOID, 
                                                      @DATACADASTRO, 
                                                      @DATAALTERACAO )  

                                SELECT IDENT_CURRENT('Transporte.ROTATRAJETO') ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaTrajeto.RotaId);
            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, rotaTrajeto.TipoContratacaoId);
            contextQuery.Parameters.Add("@VALORROTA", SqlDbType.Decimal, rotaTrajeto.ValorRota);
            contextQuery.Parameters.Add("@IDA", SqlDbType.Bit, rotaTrajeto.Ida);
            contextQuery.Parameters.Add("@QUANTIDADEKM", SqlDbType.Decimal, rotaTrajeto.QuantidadeKm);
            contextQuery.Parameters.Add("@TEMPO", SqlDbType.Int, rotaTrajeto.Tempo);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, rotaTrajeto.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rotaTrajeto.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            rotaTrajeto.RotaTrajetoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(DataContext contexto, Entidades.RotaTrajeto rotaTrajeto)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Transporte.ROTATRAJETO
                                        SET    PRESTADORID = @PRESTADORID, 
                                               CONDUTORID = @CONDUTORID, 
                                               VEICULOID = @VEICULOID, 
                                               TIPOCONTRATACAOID = @TIPOCONTRATACAOID, 
                                               VALORROTA = @VALORROTA,
                                               QUANTIDADEKM = @QUANTIDADEKM,
                                               TEMPO = @TEMPO,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  ROTATRAJETOID = @ROTATRAJETOID ";

            contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajeto.RotaTrajetoId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, rotaTrajeto.PrestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, rotaTrajeto.CondutorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, rotaTrajeto.VeiculoId);
            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, rotaTrajeto.TipoContratacaoId);
            contextQuery.Parameters.Add("@VALORROTA", SqlDbType.Decimal, rotaTrajeto.ValorRota);
            contextQuery.Parameters.Add("@QUANTIDADEKM", SqlDbType.Decimal, rotaTrajeto.QuantidadeKm);
            contextQuery.Parameters.Add("@TEMPO", SqlDbType.Int, rotaTrajeto.Tempo);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, rotaTrajeto.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rotaTrajeto.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }       

        public void Remove(DataContext contexto, int rotaI)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Transporte.ROTATRAJETO
                                      WHERE  ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaI);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
