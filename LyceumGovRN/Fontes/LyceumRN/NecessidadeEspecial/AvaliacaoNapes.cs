using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class AvaliacaoNapes : RNBase
    {
        public Entidades.AvaliacaoNapes ObtemPor(string aluno, int tipoRecursoId)
        {
            Entidades.AvaliacaoNapes avaliacaoNapes = new Entidades.AvaliacaoNapes();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                avaliacaoNapes = ObtemPor(contexto, aluno, tipoRecursoId);

                return avaliacaoNapes;
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

        public Entidades.AvaliacaoNapes ObtemPor(DataContext contexto, string aluno, int tipoRecursoId)
        {
            Entidades.AvaliacaoNapes avaliacaoNapes = new Entidades.AvaliacaoNapes();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                        FROM [NECESSIDADEESPECIAL].[AVALIACAONAPES] (NOLOCK)
                                        WHERE ALUNOID = @ALUNO
	                                        AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSO", SqlDbType.Int, tipoRecursoId);

                avaliacaoNapes = contexto.TryToBindEntity<Entidades.AvaliacaoNapes>(contextQuery);

                return avaliacaoNapes;
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
        }

        public DataTable ListaAlunoPor(string unidadeEnsino, bool? avaliado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT A.ALUNO, 
                                        P.NOME_COMPL, 
                                        N.DESCRICAO AS NECESSIDADE_ESPECIAL,
                                        CASE 
                                            WHEN AV.AVALIACAONAPESID IS NOT NULL THEN 'Avaliado' 
                                            ELSE 'Pendente' 
                                        END AS AVALIACAONAPES,
                                        AV.USUARIOID AS AVALIADOR 
                                FROM   LY_ALUNO A (NOLOCK) 
                                        INNER JOIN LY_PESSOA P (NOLOCK) 
                                                ON A.PESSOA = P.PESSOA           
                                        LEFT JOIN NECESSIDADEESPECIAL.AVALIACAONAPES AV (NOLOCK) 
                                                ON AV.ALUNOID = A.ALUNO 
                                        LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL N (NOLOCK) 
	                                            ON P.NECESSIDADEESPECIALID = N.NECESSIDADEESPECIALID AND N.ATIVO = 1 
                                WHERE  SIT_ALUNO = 'Ativo' 
                                        AND A.UNIDADE_ENSINO = @UNIDADEENSINO 
                                        AND P.NECESSIDADEESPECIALID <> 30
                                ");

                if (avaliado != null)
                {
                    if (Convert.ToBoolean(avaliado))
                    {
                        sql.Append(@" AND AV.AVALIACAONAPESID IS NOT NULL
                                    ");
                    }
                    else
                    {
                        sql.Append(@" AND AV.AVALIACAONAPESID IS NULL
                                    ");
                    }
                }
                sql.Append("ORDER BY P.NOME_COMPL ");


                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@UNIDADEENSINO", TechneDbType.T_CODIGO, unidadeEnsino);

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

        public ValidacaoDados Valida(List<Entidades.AvaliacaoNapes> listaAvaliacao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            TipoRecursoNecessidadeEspecial.TipoRecurso tipoRecurso = new TipoRecursoNecessidadeEspecial.TipoRecurso();
            TipoRecursoNecessidadeEspecial rnTipoRecursoNecessidadeEspecial = new TipoRecursoNecessidadeEspecial();
            NecessidadeEspecial rnNecessidadeEspecial = new NecessidadeEspecial();
            CuidadorAluno rnCuidadorAluno = new CuidadorAluno();
            LedorAluno rnLedorAluno = new LedorAluno();
            InterpreteTurma rnInterpreteTurma = new InterpreteTurma();
            DataTable listaTipoAtivo = new DataTable();
            DateTime[] datas = null;
            DateTime dataInicio = DateTime.MinValue;
            DateTime dataFim = DateTime.MinValue;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (listaAvaliacao == null)
            {
                return validacaoDados;
            }

            //Busca os tipos de Recursos ativos
            listaTipoAtivo = rnTipoRecursoNecessidadeEspecial.ListaTipoRecursoNecessidadeEspecialAtivo();

            //Verifica se todos os tipos foram avaliados
            foreach (DataRow tipo in listaTipoAtivo.Rows)
            {
                int tipoId = Convert.ToInt32(tipo["TIPORECURSONECESSIDADEESPECIALID"]);
                tipoRecurso = rnTipoRecursoNecessidadeEspecial.RetornaTipoRecursoPor(tipoId);

                if (listaAvaliacao.Where(x => x.TipoRecursoNecessidadeEspecialId == tipoId).Count() == 0)
                {
                    mensagens.Add(string.Format("A AVALIAÇÃO para {0} é obrigatória.", tipoRecurso.GetStringValue()));
                }
            }

            foreach (Entidades.AvaliacaoNapes avaliacao in listaAvaliacao)
            {
                if (avaliacao.TipoRecursoNecessidadeEspecialId <= 0)
                {
                    mensagens.Add("Campo TIPO é obrigatório.");
                }
                else
                {
                    //Busca qual o tipo de recurso da avaliacao
                    tipoRecurso = rnTipoRecursoNecessidadeEspecial.RetornaTipoRecursoPor(avaliacao.TipoRecursoNecessidadeEspecialId);

                    //Verifica se é alteração
                    if (!cadastro)
                    {
                        if (avaliacao.AvaliacaoNapesId <= 0)
                        {
                            mensagens.Add(string.Format("Campo CÓDIGO do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                        }
                    }

                    if (avaliacao.AlunoId.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("Campo ALUNO do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                    }

                    if (avaliacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("Campo USUARIO RESPONSÁVEL do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                    }                   

                    if (avaliacao.Justificativa.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("Campo JUSTIFICATIVA do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                    }
                    else if (avaliacao.Justificativa.Length > 200)
                    {
                        mensagens.Add(string.Format("O campo JUSTIFICATIVA do {0} deve ter no máximo de 200 caracteres.", tipoRecurso.GetStringValue()));
                    }

                    if (avaliacao.NecessitaRecurso == null)
                    {
                        mensagens.Add(string.Format("Campo NECESSITA RECURSO do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                    }
                    else
                    {
                        if (!Convert.ToBoolean(avaliacao.NecessitaRecurso))
                        {
                            avaliacao.Transitorio = false;
                            avaliacao.DataInicio = null;
                            avaliacao.DataFim = null;
                        }
                        else
                        {
                            if (avaliacao.Transitorio == null)
                            {
                                mensagens.Add(string.Format("Campo TIPO do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                            }
                            else
                            {
                                if (Convert.ToBoolean(avaliacao.Transitorio))
                                {
                                    //Verifica preenchimento datas se for transitorio
                                    if (avaliacao.DataInicio == null || avaliacao.DataInicio == DateTime.MinValue)
                                    {
                                        mensagens.Add(string.Format("Campo DATA INICIO do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                                    }

                                    if (avaliacao.DataFim == null || avaliacao.DataFim == DateTime.MinValue)
                                    {
                                        mensagens.Add(string.Format("Campo DATA FIM do {0} é obrigatório.", tipoRecurso.GetStringValue()));
                                    }
                                    else
                                    {
                                        if (avaliacao.DataInicio != null && avaliacao.DataInicio != DateTime.MinValue && avaliacao.DataInicio > avaliacao.DataFim)
                                        {
                                            mensagens.Add(string.Format("A DATA INICIO do {0} deve ser menor ou igual a DATA FIM.", tipoRecurso.GetStringValue()));
                                        }
                                    }
                                }
                                else
                                {
                                    avaliacao.DataInicio = null;
                                    avaliacao.DataFim = null;
                                }
                            }
                        }
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    foreach (Entidades.AvaliacaoNapes avaliacao in listaAvaliacao)
                    {
                        //Busca qual o tipo de recurso da avaliacao
                        tipoRecurso = rnTipoRecursoNecessidadeEspecial.RetornaTipoRecursoPor(avaliacao.TipoRecursoNecessidadeEspecialId);

                        if (tipoRecurso == RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador && Convert.ToBoolean(avaliacao.NecessitaRecurso))
                        {
                            //Verifica se aluno possui necessidade especial que necessite de Cuidador
                            if (!rnNecessidadeEspecial.NecessitaCuidadorPor(contexto, avaliacao.AlunoId))
                            {
                                mensagens.Add("A necessidade especial do aluno não pode ser associada a um Cuidador");
                            }
                        }
                        else if (tipoRecurso == RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor && Convert.ToBoolean(avaliacao.NecessitaRecurso))
                        {
                            //Verifica se aluno possui necessidade especial que necessite de Ledor
                            if (!rnNecessidadeEspecial.NecessitaLedorPor(contexto, avaliacao.AlunoId))
                            {
                                mensagens.Add("A necessidade especial do aluno não pode ser associada a um Ledor");
                            }
                        }
                        else if (tipoRecurso == RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete && Convert.ToBoolean(avaliacao.NecessitaRecurso))
                        {
                            //Verifica se aluno possui necessidade especial que necessite de Intérprete
                            if (!rnNecessidadeEspecial.NecessitaInterpretePor(contexto, avaliacao.AlunoId))
                            {
                                mensagens.Add("A necessidade especial do aluno não pode ser associada a um Intérprete");
                            }
                        }                        

                        //Verifica se eh cadastro
                        if (cadastro)
                        {
                            //verifica se já existe avaliação para aquele aluno / tipo  
                            if (this.PossuiAvaliacaoPor(contexto, avaliacao.AlunoId, avaliacao.TipoRecursoNecessidadeEspecialId))
                            {
                                mensagens.Add(string.Format("A necessidade de {0} deste aluno já foi avaliada.", tipoRecurso.GetStringValue()));
                            }
                        }
                        else
                        {
                            //Busca datas das associações do aluno
                            if (avaliacao.TipoRecursoNecessidadeEspecialId == (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador)
                            {
                                datas = rnCuidadorAluno.RetornaDataMinimaMaximaPor(contexto, avaliacao.AlunoId);
                            }
                            else if (avaliacao.TipoRecursoNecessidadeEspecialId == (int)TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor)
                            {
                                datas = rnLedorAluno.RetornaDataMinimaMaximaPor(contexto, avaliacao.AlunoId);
                            }

                            if (datas != null)
                            {
                                dataInicio = Convert.ToDateTime(datas[0]);
                                dataFim = Convert.ToDateTime(datas[1]);
                            }

                            //se for alteração e desmarcar opção verifica se possui associaçoes
                            if (!Convert.ToBoolean(avaliacao.NecessitaRecurso) && datas != null)
                            {
                                if (Convert.ToBoolean(avaliacao.Transitorio))
                                {
                                    if (dataFim.Date >= Convert.ToDateTime(avaliacao.DataInicio).Date)
                                    {
                                        mensagens.Add(string.Format("A avaliação não pode ser modificada para NÂO pois possui associação com {0}.", tipoRecurso.GetStringValue()));
                                    }
                                }
                                else
                                {
                                    if (dataFim.Date >= DateTime.Now.Date)
                                    {
                                        mensagens.Add(string.Format("A avaliação não pode ser modificada para NÂO pois possui associação com {0}.", tipoRecurso.GetStringValue()));
                                    }
                                }
                            }
                            else if (Convert.ToBoolean(avaliacao.Transitorio))
                            {
                                //Se for transitorio, verifica datas utilizadas nas associaçoes
                                if (dataInicio != DateTime.MinValue)
                                {
                                    if (Convert.ToDateTime(avaliacao.DataInicio).Date > dataInicio.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA INICIO da necessidade de {1} deve ser menor ou igual a data da associação mais antiga - {0}", dataInicio.ToString("dd/MM/yyyy"), tipoRecurso.GetStringValue()));
                                    }
                                }
                                if (dataFim != DateTime.MinValue)
                                {
                                    if (Convert.ToDateTime(avaliacao.DataFim).Date < dataFim.Date)
                                    {
                                        mensagens.Add(string.Format("A DATA FIM da necessidade de {1} deve ser maior ou igual a data da associação mais recente - {0}", dataFim.ToString("dd/MM/yyyy"), tipoRecurso.GetStringValue()));
                                    }
                                }
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool PossuiAvaliacaoPositivaPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  ALUNOID = @ALUNOID
                                                   AND NECESSITARECURSO = 1 ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

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

        public bool PossuiAvaliacaoNapes(DataContext ctx, string aluno, int tipoRecurso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  ALUNOID = @ALUNOID
                                                   AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecurso);

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

        public bool PossuiOutraAvaliacaoPositivaPor(DataContext ctx, string aluno, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  ALUNOID = @ALUNOID
	                                               AND NECESSITARECURSO = 1
	                                               AND TIPORECURSONECESSIDADEESPECIALID NOT IN (SELECT NT.TIPORECURSONECESSIDADEESPECIALID
						                                            FROM HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
							                                            WHERE NT.NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID ) ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, necessidadeEspecialId);

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

        public bool PossuiRecursoPor(string aluno, int tipoRecurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  NECESSITARECURSO = 1 
                                                   and ALUNOID = @ALUNOID 
                                                   and TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecurso);

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

        public bool PossuiAvaliacaoPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  ALUNOID = @ALUNOID ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);

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

        private bool PossuiAvaliacaoPor(DataContext ctx, string aluno, int tipoRecursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.AVALIACAONAPES (NOLOCK)
                                            WHERE  ALUNOID = @ALUNOID 
                                                   AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID  ";

                contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoId);

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

        public bool PossuiAvaliacaoPor(DataContext ctx, string turma, int ano, int periodo, int tipoRecursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                                    FROM   NECESSIDADEESPECIAL.AVALIACAONAPES AN (NOLOCK) 
                                                           INNER JOIN LY_MATRICULA M (NOLOCK) 
                                                                   ON AN.ALUNOID = M.ALUNO 
                                                    WHERE  TURMA = @TURMA 
                                                           AND ANO = @ANO 
                                                           AND SEMESTRE = @SEMESTRE
                                                           AND AN.TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoId);

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

        public bool PossuiAvaliacaoPositivaVigentePor(DataContext ctx, string aluno, int tipoRecursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                                    FROM NECESSIDADEESPECIAL.AVALIACAONAPES 
													WHERE NECESSITARECURSO = 1
														AND (TRANSITORIO = 0
															OR (TRANSITORIO = 1 AND GETDATE() BETWEEN DATAINICIO AND DATAFIM))
														AND ALUNOID = @ALUNO
														AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoId);

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

        public void Insere(List<Entidades.AvaliacaoNapes> listaAvaliacao)
        {
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();            
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remove as recursos anteriores do aluno
                string aluno = listaAvaliacao.Select(x => x.AlunoId).First();                
                rnAlunoRecursoNecessidadeEspecial.Remove(ctx, aluno);

                foreach (Entidades.AvaliacaoNapes avaliacao in listaAvaliacao)
                {
                    this.Insere(ctx, avaliacao);

                    if (Convert.ToBoolean(avaliacao.NecessitaRecurso))
                    {
                        //Insere AlunoRecursoNecessidadeEspecial. caso responda sim
                        rnAlunoRecursoNecessidadeEspecial.Insere(ctx, avaliacao.TipoRecursoNecessidadeEspecialId, avaliacao.AlunoId, avaliacao.UsuarioId);
                    }
                }
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

        private void Insere(DataContext ctx, Entidades.AvaliacaoNapes avaliacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO NecessidadeEspecial.AVALIACAONAPES 
                                                (ALUNOID, 
                                                 TIPORECURSONECESSIDADEESPECIALID, 
                                                 NECESSITARECURSO, 
                                                 JUSTIFICATIVA,
                                                 TRANSITORIO, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      ( @ALUNOID, 
                                                  @TIPORECURSONECESSIDADEESPECIALID, 
                                                  @NECESSITARECURSO, 
                                                  @JUSTIFICATIVA, 
                                                  @TRANSITORIO, 
                                                  @DATAINICIO, 
                                                  @DATAFIM, 
                                                  @USUARIOID, 
                                                  @DATACADASTRO, 
                                                  @DATAALTERACAO ) ";

            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, avaliacao.AlunoId);
            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, avaliacao.TipoRecursoNecessidadeEspecialId);
            contextQuery.Parameters.Add("@NECESSITARECURSO", SqlDbType.Bit, avaliacao.NecessitaRecurso);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, avaliacao.Justificativa);
            contextQuery.Parameters.Add("@TRANSITORIO", SqlDbType.Bit, avaliacao.Transitorio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (avaliacao.DataInicio == null || avaliacao.DataInicio == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, Convert.ToDateTime(avaliacao.DataInicio).Date);
            }

            if (avaliacao.DataFim == null || avaliacao.DataFim == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, Convert.ToDateTime(avaliacao.DataFim).Date);
            }

            ctx.ApplyModifications(contextQuery);
        }

        public void Atualiza(List<Entidades.AvaliacaoNapes> listaAvaliacao)
        {
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remove as recursos anteriores do aluno
                string aluno = listaAvaliacao.Select(x => x.AlunoId).First();
                rnAlunoRecursoNecessidadeEspecial.Remove(ctx, aluno);

                foreach (Entidades.AvaliacaoNapes avaliacao in listaAvaliacao)
                {
                    if (PossuiAvaliacaoNapes(ctx, avaliacao.AlunoId, avaliacao.TipoRecursoNecessidadeEspecialId))
                    {
                        this.Atualiza(ctx, avaliacao);
                    }
                    else
                    {
                        this.Insere(ctx, avaliacao);
                    }

                    //Insere AlunoRecursoNecessidadeEspecial
                    rnAlunoRecursoNecessidadeEspecial.Insere(ctx, avaliacao.TipoRecursoNecessidadeEspecialId, avaliacao.AlunoId, avaliacao.UsuarioId);
                }
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

        private void Atualiza(DataContext ctx, Entidades.AvaliacaoNapes avaliacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE NecessidadEespecial.AVALIACAONAPES 
                            SET    TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID, 
                                   NECESSITARECURSO = @NECESSITARECURSO, 
                                   JUSTIFICATIVA = @JUSTIFICATIVA, 
                                   TRANSITORIO = @TRANSITORIO, 
                                   DATAINICIO = @DATAINICIO, 
                                   DATAFIM = @DATAFIM, 
                                   USUARIOID = @USUARIOID, 
                                   DATAALTERACAO = @DATAALTERACAO 
                            WHERE  AVALIACAONAPESID = @AVALIACAONAPESID ";

            contextQuery.Parameters.Add("@ALUNOID", SqlDbType.VarChar, avaliacao.AlunoId);
            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, avaliacao.TipoRecursoNecessidadeEspecialId);
            contextQuery.Parameters.Add("@NECESSITARECURSO", SqlDbType.Bit, avaliacao.NecessitaRecurso);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, avaliacao.Justificativa);
            contextQuery.Parameters.Add("@TRANSITORIO", SqlDbType.Bit, avaliacao.Transitorio);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioId);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@AVALIACAONAPESID", SqlDbType.Int, avaliacao.AvaliacaoNapesId);

            if (avaliacao.DataInicio == null || avaliacao.DataInicio == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, Convert.ToDateTime(avaliacao.DataInicio).Date);
            }

            if (avaliacao.DataFim == null || avaliacao.DataFim == DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, Convert.ToDateTime(avaliacao.DataFim).Date);
            }

            ctx.ApplyModifications(contextQuery);
        }

        public DTOs.DadosAvaliacaoNapes ObtemDadosAvaliacaoPor(string aluno, int tipoRecursoId)
        {
            DTOs.DadosAvaliacaoNapes dados = new DTOs.DadosAvaliacaoNapes();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT AVALIACAONAPESID, 
                                               ALUNOID, 
                                               TIPORECURSONECESSIDADEESPECIALID, 
                                               NECESSITARECURSO, 
                                               JUSTIFICATIVA,
                                               TRANSITORIO, 
                                               DATAINICIO, 
                                               DATAFIM, 
                                               USUARIOID, 
                                               DATACADASTRO, 
                                               DATAALTERACAO, 
                                               U.NOME 
                                        FROM   NECESSIDADEESPECIAL.AVALIACAONAPES A (NOLOCK) 
                                               INNER JOIN HADES..HD_USUARIO U (NOLOCK) 
                                                       ON A.USUARIOID = U.USUARIO 
                                        WHERE  ALUNOID = @ALUNO 
                                               AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSO  ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPORECURSO", SqlDbType.Int, tipoRecursoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.AvaliacaoNapesId = Convert.ToInt32(reader["AVALIACAONAPESID"]);
                    dados.AlunoId = Convert.ToString(reader["ALUNOID"]);
                    dados.TipoRecursoNecessidadeEspecialId = Convert.ToInt32(reader["TIPORECURSONECESSIDADEESPECIALID"]);
                    dados.NecessitaRecurso = Convert.ToBoolean(reader["NECESSITARECURSO"]);
                    dados.Justificativa = Convert.ToString(reader["JUSTIFICATIVA"]);

                    dados.Transitorio = Convert.ToBoolean(reader["TRANSITORIO"]);
                    if (reader["DATAINICIO"] != DBNull.Value)
                    {
                        dados.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    }
                    if (reader["DATAFIM"] != DBNull.Value)
                    {
                        dados.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    }
                    dados.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    dados.UsuarioId = Convert.ToString(reader["USUARIOID"]);
                    dados.NomeUsuario = Convert.ToString(reader["NOME"]);

                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        dados.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                }

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }
    }
}