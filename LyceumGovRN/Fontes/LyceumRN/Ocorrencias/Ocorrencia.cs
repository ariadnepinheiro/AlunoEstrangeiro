using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class Ocorrencia
    {
        public bool PossuiClassePor(DataContext contexto, int classeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.OCORRENCIA (NOLOCK)
                                    WHERE CLASSEID = @CLASSEID ";

            contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, classeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMotivoCancelamentoPor(DataContext contexto, int motivoCancelamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.OCORRENCIA (NOLOCK)
                                    WHERE MOTIVOCANCELAMENTOID = @MOTIVOCANCELAMENTOID ";

            contextQuery.Parameters.Add("@MOTIVOCANCELAMENTOID", SqlDbType.Int, motivoCancelamentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiSubClassePor(DataContext contexto, int subClasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.OCORRENCIA (NOLOCK)
                                    WHERE SUBCLASSEID = @SUBCLASSEID ";

            contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, subClasseId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMeioPor(DataContext contexto, int meioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.OCORRENCIA (NOLOCK)
                                    WHERE MEIOID = @MEIOID ";

            contextQuery.Parameters.Add("@MEIOID", SqlDbType.Int, meioId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaOcorrenciaAtivoPor(string censo, int ano, bool somenteAtivos)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT  O.OCORRENCIAID,
												O.NUMEROOCORRENCIA,
												O.DATAOCORRENCIA, 
                                                o.SUBCLASSEID,
                                                SC.DESCRICAO AS SUBCLASSE,
												C.CLASSEID, 
												C.DESCRICAO AS CLASSE,
												O.DELEGACIAID,
												D.DESCRICAO AS DELEGACIA,
												O.BATALHAOID,
												B.DESCRICAO AS BATALHAO,
												CASE	
													WHEN O.ATIVO = 0 THEN 'Cancelado - ' + ISNULL(MC.DESCRICAO, '')
													WHEN ARQUIVADA = 1 THEN 'Arquivado'
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.OCORRENCIAENCAMINHAMENTO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) > 0 THEN 'Encaminhado'													
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.VITIMA E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) = 0 THEN 'Incompleto - Sem Alvo'
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.ACUSADO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) = 0 THEN 'Incompleto - Sem Autor'
													ELSE 'Em aberto'
												END SITUACAO
                                            FROM Ocorrencias.OCORRENCIA o (NOLOCK)
												INNER JOIN Ocorrencias.CLASSE c (NOLOCK) ON o.CLASSEID = c.CLASSEID
                                                LEFT JOIN Ocorrencias.MOTIVOCANCELAMENTO MC (NOLOCK) ON o.MOTIVOCANCELAMENTOID = MC.MOTIVOCANCELAMENTOID
                                                LEFT JOIN Ocorrencias.SUBCLASSE sc (NOLOCK) ON o.SUBCLASSEID = sc.SUBCLASSEID	
												LEFT JOIN Ocorrencias.DELEGACIA d (nolock) on o.DELEGACIAID = d.DELEGACIAID
												LEFT JOIN Ocorrencias.BATALHAO b (NOLOCK) ON o.BATALHAOID = b.BATALHAOID
                                            WHERE CENSO = @CENSO
												AND YEAR(DATAOCORRENCIA) = @ANO
                                                ");

                if (somenteAtivos)
                {
                    sql.Append(@"                   AND O.ATIVO = 1
												 ");
                }

                sql.Append(@"               ORDER BY DATAOCORRENCIA DESC   ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
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

        public DataTable ListaOcorrenciaAtivaPor(int ano, int? regional, string municipio, string censo, DateTime? dataOcorrencia, int? classe, int? subclasse, int? tratamento, string situacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT O.OCORRENCIAID,
												O.NUMEROOCORRENCIA,
												O.DATAOCORRENCIA, 
												R.REGIONAL,
                                                CENSO,
                                                UE.NOME_COMP AS ESCOLA,
												M.NOME AS MUNICIPIO,
                                                O.SUBCLASSEID,
                                                SC.DESCRICAO AS SUBCLASSE,
												C.CLASSEID, 
												C.DESCRICAO AS CLASSE,
												CASE	
													WHEN O.ATIVO = 0 THEN 'Cancelado - ' + ISNULL(MC.DESCRICAO, '')
													WHEN ARQUIVADA = 1 THEN 'Arquivado'
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.OCORRENCIAENCAMINHAMENTO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) > 0 THEN 'Encaminhado'													
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.VITIMA E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) = 0 THEN 'Incompleto - Sem Alvo'
													WHEN (SELECT COUNT(1)
														FROM OCORRENCIAS.ACUSADO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID) = 0 THEN 'Incompleto - Sem Autor'
													ELSE 'Em aberto'
												END SITUACAO
                                            FROM Ocorrencias.OCORRENCIA o (NOLOCK)
												INNER JOIN Ocorrencias.CLASSE c (NOLOCK) ON o.CLASSEID = c.CLASSEID
                                                LEFT JOIN Ocorrencias.MOTIVOCANCELAMENTO MC (NOLOCK) ON o.MOTIVOCANCELAMENTOID = MC.MOTIVOCANCELAMENTOID
												INNER JOIN LY_UNIDADE_ENSINO UE (NOLock) on o.CENSO = ue.UNIDADE_ENS
												INNER JOIN TCE_REGIONAL R (NOLOCK) ON UE.ID_REGIONAL = R.ID_REGIONAL
												INNER JOIN MUNICIPIO M (NOLOCK) ON m.CODIGO = UE.MUNICIPIO
                                                LEFT JOIN Ocorrencias.SUBCLASSE sc (NOLOCK) ON o.SUBCLASSEID = sc.SUBCLASSEID	
												LEFT JOIN Ocorrencias.DELEGACIA d (nolock) on o.DELEGACIAID = d.DELEGACIAID
												LEFT JOIN Ocorrencias.BATALHAO b (NOLOCK) ON o.BATALHAOID = b.BATALHAOID
												LEFT JOIN Ocorrencias.OCORRENCIATRATAMENTO T (NOLOCK) ON O.OCORRENCIAID = T.OCORRENCIAID
                                            WHERE O.ATIVO = 1
												AND YEAR(DATAOCORRENCIA) = @ANO  
                                                ");

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                if (regional != null && regional > 0)
                {
                    sql.Append(@" AND UE.ID_REGIONAL = @REGIONAL 
                                                ");
                    contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
                }

                if (!municipio.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" AND UE.MUNICIPIO = @MUNICIPIO
                                                ");
                    contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                }

                if (!censo.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" AND CENSO = @CENSO
                                                ");
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                }

                if (dataOcorrencia != null && dataOcorrencia > DateTime.MinValue)
                {
                    sql.Append(@" AND CONVERT(DATE, O.DATAOCORRENCIA) = @DATAOCORRENCIA
                                                ");
                    contextQuery.Parameters.Add("@DATAOCORRENCIA", SqlDbType.DateTime, dataOcorrencia);
                }

                if (classe != null && classe > 0)
                {
                    sql.Append(@" AND O.CLASSEID = @CLASSE
                                                ");
                    contextQuery.Parameters.Add("@CLASSE", SqlDbType.Int, classe);
                }

                if (subclasse != null && subclasse > 0)
                {
                    sql.Append(@" AND O.SUBCLASSEID = @SUBCLASSE
                                                ");
                    contextQuery.Parameters.Add("@SUBCLASSE", SqlDbType.Int, subclasse);
                }

                if (tratamento != null && tratamento > 0)
                {
                    sql.Append(@" AND T.TRATAMENTOID = @TRATAMENTOID
                                                ");
                    contextQuery.Parameters.Add("@TRATAMENTOID", SqlDbType.Int, tratamento);
                }

                if (situacao == "Encaminhado")
                {
                    sql.Append(@" AND ARQUIVADA = 0
                                  AND EXISTS (SELECT TOP 1 1 
														FROM OCORRENCIAS.OCORRENCIAENCAMINHAMENTO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID)
                                                ");
                }
                else if (situacao == "EmAberto")
                {

                    sql.Append(@" AND ARQUIVADA = 0
                                  AND NOT EXISTS (SELECT TOP 1 1 
														FROM OCORRENCIAS.OCORRENCIAENCAMINHAMENTO E
														WHERE  O.OCORRENCIAID = E.OCORRENCIAID)
                                                ");
                }
                else if (situacao == "Arquivado")
                {

                    sql.Append(@" AND ARQUIVADA = 1
                                                ");
                }

                sql.Append(@" ORDER BY REGIONAL, MUNICIPIO, ESCOLA, DATAOCORRENCIA DESC");

                contextQuery.Command = sql.ToString();

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

        public DadosOcorrencia ObtemDadosOcorrenciaPor(int ocorrenciaId)
        {
            DadosOcorrencia dadosOcorrencia = new DadosOcorrencia();
            OcorrenciaArma rnOcorrenciaArma = new OcorrenciaArma();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                dadosOcorrencia = this.ObtemDadosOcorrenciaPor(contexto, ocorrenciaId);

                if (Convert.ToBoolean(dadosOcorrencia.UsoArma))
                {
                    //Busca armas
                    dadosOcorrencia.TiposArma = rnOcorrenciaArma.ObtemListaPor(contexto, dadosOcorrencia.OcorrenciaId);
                }

                return dadosOcorrencia;
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

        private DadosOcorrencia ObtemDadosOcorrenciaPor(DataContext contexto, int ocorrenciaId)
        {
            Pessoa rnPessoa = new Pessoa();
            DadosOcorrencia dadosOcorrencia = new DadosOcorrencia();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT O.OCORRENCIAID,
	                                            O.NUMEROOCORRENCIA,
	                                            O.CENSO,
	                                            R.REGIONAL,
	                                            UE.NOME_COMP AS ESCOLA,
	                                            M.NOME AS MUNICIPIO,
	                                            B.DESCRICAO AS BAIRRO,
	                                            O.DATAOCORRENCIA,
	                                            O.SUBCLASSEID,
												O.CLASSEID,
	                                            O.USOARMA,
	                                            O.RELATO, 
	                                            O.MEIOID,
	                                            O.DELEGACIAID,
	                                            O.BATALHAOID,
	                                            O.REGISTROOCORRENCIA,
	                                            O.OBSERVACAO,
	                                            O.INTERRUPCAO,
	                                            O.ARQUIVADA,
	                                            O.ATIVO, 
	                                            O.USUARIOID,
                                                O.MOTIVOCANCELAMENTOID,
                                                O.USUARIOCADASTRO
                                            FROM Ocorrencias.OCORRENCIA O (NOLOCK)
	                                            INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON O.CENSO = UE.UNIDADE_ENS
	                                            LEFT JOIN HADES..BAIRRO B (NOLOCK) ON UE.BAIRRO = B.CODIGO
	                                            INNER JOIN TCE_REGIONAL R (NOLOCK) ON UE.ID_REGIONAL = R.ID_REGIONAL
	                                            INNER JOIN HADES..HD_MUNICIPIO M (NOLOCK) ON UE.MUNICIPIO = M.MUNICIPIO
	                                            INNER JOIN [Ocorrencias].[CLASSE] c (NOLOCK) ON O.CLASSEID = c.CLASSEID
												LEFT JOIN [Ocorrencias].[SUBCLASSE] S (NOLOCK) ON O.SUBCLASSEID = S.SUBCLASSEID
                                            WHERE O.OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosOcorrencia.OcorrenciaId = Convert.ToInt32(reader["OCORRENCIAID"]);
                    dadosOcorrencia.Censo = Convert.ToString(reader["CENSO"]);
                    dadosOcorrencia.NumeroOcorrencia = Convert.ToString(reader["NUMEROOCORRENCIA"]);
                    dadosOcorrencia.Regional = Convert.ToString(reader["REGIONAL"]);
                    dadosOcorrencia.Escola = Convert.ToString(reader["ESCOLA"]);
                    dadosOcorrencia.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosOcorrencia.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosOcorrencia.DataOcorrencia = Convert.ToDateTime(reader["DATAOCORRENCIA"]);
                    dadosOcorrencia.ClasseId = Convert.ToInt32(reader["CLASSEID"]);

                    if (reader["SUBCLASSEID"] != DBNull.Value)
                    {
                        dadosOcorrencia.SubClasseId = Convert.ToInt32(reader["SUBCLASSEID"]);
                    }

                    dadosOcorrencia.UsoArma = Convert.ToBoolean(reader["USOARMA"]);
                    dadosOcorrencia.Interrupcao = Convert.ToBoolean(reader["INTERRUPCAO"]);
                    dadosOcorrencia.Relato = Convert.ToString(reader["RELATO"]);
                    dadosOcorrencia.MeioId = Convert.ToInt32(reader["MEIOID"]);
                    dadosOcorrencia.RegistroOcorrencia = Convert.ToString(reader["REGISTROOCORRENCIA"]);
                    dadosOcorrencia.Observacao = Convert.ToString(reader["OBSERVACAO"]);
                    dadosOcorrencia.Arquivada = Convert.ToBoolean(reader["ARQUIVADA"]);
                    dadosOcorrencia.Ativo = Convert.ToBoolean(reader["ATIVO"]);

                    if (reader["MOTIVOCANCELAMENTOID"] != DBNull.Value)
                    {
                        dadosOcorrencia.MotivoCancelamentoId = Convert.ToInt32(reader["MOTIVOCANCELAMENTOID"]);
                    }

                    dadosOcorrencia.UsuarioId = Convert.ToString(reader["USUARIOID"]);
                    dadosOcorrencia.UsuarioCadastro = Convert.ToString(reader["USUARIOCADASTRO"]);

                    if (reader["DELEGACIAID"] != DBNull.Value)
                    {
                        dadosOcorrencia.DelegaciaId = Convert.ToInt32(reader["DELEGACIAID"]);
                    }

                    if (reader["BATALHAOID"] != DBNull.Value)
                    {
                        dadosOcorrencia.BatalhaoId = Convert.ToInt32(reader["BATALHAOID"]);
                    }
                }

                return dadosOcorrencia;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados Valida(Entidades.Ocorrencia ocorrencia, List<int> tiposArma, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new OcorrenciaEncaminhamento();
            Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrencia == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (ocorrencia.OcorrenciaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }
            else
            {
                ocorrencia.Interrupcao = false;
                ocorrencia.Arquivada = false;
            }

            if (ocorrencia.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (ocorrencia.DataOcorrencia == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DA OCORRÊNCIA é obrigatório.");
            }
            else if (ocorrencia.DataOcorrencia.Date > DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA DA OCORRÊNCIA não pode ser maior que a data atual.");
            }
            else if (ocorrencia.DataOcorrencia.Date < DateTime.Now.Date.AddDays(-7) && !rnPerfil.PossuiPerfilAdministradorRVEPor(ocorrencia.UsuarioId) && !RN.Usuarios.UsuarioPrivilegiado(ocorrencia.UsuarioId))
            {
                mensagens.Add("Campo DATA DA OCORRÊNCIA não pode ultrapassar o limite de 7 dias.");
            }

            if (ocorrencia.ClasseId <= 0)
            {
                mensagens.Add("Campo CLASSE é obrigatório.");
            }

            if (ocorrencia.UsoArma == null)
            {
                mensagens.Add("Campo USO DE ARMA é obrigatório.");
            }
            else
            {
                if (Convert.ToBoolean(ocorrencia.UsoArma))
                {
                    if (tiposArma == null)
                    {
                        mensagens.Add("No campo TIPO ARMA é obrigatório a seleção de ao menos uma arma.");
                    }
                    else if (tiposArma.Count <= 0)
                    {
                        mensagens.Add("No campo TIPO ARMA é obrigatório a seleção de ao menos uma arma.");
                    }
                    else if (tiposArma.Count != tiposArma.Distinct().ToList().Count)
                    {
                        mensagens.Add("No campo TIPO ARMA não pode ter nenhum tipo repetido.");
                    }
                }
                else
                {
                    if (tiposArma != null)
                    {
                        if (tiposArma.Count > 0)
                        {
                            mensagens.Add("Não é possivel selecionar uma arma caso o campo uso arma não seja SIM.");
                        }
                    }
                }
            }

            if (ocorrencia.Relato.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO DO OCORRIDO é obrigatório.");
            }
            else if (ocorrencia.Relato.Length > 5000)
            {
                mensagens.Add("Campo DESCRIÇÃO DO OCORRIDO deve conter no máximo por 5000 caracteres.");
            }

            if (ocorrencia.MeioId <= 0)
            {
                mensagens.Add("Campo MEIO é obrigatório.");
            }

            if (!ocorrencia.Observacao.IsNullOrEmptyOrWhiteSpace() && ocorrencia.Observacao.Length > 5000)
            {
                mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo por 5000 caracteres.");
            }

            if (ocorrencia.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }
            else if (cadastro)
            {
                ocorrencia.UsuarioCadastro = ocorrencia.UsuarioCadastro;
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        //Buscar quantidade ocorrencia no ano
                        int ano = DateTime.Now.Year;
                        int quantidade = this.ObtemQuantidadeOcorrenciaPor(contexto, ano);

                        //Gerar numero ocorrencia
                        ocorrencia.NumeroOcorrencia = string.Format("{0}/{1}", Convert.ToString(quantidade + 1).PadLeft(8, '0'), Convert.ToString(ano));
                                               
                    }
                    else
                    {
                        //Verificar se a ocorrencia possui encaminhamento, caso seja alteracao
                        if (rnOcorrenciaEncaminhamento.PossuiEncaminhamentoPor(contexto, ocorrencia.OcorrenciaId))
                        {
                            //Caso exita encaminhamento verifica se o usuario tem perfil de adm
                            if (!rnUsuarios.EhPrivilegiado(contexto, ocorrencia.UsuarioId) && !rnPerfil.PossuiPerfilAdministradorRVEPor(contexto, ocorrencia.UsuarioId))
                            {
                                //verifica se o usuario tem perfil de adm
                                mensagens.Add("Este registro não pode ser alterado pois já existem encaminhamentos cadastrados, com isso apenas usuários com Perfil ADMINISTRADOR RVE podem alterar.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
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

        private int ObtemQuantidadeOcorrenciaPor(DataContext contexto, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QTDE
                                        FROM    Ocorrencias.OCORRENCIA
                                        WHERE   YEAR(DATACADASTRO) = @ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
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

        public void Insere(Entidades.Ocorrencia ocorrencia, List<int> tiposArma)
        {
            OcorrenciaArma rnOcorrenciaArma = new OcorrenciaArma();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere Ocorrencia
                this.Insere(contexto, ocorrencia);

                if (Convert.ToBoolean(ocorrencia.UsoArma))
                {
                    //Insere armas
                    foreach (int tipoArma in tiposArma)
                    {
                        rnOcorrenciaArma.Insere(contexto, ocorrencia.OcorrenciaId, tipoArma, ocorrencia.UsuarioId);
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

        private void Insere(DataContext contexto, Entidades.Ocorrencia ocorrencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Ocorrencias.OCORRENCIA
                                           (NUMEROOCORRENCIA
                                           ,CENSO
                                           ,DATAOCORRENCIA
                                           ,CLASSEID
                                           ,SUBCLASSEID                                           
                                           ,RELATO
                                           ,MEIOID
                                           ,DELEGACIAID
                                           ,BATALHAOID
                                           ,REGISTROOCORRENCIA
                                           ,USOARMA
                                           ,OBSERVACAO
                                           ,INTERRUPCAO
                                           ,ARQUIVADA
                                           ,ATIVO 
                                           ,USUARIOCADASTRO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@NUMEROOCORRENCIA,
                                           @CENSO, 
                                           @DATAOCORRENCIA, 
                                           @CLASSEID,
                                           @SUBCLASSEID,                                           
                                           @RELATO, 
                                           @MEIOID,
                                           @DELEGACIAID,
                                           @BATALHAOID,
                                           @REGISTROOCORRENCIA,
                                           @USOARMA,
                                           @OBSERVACAO,
                                           @INTERRUPCAO,
                                           @ARQUIVADA,
                                           @ATIVO, 
                                           @USUARIOCADASTRO,
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO )

                                    SELECT IDENT_CURRENT('Ocorrencias.OCORRENCIA') ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, ocorrencia.Censo);
            contextQuery.Parameters.Add("@NUMEROOCORRENCIA", SqlDbType.VarChar, ocorrencia.NumeroOcorrencia);
            contextQuery.Parameters.Add("@DATAOCORRENCIA", SqlDbType.DateTime, ocorrencia.DataOcorrencia);
            contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, ocorrencia.ClasseId);
            contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, ocorrencia.SubClasseId != null && Convert.ToInt32(ocorrencia.SubClasseId) > 0 ? (int?)ocorrencia.SubClasseId : (int?)null);
            contextQuery.Parameters.Add("@MEIOID", SqlDbType.Int, ocorrencia.MeioId);
            contextQuery.Parameters.Add("@RELATO", SqlDbType.VarChar, ocorrencia.Relato);
            contextQuery.Parameters.Add("@DELEGACIAID", SqlDbType.Int, ocorrencia.DelegaciaId != null && Convert.ToInt32(ocorrencia.DelegaciaId) > 0 ? (int?)ocorrencia.DelegaciaId : (int?)null);
            contextQuery.Parameters.Add("@BATALHAOID", SqlDbType.Int, ocorrencia.BatalhaoId != null && Convert.ToInt32(ocorrencia.BatalhaoId) > 0 ? (int?)ocorrencia.BatalhaoId : (int?)null);
            contextQuery.Parameters.Add("@REGISTROOCORRENCIA", SqlDbType.VarChar, ocorrencia.RegistroOcorrencia);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, ocorrencia.Observacao);
            contextQuery.Parameters.Add("@USOARMA", SqlDbType.Bit, Convert.ToBoolean(ocorrencia.UsoArma));
            contextQuery.Parameters.Add("@INTERRUPCAO", SqlDbType.Bit, Convert.ToBoolean(ocorrencia.Interrupcao));
            contextQuery.Parameters.Add("@ARQUIVADA", SqlDbType.Bit, Convert.ToBoolean(ocorrencia.Interrupcao));
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ocorrencia.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrencia.UsuarioId);
            contextQuery.Parameters.Add("@USUARIOCADASTRO", SqlDbType.VarChar, ocorrencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ocorrencia.OcorrenciaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.Ocorrencia ocorrencia, List<int> tiposArma)
        {
            OcorrenciaArma rnOcorrenciaArma = new OcorrenciaArma();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere Ocorrencia
                this.Atualiza(contexto, ocorrencia);

                //Remove armas anteriores
                rnOcorrenciaArma.RemoveTodosPor(contexto, ocorrencia.OcorrenciaId);

                if (Convert.ToBoolean(ocorrencia.UsoArma))
                {
                    //Insere armas
                    foreach (int tipoArma in tiposArma)
                    {
                        rnOcorrenciaArma.Insere(contexto, ocorrencia.OcorrenciaId, tipoArma, ocorrencia.UsuarioId);
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

        private void Atualiza(DataContext contexto, Entidades.Ocorrencia ocorrencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Ocorrencias.OCORRENCIA
                                       SET DATAOCORRENCIA = @DATAOCORRENCIA, 
                                          CLASSEID = @CLASSEID, 
                                          SUBCLASSEID = @SUBCLASSEID,                                          
                                          MEIOID = @MEIOID,
                                          RELATO = @RELATO, 
                                          DELEGACIAID = @DELEGACIAID, 
                                          BATALHAOID = @BATALHAOID, 
                                          REGISTROOCORRENCIA = @REGISTROOCORRENCIA, 
                                          USOARMA = @USOARMA,  
                                          OBSERVACAO = @OBSERVACAO,  
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE OCORRENCIAID = @OCORRENCIAID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrencia.OcorrenciaId);
            contextQuery.Parameters.Add("@DATAOCORRENCIA", SqlDbType.DateTime, ocorrencia.DataOcorrencia);
            contextQuery.Parameters.Add("@CLASSEID", SqlDbType.Int, ocorrencia.ClasseId);
            contextQuery.Parameters.Add("@SUBCLASSEID", SqlDbType.Int, ocorrencia.SubClasseId != null && Convert.ToInt32(ocorrencia.SubClasseId) > 0 ? (int?)ocorrencia.SubClasseId : (int?)null);
            contextQuery.Parameters.Add("@MEIOID", SqlDbType.Int, ocorrencia.MeioId);
            contextQuery.Parameters.Add("@RELATO", SqlDbType.VarChar, ocorrencia.Relato);
            contextQuery.Parameters.Add("@DELEGACIAID", SqlDbType.Int, ocorrencia.DelegaciaId != null && Convert.ToInt32(ocorrencia.DelegaciaId) > 0 ? (int?)ocorrencia.DelegaciaId : (int?)null);
            contextQuery.Parameters.Add("@BATALHAOID", SqlDbType.Int, ocorrencia.BatalhaoId != null && Convert.ToInt32(ocorrencia.BatalhaoId) > 0 ? (int?)ocorrencia.BatalhaoId : (int?)null);
            contextQuery.Parameters.Add("@REGISTROOCORRENCIA", SqlDbType.VarChar, ocorrencia.RegistroOcorrencia);
            contextQuery.Parameters.Add("@USOARMA", SqlDbType.Bit, Convert.ToBoolean(ocorrencia.UsoArma));
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, ocorrencia.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrencia.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contexto.ApplyModifications(contextQuery);
        }

        public void Desativa(int ocorrenciaId, string usuarioId, int motivoCancelamentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Ocorrencias.OCORRENCIA
                                       SET ATIVO = 0, 
                                          MOTIVOCANCELAMENTOID = @MOTIVOCANCELAMENTOID,
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO
                                     WHERE OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
                contextQuery.Parameters.Add("@MOTIVOCANCELAMENTOID", SqlDbType.Int, motivoCancelamentoId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaArquivamento(int ocorrenciaId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Ocorrencias.OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new OcorrenciaEncaminhamento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrenciaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja possui ao menos um encaminhamento
                    if (!rnOcorrenciaEncaminhamento.PossuiEncaminhamentoPor(contexto, ocorrenciaId))
                    {
                        mensagens.Add("O Registro de Violência Escolar não pode ser arquivado pois não possui nenhum encaminhamento.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AtualizaInterrupcao(DataContext ctx, int ocorrenciaId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Ocorrencias.OCORRENCIA
                                        SET    INTERRUPCAO = 1, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID
                                               AND INTERRUPCAO = 0 ";

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            ctx.ApplyModifications(contextQuery);
        }

        public void Arquiva(int ocorrenciaId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Ocorrencias.OCORRENCIA
                                        SET    ARQUIVADA = 1, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

                ctx.ApplyModifications(contextQuery);
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

        public bool PossuiArquivamentoPor(int ocorrenciaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiArquivamentoPor(ctx, ocorrenciaId);
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

        public bool PossuiArquivamentoPor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM Ocorrencias.OCORRENCIA
                                        WHERE OCORRENCIAID = @OCORRENCIAID 
                                        AND ARQUIVADA = 1";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }
    }
}