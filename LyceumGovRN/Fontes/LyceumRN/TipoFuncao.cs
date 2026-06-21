using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class TipoFuncao : RNBase
    {
        #region Propriedades e Enum
        public enum EnumTipoFuncao
        {
            [StringValue("AAGE")]
            AAGE = 1,
            [StringValue("MEDIADOR DE TECNOLOGIA")]
            MediadorDeTecnologia = 2,
            [StringValue("MEDIADOR ARTICULADOR")]
            MediadorArticulador = 3,
            [StringValue("OUTROS")]
            Outros = 4
        }
        #endregion

        public bool PossuiTipoFuncaoComAssociacaoAtivaPor(DataContext ctx, int tipoFuncaoId, int funcaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            bool possui = false;

            try
            {
                sql.Append(@" SELECT COUNT(DISTINCT L.MATRICULA) 
                            FROM   LY_LOTACAO L 
                                   INNER JOIN LY_DOCENTE D 
                                           ON ( L.MATRICULA = D.MATRICULA ) ");

                if (tipoFuncaoId == 1)
                {
                    sql.Append(@" INNER JOIN AAGE.DOCENTEAAGE_UNIDADEENSINO DO 
                            ON ( D.NUM_FUNC = DO.DOCENTEID ) ");
                }
                else if (tipoFuncaoId == 2)
                {
                    sql.Append(@" INNER JOIN AAGE.DOCENTEMEDIADOR_UNIDADEENSINO DO 
                            ON ( D.NUM_FUNC = DO.DOCENTEID ) ");
                }
                else if (tipoFuncaoId == 3)
                {
                    sql.Append(@" INNER JOIN AAGE.DOCENTEARTICULADOR_REGIONAL DO 
                            ON ( D.NUM_FUNC = DO.DOCENTEID ) ");
                }

                sql.Append(@" WHERE  L.FUNCAO = @FUNCAOID 
                           AND L.DATA_NOMEACAO < GETDATE() 
                           AND ( L.DATA_DESATIVACAO >= GETDATE() 
                                  OR L.DATA_DESATIVACAO IS NULL ) 
                           AND GETDATE() BETWEEN DO.DATAINICIO_VINCULO AND DO.DATAFIM_VINCULO ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@FUNCAOID", funcaoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public bool PossuiTipoFuncaoEmLotacaoAtivaPor(DataContext ctx, int tipoFuncao, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    LY_LOTACAO L ( NOLOCK )
                                                    INNER JOIN LY_DOCENTE D ( NOLOCK ) ON L.MATRICULA = D.MATRICULA
                                                    INNER JOIN dbo.FUNCAO F ( NOLOCK ) ON L.FUNCAO = F.FUNCAOID
                                            WHERE   NUM_FUNC = @NUM_FUNC
                                                    AND f.TIPOFUNCAOID = @TIPOFUNCAOID
                                                    AND ( L.DATA_DESATIVACAO IS NULL
                                                          OR L.DATA_DESATIVACAO >= GETDATE()
                                                        ) ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@TIPOFUNCAOID", tipoFuncao);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public List<DadosTipoFuncao> ListaTipoFuncao()
        {
            DadosTipoFuncao tipoFuncao = new DadosTipoFuncao();
            List<DadosTipoFuncao> listaTipoFuncao = new List<DadosTipoFuncao>();

            try
            {
                tipoFuncao.TipoFuncaoId = (int)EnumTipoFuncao.AAGE;
                tipoFuncao.TipoFuncao = "AAGE";
                listaTipoFuncao.Add(tipoFuncao);

                tipoFuncao = new DadosTipoFuncao();
                tipoFuncao.TipoFuncaoId = (int)EnumTipoFuncao.MediadorArticulador;
                tipoFuncao.TipoFuncao = "MEDIADOR ARTICULADOR";
                listaTipoFuncao.Add(tipoFuncao);

                tipoFuncao = new DadosTipoFuncao();
                tipoFuncao.TipoFuncaoId = (int)EnumTipoFuncao.MediadorDeTecnologia;                
                tipoFuncao.TipoFuncao = "MEDIADOR DE TECNOLOGIA";
                listaTipoFuncao.Add(tipoFuncao);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaTipoFuncao;
        }

        public DataTable ListaFuncaoTipoFuncao()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT FUNCAOID, 
                                               DESCRICAO, 
                                               TIPOFUNCAOID, 
                                               CASE 
                                                 WHEN TIPOFUNCAOID = 1 THEN 'AAGE' 
                                                 WHEN TIPOFUNCAOID = 2 THEN 'MEDIADOR DE TECNOLOGIA' 
                                                 WHEN TIPOFUNCAOID = 3 THEN 'MEDIADOR ARTICULADOR'
                                               END TIPOFUNCAO 
                                        FROM   FUNCAO F ( NOLOCK ) 
                                        WHERE TIPOFUNCAOID <> @TIPOFUNCAOIDOUTROS
                                        ORDER  BY DESCRICAO ";

                contextQuery.Parameters.Add("@TIPOFUNCAOIDOUTROS", (int)EnumTipoFuncao.Outros);

                lista = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lista;
        }
    }
}