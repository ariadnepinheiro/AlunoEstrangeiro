using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Etapa
    {
        public DataTable ListaSeries()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT SERIE 
                                         FROM AVALIACAOEXTERNA.ETAPA (NOLOCK) ";

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

        public Entidades.Etapa ObtemPor(int etapaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Etapa etapa = new Entidades.Etapa();          

            try
            {
                etapa = this.ObtemPor(contexto, etapaId);
                return etapa;
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

        public Entidades.Etapa ObtemPor(DataContext contexto, int etapaId)
        {
            Entidades.Etapa etapa = new Entidades.Etapa();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" select *
                                            from AvaliacaoExterna.ETAPA (NOLOCK)
                                            where ETAPAID = @ETAPAID ";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);

            etapa = contexto.TryToBindEntity<Entidades.Etapa>(contextQuery);

            return etapa;
        }

        public DataRow ObtemPorEtapaId(int etapaId)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT E.*, LYC.NOME as NOMECURSO 
                                            FROM AVALIACAOEXTERNA.ETAPA E 
                                            INNER JOIN LY_CURSO LYC 
		                                            ON LYC.CURSO = E.CURSO 
                                            WHERE ETAPAID = @ETAPAID ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        public DataTable ListaAtivoPor(int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT E.ETAPAID,
	                                           E.CURSO + ' - ' + LYC.NOME + ' - Série: ' + CONVERT(varchar, serie) AS DESCRICAO
                                        FROM AVALIACAOEXTERNA.ETAPA E (NOLOCK)
	                                        INNER JOIN LY_CURSO LYC  (NOLOCK)
                                                        ON LYC.CURSO = E.CURSO
                                        WHERE E.PROVAID = @PROVAID
											AND E.ATIVO = 1 ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

        public DataTable ListaAtivoPor(int provaId, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT E.ETAPAID,
	                                           E.CURSO + ' - ' + LYC.NOME + ' - SÉRIE: ' + CONVERT(VARCHAR, SERIE) AS DESCRICAO
                                        FROM AVALIACAOEXTERNA.ETAPA E (NOLOCK)
	                                        INNER JOIN LY_CURSO LYC  (NOLOCK)
                                                        ON LYC.CURSO = E.CURSO
											INNER JOIN LY_UNIDADE_ENSINO_CURSOS UC  (NOLOCK)
                                                        ON UC.CURSO = E.CURSO
                                        WHERE E.PROVAID = @PROVAID
										    AND UC.UNIDADE_ENS = @CENSO
											AND E.ATIVO = 1
										ORDER BY DESCRICAO ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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


        public DataTable ListaPorProva(int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT E.ETAPAID,
	                                           E.PROVAID,
	                                           E.CURSO,
	                                           E.CURSO + ' - ' + LYC.NOME AS CURSONOME,
	                                           E.SERIE,
	                                           P.DESCRICAO AS PROVA,
	                                           E.INICIOREALIZACAO,
	                                           E.FIMREALIZACAO,
	                                           E.INICIOTRANSCRICAO,
	                                           E.FIMTRANSCRICAO,
	                                           E.ATIVO
                                        FROM AVALIACAOEXTERNA.ETAPA E (NOLOCK)
	                                        INNER JOIN LY_CURSO LYC  (NOLOCK)
                                                        ON LYC.CURSO = E.CURSO
	                                        INNER JOIN AVALIACAOEXTERNA.PROVA P (NOLOCK)
				                                        ON E.PROVAID = P.PROVAID
                                        WHERE E.PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

        public void Insere(Entidades.Etapa etapa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO AvaliacaoExterna.ETAPA
                                               (PROVAID
                                               ,CURSO
                                               ,SERIE
                                               ,INICIOREALIZACAO
                                               ,FIMREALIZACAO
                                               ,INICIOTRANSCRICAO
                                               ,FIMTRANSCRICAO
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PROVAID, 
                                               @CURSO, 
                                               @SERIE, 
                                               @INICIOREALIZACAO, 
                                               @FIMREALIZACAO, 
                                               @INICIOTRANSCRICAO, 
                                               @FIMTRANSCRICAO,
                                               @ATIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)

                                            SELECT IDENT_CURRENT('AvaliacaoExterna.ETAPA') ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, etapa.ProvaId);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, etapa.Curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, etapa.Serie);
                contextQuery.Parameters.Add("@INICIOREALIZACAO", SqlDbType.Date, etapa.InicioRealizacao);
                contextQuery.Parameters.Add("@FIMREALIZACAO", SqlDbType.Date, etapa.FimRealizacao);
                contextQuery.Parameters.Add("@INICIOTRANSCRICAO", SqlDbType.Date, etapa.InicioTranscricao);
                contextQuery.Parameters.Add("@FIMTRANSCRICAO", SqlDbType.Date, etapa.FimTranscricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, etapa.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, etapa.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                etapa.EtapaId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.Etapa etapa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.ETAPA
                                           SET INICIOREALIZACAO = @INICIOREALIZACAO, 
                                              FIMREALIZACAO = @FIMREALIZACAO, 
                                              INICIOTRANSCRICAO = @INICIOTRANSCRICAO, 
                                              FIMTRANSCRICAO = @FIMTRANSCRICAO, 
                                              ATIVO = @ATIVO, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE ETAPAID = @ETAPAID ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapa.EtapaId);
                contextQuery.Parameters.Add("@INICIOREALIZACAO", SqlDbType.Date, etapa.InicioRealizacao);
                contextQuery.Parameters.Add("@FIMREALIZACAO", SqlDbType.Date, etapa.FimRealizacao);
                contextQuery.Parameters.Add("@INICIOTRANSCRICAO", SqlDbType.Date, etapa.InicioTranscricao);
                contextQuery.Parameters.Add("@FIMTRANSCRICAO", SqlDbType.Date, etapa.FimTranscricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, etapa.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, etapa.UsuarioID);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public bool PossuiEtapaPor(DataContext contexto, int etapaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"select count(0) 
                                    from AvaliacaoExterna.ETAPA e 
                                    where e.ETAPAID = @ETAPAID";

            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);

            return (contexto.GetReturnValue<int>(contextQuery) > 0);
        }

        public void Remove(int etapaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AvaliacaoExterna.ETAPA 
                                          WHERE ETAPAID = @ETAPAID ";

                contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);

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

        public ValidacaoDados Valida(Entidades.Etapa etapa, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();
            DataContext contexto = null;
            RN.Curso rnCurso = new Curso();
            RN.AvaliacaoExterna.TranscricaoTurma rnTranscricaoTurma = new TranscricaoTurma();
            RN.AvaliacaoExterna.ReaberturaTurma rnReaberturaTurma = new ReaberturaTurma();

            if (etapa == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (etapa.EtapaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (etapa.ProvaId <= 0)
            {
                mensagens.Add("Campo PROVA é obrigatório.");
            }

            //Verifica se o curso foi preenchido
            if (etapa.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            try
            {
                var data = new System.Data.SqlTypes.SqlDateTime(etapa.InicioRealizacao);
            }
            catch
            {
                mensagens.Add("A data de início da realização é inválida.");
            }

            //Verifica se a data de inicio da realização é maior ou igual ao dia do cadastro
            if (etapa.InicioRealizacao.Date < etapa.DataCadastro.Date)
            {
                mensagens.Add("A data de início da realização não pode ser menor do que o dia de cadastro.");
            }

            //Verifica se a data de fim da realização é válida
            try
            {
                var data = new System.Data.SqlTypes.SqlDateTime(etapa.FimRealizacao);
            }
            catch
            {
                mensagens.Add("A data de fim da realização é inválida.");
            }

            //Verifica se a data de fim da realização é maior ou igual ao dia de cadastro
            if (etapa.FimRealizacao.Date < etapa.DataCadastro.Date)
            {
                mensagens.Add("A data de fim da realização não pode ser menor do que o dia de cadastro.");
            }

            //Verifica se a data de fim da realização é maior ou igual a data de inicio da realização
            if (etapa.FimRealizacao.Date < etapa.InicioRealizacao.Date)
            {
                mensagens.Add("A data de fim da realização não pode ser menor do que o início da realização.");
            }

            //Verifica se a data de início da transcrição é válida
            try
            {
                var data = new System.Data.SqlTypes.SqlDateTime(etapa.InicioTranscricao);
            }
            catch
            {
                mensagens.Add("A data de início da transcrição é inválida.");
            }

            //Verifica se a data de inicio da transcrição é maior ou igual ao dia do cadastro
            if (etapa.InicioTranscricao.Date < etapa.DataCadastro.Date)
            {
                mensagens.Add("A data de início da transcrição não pode ser menor do que o dia de cadastro.");
            }

            //Verifica se a data de fim da transcrição é válida
            try
            {
                var data = new System.Data.SqlTypes.SqlDateTime(etapa.FimTranscricao);
            }
            catch
            {
                mensagens.Add("A data de fim da transcrição é inválida.");
            }

            //Verifica se a data de fim da transcrição é maior ou igual ao dia de cadastro
            if (etapa.FimTranscricao.Date < etapa.DataCadastro.Date)
            {
                mensagens.Add("A data de fim da transcrição não pode ser menor do que o dia de cadastro.");
            }

            //Verifica se a data de fim da transcrição é maior ou igual a data de inicio da transcrição
            if (etapa.FimTranscricao.Date < etapa.InicioTranscricao.Date)
            {
                mensagens.Add("A data de fim da transcrição não pode ser menor do que o início da transcrição.");
            }

            //Verifica se a data de inicio da transcrição é maior ou igual a data de inicio da realizacao
            if (etapa.InicioTranscricao.Date < etapa.InicioRealizacao.Date)
            {
                mensagens.Add("A data de início da transcrição não pode ser menor do que o início da realização.");
            }

            //Verificar se o id do usuário foi preenchido
            if (etapa.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o curso com a série já foi cadastrado para a prova
                    if (CursoESerieJaFoiUtilizado(contexto, etapa.ProvaId, etapa.EtapaId, etapa.Curso, etapa.Serie))
                    {
                        mensagens.Add("Este CURSO com esta SÉRIE já foi cadastrado para esta prova.");
                    }

                    //Verifica se a série existe dentro do curso especificado
                    if (rnCurso.SerieNaoExisteDentroDoCurso(contexto, etapa.Curso, etapa.Serie))
                    {
                        mensagens.Add("Esta SÉRIE não existe dentro do CURSO especificado.");
                    }

                    //verifica se é alteração
                    if (!cadastro)
                    {
                        //Verifica se tem turmas com transcriçoes associadas
                        if (rnTranscricaoTurma.PossuiEtapaPor(contexto, etapa.EtapaId))
                        {
                            //Busca dados atuais
                            var etapaBase = this.ObtemPor(contexto, etapa.EtapaId);                            

                            if (etapaBase.InicioRealizacao != etapa.InicioRealizacao || etapaBase.InicioTranscricao != etapa.InicioTranscricao)
                            {
                                mensagens.Add("As datas de inicio da REALIZAÇÃO / TRANSCRIÇÃO não podem ser alteradas pois existem turmas com transcrições de respostas associadas!");
                            }

                            //Apenas permitir alterar as datas final para datas maiores que o dia atual
                            if ((etapaBase.FimRealizacao != etapa.FimRealizacao && etapa.FimRealizacao.Date < DateTime.Now.Date)
                                || (etapaBase.FimTranscricao != etapa.FimTranscricao && etapa.FimTranscricao.Date < DateTime.Now.Date))
                            {
                                mensagens.Add("As datas de inicio da REALIZAÇÃO / TRANSCRIÇÃO não podem ser menores que a data atual pois existem turmas com transcrições de respostas associadas!");
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

        public ValidacaoDados ValidaRemocao(int etapaId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();
            DataContext contexto = null;
            RN.AvaliacaoExterna.TranscricaoTurma rnTranscricaoTurma = new TranscricaoTurma();
            RN.AvaliacaoExterna.ReaberturaTurma rnReaberturaTurma = new ReaberturaTurma();

            //Verifica se o ID foi fornecido
            if (etapaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se tem turmas com transcriçoes associadas
                    if (rnTranscricaoTurma.PossuiEtapaPor(contexto, etapaId))
                    {
                        mensagens.Add("Não pode ser excluída, pois existem turmas com transcrições de respostas associadas!");
                    }

                    //Verifica se tem turmas com solicitação de reabertura associadas
                    if (rnReaberturaTurma.PossuiEtapaPor(contexto, etapaId))
                    {
                        mensagens.Add("Não pode ser excluída, pois existem turmas com solicitação de reabertura associadas!");
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

        private bool CursoESerieJaFoiUtilizado(DataContext ctx, int provaId, int etapaId, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                FROM AvaliacaoExterna.ETAPA (NOLOCK)
                                WHERE  ETAPAID <> @ETAPAID
                                       AND PROVAID = @PROVAID
                                       AND CURSO = @CURSO
                                       AND SERIE = @SERIE ";

            contextQuery.Parameters.Clear();
            contextQuery.Parameters.Add("@ETAPAID", SqlDbType.Int, etapaId);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool PossuiProvaPor(DataContext contexto, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM AvaliacaoExterna.ETAPA (NOLOCK)
                                    WHERE PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}