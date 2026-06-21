using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class Curso : RNBase
    {
        public const string QueryListaTipoCurso = "Select TIPO, DESCRICAO from LY_TIPO_CURSO";

        public const string QueryListaModalidadeCurso = "Select MODALIDADE, DESCRICAO from LY_MODALIDADE_CURSO";

        public static QueryTable Consultar()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT NOME, CURSO FROM LY_CURSO ORDER BY NOME";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarSUPED()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = @" SELECT  C.curso ,
                            C.nome ,
                            MC.DESCRICAO AS modalidade ,
                            TC.DESCRICAO AS nivel
                    FROM    LY_CURSO C
                            INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                            INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                            INNER JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                            INNER JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                    WHERE   PE.DESCRICAO = 'SUPED'
                    ORDER BY C.NOME  ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarSUPLAN()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = @" SELECT  C.curso ,
                            C.nome ,
                            MC.DESCRICAO AS modalidade ,
                            TC.DESCRICAO AS nivel
                    FROM    LY_CURSO C
                            INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                            INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                            INNER JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                            INNER JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                    WHERE   PE.DESCRICAO = 'SUPLAN'
                    ORDER BY C.NOME  ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarPorUnidadeEnsino(string unidadeEns)
        {
            if (string.IsNullOrEmpty(unidadeEns))
            {
                return null;
            }

            String sql = string.Format(@"
                SELECT DISTINCT c.nome, 
                                c.curso, 
                                tc.descricao DESCRICAO_NIVEL, 
                                mc.descricao DESCRICAO_MODALIDADE,
                                c.salaexterna
                FROM   ly_curso c 
                       INNER JOIN ly_unidade_ensino_cursos uec 
                               ON uec.curso = c.curso 
                                  AND unidade_ens = '{0}'
                       LEFT JOIN ly_tipo_curso tc 
                              ON tc.tipo = c.tipo 
                       LEFT JOIN ly_modalidade_curso mc 
                              ON mc.modalidade = c.modalidade 
                ORDER  BY c.nome ", unidadeEns);
            return RNBase.Consultar(sql);
        }

        public static QueryTable ConsultarCursosPorUnidadeEnsino(string unidadeEns)
        {
            String sql = @"
                select nome, curso, descricao_nivel, descricao_modalidade from 
                (SELECT distinct top 100 c.NOME, c.CURSO, tc.DESCRICAO DESCRICAO_NIVEL, mc.DESCRICAO DESCRICAO_MODALIDADE FROM LY_CURSO c
                inner join ly_unidade_ensino_cursos uec on uec.curso = c.curso and unidade_ens = ?
                left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO
                left join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE order by c.NOME) as tabela                
                UNION ALL
                SELECT 'Selecione' NOME, '' CURSO, 'Selecione' DESCRICAO_NIVEL, 'Selecione' DESCRICAO_MODALIDADE";
            return RNBase.Consultar(sql, unidadeEns);
        }

        public static string ObterTipoCurso(TConnection connection, string curso)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select TOP 1 modalidade from ly_curso WHERE curso = ? ", curso);

            if (!valorConsulta.IsNull)
            {
                return (string)valorConsulta;
            }

            return string.Empty;
        }

        public static string ObterModalidadeCurso(TConnection connection, string curso)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select detalhe from LY_TIPO_CURSO tipo_curso inner join LY_CURSO curso on curso.TIPO = tipo_curso.TIPO and curso.CURSO = ?", curso);
            //            DbObject valorConsultaOld = TCommand.ExecuteScalar(connection, "select top 1 MC.TIPO from LY_CURSO C INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE WHERE C.CURSO = ? ", curso);

            if (!valorConsulta.IsNull)
            {
                return (string)valorConsulta;
            }
            //          else if (!valorConsultaOld.IsNull)
            //            return (string)valorConsultaOld;

            return string.Empty;
        }

        public static QueryTable ConsultarDetalhesCurso()
        {
            return RNBase.Consultar(@"select c.curso, c.nome, mc.DESCRICAO as modalidade, tc.DESCRICAO as nivel from ly_curso c
                             inner join LY_MODALIDADE_CURSO mc on c.MODALIDADE = mc.MODALIDADE
                             inner join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO");
        }

        //Gera código de curso a partir do último do banco
        public static decimal GeraCurso()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            decimal curso;
            DbObject dbcurso;
            try
            {
                dbcurso = TCommand.ExecuteScalar(connection, "Select max(convert(dec, curso)) From Ly_curso", 1);
            }
            finally
            {
                connection.Close();
            }
            if (!dbcurso.IsNull)
            {
                curso = (decimal)dbcurso;
                return curso + 1;
            }
            return 1;
        }

        //Consulta Tipos de Curso
        public static QueryTable ConsultarTipoCurso()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = QueryListaTipoCurso;

            try
            {
                DbObject[] parametros = new DbObject[] { };

                qt = new QueryTable(sql);

                qt.Query(connection, parametros);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //Consulta Modalidade de Curso
        public static QueryTable ConsultarModalidadeCurso()
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = QueryListaModalidadeCurso;

            try
            {
                DbObject[] parametros = new DbObject[] { };

                qt = new QueryTable(sql);

                qt.Query(connection, parametros);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //Consulta Periodo por ano letivo
        public static DataTable ConsultaraUnidadeRecuEspecial()
        {
            var contextQuery = new ContextQuery(string.Format(@"SELECT DISTINCT E.UNIDADE_ENS, E.NOME_COMP
                                                                    FROM dbo.LY_UNIDADE_ENSINO E
                                                                    INNER JOIN dbo.LY_DEPENDENCIA D ON E.UNIDADE_ENS = D.FACULDADE
                                                              WHERE D.TIPO_DEPEND = 'SALAAEE'"));

            return Consultar(contextQuery);
        }

        //Consulta Departamentos de uma Unidade de Ensino
        public static QueryTable ConsultarDeptoUnidade(string unidade)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "Select DEPTO, NOME from LY_DEPTO where FACULDADE = ?";

            try
            {
                DbObject[] parametros = new DbObject[] { unidade };

                qt = new QueryTable(sql);

                qt.Query(connection, parametros);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //Consulta top 1 departamento
        public static string ConsultarTopDepto(string unidade)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, "Select top 1 DEPTO from LY_DEPTO where FACULDADE = ?", unidade);

                if (!valorConsulta.IsNull)
                {
                    return (string)valorConsulta;
                }

                return string.Empty;
            }
            finally
            {
                connection.Close();
            }
        }

        public string ObtemTituloPor(string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT TITULO
                        FROM LY_CURSO (NOLOCK)
                        WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.Variant, curso);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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
                contexto.Dispose();
            }
        }

        public DTOs.DadosCurso ObtemDadosCursoPor(string curso)
        {
            DTOs.DadosCurso dadosCurso = new Techne.Lyceum.RN.DTOs.DadosCurso();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                //Busca dados da tabela curso
                dadosCurso = this.ObtemDadosCursoPor(contexto, curso);

                //Busca lista de UnidadesCurricular
                dadosCurso.UnidadesCurricular = this.ObtemUnidadesCurricularesPor(contexto, curso);

                //Busca lista de AreaItinerarioFormativo
                dadosCurso.AreaItinerarioFormativo = this.ObtemAreaItinerarioFormativoPor(contexto, curso);

                //Busca lista de ComposicaoItinerarioFormativoIntegrado
                dadosCurso.ComposicaoItinerarioFormativoIntegrado = this.ObtemComposicaoItinerarioFormativoPor(contexto, curso, 6);

                //Busca lista de TipoItinerarioFormacaoTecnicaProfissional
                dadosCurso.TipoItinerarioFormacaoTecnicaProfissional = this.ObtemComposicaoItinerarioFormativoPor(contexto, curso, 5);

                return dadosCurso;
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
                contexto.Dispose();
            }
        }

        private DTOs.DadosCurso ObtemDadosCursoPor(DataContext contexto, string curso)
        {
            DTOs.DadosCurso dadosCurso = new Techne.Lyceum.RN.DTOs.DadosCurso();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT CURSO,
                                                FACULDADE, 
                                                DEPTO,   
                                                MNEMONICO,
                                                NOME,
                                                TITULO,
                                                HABILITACAO,
                                                DECRETO,
                                                VAGAS,
                                                DT_DOU,
                                                C.TIPO,
                                                MODALIDADE,
                                                TIPO_CURSO,
                                                C.ATIVO,
                                                TEM_RECLASSIFICACAO,
                                                FORMATURA,
                                                CONCOMITANTE,
                                                PARTICIPACALCULONOVASTURMASTURNOSVAGAS,
                                                PARTICIPAFECHAMENTOAUTOMATICO,
                                                PERMITETRANSFERENCIATURMATOTAL,
                                                PERMITECHOQUETURNOINTEGRALTURNOSVAGAS,
                                                SALAEXTERNA,
                                                OFERTAELETIVA,
                                                FORMACAOBASICA,
                                                ITINERARIOFORMATIVO,
                                                C.TRILHAAPRENDIZAGEMID,
                                                T.ITINERARIOFORMATIVOID,
                                                MAXIMOCOMPONENTES 
                                        FROM LY_CURSO C
                                        LEFT JOIN PEDAGOGICO.[TRILHAAPRENDIZAGEM] T ON C.TRILHAAPRENDIZAGEMID = T.TRILHAAPRENDIZAGEMID
                                        WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosCurso.Curso = Convert.ToString(reader["CURSO"]);
                    dadosCurso.Faculdade = Convert.ToString(reader["FACULDADE"]);
                    dadosCurso.Depto = Convert.ToString(reader["DEPTO"]);
                    dadosCurso.Mnemonico = Convert.ToString(reader["MNEMONICO"]);
                    dadosCurso.Nome = Convert.ToString(reader["NOME"]);
                    dadosCurso.Titulo = Convert.ToString(reader["TITULO"]);
                    dadosCurso.Habilitacao = Convert.ToString(reader["HABILITACAO"]);
                    dadosCurso.Decreto = Convert.ToString(reader["DECRETO"]);

                    if (reader["VAGAS"] != DBNull.Value)
                    {
                        dadosCurso.Vagas = Convert.ToInt32(reader["VAGAS"]);
                    }

                    if (reader["DT_DOU"] != DBNull.Value)
                    {
                        dadosCurso.Dt_dou = Convert.ToDateTime(reader["DT_DOU"]);
                    }

                    dadosCurso.Tipo = Convert.ToString(reader["TIPO"]);
                    dadosCurso.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                    dadosCurso.Tipo_curso = Convert.ToString(reader["TIPO_CURSO"]);
                    dadosCurso.Ativo = Convert.ToString(reader["ATIVO"]);
                    dadosCurso.Tem_reclassificacao = Convert.ToString(reader["TEM_RECLASSIFICACAO"]);
                    dadosCurso.Formatura = Convert.ToString(reader["FORMATURA"]);
                    dadosCurso.Concomitante = Convert.ToString(reader["CONCOMITANTE"]);
                    dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas = Convert.ToString(reader["PARTICIPACALCULONOVASTURMASTURNOSVAGAS"]);
                    dadosCurso.ParticipaFechamentoAutomatico = Convert.ToString(reader["PARTICIPAFECHAMENTOAUTOMATICO"]);
                    dadosCurso.PermiteTransferenciaTurmaTotal = Convert.ToString(reader["PERMITETRANSFERENCIATURMATOTAL"]);
                    dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas = Convert.ToString(reader["PERMITECHOQUETURNOINTEGRALTURNOSVAGAS"]);
                    dadosCurso.Salaexterna = Convert.ToString(reader["SALAEXTERNA"]);
                    dadosCurso.OfertaEletiva = Convert.ToString(reader["OFERTAELETIVA"]);
                    dadosCurso.FormacaoBasica = Convert.ToString(reader["FORMACAOBASICA"]);
                    dadosCurso.ItinerarioFormativo = Convert.ToString(reader["ITINERARIOFORMATIVO"]);

                    //Marcar opção Não se aplica apenas quando a FormacaoBasica e ItinerarioFormativo já tenham sido salvos como N
                    if (dadosCurso.FormacaoBasica == "N" && dadosCurso.ItinerarioFormativo == "N")
                    {
                        dadosCurso.NaoSeAplica = "S";
                    }

                    if (reader["TRILHAAPRENDIZAGEMID"] != DBNull.Value)
                    {
                        dadosCurso.TrilhaAprendizagem = Convert.ToInt32(reader["TRILHAAPRENDIZAGEMID"]);
                    }

                    if (reader["MAXIMOCOMPONENTES"] != DBNull.Value)
                    {
                        dadosCurso.MaximoComponentes = Convert.ToInt32(reader["MAXIMOCOMPONENTES"]);
                    }

                    if (reader["ITINERARIOFORMATIVOID"] != DBNull.Value)
                    {
                        dadosCurso.ItinerarioFormativoId = Convert.ToInt32(reader["ITINERARIOFORMATIVOID"]);
                    }
                }

                return dadosCurso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private List<int> ObtemUnidadesCurricularesPor(DataContext contexto, string curso)
        {
            List<int> lista = new List<int>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT UNIDADECURRICULARID
                                        FROM Pedagogico.CURSO__UNIDADECURRICULAR
                                        WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    int unidadeCurricularId = Convert.ToInt32(reader["UNIDADECURRICULARID"]);

                    lista.Add(unidadeCurricularId);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private List<int> ObtemAreaItinerarioFormativoPor(DataContext contexto, string curso)
        {
            List<int> lista = new List<int>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT AREAITINERARIOFORMATIVOID
                                        FROM Pedagogico.CURSO__AREAITINERARIOFORMATIVO
                                        WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    int unidadeCurricularId = Convert.ToInt32(reader["AREAITINERARIOFORMATIVOID"]);

                    lista.Add(unidadeCurricularId);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int ObtemTrilhaAprendizagemIdPor(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT TRILHAAPRENDIZAGEMID
                                    FROM LY_CURSO
                                    WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["TRILHAAPRENDIZAGEMID"] != DBNull.Value ? Convert.ToInt32(reader["TRILHAAPRENDIZAGEMID"]) : 0;
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

        public int? ObtemMaximoComponentesPor(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int? retorno = null;
            try
            {
                contextQuery.Command = @" SELECT MAXIMOCOMPONENTES
		                                FROM LY_CURSO
		                                WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["MAXIMOCOMPONENTES"] != DBNull.Value)
                    {
                        retorno = Convert.ToInt32(reader["MAXIMOCOMPONENTES"]);
                    }
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

        public bool EhItinerarioFormativoTrihaPor(string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM LY_CURSO
                                        WHERE ITINERARIOFORMATIVO = 'S'
	                                        and CURSO = @CURSO
	                                        AND TRILHAAPRENDIZAGEMID IS NOT NULL
	                                        AND TRILHAAPRENDIZAGEMID <> 31 --Tipo interno ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
                contexto.Dispose();
            }
        }

        private List<int> ObtemComposicaoItinerarioFormativoPor(DataContext contexto, string curso, int areaItinerarioFormativoId)
        {
            List<int> lista = new List<int>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT CC.COMPOSICAOITINERARIOFORMATIVOID
                                        FROM PEDAGOGICO.CURSO__COMPOSICAOITINERARIOFORMATIVO CC
											INNER JOIN PEDAGOGICO.COMPOSICAOITINERARIOFORMATIVO C 
													ON CC.COMPOSICAOITINERARIOFORMATIVOID = C.COMPOSICAOITINERARIOFORMATIVOID
                                        WHERE CURSO = @CURSO
											AND AREAITINERARIOFORMATIVOID = @AREAITINERARIOFORMATIVOID ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioFormativoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    int unidadeCurricularId = Convert.ToInt32(reader["COMPOSICAOITINERARIOFORMATIVOID"]);

                    lista.Add(unidadeCurricularId);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados Valida(DTOs.DadosCurso dadosCurso, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();
            RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosCurso == null)
            {
                return validacaoDados;
            }

            if (dadosCurso.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (dadosCurso.Mnemonico.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SIGLA é obrigatório.");
            }
            else if (dadosCurso.Mnemonico.Length > 6)
            {
                mensagens.Add("Campo SIGLA deve ter no máximo 6 caracteres.");
            }

            if (dadosCurso.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else if (dadosCurso.Nome.ToString().Length > 100)
            {
                mensagens.Add("Campo NOME deve ter no máximo 100 caracteres.");
            }

            if (dadosCurso.Modalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MODALIDADE é obrigatório.");
            }

            if (dadosCurso.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÍVEL/SEGMENTO é obrigatório.");
            }

            if (dadosCurso.Titulo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CERTIFICAÇÃO é obrigatório.");
            }
            else
            {
                if (dadosCurso.Titulo.ToString().Length > 200)
                {
                    mensagens.Add("Campo CERTIFICAÇÃO deve ter no máximo 200 caracteres.");
                }

                if (!Validacao.Validou(dadosCurso.Titulo, Validacao.Tipo.texto))
                {
                    mensagens.Add("CERTIFICAÇÃO inválida.<br>O título deve ter apenas letras ou números.");
                }
            }

            if (dadosCurso.Ativo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ATIVO é obrigatório.");
            }
            else if (dadosCurso.Ativo != "S" && dadosCurso.Ativo != "N")
            {
                mensagens.Add("Campo ATIVO deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.OfertaEletiva.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo OFERTA ELETIVA é obrigatório.");
            }
            else if (dadosCurso.OfertaEletiva != "S" && dadosCurso.OfertaEletiva != "N")
            {
                mensagens.Add("Campo OFERTA ELETIVA deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.Tem_reclassificacao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TEM RECLASSIFICAÇÃO é obrigatório.");
            }
            else if (dadosCurso.Tem_reclassificacao != "S" && dadosCurso.Tem_reclassificacao != "N")
            {
                mensagens.Add("Campo TEM RECLASSIFICAÇÃO deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.Formatura.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONCLUINTES ANTERIORES é obrigatório.");
            }
            else if (dadosCurso.Formatura != "S" && dadosCurso.Formatura != "N")
            {
                mensagens.Add("Campo CONCLUINTES ANTERIORES deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.Concomitante.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo EDUCAÇÃO PROFISSIONAL CONCOMITANTE é obrigatório.");
            }
            else if (dadosCurso.Concomitante != "S" && dadosCurso.Concomitante != "N")
            {
                mensagens.Add("Campo EDUCAÇÃO PROFISSIONAL CONCOMITANTE deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.Salaexterna.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SALA EXTERNA é obrigatório.");
            }
            else if (dadosCurso.Salaexterna != "S" && dadosCurso.Salaexterna != "N")
            {
                mensagens.Add("Campo SALA EXTERNA deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PARTICIPA DO CÁLCULO DE PERCENTUAL DA CRIAÇÃO DE TURMAS NOVAS é obrigatório.");
            }
            else if (dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas != "S" && dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas != "N")
            {
                mensagens.Add("Campo PARTICIPA DO CÁLCULO DE PERCENTUAL DA CRIAÇÃO DE TURMAS NOVAS deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PERMITE CHOQUE DE TURNO INTEGRAL é obrigatório.");
            }
            else if (dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas != "S" && dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas != "N")
            {
                mensagens.Add("Campo PERMITE CHOQUE DE TURNO INTEGRAL deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.ParticipaFechamentoAutomatico.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PARTICIPA FECHAMENTO AUTOMÁTICO ANO LETIVO é obrigatório.");
            }
            else if (dadosCurso.ParticipaFechamentoAutomatico != "S" && dadosCurso.ParticipaFechamentoAutomatico != "N")
            {
                mensagens.Add("Campo PARTICIPA FECHAMENTO AUTOMÁTICO ANO LETIVO deve ser 'S' ou 'N'.");
            }

            if (dadosCurso.PermiteTransferenciaTurmaTotal.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PERMITE TRANSFERÊNCIA IRRESTRITA é obrigatório.");
            }
            else if (dadosCurso.PermiteTransferenciaTurmaTotal != "S" && dadosCurso.PermiteTransferenciaTurmaTotal != "N")
            {
                mensagens.Add("Campo PERMITE TRANSFERÊNCIA IRRESTRITA deve ser 'S' ou 'N'.");
            }

            if (!dadosCurso.Habilitacao.IsNullOrEmptyOrWhiteSpace() && dadosCurso.Habilitacao.ToString().Length > 255)
            {
                mensagens.Add("Campo HABILITAÇÃO deve ter no máximo 255 caracteres.");
            }

            if (!dadosCurso.Decreto.IsNullOrEmptyOrWhiteSpace() && dadosCurso.Decreto.ToString().Length > 255)
            {
                mensagens.Add("Campo DECRETO deve ter no máximo 255 caracteres.");
            }

            if (dadosCurso.Dt_dou != null && !Validacao.ValidouDataPodeHoje(dadosCurso.Dt_dou, Validacao.Tipo.data))
            {
                mensagens.Add("DATA D.O. inválida.<br>A DATA D.O. deve ser maior que 1900 e não pode ser maior que a data de hoje.");
            }

            if (dadosCurso.Ativo == "S")
            {
                if (dadosCurso.Vagas == null)
                {
                    mensagens.Add("CAPACIDADE DE ATENDIMENTO não pode ser nula caso o curso esteja ATIVO.");
                }
                else if (dadosCurso.Vagas <= 0)
                {
                    mensagens.Add("CAPACIDADE DE ATENDIMENTO deve ser maior que zero.");
                }
                else if (dadosCurso.Vagas.ToString().Length > 10)
                {
                    mensagens.Add("CAPACIDADE DE ATENDIMENTO deve ter no máximo 10 dígitos.");
                }
            }

            if (dadosCurso.FormacaoBasica.IsNullOrEmptyOrWhiteSpace()
                || dadosCurso.ItinerarioFormativo.IsNullOrEmptyOrWhiteSpace()
                || dadosCurso.NaoSeAplica.IsNullOrEmptyOrWhiteSpace()
                || (dadosCurso.FormacaoBasica != "S" && dadosCurso.ItinerarioFormativo != "S" && dadosCurso.NaoSeAplica != "S"))
            {
                mensagens.Add("Campo ESTRUTURA CURRICULAR é obrigatório marcar ao menos uma opção.");
            }
            else
            {
                if (dadosCurso.NaoSeAplica == "S" && (dadosCurso.FormacaoBasica == "S" || dadosCurso.ItinerarioFormativo == "S"))
                {
                    mensagens.Add("Na ESTRUTURA CURRICULAR a opção Não se aplica apenas pode ser marcada caso seja única.");
                }

                //Verifica se o curso é do ITINERÁRIO FORMATIVO
                if (dadosCurso.ItinerarioFormativo == "S")
                {
                    if (dadosCurso.TrilhaAprendizagem == null || dadosCurso.TrilhaAprendizagem < 1)
                    {
                        mensagens.Add("Caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado deve ser informado o ITINERÁRIO FORMATIVO e sua TRILHA DE APRENDIZAGEM.");
                    }

                    if (dadosCurso.MaximoComponentes == null || dadosCurso.MaximoComponentes < 1)
                    {
                        mensagens.Add("Caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado deve ser informado o número máximo de componentes curriculares por série.");
                    }

                    if (dadosCurso.UnidadesCurricular == null || dadosCurso.UnidadesCurricular.Count < 1)
                    {
                        mensagens.Add("Caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado deve ser informada ao menos uma opção de UNIDADE CURRICULAR.");
                    }

                    if (dadosCurso.AreaItinerarioFormativo == null ||
                        dadosCurso.AreaItinerarioFormativo.Count < 1 || dadosCurso.AreaItinerarioFormativo.Count > 4)
                    {
                        mensagens.Add("Caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado deve ser informada de 1 a 4 opções de ÁREA DO ITINERÁRIO FORMATIVO.");
                    }
                    else
                    {
                        //Verifica se foi marcado ITINERÁRIO FORMATIVO INTEGRADO (codigo 6) na área do itinerario formativo 
                        if (dadosCurso.AreaItinerarioFormativo.Contains(6))
                        {
                            if (dadosCurso.ComposicaoItinerarioFormativoIntegrado == null || dadosCurso.ComposicaoItinerarioFormativoIntegrado.Count < 2 || dadosCurso.ComposicaoItinerarioFormativoIntegrado.Count > 4)
                            {
                                mensagens.Add("Caso na área do itinerário formativo o ITINERÁRIO FORMATIVO INTEGRADO esteja marcado deve ser informada de 2 a 4 opções de COMPOSIÇÃO DO ITINERÁRIO FORMATIVO INTEGRADO.");
                            }

                        }
                        else if (dadosCurso.ComposicaoItinerarioFormativoIntegrado != null && dadosCurso.ComposicaoItinerarioFormativoIntegrado.Count > 0)
                        {
                            mensagens.Add("Apenas pode ser informada COMPOSIÇÃO DO ITINERÁRIO FORMATIVO INTEGRADO caso na área do itinerário formativo o ITINERÁRIO FORMATIVO INTEGRADO esteja marcado.");
                        }

                        //Verifica se foi marcado FORMAÇÃO TECNICA E PROFISSIONAL (codigo 5) na área do itinerario formativo 
                        if (dadosCurso.AreaItinerarioFormativo.Contains(5))
                        {
                            if (dadosCurso.TipoItinerarioFormacaoTecnicaProfissional == null || dadosCurso.TipoItinerarioFormacaoTecnicaProfissional.Count != 1)
                            {
                                mensagens.Add("Caso na área do itinerário formativo o FORMAÇÃO TÉCNICA E PROFISSIONAL esteja marcado deve ser informada uma opção de TIPO DO CURSO DO ITINERÁRIO DE FORMAÇÃO TÉCNICA E PROFISSIONAL.");
                            }
                        }
                        else if (dadosCurso.TipoItinerarioFormacaoTecnicaProfissional != null && dadosCurso.TipoItinerarioFormacaoTecnicaProfissional.Count > 0)
                        {
                            mensagens.Add("Apenas pode ser informada TIPO DO CURSO DO ITINERÁRIO DE FORMAÇÃO TÉCNICA E PROFISSIONAL caso na área do itinerário formativo o FORMAÇÃO TÉCNICA E PROFISSIONAL esteja marcado.");
                        }
                    }
                }
                else
                {
                    //Caso o curso não seja do ITINERÁRIO FORMATIVO, nenhuma opção pode ser incluida

                    if (dadosCurso.TrilhaAprendizagem != null)
                    {
                        mensagens.Add("Apenas pode ser informada ITINERÁRIO FORMATIVO e sua TRILHA DE APRENDIZAGEM caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado.");
                    }

                    if (dadosCurso.MaximoComponentes != null)
                    {
                        mensagens.Add("Apenas pode ser informada número máximo de componentes curriculares por série caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado.");
                    }

                    if (dadosCurso.UnidadesCurricular != null && dadosCurso.UnidadesCurricular.Count > 0)
                    {
                        mensagens.Add("Apenas pode ser informada UNIDADE CURRICULAR caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado.");
                    }

                    if (dadosCurso.AreaItinerarioFormativo != null && dadosCurso.AreaItinerarioFormativo.Count > 0)
                    {
                        mensagens.Add("Apenas pode ser informada ÁREA DO ITINERÁRIO FORMATIVO caso na estrutura curricular o ITINERÁRIO FORMATIVO esteja marcado.");
                    }

                    if (dadosCurso.ComposicaoItinerarioFormativoIntegrado != null && dadosCurso.ComposicaoItinerarioFormativoIntegrado.Count > 0)
                    {
                        mensagens.Add("Apenas pode ser informada COMPOSIÇÃO DO ITINERÁRIO FORMATIVO INTEGRADO caso na área do itinerário formativo o ITINERÁRIO FORMATIVO INTEGRADO esteja marcado.");
                    }

                    if (dadosCurso.TipoItinerarioFormacaoTecnicaProfissional != null && dadosCurso.TipoItinerarioFormacaoTecnicaProfissional.Count > 0)
                    {
                        mensagens.Add("Apenas pode ser informada TIPO DO CURSO DO ITINERÁRIO DE FORMAÇÃO TÉCNICA E PROFISSIONAL caso na área do itinerário formativo o FORMAÇÃO TÉCNICA E PROFISSIONAL esteja marcado.");
                    }
                }
            }

            if (dadosCurso.Faculdade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Escolaridade não pode ser cadastrada sem unidade de ensino.");
            }

            if (dadosCurso.Depto.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Escolaridade não pode ser cadastrada pois a unidade de ensino de código 99999999 não possui departamento.");
            }

            if (dadosCurso.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (dadosCurso.ItinerarioFormativoId != null)
            {
                if (!rnItinerarioFormativo.EhItinerarioAtivoPor(dadosCurso.ItinerarioFormativoId.Value))
                {
                    mensagens.Add("O itinerário formativo escolhido não consta mais ativo.");
                }
            }

            if (dadosCurso.TrilhaAprendizagem != null)
            {
                if (!rnTrilhaAprendizagem.EhTrilhaAtivaPor(dadosCurso.TrilhaAprendizagem.Value))
                {
                    mensagens.Add("A trilha escolhida não consta mais ativa.");
                }
            }

            //if (hdnSalaExterna.Value == "S" && !chkSalaExterna.Checked)
            //{
            //    if (rnTurma.ConsultaTurmaAtivaPor(txtCurso.Text))
            //    {
            //        lblMensagem.Text = "O campo Sala Externa não pode ser desmarcado devido ter turmas ativas com ambiente externo.";
            //        return;
            //    }
            //}

            if (mensagens.Count == 0)
            {
                if (cadastro)
                {
                    //Verifica se o codigo já existe
                    if (this.PossuiCursoPor(dadosCurso.Curso))
                    {
                        mensagens.Add("Este CÓDIGO já foi utilizado para outro curso.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(DTOs.DadosCurso dadosCurso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Inclui Curso
                this.Insere(contexto, dadosCurso);

                if (dadosCurso.UnidadesCurricular != null)
                {
                    foreach (int unidadeCurricularId in dadosCurso.UnidadesCurricular.Distinct())
                    {
                        //Insere opções de Unidades Curricular
                        this.InsereUnidadeCurricular(contexto, dadosCurso.Curso, unidadeCurricularId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.AreaItinerarioFormativo != null)
                {
                    foreach (int areaItinerarioFormativoId in dadosCurso.AreaItinerarioFormativo)
                    {
                        //Insere opções de AreaItinerarioFormativo
                        this.InsereAreaItinerarioFormativo(contexto, dadosCurso.Curso, areaItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.ComposicaoItinerarioFormativoIntegrado != null)
                {
                    foreach (int composicaoItinerarioFormativoId in dadosCurso.ComposicaoItinerarioFormativoIntegrado)
                    {
                        //Insere opções de Composicao Itinerario Formativo para Area Itinerario Formativo 6 - ITINERÁRIO FORMATIVO INTEGRADO
                        this.InsereComposicaoItinerarioFormativo(contexto, dadosCurso.Curso, composicaoItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.TipoItinerarioFormacaoTecnicaProfissional != null)
                {
                    foreach (int tipoItinerarioFormativoId in dadosCurso.TipoItinerarioFormacaoTecnicaProfissional)
                    {
                        //Insere opções de Composicao Itinerario Formativo para Area Itinerario Formativo 5 - FORMAÇÃO TECNICA E PROFISSIONAL
                        this.InsereComposicaoItinerarioFormativo(contexto, dadosCurso.Curso, tipoItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }
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
                contexto.Dispose();
            }
        }

        private void Insere(DataContext contexto, DTOs.DadosCurso dadosCurso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_CURSO
		                                (CURSO, 
		                                FACULDADE, 
		                                DEPTO, 
		                                MNEMONICO, 
		                                NOME, 
		                                TITULO, 
		                                HABILITACAO, 
		                                DECRETO, 
		                                VAGAS,
		                                DT_DOU, 
		                                TIPO, 	
		                                MODALIDADE, 
		                                TIPO_CURSO,
		                                ATIVO, 
		                                TEM_RECLASSIFICACAO, 
		                                FORMATURA, 
		                                CONCOMITANTE, 
		                                PARTICIPACALCULONOVASTURMASTURNOSVAGAS,
		                                PARTICIPAFECHAMENTOAUTOMATICO, 
		                                PERMITETRANSFERENCIATURMATOTAL, 
		                                PERMITECHOQUETURNOINTEGRALTURNOSVAGAS,
		                                SALAEXTERNA, 
		                                OFERTAELETIVA, 
		                                FORMACAOBASICA, 
		                                ITINERARIOFORMATIVO,
		                                TRILHAAPRENDIZAGEMID,
		                                MAXIMOCOMPONENTES,
                                        USUARIOID, 
                                        DATACADASTRO, 
                                        DATAALTERACAO)
	                                VALUES
		                                (@CURSO, 
		                                @FACULDADE, 
		                                @DEPTO, 
		                                @MNEMONICO, 
		                                @NOME, 
		                                @TITULO, 
		                                @HABILITACAO, 
		                                @DECRETO, 
		                                @VAGAS,
		                                @DT_DOU, 
		                                @TIPO, 	
		                                @MODALIDADE, 
		                                @TIPO_CURSO,
		                                @ATIVO, 
		                                @TEM_RECLASSIFICACAO, 
		                                @FORMATURA, 
		                                @CONCOMITANTE, 
		                                @PARTICIPACALCULONOVASTURMASTURNOSVAGAS,
		                                @PARTICIPAFECHAMENTOAUTOMATICO, 
		                                @PERMITETRANSFERENCIATURMATOTAL, 
		                                @PERMITECHOQUETURNOINTEGRALTURNOSVAGAS,
		                                @SALAEXTERNA, 
		                                @OFERTAELETIVA, 
		                                @FORMACAOBASICA, 
		                                @ITINERARIOFORMATIVO,
		                                @TRILHAAPRENDIZAGEMID,
		                                @MAXIMOCOMPONENTES,
                                        @USUARIOID, 
                                        @DATACADASTRO, 
                                        @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@CURSO", dadosCurso.Curso);
            contextQuery.Parameters.Add("@FACULDADE", dadosCurso.Faculdade);
            contextQuery.Parameters.Add("@DEPTO", dadosCurso.Depto);
            contextQuery.Parameters.Add("@MNEMONICO", dadosCurso.Mnemonico);
            contextQuery.Parameters.Add("@NOME", dadosCurso.Nome);
            contextQuery.Parameters.Add("@TITULO", dadosCurso.Titulo);
            contextQuery.Parameters.Add("@HABILITACAO", dadosCurso.Habilitacao);
            contextQuery.Parameters.Add("@DECRETO", dadosCurso.Decreto);
            contextQuery.Parameters.Add("@VAGAS", dadosCurso.Vagas == null ? DBNull.Value : (object)dadosCurso.Vagas);
            contextQuery.Parameters.Add("@DT_DOU", dadosCurso.Dt_dou == null ? DBNull.Value : (object)dadosCurso.Dt_dou);
            contextQuery.Parameters.Add("@TIPO", dadosCurso.Tipo);
            contextQuery.Parameters.Add("@MODALIDADE", dadosCurso.Modalidade);
            contextQuery.Parameters.Add("@TIPO_CURSO", dadosCurso.Tipo_curso);
            contextQuery.Parameters.Add("@ATIVO", dadosCurso.Ativo);
            contextQuery.Parameters.Add("@TEM_RECLASSIFICACAO", dadosCurso.Tem_reclassificacao);
            contextQuery.Parameters.Add("@FORMATURA", dadosCurso.Formatura);
            contextQuery.Parameters.Add("@CONCOMITANTE", dadosCurso.Concomitante);
            contextQuery.Parameters.Add("@PARTICIPACALCULONOVASTURMASTURNOSVAGAS", dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas);
            contextQuery.Parameters.Add("@PARTICIPAFECHAMENTOAUTOMATICO", dadosCurso.ParticipaFechamentoAutomatico);
            contextQuery.Parameters.Add("@PERMITETRANSFERENCIATURMATOTAL", dadosCurso.PermiteTransferenciaTurmaTotal);
            contextQuery.Parameters.Add("@PERMITECHOQUETURNOINTEGRALTURNOSVAGAS", dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas);
            contextQuery.Parameters.Add("@SALAEXTERNA", dadosCurso.Salaexterna);
            contextQuery.Parameters.Add("@OFERTAELETIVA", dadosCurso.OfertaEletiva);
            contextQuery.Parameters.Add("@FORMACAOBASICA", dadosCurso.FormacaoBasica);
            contextQuery.Parameters.Add("@ITINERARIOFORMATIVO", dadosCurso.ItinerarioFormativo);
            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", dadosCurso.TrilhaAprendizagem == null ? DBNull.Value : (object)dadosCurso.TrilhaAprendizagem);
            contextQuery.Parameters.Add("@MAXIMOCOMPONENTES", dadosCurso.MaximoComponentes == null ? DBNull.Value : (object)dadosCurso.MaximoComponentes);
            contextQuery.Parameters.Add("@USUARIOID", dadosCurso.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereUnidadeCurricular(DataContext contexto, string curso, int unidadeCurricularId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Pedagogico.CURSO__UNIDADECURRICULAR
                                           (CURSO
                                           ,UNIDADECURRICULARID
                                           ,USUARIOID
                                           ,DATACADASTRO)
                                     VALUES
                                           (@CURSO, 
                                           @UNIDADECURRICULARID, 
                                           @USUARIOID, 
                                           @DATACADASTRO) ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@UNIDADECURRICULARID", SqlDbType.Int, unidadeCurricularId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereAreaItinerarioFormativo(DataContext contexto, string curso, int areaItinerarioFormativoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Pedagogico.CURSO__AREAITINERARIOFORMATIVO
                                           (CURSO
                                           ,AREAITINERARIOFORMATIVOID
                                           ,USUARIOID
                                           ,DATACADASTRO)
                                     VALUES
                                           (@CURSO, 
                                           @AREAITINERARIOFORMATIVOID, 
                                           @USUARIOID, 
                                           @DATACADASTRO) ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioFormativoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereComposicaoItinerarioFormativo(DataContext contexto, string curso, int composicaoItinerarioFormativoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Pedagogico.CURSO__COMPOSICAOITINERARIOFORMATIVO
                                           (CURSO
                                           ,COMPOSICAOITINERARIOFORMATIVOID
                                           ,USUARIOID
                                           ,DATACADASTRO)
                                     VALUES
                                           (@CURSO, 
                                           @COMPOSICAOITINERARIOFORMATIVOID,
                                           @USUARIOID, 
                                           @DATACADASTRO) ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@COMPOSICAOITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerarioFormativoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DTOs.DadosCurso dadosCurso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza Curso
                this.Atualiza(contexto, dadosCurso);

                //Deleta as opções de Unidades Curricular anteriores
                this.RemoveUnidadeCurricular(contexto, dadosCurso.Curso);

                //Deleta as opções de Area Itinerario Formativo anteriores
                this.RemoveAreaItinerarioFormativo(contexto, dadosCurso.Curso);

                //Deleta as opções de Composicao Itinerario Formativo anteriores
                this.RemoveComposicaoItinerarioFormativo(contexto, dadosCurso.Curso);

                if (dadosCurso.UnidadesCurricular != null)
                {
                    foreach (int unidadeCurricularId in dadosCurso.UnidadesCurricular.Distinct())
                    {
                        //Insere opções de Unidades Curricular
                        this.InsereUnidadeCurricular(contexto, dadosCurso.Curso, unidadeCurricularId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.AreaItinerarioFormativo != null)
                {
                    foreach (int areaItinerarioFormativoId in dadosCurso.AreaItinerarioFormativo)
                    {
                        //Insere opções de Area Itinerario Formativo
                        this.InsereAreaItinerarioFormativo(contexto, dadosCurso.Curso, areaItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.ComposicaoItinerarioFormativoIntegrado != null)
                {
                    foreach (int composicaoItinerarioFormativoId in dadosCurso.ComposicaoItinerarioFormativoIntegrado)
                    {
                        //Insere opções de Composicao Itinerario Formativo para Area Itinerario Formativo 6 - ITINERÁRIO FORMATIVO INTEGRADO
                        this.InsereComposicaoItinerarioFormativo(contexto, dadosCurso.Curso, composicaoItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }

                if (dadosCurso.TipoItinerarioFormacaoTecnicaProfissional != null)
                {
                    foreach (int tipoItinerarioFormativoId in dadosCurso.TipoItinerarioFormacaoTecnicaProfissional)
                    {
                        //Insere opções de Composicao Itinerario Formativo para Area Itinerario Formativo 5 - FORMAÇÃO TECNICA E PROFISSIONAL
                        this.InsereComposicaoItinerarioFormativo(contexto, dadosCurso.Curso, tipoItinerarioFormativoId, dadosCurso.UsuarioId);
                    }
                }
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
                contexto.Dispose();
            }
        }

        private void Atualiza(DataContext contexto, DTOs.DadosCurso dadosCurso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_CURSO
	                                SET MNEMONICO = @MNEMONICO, 
		                                NOME = @NOME, 
		                                TITULO = @TITULO, 
		                                HABILITACAO = @HABILITACAO, 
		                                DECRETO = @DECRETO,
		                                VAGAS = @VAGAS,
		                                DT_DOU = @DT_DOU, 
		                                TIPO = @TIPO, 	
		                                MODALIDADE = @MODALIDADE, 
		                                TIPO_CURSO = @TIPO_CURSO,
		                                ATIVO = @ATIVO, 
		                                TEM_RECLASSIFICACAO = @TEM_RECLASSIFICACAO,
		                                FORMATURA = @FORMATURA, 
		                                CONCOMITANTE = @CONCOMITANTE, 
		                                PARTICIPACALCULONOVASTURMASTURNOSVAGAS = @PARTICIPACALCULONOVASTURMASTURNOSVAGAS,
		                                PARTICIPAFECHAMENTOAUTOMATICO = @PARTICIPAFECHAMENTOAUTOMATICO, 
		                                PERMITETRANSFERENCIATURMATOTAL = @PERMITETRANSFERENCIATURMATOTAL, 
		                                PERMITECHOQUETURNOINTEGRALTURNOSVAGAS = @PERMITECHOQUETURNOINTEGRALTURNOSVAGAS,
		                                SALAEXTERNA = @SALAEXTERNA, 
		                                OFERTAELETIVA = @OFERTAELETIVA, 
		                                FORMACAOBASICA = @FORMACAOBASICA, 
		                                ITINERARIOFORMATIVO = @ITINERARIOFORMATIVO,
		                                TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID,
		                                MAXIMOCOMPONENTES = @MAXIMOCOMPONENTES,
		                                USUARIOID = @USUARIOID, 
		                                DATAALTERACAO = @DATAALTERACAO
	                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", dadosCurso.Curso);
            contextQuery.Parameters.Add("@MNEMONICO", dadosCurso.Mnemonico);
            contextQuery.Parameters.Add("@NOME", dadosCurso.Nome);
            contextQuery.Parameters.Add("@TITULO", dadosCurso.Titulo);
            contextQuery.Parameters.Add("@HABILITACAO", dadosCurso.Habilitacao);
            contextQuery.Parameters.Add("@DECRETO", dadosCurso.Decreto);
            contextQuery.Parameters.Add("@VAGAS", dadosCurso.Vagas == null ? DBNull.Value : (object)dadosCurso.Vagas);
            contextQuery.Parameters.Add("@DT_DOU", dadosCurso.Dt_dou == null ? DBNull.Value : (object)dadosCurso.Dt_dou);
            contextQuery.Parameters.Add("@TIPO", dadosCurso.Tipo);
            contextQuery.Parameters.Add("@MODALIDADE", dadosCurso.Modalidade);
            contextQuery.Parameters.Add("@TIPO_CURSO", dadosCurso.Tipo_curso);
            contextQuery.Parameters.Add("@ATIVO", dadosCurso.Ativo);
            contextQuery.Parameters.Add("@TEM_RECLASSIFICACAO", dadosCurso.Tem_reclassificacao);
            contextQuery.Parameters.Add("@FORMATURA", dadosCurso.Formatura);
            contextQuery.Parameters.Add("@CONCOMITANTE", dadosCurso.Concomitante);
            contextQuery.Parameters.Add("@PARTICIPACALCULONOVASTURMASTURNOSVAGAS", dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas);
            contextQuery.Parameters.Add("@PARTICIPAFECHAMENTOAUTOMATICO", dadosCurso.ParticipaFechamentoAutomatico);
            contextQuery.Parameters.Add("@PERMITETRANSFERENCIATURMATOTAL", dadosCurso.PermiteTransferenciaTurmaTotal);
            contextQuery.Parameters.Add("@PERMITECHOQUETURNOINTEGRALTURNOSVAGAS", dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas);
            contextQuery.Parameters.Add("@SALAEXTERNA", dadosCurso.Salaexterna);
            contextQuery.Parameters.Add("@OFERTAELETIVA", dadosCurso.OfertaEletiva);
            contextQuery.Parameters.Add("@FORMACAOBASICA", dadosCurso.FormacaoBasica);
            contextQuery.Parameters.Add("@ITINERARIOFORMATIVO", dadosCurso.ItinerarioFormativo);
            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", dadosCurso.TrilhaAprendizagem == null ? DBNull.Value : (object)dadosCurso.TrilhaAprendizagem);
            contextQuery.Parameters.Add("@MAXIMOCOMPONENTES", dadosCurso.MaximoComponentes == null ? DBNull.Value : (object)dadosCurso.MaximoComponentes);
            contextQuery.Parameters.Add("@USUARIOID", dadosCurso.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(string curso)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Turma rnTurma = new Turma();
            RN.CursoDuracao rnCursoDuracao = new CursoDuracao();
            RN.Aluno rnAluno = new Aluno();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RN.UnidadeEnsinoCursos rnUnidadeEnsinoCursos = new UnidadeEnsinoCursos();
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.ProgressaoSerie rnProgressaoSerie = new ProgressaoSerie();
            RN.CtvAgendaConfTurnoVaga rnCtvAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
            RN.HorarioOperacional rnHorarioOperacional = new HorarioOperacional();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    ////Verifica se ja foi utilizado
                    if (rnCursoDuracao.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois possui duração cadastrada.");
                    }

                    if (rnTurma.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já foi associada a uma turma.");
                    }

                    if (rnRenovacao.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já foi associada a uma renovação de matricula.");
                    }

                    if (rnAluno.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já foi associada a um aluno.");
                    }

                    if (rnUnidadeEnsinoCursos.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já foi associada a uma unidade de ensino.");
                    }

                    if (rnCurriculo.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já foi associada a uma matriz curricular.");
                    }

                    if (rnControleVaga.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já possui controle de vagas cadastrado.");
                    }

                    if (rnProgressaoSerie.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já possui progressão de série cadastrada.");
                    }

                    if (rnCtvAgendaConfTurnoVaga.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já possui agenda de turnos e vagas cadastrada.");
                    }

                    if (rnHorarioOperacional.PossuiCursoPor(contexto, curso))
                    {
                        mensagens.Add("Este curso não pode ser excluido pois já possui horario operacional cadastrado.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Deleta as opções de Unidades Curricular 
                this.RemoveUnidadeCurricular(contexto, curso);

                //Deleta as opções de Area Itinerario Formativo 
                this.RemoveAreaItinerarioFormativo(contexto, curso);

                //Deleta as opções de Composicao Itinerario Formativo 
                this.RemoveComposicaoItinerarioFormativo(contexto, curso);

                //Remove Curso
                this.Remove(contexto, curso);
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
                contexto.Dispose();
            }
        }

        private void RemoveUnidadeCurricular(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Pedagogico.CURSO__UNIDADECURRICULAR
                                      WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemoveAreaItinerarioFormativo(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Pedagogico.CURSO__AREAITINERARIOFORMATIVO
                                      WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemoveComposicaoItinerarioFormativo(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Pedagogico.CURSO__COMPOSICAOITINERARIOFORMATIVO
                                      WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            contexto.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE LY_CURSO
                                      WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiAreaItinerarioFormativoPor(DataContext ctx, int areaItinerarioFormativoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CURSO__AREAITINERARIOFORMATIVO
                                WHERE AREAITINERARIOFORMATIVOID = @AREAITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@AREAITINERARIOFORMATIVOID", SqlDbType.Int, areaItinerarioFormativoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTrilhaAprendizagemPor(DataContext ctx, int trilhaAprendizagemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LY_CURSO
                                WHERE TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID ";

            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagemId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiComposicaoItinerarioFormativoPor(DataContext ctx, int composicaoItinerarioFormativoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CURSO__COMPOSICAOITINERARIOFORMATIVO
                                WHERE COMPOSICAOITINERARIOFORMATIVOID = @COMPOSICAOITINERARIOFORMATIVOID ";

            contextQuery.Parameters.Add("@COMPOSICAOITINERARIOFORMATIVOID", SqlDbType.Int, composicaoItinerarioFormativoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiUnidadeCurricularPor(DataContext ctx, int unidadeCurricularId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CURSO__UNIDADECURRICULAR
                                WHERE UNIDADECURRICULARID = @UNIDADECURRICULARID ";

            contextQuery.Parameters.Add("@UNIDADECURRICULARID", SqlDbType.Int, unidadeCurricularId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        #region Curso Por Unidade

        public static QueryTable ConsultarCursoPorUnidade(string unidade)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "select uc.UNIDADE_ENS AS unidade_ens, uc.ATO as ato, uc.DT_IMPLANTACAO as dt_implantacao, " +
                         "uc.DT_DO as dt_do, c.NOME AS nome, t.DESCRICAO AS descricao, uc.ORDEM as ordem, c.CURSO AS codigo, " +
                         "mc.DESCRICAO as modalidade, tp.DESCRICAO as nivel, OBSERVACOES " +
                         "from LY_UNIDADE_ENSINO_CURSOS uc join LY_CURSO c on uc.CURSO = c.CURSO join LY_TURNO t " +
                         "on uc.TURNO = t.TURNO left join LY_MODALIDADE_CURSO mc on c.MODALIDADE = mc.MODALIDADE " +
                         "left join LY_TIPO_CURSO tp on tp.TIPO = c.TIPO where uc.UNIDADE_ENS = ? order by c.NOME";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, unidade);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        //gera ordem de lotação a partir da última de uma matrícula
        public static decimal GeraOrdem(string unidade)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            decimal ordem;
            string ordem0;

            try
            {
                ordem0 = Convert.ToString(TCommand.ExecuteScalar(connection, "Select max(ordem) From Ly_unidade_ensino_cursos where unidade_ens = ?", unidade));
                if (!string.IsNullOrEmpty(ordem0))
                {
                    ordem = Convert.ToDecimal(ordem0);
                }
                else
                {
                    ordem = 0;
                }
            }
            finally
            {
                connection.Close();
            }

            return ordem + 1;
        }

        public static RetValue IncluirCursoPorUnidade(Ly_unidade_ensino_cursos dtUnidadeEnsinoCurso)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtUnidadeEnsinoCurso != null)
                {
                    if (dtUnidadeEnsinoCurso.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtUnidadeEnsinoCurso);

                        Ly_unidade_ensino_cursos.Row.Insert(connection, dtUnidadeEnsinoCurso.Rows[0].Unidade_ens, dtUnidadeEnsinoCurso.Rows[0].Ordem, dtUnidadeEnsinoCurso.Rows[0].Curso, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Escolaridade inserida com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue AlterarCursoPorUnidade(Ly_unidade_ensino_cursos dtUnidadeEnsinoCurso)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtUnidadeEnsinoCurso != null)
                {
                    if (dtUnidadeEnsinoCurso.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtUnidadeEnsinoCurso);

                        Ly_unidade_ensino_cursos.Row.Update(connection, dtUnidadeEnsinoCurso.Rows[0].Unidade_ens, dtUnidadeEnsinoCurso.Rows[0].Ordem, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Registro alterado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue ExcluirCursoPorUnidade(string unidadeEns, decimal ordem)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                object[] parametros = new object[] { unidadeEns, ordem };
                Ly_unidade_ensino_cursos dtUnidadeEnsinoCursos = Ly_unidade_ensino_cursos.Query(connection, "UNIDADE_ENS = ? AND ORDEM = ?", parametros);

                if (dtUnidadeEnsinoCursos != null)
                {
                    if (dtUnidadeEnsinoCursos.Rows != null)
                    {
                        foreach (Ly_unidade_ensino_cursos.Row linha in dtUnidadeEnsinoCursos.Rows)
                        {
                            linha.Delete();
                        }

                        dtUnidadeEnsinoCursos.Put(connection);
                        retorno = VerificarErro(dtUnidadeEnsinoCursos);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Escolaridade removida com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static QueryTable ConsultarCursoPorUnidade(string unidade, string curso, string turno, decimal ordem)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;
            if (ordem == 0)
            {
                string sql = "select 1 from LY_UNIDADE_ENSINO_CURSOS where UNIDADE_ENS = ? AND CURSO = ? AND TURNO = ?";

                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, unidade, curso, turno);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                string sql = "select 1 from LY_UNIDADE_ENSINO_CURSOS where UNIDADE_ENS = ? AND CURSO = ? AND TURNO = ? AND ORDEM <> ?";
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, unidade, curso, turno, ordem);
                }
                finally
                {
                    connection.Close();
                }
            }
            return qt;
        }

        //public static QueryTable ConsultarCursoPorUnidade(string unidade, string curso, decimal ordem)
        //{
        //    TConnection connection = Config.CreateConnection();

        //    connection.Open();
        //    QueryTable qt = null;
        //    if (ordem == 0)
        //    {
        //        string sql = "select 1 from LY_UNIDADE_ENSINO_CURSOS where UNIDADE_ENS = ? AND CURSO = ? ";

        //        try
        //        {
        //            qt = new QueryTable(sql);

        //            qt.Query(connection, unidade, curso);
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //    else
        //    {
        //        string sql = "select 1 from LY_UNIDADE_ENSINO_CURSOS where UNIDADE_ENS = ? AND CURSO = ? AND ORDEM <> ?";
        //        try
        //        {
        //            qt = new QueryTable(sql);

        //            qt.Query(connection, unidade, curso, ordem);
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //    return qt;
        //}

        public static DataTable ConsultarCursoPorUnidade(string unidade, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"select 1 from LY_UNIDADE_ENSINO_CURSOS 
                    where UNIDADE_ENS = @UNIDADE_ENS AND CURSO = @CURSO");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidade);
                contextQuery.Parameters.Add("@CURSO", curso);

                return ctx.GetDataTable(contextQuery);
            }
        }
        #endregion

        //Consultar nivel 
        public static string ConsultaNivel(string curso)
        {
            string sql = "select tipo from ly_curso where curso = ?";
            return ConsultarCampo(sql, curso);
        }

        //Consultar modalidade 
        public static string ConsultaModalidade(string curso)
        {
            string sql = "select modalidade from ly_curso where curso = ?";
            return ConsultarCampo(sql, curso);
        }

        public static string ConsultarTipoProfCurso(string curso)
        {
            return ConsultarCampo(
                new ContextQuery(
                    @"SELECT TOP 1
                            TIPO_CURSO
                    FROM    LY_CURSO
                    WHERE   CURSO = @CURSO",
                    new ContextQueryParameter("@CURSO", curso)));
        }

        public static DataTable ListarModalidadeSerie()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT MC.MODALIDADE,MC.DESCRICAO  
                        FROM LY_MODALIDADE_CURSO MC                        
                    ORDER BY MC.DESCRICAO");

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarTipoCurso()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT TC.TIPO, DESCRICAO
                        FROM LY_TIPO_CURSO TC                       
                    ORDER BY DESCRICAO");

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarEscolaridade(string nivel, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO,C.NOME 
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        WHERE TC.TIPO= @TIPO
                        AND MC.MODALIDADE=@MODALIDADE
                    ORDER BY C.NOME");

                contextQuery.Parameters.Add("@TIPO", nivel);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarModalidadeRestricaoMatricula(string censo, int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT MC.MODALIDADE,MC.DESCRICAO  
                        FROM LY_CURSO C
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN TCE_CVT_CURSO_SERIE TCS ON T.CURSO = TCS.CURSO and t.SERIE = tcs.SERIE  AND T.ANO = TCS.REFERENCIA_ANO
                        INNER JOIN TCE_CVT_PERIODO P ON P.ID_CVT_PERIODO = TCS.ID_CVT_PERIODO 
                    WHERE T.FACULDADE = @CENSO                       
                        AND P.ANO = @ANO
                    ORDER BY MC.DESCRICAO");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarNivelCursoRestricaoMatricula(string censo, int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT TC.TIPO, DESCRICAO
                        FROM LY_CURSO C
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN TCE_CVT_CURSO_SERIE TCS ON T.CURSO = TCS.CURSO and t.SERIE = tcs.SERIE  AND T.ANO = TCS.REFERENCIA_ANO
                        INNER JOIN TCE_CVT_PERIODO P ON P.ID_CVT_PERIODO = TCS.ID_CVT_PERIODO  
                    WHERE T.FACULDADE = @CENSO                       
                        AND P.ANO = @ANO
                    ORDER BY DESCRICAO");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarEscolaridadeRestricaoMatricula(int ano, string nivel, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO,C.NOME 
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN TCE_CVT_CURSO_SERIE TCS ON T.CURSO = TCS.CURSO and t.SERIE = tcs.SERIE  AND T.ANO = TCS.REFERENCIA_ANO
                        INNER JOIN TCE_CVT_PERIODO P ON P.ID_CVT_PERIODO = TCS.ID_CVT_PERIODO 
                        WHERE P.ANO = @ANO
                        AND TC.TIPO= @TIPO
                        AND MC.MODALIDADE = @MODALIDADE
                    ORDER BY C.NOME");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPO", nivel);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarEscolaridadeRestricaoMatricula(string censo, int ano, string nivel, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO,C.NOME 
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN TCE_CVT_CURSO_SERIE TCS ON T.CURSO = TCS.CURSO and t.SERIE = tcs.SERIE  AND T.ANO = TCS.REFERENCIA_ANO
                        INNER JOIN TCE_CVT_PERIODO P ON P.ID_CVT_PERIODO = TCS.ID_CVT_PERIODO 
                        WHERE t.FACULDADE = @CENSO                       
                        AND P.ANO = @ANO
                        AND TC.TIPO= @TIPO
                        AND MC.MODALIDADE=@MODALIDADE
                    ORDER BY C.NOME");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPO", nivel);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSerieRestricaoMatricula(string censo, int ano, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT S.SERIE
                        FROM LY_SERIE S
                        INNER JOIN LY_CURSO C ON C.CURSO=S.CURSO
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO AND T.SERIE=S.SERIE
                        INNER JOIN TCE_CVT_CURSO_SERIE TCS ON T.CURSO = TCS.CURSO and t.SERIE = tcs.SERIE  AND T.ANO = TCS.REFERENCIA_ANO
                        INNER JOIN TCE_CVT_PERIODO P ON P.ID_CVT_PERIODO = TCS.ID_CVT_PERIODO 
                        WHERE t.FACULDADE = @CENSO                       
                        AND P.ANO = @ANO
                        AND T.CURSO= @CURSO
                        ORDER BY S.SERIE");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURSO", curso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarModalidadePorUE(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  DISTINCT mc.MODALIDADE AS modalidade,
                            mc.DESCRICAO 
                          
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                            JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS = @CENSO
                    ORDER BY  mc.DESCRICAO ");

                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarNivelPorUE(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  DISTINCT tp.TIPO,
                                tp.DESCRICAO
                          
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                            JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS = @CENSO
                    ORDER BY   tp.DESCRICAO ");

                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarCursoPorUE(string censo, string modalidade, string nivel)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO, C.NOME
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                            JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS = @CENSO
                    AND MC.MODALIDADE= @MODALIDADE
                    AND TP.TIPO= @NIVEL
                    ORDER BY  C.NOME ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);
                contextQuery.Parameters.Add("@NIVEL", nivel);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarCursosPorUE(string censo, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO, C.NOME
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                            JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS = @CENSO
                    AND MC.MODALIDADE= @MODALIDADE
                    ORDER BY  C.NOME ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarCursoPorTurmaUE(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO,tc.DESCRICAO + ' - ' + C.NOME AS NOME
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                       WHERE T.FACULDADE = @CENSO                       
                        AND T.ANO = @ANO
                        AND T.SEMESTRE = @PERIODO
                    ORDER BY NOME ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaPorAno(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                        SELECT DISTINCT lyc.NOME, 
                                        lyc.CURSO, 
                                        lyc.CURSO + ' - ' + lyc.NOME AS CURSONOME 
                        FROM   LY_CURSO lyc 
                               INNER JOIN LY_CURRICULO lycr 
                                       ON lycr.CURSO = lyc.CURSO 
                        WHERE  1 = 1 
                               AND ( lycr.DT_EXTINCAO IS NULL 
                                      OR CONVERT(DATE, lycr.DT_EXTINCAO) > CONVERT(DATE, Getdate()) ) 
                               AND lyc.CURSO <> '9999.99' 
                               AND lycr.ANO_INI = @ANO
                               AND exists (select top 1 1 from LY_SERIE lys where lys.CURSO = lyc.CURSO) 
                        ORDER  BY NOME
                ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public static DataTable ConsultarEscolaridade()
        {
            return Consultar(
                  new ContextQuery(
                      @"SELECT DISTINCT C.CURSO,C.NOME 
                        FROM LY_CURSO C 
                        order by c.nome"));
        }

        public static DataTable ListarModalidadeRestricaoMatricula(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  DISTINCT
                                MC.MODALIDADE ,
                                MC.DESCRICAO
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                                INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                                INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                        WHERE   TI.CENSO = @CENSO
                                AND AG.ANO = @ANO
                                AND AG.PERIODO = @PERIODO
                        ORDER BY MC.DESCRICAO ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarNivelCursoRestricaoMatricula(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  DISTINCT
                                TC.TIPO ,
                                DESCRICAO
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                                INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                                INNER JOIN LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                        WHERE   TI.CENSO = @CENSO
                                AND AG.ANO = @ANO
                                AND AG.PERIODO = @PERIODO
                        ORDER BY DESCRICAO ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarEscolaridadeRestricaoMatricula(string censo, int ano, int periodo, string nivel, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  DISTINCT
                                    C.CURSO ,
                                    C.NOME
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                    INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                            WHERE   ti.censo = @CENSO
                                    AND Ag.ANO = @ANO
                                    AND Ag.PERIODO = @PERIODO
                                    AND c.TIPO = @TIPO
                                    AND c.MODALIDADE = @MODALIDADE
                            ORDER BY C.NOME ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TIPO", nivel);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSerieRestricaoMatricula(string censo, int ano, int periodo, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  DISTINCT
                                    AG.SERIE
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   TI.CENSO = @CENSO
                                    AND AG.ANO = @ANO
                                    AND AG.PERIODO = @PERIODO
                                    AND AG.CURSO = @CURSO
                            ORDER BY AG.SERIE ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarModalidadePerfil(DbObject perfilid)
        {
            if (perfilid != null && !perfilid.IsNull)
            {
                var contextQuery = new ContextQuery(
                        @"SELECT DISTINCT
                            MC.MODALIDADE ,
                            MC.DESCRICAO,
                             MC.MODALIDADE + ' - ' +  MC.DESCRICAO AS DESCR_MODALIDADE
                    FROM    LY_CURSO C
                            INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
                    WHERE   C.MODALIDADE NOT IN  ( SELECT MODALIDADEID
                                         FROM   PERFILMODALIDADE
                                         WHERE PERFILID = @PERFIL )
                    ORDER BY MC.DESCRICAO");

                contextQuery.Parameters.Add("@PERFIL", Convert.ToString(perfilid));

                return Consultar(contextQuery);
            }

            return null;
        }

        public static DataTable ObtemModalidadeAgendaPor(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT MC.MODALIDADE,MC.DESCRICAO  
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                    ORDER BY MC.DESCRICAO");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ObtemNivelAgendaPor(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT Tp.TIPO, tp.DESCRICAO
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                    ORDER BY tp.DESCRICAO");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ObtemEscolaridadeAgendaPor(string censo, int ano, int periodo, string nivel, string modalidade)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT C.CURSO,C.NOME
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                        AND TP.TIPO= @TIPO
                        AND MC.MODALIDADE=@MODALIDADE
                    ORDER BY C.NOME");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TIPO", nivel);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ObtemSerieAgendaPor(string censo, int ano, int periodo, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT A.SERIE
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                        AND uc.CURSO= @CURSO
                    ORDER BY A.SERIE");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ObtemModalidadeAgendaSemRestricaoComTurnoPor(string censo, int ano, int periodo, int codPerfil)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable modalidades = null;

            try
            {
                contextQuery.Command =
                     @"SELECT DISTINCT MC.MODALIDADE,MC.DESCRICAO
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO ct ON ct.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                            AND uc.UNIDADE_ENS = ct.CENSO
                        ";

//                if (codPerfil == 8)
//                {
//                    contextQuery.Command += @" INNER JOIN [LYCEUM].[Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] TE 
//                             ON TE.CENSO = UC.UNIDADE_ENS 
//                                AND TE.CURSO = UC.CURSO 
//                                AND (TE.ANO = A.ANO or TE.ANO = A.ANO - 1)"; //Cursos do ano atual e anterios
//                }

                contextQuery.Command += @" WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                        AND NOT EXISTS (SELECT 1
								 FROM   DBO.TCE_CTV_RESTRICAO RE
								 WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA=A.ID_AGENDA_CONF_TURNO_VAGA
										AND RE.CENSO = UC.UNIDADE_ENS)
                        ";

                if (codPerfil == 8)
                {
                    contextQuery.Command += @" AND UC.CURSO IN ('2026.01','2026.02','2026.03','2026.04',
						'2026.05','2026.06','2026.07','2026.08','2026.09','2026.10',
						'2026.11','2026.12','2026.13','2026.14','2026.15','2026.16',
						'2026.17','2026.18','2026.19','2026.20','2026.21','2026.22',
						'2026.23','2026.24','2026.25','2026.26','2026.27')
                            AND A.ANO = 2026 
                        "; //Cursos enviados pela area para incluir em 2026
                }

                contextQuery.Command += @" ORDER BY MC.DESCRICAO ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                modalidades = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return modalidades;
        }

        public static DataTable ObtemEscolaridadeAgendaSemRestricaoComTurnoPor(string censo, int ano, int periodo, string modalidade, int codPerfil)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cursos = null;

            try
            {
                contextQuery.Command =
                    @"SELECT DISTINCT C.CURSO,C.NOME
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO ct ON ct.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                 AND uc.UNIDADE_ENS = ct.CENSO";

//                if (codPerfil == 8)
//                {
//                    contextQuery.Command += @" INNER JOIN [LYCEUM].[Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] TE 
//                             ON TE.CENSO = UC.UNIDADE_ENS 
//                                AND TE.CURSO = UC.CURSO 
//                                AND (TE.ANO = A.ANO or TE.ANO = A.ANO - 1)"; //Cursos do ano atual e anterios
//                }

                contextQuery.Command += @"
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                        AND MC.MODALIDADE=@MODALIDADE
                        AND NOT EXISTS (SELECT 1
                                         FROM   DBO.TCE_CTV_RESTRICAO RE
                                         WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA=A.ID_AGENDA_CONF_TURNO_VAGA
                                                AND RE.CENSO = UC.UNIDADE_ENS)

                    ";

                if (codPerfil == 8)
                {
                    contextQuery.Command += @" AND UC.CURSO IN ('2026.01','2026.02','2026.03','2026.04',
						'2026.05','2026.06','2026.07','2026.08','2026.09','2026.10',
						'2026.11','2026.12','2026.13','2026.14','2026.15','2026.16',
						'2026.17','2026.18','2026.19','2026.20','2026.21','2026.22',
						'2026.23','2026.24','2026.25','2026.26','2026.27')
                            AND A.ANO = 2026 
                        "; //Cursos enviados pela area para incluir em 2026
                }

                contextQuery.Command += @" ORDER BY C.NOME ";


                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);

                cursos = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return cursos;
        }

        public bool SerieNaoExisteDentroDoCurso(DataContext ctx, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"
                    SELECT count(0)
                    FROM  LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                    JOIN LY_SERIE LS ON ls.CURSO=c.CURSO
                    INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO
                    INNER JOIN dbo.LY_UNIDADE_ENSINO ue ON uc.UNIDADE_ENS = ue.UNIDADE_ENS
                    WHERE LS.CURSO = @CURSO
                    AND LS.SERIE = @SERIE
                    AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                ";

            contextQuery.Parameters.Clear();
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            return ctx.GetReturnValue<int>(contextQuery) == 0;
        }

        public bool EhCursoMinistradoSalaExterna(string curso)
        {
            bool podeCurso;

            ContextQuery contextQuery = new ContextQuery(
                     @"SELECT  COUNT(*)
                        FROM    LY_CURSO
                        WHERE   CURSO = @CURSO
                            AND SALAEXTERNA='S'
                             ");

            contextQuery.Parameters.Add("@CURSO", curso);

            podeCurso = (ExecutarFuncao<int>(contextQuery) > 0);

            return podeCurso;
        }

        public string ObtemModalidadePor(int ano, int periodo, string censo, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            string modalidade = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT mc.MODALIDADE
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON UC.CURSO = A.CURSO  
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO ct ON ct.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                 AND uc.UNIDADE_ENS = ct.CENSO
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND A.ANO = @ANO
                        AND A.PERIODO = @PERIODO
                        AND C.CURSO=@CURSO
                        AND NOT EXISTS (SELECT 1
                                         FROM   DBO.TCE_CTV_RESTRICAO RE
                                         WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA=A.ID_AGENDA_CONF_TURNO_VAGA
                                                AND RE.CENSO = UC.UNIDADE_ENS)
                    ORDER BY mc.MODALIDADE"
                };
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);


                modalidade = ctx.GetReturnValue<string>(contextQuery);

                return modalidade;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

        }

        public string RetonaModalidadePor(DataContext ctx, string curso)
        {
            string modalidade = string.Empty;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT MODALIDADE
                                FROM LY_CURSO
                                WHERE CURSO = @CURSO "
            };

            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);

            modalidade = ctx.GetReturnValue<string>(contextQuery);

            return modalidade;
        }

        public int RetonaItinerarioPor(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT T.ITINERARIOFORMATIVOID
                                        FROM LY_CURSO C
	                                        INNER JOIN PEDAGOGICO.TRILHAAPRENDIZAGEM T ON C.TRILHAAPRENDIZAGEMID = T.TRILHAAPRENDIZAGEMID
                                        WHERE CURSO =  @CURSO ";

                contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ITINERARIOFORMATIVOID"]);
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

        public int RetonaTrilhaPor(DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT C.TRILHAAPRENDIZAGEMID
                                          FROM LY_CURSO C
                                          WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TRILHAAPRENDIZAGEMID"]);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="cursos_agenda"></param>
        /// <returns></returns>
        public static DataTable RetornaCursosAlunosDasCompartilhadasPor(string unidadeEnsinoDestino, string anoPeriodo, DataTable cursosAgenda)
        {
            #region Propriedades

            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cursos = null;
            string cursosAgendaDistinct = string.Empty;

            #endregion

            #region Distinct dos Cursos Agenda

            if (cursosAgenda.Rows.Count > 0)
            {
                DataTable dtDistinct = cursosAgenda.DefaultView.ToTable(true, "CURSO");

                foreach (DataRow row in dtDistinct.Rows)
                {
                    cursosAgendaDistinct += "'" + row["CURSO"].ToString() + "',";
                }

                cursosAgendaDistinct = cursosAgendaDistinct.Substring(0, cursosAgendaDistinct.Length - 1);
            }

            #endregion

            #region Formatando Ano e Período

            string[] anoperiodo = anoPeriodo.Split('-');
            int ano = Convert.ToInt32(anoperiodo[0]);
            int periodo = Convert.ToInt32(anoperiodo[1]);

            #endregion

            try
            {
                contextQuery.Command = string.Format(@"
                                    SELECT DISTINCT EC.unidade_ens, 
                                                    C.curso as curso, 
                                                    C.curso + ' - ' + MC.descricao + ' / ' + TP.descricao + ' / ' + C.nome AS MOD_SEG_CURSO
                                    FROM   ly_unidade_ensino_cursos EC 
                                           INNER JOIN ly_curso C 
                                                   ON EC.curso = C.curso 
                                           LEFT JOIN ly_modalidade_curso MC 
                                                  ON C.modalidade = MC.modalidade 
                                           INNER JOIN ly_tipo_curso TP 
                                                  ON TP.tipo in (2,3) --add tipo EF ANOS FINAIS 
                                                     AND C.tipo = TP.tipo 
                                           INNER JOIN tce_compartilhada CO 
                                                   ON CO.censo = EC.unidade_ens 
                                           INNER JOIN tce_ctv_conf_turno CT 
                                                   ON CT.censo = '{0}' -- Unidade de Ensino de destino selecionada 
                                                      AND CT.continuidade = 1 
                                           INNER JOIN tce_ctv_agenda_conf_turno_vaga CV 
                                                   ON CV.ano = {1} -- ListaPeriodoLetivoPorAgenda(int AgendaId)  Proximo Ano  
                                                      AND CV.periodo = {2} -- ListaPeriodoLetivoPorAgenda(int AgendaId) Proximo Periodo    
                                                      AND CV.curso = C.curso 
                                    WHERE  CO.censo = '{0}' -- UNIDADE DE ENSINO DE DESTINO SELECIONADA 
                                           AND EC.curso NOT IN ('{3}') -- ListaAgenda_Curso__Agenda_UnidadeEnsinoPorAgendaEParticipacao"
                    , unidadeEnsinoDestino
                    , ano
                    , periodo
                    , cursosAgendaDistinct);


                cursos = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return cursos;
        }

        public DataTable listaModalidadeSegmentoCursoPor(int ano, int perfilId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable modalidades = null;

            try
            {
                if (ano != 0 && perfilId != 0)
                {
                    contextQuery.Command = @"SELECT  ' Selecione' AS MODALIDADE ,
                                            'Modalidade/Segmento' AS SEGMENTO ,
                                            'Curso' AS  CURSO,
                                            'Nome Curso' AS NOME_CURSO
                                        UNION ALL
                                        SELECT DISTINCT
                                                MC.DESCRICAO ,
                                                TC.DESCRICAO ,
                                                C.CURSO ,
                                                C.NOME
                                        FROM    DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA TV ON TI.ID_AGENDA_CONF_TURNO_VAGA = TV.ID_AGENDA_CONF_TURNO_VAGA
                                                INNER JOIN DBO.LY_CURSO C ON TV.CURSO = C.CURSO
                                                INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                                LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                                                LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                                        WHERE   ANO = @ANO
                                                AND (PERFILID = @PERFILID OR @PERFILID = 7) -- 7 = DIESP QUE NÃO TEM TRATAMENTO DE PERFIL POR MODALIDADE
                                        ORDER BY MODALIDADE,SEGMENTO,NOME_CURSO";

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERFILID", perfilId);

                    modalidades = ctx.GetDataTable(contextQuery);
                }
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

            return modalidades;


        }


        public bool PermiteChoqueTurnoIntegralPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool permite = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   LY_CURSO 
                                            WHERE  PERMITECHOQUETURNOINTEGRALTURNOSVAGAS = 'S' 
                                                   AND CURSO = @CURSO  ";

                contextQuery.Parameters.Add("@CURSO", curso);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    permite = true;
                }

                return permite;
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
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PermitePermiteTransferenciaTurmaTotalPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool permite = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   LY_CURSO 
                                            WHERE  PERMITETRANSFERENCIATURMATOTAL = 'S' 
                                                   AND CURSO = @CURSO  ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                permite = true;
            }

            return permite;

        }

        public DataTable ObtemCursoParticipa3FasePor(int ano, int periodo, string censo, string modalidade, string nivel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable dtCurso = new DataTable();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT  C.CURSO ,
                                                C.NOME
                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_TURNO t ON uc.TURNO = t.TURNO
                        LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                        LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                        INNER JOIN dbo.TCE_CONTROLE_VAGA cv ON cv.CURSO = C.CURSO
                                 AND uc.UNIDADE_ENS = cv.CENSO
                    WHERE uc.UNIDADE_ENS = @CENSO                       
                        AND CV.ANO = @ANO
                        AND CV.PERIODO = @PERIODO
                        AND MC.MODALIDADE= @MODALIDADE
                        AND TP.TIPO= @NIVEL
                    ORDER BY C.NOME"
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@MODALIDADE", modalidade);
                contextQuery.Parameters.Add("@NIVEL", nivel);


                dtCurso = ctx.GetDataTable(contextQuery);

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

            return dtCurso;
        }


        public bool PermiteEletivaPor(string curso)
        {
            Seeduc.Infra.Data.DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.PermiteEletivaPor(contexto, curso);
                return retorno;
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
                contexto.Dispose();
            }
        }

        public bool PermiteEletivaPor(Seeduc.Infra.Data.DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT count(*)
                                        FROM   LY_CURSO
                                        WHERE CURSO = @CURSO
                                        AND OFERTAELETIVA = 'S'
                                           ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiCursoPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool permite = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1)
                                        FROM   LY_CURSO
                                        WHERE CURSO = @CURSO  ";

                contextQuery.Parameters.Add("@CURSO", curso);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    permite = true;
                }

                return permite;
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
            finally
            {
                ctx.Dispose();
            }
        }

        public string ObtemDescricaoPor(string curso)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT NOME
                        FROM LY_CURSO (NOLOCK)
                        WHERE CURSO = @CURSO ";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.Variant, curso);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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
                contexto.Dispose();
            }
        }

        public bool EhItinerarioFormativoTrihaComMatrizPor(string curso, decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM LY_CURRICULO CU
										INNER JOIN LY_CURSO C ON C.CURSO=CU.CURSO
                                        WHERE ITINERARIOFORMATIVO = 'S'	                                        
	                                        AND TRILHAAPRENDIZAGEMID IS NOT NULL
	                                        AND TRILHAAPRENDIZAGEMID <> 31 --Tipo interno 
                                            AND ANO_INI = @ANO
											AND SEM_INI = @PERIODO";

                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Decimal, periodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
                contexto.Dispose();
            }
        }
    }
}
