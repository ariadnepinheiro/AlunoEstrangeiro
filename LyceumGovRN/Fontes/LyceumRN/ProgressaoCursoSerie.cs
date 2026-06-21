using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Collections;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class ProgressaoCursoSerie : RNBase
    {
        public IList<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie> RetornaTodos()
        {
            SqlDataReader reader = null;
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            var ctxQuery = new ContextQuery
            {
                Command = @"
                                SELECT P.progressaoserieid                      AS PROGRESSAOSERIEID, 
                                       CO.nome                                  AS NOMECURSO, 
                                       P.cursoid                                AS CURSOID, 
                                       CONVERT(VARCHAR(10), CO.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.cursoid)        AS CURSO, 
                                       TC.tipo									AS TIPOCURSOID,
                                       TC.descricao                             AS TIPOCURSO, 
                                       P.serieid                                AS SERIEID, 
                                       MO.modalidade                            AS CODMODALIDADE, 
                                       MO.descricao                             AS MODALIDADE,  
                                       CD.nome                                  AS PROXIMONOMECURSO, 
                                       P.proximocursoid                         AS PROXIMOCURSOID, 
                                       CONVERT(VARCHAR(10), CD.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.proximocursoid) AS PROXIMOCURSO,
                                       TD.tipo									AS PROXIMOTIPOCURSOID,
                                       TD.descricao                             AS PROXIMOTIPOCURSO, 
                                       P.proximoserieid                         AS PROXIMOSERIEID, 
                                       MD.modalidade                            AS PROXIMOCODMODALIDADE, 
                                       MD.descricao                             AS PROXIMOMODALIDADE, 
                                       P.datacadastro                           AS DATACADASTRO, 
                                       P.matricula                              AS MATRICULA ,
                                       PARTICIPAFASE1,
                                       PARTICIPAFASE2
                                FROM   progressaoserie P 
                                       INNER JOIN ly_curso CO 
                                               ON ( P.cursoid = CO.curso ) 
                                       INNER JOIN ly_tipo_curso TC 
                                               ON ( CO.tipo = TC.tipo ) 
                                       INNER JOIN ly_curso CD 
                                               ON ( P.proximocursoid = CD.curso ) 
                                       INNER JOIN ly_tipo_curso TD 
                                               ON ( CO.tipo = TD.tipo ) 
                                       INNER JOIN ly_modalidade_curso MO 
                                               ON ( MO.modalidade = CO.modalidade ) 
                                       INNER JOIN ly_modalidade_curso MD 
                                               ON ( MD.modalidade = CO.modalidade )
                                "
            };

            var lista = new List<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie>();

            try
            {
                reader = ctx.GetDataReader(ctxQuery);

                while (reader.Read())
                {
                    var item = new Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie();

                    item.IdProgressaoSerie = !String.IsNullOrEmpty(reader["PROGRESSAOSERIEID"].ToString()) ? Convert.ToInt32(reader["PROGRESSAOSERIEID"]) : default(int);
                    item.NomeCurso = !String.IsNullOrEmpty(reader["NOMECURSO"].ToString()) ? Convert.ToString(reader["NOMECURSO"]) : default(string);
                    item.CursoId = !String.IsNullOrEmpty(reader["CURSOID"].ToString()) ? Convert.ToString(reader["CURSOID"]) : default(string);
                    item.Curso = !String.IsNullOrEmpty(reader["CURSO"].ToString()) ? Convert.ToString(reader["CURSO"]) : default(string);
                    item.TipoCurso = !String.IsNullOrEmpty(reader["TIPOCURSO"].ToString()) ? Convert.ToString(reader["TIPOCURSO"]) : default(string);
                    item.TipoCursoId = !String.IsNullOrEmpty(reader["TIPOCURSOID"].ToString()) ? Convert.ToString(reader["TIPOCURSOID"]) : default(string);
                    item.Serie = !String.IsNullOrEmpty(reader["SERIEID"].ToString()) ? Convert.ToString(reader["SERIEID"]) : default(string);
                    item.Modalidade = !String.IsNullOrEmpty(reader["MODALIDADE"].ToString()) ? Convert.ToString(reader["MODALIDADE"]) : default(string);
                    item.CodModalidade = !String.IsNullOrEmpty(reader["CODMODALIDADE"].ToString()) ? Convert.ToString(reader["CODMODALIDADE"]) : default(string);
                    item.ProxNomeCurso = !String.IsNullOrEmpty(reader["PROXIMONOMECURSO"].ToString()) ? Convert.ToString(reader["PROXIMONOMECURSO"]) : default(string);
                    item.ProxCursoId = !String.IsNullOrEmpty(reader["PROXIMOCURSOID"].ToString()) ? Convert.ToString(reader["PROXIMOCURSOID"]) : default(string);
                    item.ProxCurso = !String.IsNullOrEmpty(reader["PROXIMOCURSO"].ToString()) ? Convert.ToString(reader["PROXIMOCURSO"]) : default(string);
                    item.ProxTipoCursoId = !String.IsNullOrEmpty(reader["PROXIMOTIPOCURSOID"].ToString()) ? Convert.ToString(reader["PROXIMOTIPOCURSOID"]) : default(string);
                    item.ProxTipoCurso = !String.IsNullOrEmpty(reader["PROXIMOTIPOCURSO"].ToString()) ? Convert.ToString(reader["PROXIMOTIPOCURSO"]) : default(string);
                    item.ProxSerie = !String.IsNullOrEmpty(reader["PROXIMOSERIEID"].ToString()) ? Convert.ToString(reader["PROXIMOSERIEID"]) : default(string);
                    item.ProxModalidade = !String.IsNullOrEmpty(reader["PROXIMOMODALIDADE"].ToString()) ? Convert.ToString(reader["PROXIMOMODALIDADE"]) : default(string);
                    item.ProxCodModalidade = !String.IsNullOrEmpty(reader["PROXIMOCODMODALIDADE"].ToString()) ? Convert.ToString(reader["PROXIMOCODMODALIDADE"]) : default(string);
                    item.DataCadastro = !String.IsNullOrEmpty(reader["DATACADASTRO"].ToString()) ? Convert.ToString(reader["DATACADASTRO"]) : default(string);
                    item.Matricula = !String.IsNullOrEmpty(reader["MATRICULA"].ToString()) ? Convert.ToString(reader["MATRICULA"]) : default(string);
                    item.ParticipaFase1 = !String.IsNullOrEmpty(reader["PARTICIPAFASE1"].ToString()) ? Convert.ToBoolean(reader["PARTICIPAFASE1"]) : (bool?)null;
                    item.ParticipaFase2 = !String.IsNullOrEmpty(reader["PARTICIPAFASE2"].ToString()) ? Convert.ToBoolean(reader["PARTICIPAFASE2"]) : (bool?)null;

                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie RetornaPorId(int pId)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var ctxQuery = new ContextQuery
                    {
                        Command = @"
                                SELECT P.progressaoserieid                      AS PROGRESSAOSERIEID, 
                                       CO.nome                                  AS NOMECURSO, 
                                       P.cursoid                                AS CURSOID, 
                                       CONVERT(VARCHAR(10), CO.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.cursoid)        AS CURSO, 
                                       TC.tipo									AS TIPOCURSOID,
                                       TC.descricao                             AS TIPOCURSO, 
                                       P.serieid                                AS SERIEID, 
                                       MO.modalidade                            AS CODMODALIDADE, 
                                       MO.descricao                             AS MODALIDADE,  
                                       CD.nome                                  AS PROXIMONOMECURSO, 
                                       P.proximocursoid                         AS PROXIMOCURSOID, 
                                       CONVERT(VARCHAR(10), CD.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.proximocursoid) AS PROXIMOCURSO,
                                       TD.tipo									AS PROXIMOTIPOCURSOID,
                                       TD.descricao                             AS PROXIMOTIPOCURSO, 
                                       P.proximoserieid                         AS PROXIMOSERIEID, 
                                       MD.modalidade                            AS PROXIMOCODMODALIDADE, 
                                       MD.descricao                             AS PROXIMOMODALIDADE, 
                                       P.datacadastro                           AS DATACADASTRO, 
                                       P.matricula                              AS MATRICULA ,
                                       PARTICIPAFASE1,
                                       PARTICIPAFASE2
                                FROM   progressaoserie P 
                                       INNER JOIN ly_curso CO 
                                               ON ( P.cursoid = CO.curso ) 
                                       INNER JOIN ly_tipo_curso TC 
                                               ON ( CO.tipo = TC.tipo ) 
                                       INNER JOIN ly_curso CD 
                                               ON ( P.proximocursoid = CD.curso ) 
                                       INNER JOIN ly_tipo_curso TD 
                                               ON ( CO.tipo = TD.tipo ) 
                                       INNER JOIN ly_modalidade_curso MO 
                                               ON ( MO.modalidade = CO.modalidade ) 
                                       INNER JOIN ly_modalidade_curso MD 
                                               ON ( MD.modalidade = CO.modalidade )
                                "
                    };

                ctxQuery.Parameters.Add("@PROGRESSAOSERIEID", pId);

                var item = new Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie();

                try
                {
                    var reader = ctx.GetDataReader(ctxQuery);

                    while (reader.Read())
                    {
                        item.IdProgressaoSerie = !String.IsNullOrEmpty(reader["PROGRESSAOSERIEID"].ToString()) ? Convert.ToInt32(reader["PROGRESSAOSERIEID"]) : default(int);
                        item.NomeCurso = !String.IsNullOrEmpty(reader["NOMECURSO"].ToString()) ? Convert.ToString(reader["NOMECURSO"]) : default(string);
                        item.CursoId = !String.IsNullOrEmpty(reader["CURSOID"].ToString()) ? Convert.ToString(reader["CURSOID"]) : default(string);
                        item.Curso = !String.IsNullOrEmpty(reader["CURSO"].ToString()) ? Convert.ToString(reader["CURSO"]) : default(string);
                        item.TipoCurso = !String.IsNullOrEmpty(reader["TIPOCURSO"].ToString()) ? Convert.ToString(reader["TIPOCURSO"]) : default(string);
                        item.TipoCursoId = !String.IsNullOrEmpty(reader["TIPOCURSOID"].ToString()) ? Convert.ToString(reader["TIPOCURSOID"]) : default(string);
                        item.Serie = !String.IsNullOrEmpty(reader["SERIEID"].ToString()) ? Convert.ToString(reader["SERIEID"]) : default(string);
                        item.Modalidade = !String.IsNullOrEmpty(reader["MODALIDADE"].ToString()) ? Convert.ToString(reader["MODALIDADE"]) : default(string);
                        item.CodModalidade = !String.IsNullOrEmpty(reader["CODMODALIDADE"].ToString()) ? Convert.ToString(reader["CODMODALIDADE"]) : default(string);
                        item.ProxNomeCurso = !String.IsNullOrEmpty(reader["PROXIMONOMECURSO"].ToString()) ? Convert.ToString(reader["PROXIMONOMECURSO"]) : default(string);
                        item.ProxCursoId = !String.IsNullOrEmpty(reader["PROXIMOCURSOID"].ToString()) ? Convert.ToString(reader["PROXIMOCURSOID"]) : default(string);
                        item.ProxCurso = !String.IsNullOrEmpty(reader["PROXIMOCURSO"].ToString()) ? Convert.ToString(reader["PROXIMOCURSO"]) : default(string);
                        item.ProxTipoCursoId = !String.IsNullOrEmpty(reader["PROXIMOTIPOCURSOID"].ToString()) ? Convert.ToString(reader["PROXIMOTIPOCURSOID"]) : default(string);
                        item.ProxTipoCurso = !String.IsNullOrEmpty(reader["PROXIMOTIPOCURSO"].ToString()) ? Convert.ToString(reader["PROXIMOTIPOCURSO"]) : default(string);
                        item.ProxSerie = !String.IsNullOrEmpty(reader["PROXIMOSERIEID"].ToString()) ? Convert.ToString(reader["PROXIMOSERIEID"]) : default(string);
                        item.ProxModalidade = !String.IsNullOrEmpty(reader["PROXIMOMODALIDADE"].ToString()) ? Convert.ToString(reader["PROXIMOMODALIDADE"]) : default(string);
                        item.ProxCodModalidade = !String.IsNullOrEmpty(reader["PROXIMOCODMODALIDADE"].ToString()) ? Convert.ToString(reader["PROXIMOCODMODALIDADE"]) : default(string);
                        item.DataCadastro = !String.IsNullOrEmpty(reader["DATACADASTRO"].ToString()) ? Convert.ToString(reader["DATACADASTRO"]) : default(string);
                        item.Matricula = !String.IsNullOrEmpty(reader["MATRICULA"].ToString()) ? Convert.ToString(reader["MATRICULA"]) : default(string);
                        item.ParticipaFase1 = !String.IsNullOrEmpty(reader["PARTICIPAFASE1"].ToString()) ? Convert.ToBoolean(reader["PARTICIPAFASE1"]) : (bool?)null;
                        item.ParticipaFase2 = !String.IsNullOrEmpty(reader["PARTICIPAFASE2"].ToString()) ? Convert.ToBoolean(reader["PARTICIPAFASE2"]) : (bool?)null;
                    }

                    return item;
                }
                catch (Exception ex)
                {
                    ctx.Abandon();
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine, Convert.ToString(ex.Message));
                    throw new Exception(mensagem);
                }
                //finally
                //{
                //    ctx.Dispose();
                //}
            }
        }

        public IList<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie> RetornaProgressaoCursoSeriePor(string pSerie, string pCurso)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader resultado = null;

            var ctxQuery = new ContextQuery
            {
                Command = @"
                                SELECT P.progressaoserieid                      AS PROGRESSAOSERIEID, 
                                       CO.nome                                  AS NOMECURSO, 
                                       P.cursoid                                AS CURSOID, 
                                       CONVERT(VARCHAR(10), CO.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.cursoid)        AS CURSO, 
                                       TC.tipo									AS TIPOCURSOID,
                                       TC.descricao                             AS TIPOCURSO, 
                                       P.serieid                                AS SERIEID, 
                                       MO.modalidade                            AS CODMODALIDADE, 
                                       MO.descricao                             AS MODALIDADE,  
                                       CD.nome                                  AS PROXIMONOMECURSO, 
                                       P.proximocursoid                         AS PROXIMOCURSOID, 
                                       CONVERT(VARCHAR(10), CD.nome) + '   ' 
                                       + CONVERT(VARCHAR(10), P.proximocursoid) AS PROXIMOCURSO,
                                       TD.tipo									AS PROXIMOTIPOCURSOID,
                                       TD.descricao                             AS PROXIMOTIPOCURSO, 
                                       P.proximoserieid                         AS PROXIMOSERIEID, 
                                       MD.modalidade                            AS PROXIMOCODMODALIDADE, 
                                       MD.descricao                             AS PROXIMOMODALIDADE, 
                                       convert(varchar, P.datacadastro, 103)	AS DATACADASTRO, 
                                       convert(varchar, P.[DATAALTERACAO], 103) + ' ' + convert(varchar, P.DATAALTERACAO, 108) AS DATAALTERACAO,
                                       P.matricula                              AS MATRICULA,
									   CASE 
                                            WHEN P.[DATAALTERACAO] IS NULL THEN ''
                                            ELSE P.MATRICULA + ' - ' + ISNULL(U.NOME, '') 
                                       END NOMEALTERACAO,
                                       PARTICIPAFASE1,
                                       PARTICIPAFASE2
                                FROM   progressaoserie P 
                                       INNER JOIN ly_curso CO 
                                               ON ( P.cursoid = CO.curso ) 
                                       INNER JOIN ly_tipo_curso TC 
                                               ON ( CO.tipo = TC.tipo ) 
                                       INNER JOIN ly_curso CD 
                                               ON ( P.proximocursoid = CD.curso ) 
                                       INNER JOIN ly_tipo_curso TD 
                                               ON ( CD.tipo = TD.tipo ) 
                                       INNER JOIN ly_modalidade_curso MO 
                                               ON ( MO.modalidade = CO.modalidade ) 
                                       INNER JOIN ly_modalidade_curso MD 
                                               ON ( MD.modalidade = CD.modalidade ) 
									   LEFT JOIN HADES..HD_USUARIO U
											   ON ( P.matricula = U.USUARIO ) 
                                WHERE  P.serieid = @SERIE 
                                       AND CO.curso = @CURSO 
                                ORDER BY CO.nome
                                "
            };

            ctxQuery.Parameters.Add("@SERIE", pSerie);
            ctxQuery.Parameters.Add("@CURSO", pCurso);

            var lista = new List<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie>();

            try
            {
               resultado = ctx.GetDataReader(ctxQuery);

                while (resultado.Read())
                {
                    var item = new Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie();

                    item.IdProgressaoSerie = !String.IsNullOrEmpty(resultado["PROGRESSAOSERIEID"].ToString()) ? Convert.ToInt32(resultado["PROGRESSAOSERIEID"]) : default(int);
                    item.NomeCurso = !String.IsNullOrEmpty(resultado["NOMECURSO"].ToString()) ? Convert.ToString(resultado["NOMECURSO"]) : default(string);
                    item.CursoId = !String.IsNullOrEmpty(resultado["CURSOID"].ToString()) ? Convert.ToString(resultado["CURSOID"]) : default(string);
                    item.Curso = !String.IsNullOrEmpty(resultado["CURSO"].ToString()) ? Convert.ToString(resultado["CURSO"]) : default(string);
                    item.TipoCurso = !String.IsNullOrEmpty(resultado["TIPOCURSO"].ToString()) ? Convert.ToString(resultado["TIPOCURSO"]) : default(string);
                    item.TipoCursoId = !String.IsNullOrEmpty(resultado["TIPOCURSOID"].ToString()) ? Convert.ToString(resultado["TIPOCURSOID"]) : default(string);
                    item.Serie = !String.IsNullOrEmpty(resultado["SERIEID"].ToString()) ? Convert.ToString(resultado["SERIEID"]) : default(string);
                    item.Modalidade = !String.IsNullOrEmpty(resultado["MODALIDADE"].ToString()) ? Convert.ToString(resultado["MODALIDADE"]) : default(string);
                    item.CodModalidade = !String.IsNullOrEmpty(resultado["CODMODALIDADE"].ToString()) ? Convert.ToString(resultado["CODMODALIDADE"]) : default(string);
                    item.ProxNomeCurso = !String.IsNullOrEmpty(resultado["PROXIMONOMECURSO"].ToString()) ? Convert.ToString(resultado["PROXIMONOMECURSO"]) : default(string);
                    item.ProxCursoId = !String.IsNullOrEmpty(resultado["PROXIMOCURSOID"].ToString()) ? Convert.ToString(resultado["PROXIMOCURSOID"]) : default(string);
                    item.ProxCurso = !String.IsNullOrEmpty(resultado["PROXIMOCURSO"].ToString()) ? Convert.ToString(resultado["PROXIMOCURSO"]) : default(string);
                    item.ProxTipoCursoId = !String.IsNullOrEmpty(resultado["PROXIMOTIPOCURSOID"].ToString()) ? Convert.ToString(resultado["PROXIMOTIPOCURSOID"]) : default(string);
                    item.ProxTipoCurso = !String.IsNullOrEmpty(resultado["PROXIMOTIPOCURSO"].ToString()) ? Convert.ToString(resultado["PROXIMOTIPOCURSO"]) : default(string);
                    item.ProxSerie = !String.IsNullOrEmpty(resultado["PROXIMOSERIEID"].ToString()) ? Convert.ToString(resultado["PROXIMOSERIEID"]) : default(string);
                    item.ProxModalidade = !String.IsNullOrEmpty(resultado["PROXIMOMODALIDADE"].ToString()) ? Convert.ToString(resultado["PROXIMOMODALIDADE"]) : default(string);
                    item.ProxCodModalidade = !String.IsNullOrEmpty(resultado["PROXIMOCODMODALIDADE"].ToString()) ? Convert.ToString(resultado["PROXIMOCODMODALIDADE"]) : default(string);
                    item.DataCadastro = !String.IsNullOrEmpty(resultado["DATACADASTRO"].ToString()) ? Convert.ToString(resultado["DATACADASTRO"]) : default(string);
                    item.DataAlteracao = !String.IsNullOrEmpty(resultado["DATAALTERACAO"].ToString()) ? Convert.ToString(resultado["DATAALTERACAO"]) : default(string);
                    item.Matricula = !String.IsNullOrEmpty(resultado["MATRICULA"].ToString()) ? Convert.ToString(resultado["MATRICULA"]) : default(string);
                    item.Nome = !String.IsNullOrEmpty(resultado["NOMEALTERACAO"].ToString()) ? Convert.ToString(resultado["NOMEALTERACAO"]) : default(string);
                    item.ParticipaFase1 = !String.IsNullOrEmpty(resultado["PARTICIPAFASE1"].ToString()) ? Convert.ToBoolean(resultado["PARTICIPAFASE1"]) : (bool?)null;
                    item.ParticipaFase2 = !String.IsNullOrEmpty(resultado["PARTICIPAFASE2"].ToString()) ? Convert.ToBoolean(resultado["PARTICIPAFASE2"]) : (bool?)null;

                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (resultado != null)
                {
                    resultado.Close();
                }
                ctx.Dispose();
            }

        }

        public void Salva(Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie item)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var ctxQuery = new ContextQuery
                {
                    Command = @"
                                INSERT INTO progressaoserie 
                                            (cursoid, 
                                             serieid, 
                                             proximocursoid, 
                                             proximoserieid,
                                             participafase1,
                                             participafase2,
                                             datacadastro, 
                                             matricula) 
                                VALUES      (@CURSO, 
                                             @SERIE, 
                                             @PROXCURSO, 
                                             @PROXSERIE, 
                                             @PARTICIPAFASE1,
                                             @PARTICIPAFASE2,
                                             Getdate(), 
                                             @MATRICULA) "
                };

                ctxQuery.Parameters.Add("@MATRICULA", item.Matricula);
                ctxQuery.Parameters.Add("@CURSO", item.Curso);
                ctxQuery.Parameters.Add("@SERIE", item.Serie);
                ctxQuery.Parameters.Add("@PROXCURSO", item.ProxCurso);
                ctxQuery.Parameters.Add("@PROXSERIE", item.ProxSerie);
                ctxQuery.Parameters.Add("@PARTICIPAFASE1", item.ParticipaFase1);
                ctxQuery.Parameters.Add("@PARTICIPAFASE2", item.ParticipaFase2);

                ctx.ApplyModifications(ctxQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void AlteraParticipaFase(int idProgressaoSerie, bool participaFase1, bool participaFase2, string usuarioId)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var ctxQuery = new ContextQuery
                {
                    Command = @"
                                UPDATE progressaoserie 
                                SET    matricula = @MATRICULA,
                                       participafase1 = @PARTICIPAFASE1,
                                       participafase2 = @PARTICIPAFASE2,
                                       DATAALTERACAO = GETDATE()
                                WHERE  progressaoserieid = @PROGRESSAOSERIEID 
                                "
                };

                ctxQuery.Parameters.Add("@PROGRESSAOSERIEID", idProgressaoSerie);
                ctxQuery.Parameters.Add("@MATRICULA", usuarioId);
                ctxQuery.Parameters.Add("@PARTICIPAFASE1", participaFase1);
                ctxQuery.Parameters.Add("@PARTICIPAFASE2", participaFase2);

                ctx.ApplyModifications(ctxQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void Altera(Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie item)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var ctxQuery = new ContextQuery
                {
                    Command = @"
                                UPDATE progressaoserie 
                                SET    cursoid = @CURSO, 
                                       serieid = @SERIE, 
                                       proximocursoid = @PROXCURSO, 
                                       proximoserieid = @PROXSERIE, 
                                       datacadastro = Getdate(), 
                                       matricula = @MATRICULA,
                                       participafase1 = @PARTICIPAFASE1,
                                       participafase2 = @PARTICIPAFASE2,
                                WHERE  progressaoserieid = @PROGRESSAOSERIEID 
                                "
                };

                ctxQuery.Parameters.Add("@PROGRESSAOSERIEID", item.IdProgressaoSerie);
                ctxQuery.Parameters.Add("@CURSO", item.Curso);
                ctxQuery.Parameters.Add("@SERIE", item.Serie);
                ctxQuery.Parameters.Add("@PROXCURSO", item.ProxCurso);
                ctxQuery.Parameters.Add("@PROXSERIE", item.ProxSerie);
                ctxQuery.Parameters.Add("@MATRICULA", item.IdProgressaoSerie);
                ctxQuery.Parameters.Add("@PARTICIPAFASE1", item.ParticipaFase1);
                ctxQuery.Parameters.Add("@PARTICIPAFASE2", item.ParticipaFase2);

                ctx.ApplyModifications(ctxQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void Remove(int pId)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                var ctxQuery = new ContextQuery
                {
                    Command = @"
                                DELETE FROM progressaoserie 
                                WHERE  progressaoserieid = @PROGRESSAOSERIEID "
                };

                ctxQuery.Parameters.Add("@PROGRESSAOSERIEID", pId);

                ctx.ApplyModifications(ctxQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool VerificaProgressao(Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie objProgressaoSerie)
        {
            var ctxt = DataContextBuilder.FromLyceum.UsingNoLock();
            bool resultado = false;
            SqlDataReader reader = null;

            try
            {
                var sql = new ContextQuery
                {
                    Command = @"
                                SELECT P.progressaoserieid                      AS PROGRESSAOSERIEID, 
                                       CO.nome                                  AS NOMECURSO, 
                                       P.cursoid                                AS CURSOID, 
                                       CONVERT(VARCHAR, P.cursoid) + '   ' 
                                       + CONVERT(VARCHAR, CO.nome)        AS CURSO, 
                                       TC.tipo                                  AS TIPOCURSOID, 
                                       TC.descricao                             AS TIPOCURSO, 
                                       P.serieid                                AS SERIEID, 
                                       MO.modalidade                            AS CODMODALIDADE, 
                                       MO.descricao                             AS MODALIDADE, 
                                       CD.nome                                  AS PROXIMONOMECURSO, 
                                       P.proximocursoid                         AS PROXIMOCURSOID, 
                                       CONVERT(VARCHAR, P.proximocursoid) + '   ' 
                                       + CONVERT(VARCHAR, CD.nome) AS PROXIMOCURSO, 
                                       TD.tipo                                  AS PROXIMOTIPOCURSOID, 
                                       TD.descricao                             AS PROXIMOTIPOCURSO, 
                                       P.proximoserieid                         AS PROXIMOSERIEID, 
                                       MD.modalidade                            AS PROXIMOCODMODALIDADE, 
                                       MD.descricao                             AS PROXIMOMODALIDADE, 
                                       P.datacadastro                           AS DATACADASTRO, 
                                       P.matricula                              AS MATRICULA,
                                       PARTICIPAFASE1,
                                       PARTICIPAFASE2 
                                FROM   progressaoserie P 
                                       INNER JOIN ly_curso CO 
                                               ON ( P.cursoid = CO.curso ) 
                                       INNER JOIN ly_tipo_curso TC 
                                               ON ( CO.tipo = TC.tipo ) 
                                       INNER JOIN ly_curso CD 
                                               ON ( P.proximocursoid = CD.curso ) 
                                       INNER JOIN ly_tipo_curso TD 
                                               ON ( CO.tipo = TD.tipo ) 
                                       INNER JOIN ly_modalidade_curso MO 
                                               ON ( MO.modalidade = CO.modalidade ) 
                                       INNER JOIN ly_modalidade_curso MD 
                                               ON ( MD.modalidade = CO.modalidade ) 
                                WHERE  P.cursoid = @CURSO 
                                       AND P.serieid = @SERIE 
                                       AND P.proximocursoid = @PROXIMOCURSO 
                                       AND P.proximoserieid = @PROXIMASERIE 
                                "
                };

                sql.Parameters.Add("@CURSO", objProgressaoSerie.Curso);
                sql.Parameters.Add("@SERIE", objProgressaoSerie.Serie);
                sql.Parameters.Add("@PROXIMOCURSO", objProgressaoSerie.ProxCurso);
                sql.Parameters.Add("@PROXIMASERIE", objProgressaoSerie.ProxSerie);

                var item = new Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie();
                IList<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie> lista = new List<Techne.Lyceum.RN.Entidades.ProgressaoCursoSerie>();

                reader = ctxt.GetDataReader(sql);

                while (reader.Read())
                {
                    item.IdProgressaoSerie = !String.IsNullOrEmpty(reader["PROGRESSAOSERIEID"].ToString()) ? Convert.ToInt32(reader["PROGRESSAOSERIEID"]) : default(int);

                    lista.Add(item);
                }

                if (lista.Count > 0)
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                ctxt.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctxt.Dispose();
            }

        }
    }
}

