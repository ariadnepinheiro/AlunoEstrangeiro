using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class Acervo
    {
        public DataTable ListaPor(int campanhaEscolaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT [ACERVOID]
                                              ,[SITUACAO]
                                              ,[ATO]
                                              ,[VOLUME]
                                              ,A.[MEDIDAID]
	                                          ,M.DESCRICAO
                                              ,A.[INSTITUICAOID] 
	                                          ,I.NOME_COMP 
	                                          ,(A.[INSTITUICAOID]  + ' - ' + I.NOME_COMP) AS UNIDADE
                                              ,A.[CAMPANHAESCOLAID]     
                                              ,i.TIPO_ORIGEM
                                          FROM [LYCEUM].[InspecaoEscolar].[ACERVO] A
                                            INNER JOIN InspecaoEscolar.CAMPANHAESCOLA CE ON CE.CAMPANHAESCOLAID = A.CAMPANHAESCOLAID
                                            INNER JOIN LY_INSTITUICAO I ON I.OUTRA_FACULDADE = A.INSTITUICAOID
                                            INNER JOIN InspecaoEscolar.MEDIDA M ON M.MEDIDAID = A.MEDIDAID
                                           WHERE A.CAMPANHAESCOLAID = @CAMPANHAESCOLAID
                                                 ";

                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public bool PossuiAcervoPor(DataContext contexto, int medidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM InspecaoEscolar.ACERVO
                                    where MEDIDAID = @MEDIDAID ";

            contextQuery.Parameters.Add("@MEDIDAID", medidaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiAcervoPorCampanhaEscola(DataContext contexto, int campanhaEscolaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM InspecaoEscolar.ACERVO
                                    where CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", campanhaEscolaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.Acervo acervo, bool? possuiAcervo, int campanhaEscolaId, int campanhaId, string unidadeEnsino, string usuario, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanhaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA CAMPANHA é obrigatório.");
            }

            if (unidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (campanhaEscolaId <= 0)
                {
                    mensagens.Add("Campo CAMPANHA ESCOLA é obrigatório.");
                }
            }

            if (possuiAcervo != null)
            {
                if (possuiAcervo.Value)
                {
                    acervo.UsuarioId = usuario;

                    //Verifica se é alteração
                    if (!cadastro)
                    {
                        if (acervo.AcervoId <= 0)
                        {
                            mensagens.Add("Campo CÓDIGO é obrigatório.");
                        }
                    }

                    if (acervo.Situacao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo SITUAÇÃO é obrigatório.");
                    }

                    if (acervo.Ato.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo ATO é obrigatório.");
                    }
                    else if (acervo.Ato.Length > 100)
                    {
                        mensagens.Add("Campo ATO deve conter no máximo 100 caracteres.");
                    }

                    if (acervo.Volume <= 0)
                    {
                        mensagens.Add("Campo VOLUME é obrigatório.");
                    }

                    if (acervo.MedidaId <= 0)
                    {
                        mensagens.Add("Campo MEDIDA é obrigatório.");
                    }

                    if (mensagens.Count == 0)
                    {
                        try
                        {
                            contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                            //Verifica se a campanha ja foi finalizada pela escola
                            if (rnCampanhaEscola.EhCampanhaEscolaFinalizadaPor(contexto, campanhaId, unidadeEnsino))
                            {
                                mensagens.Add("Esta CAMPANHA / ESCOLA já foi finalizada.");
                            }

                            if (possuiAcervo != null)
                            {
                                //Verifica se informou que não possui acervo
                                if (!cadastro && !possuiAcervo.Value)
                                {
                                    //Verifica se possui acervos cadastrado
                                    if (this.PossuiAcervoPorCampanhaEscola(contexto, campanhaEscolaId))
                                    {
                                        mensagens.Add("Para o opção Possui Acervo ser modificada para não é necessario que todos os acervos sejam excluidos.");
                                    }
                                }

                                // Verifica se já existe a Descricao cadastrada 
                                if (Convert.ToBoolean(possuiAcervo) && this.PossuiOutroCadastroPor(contexto, acervo.Ato, acervo.MedidaId, acervo.InstituicaoId, acervo.CampanhaEscolaId, acervo.AcervoId))
                                {
                                    mensagens.Add("O acervo para esta unidade / ato / medida já foi cadastrado!");
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
                }
                else
                {
                    acervo = null;                    
                }
            }
            else
            {
                acervo = null;
                mensagens.Add("Para salvar é necessário responder se a unidade possui ou não acervo de unidade extinta/paralisada.");
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

        private bool PossuiOutroCadastroPor(DataContext ctx, string ato, int medidaId, string instituicaoId, int campanhaEscolaId, int acervoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM InspecaoEscolar.ACERVO
                                WHERE ACERVOID <> @ACERVOID 
									AND ATO = @ATO
									AND MEDIDAID = @MEDIDAID
									AND INSTITUICAOID = @INSTITUICAOID
									AND CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

            contextQuery.Parameters.Add("@ATO", ato);
            contextQuery.Parameters.Add("@MEDIDAID", medidaId);
            contextQuery.Parameters.Add("@INSTITUICAOID", instituicaoId);
            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", campanhaEscolaId);
            contextQuery.Parameters.Add("@ACERVOID", acervoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Acervo acervo, bool? possuiAcervo, int campanhaId, string unidadeEnsino, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();

            try
            { 
                
                //Verifica se existe campanha cadastrada para a escola               
                campanhaEscola = rnCampanhaEscola.ObtemPor(contexto, campanhaId, unidadeEnsino);

                if (acervo.CampanhaEscolaId <= 0)
                {
                    if (campanhaEscola.CampanhaEscolaId <= 0)
                    {
                        //Monta campanha 
                        campanhaEscola.CampanhaId = campanhaId;
                        campanhaEscola.Unidade_Ens = unidadeEnsino;
                        campanhaEscola.Finalizado = false;
                        campanhaEscola.DataFinalizacao = null;
                        campanhaEscola.UsuarioId = acervo.UsuarioId;
                        campanhaEscola.PossuiAcervo = possuiAcervo;

                        //Insere a campanha para a escola      
                        rnCampanhaEscola.Insere(contexto, campanhaEscola);
                        acervo.CampanhaEscolaId = campanhaEscola.CampanhaEscolaId;
                    }
                }
                else
                {
                    if (possuiAcervo != null)
                    {
                        rnCampanhaEscola.AtualizaPossuiAcervo(contexto, campanhaEscola.CampanhaEscolaId, possuiAcervo.Value, acervo.UsuarioId);
                    }
                }

                if (possuiAcervo != null)
                {
                    //Verifica se possui acervo
                    if (possuiAcervo.Value)
                    {
                        //Insere o acervo
                        this.Insere(contexto, acervo, campanhaId, unidadeEnsino);
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

        private void Insere(DataContext contexto, Entidades.Acervo acervo, int campanhaId, string unidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO InspecaoEscolar.ACERVO
                                                 (
                                                    [SITUACAO]
                                                   ,[ATO]
                                                   ,[VOLUME]
                                                   ,[MEDIDAID]
                                                   ,[INSTITUICAOID]
                                                   ,[CAMPANHAESCOLAID]
                                                   ,[USUARIOID]
                                                   ,[DATACADASTRO]
                                                   ,[DATAALTERACAO])
                                    VALUES      (@SITUACAO
                                                   ,@ATO
                                                   ,@VOLUME
                                                   ,@MEDIDAID
                                                   ,@INSTITUICAOID
                                                   ,@CAMPANHAESCOLAID
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO) ";


            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, acervo.Situacao);
            contextQuery.Parameters.Add("@ATO", SqlDbType.VarChar, acervo.Ato);
            contextQuery.Parameters.Add("@VOLUME", SqlDbType.Int, acervo.Volume);
            contextQuery.Parameters.Add("@MEDIDAID", SqlDbType.Int, acervo.MedidaId);
            contextQuery.Parameters.Add("@INSTITUICAOID", SqlDbType.VarChar, acervo.InstituicaoId);
            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, acervo.CampanhaEscolaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, acervo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(Entidades.Acervo acervo, bool? possuiAcervo, int campanhaEscolaId, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();

            try
            {
                rnCampanhaEscola.AtualizaPossuiAcervo(contexto, campanhaEscolaId, possuiAcervo.Value, usuario);

                if (possuiAcervo != null)
                {
                    //Verifica se possui acervo
                    if (possuiAcervo.Value)
                    {
                        this.Atualiza(contexto, acervo);
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

        private void Atualiza(DataContext contexto, Entidades.Acervo acervo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE InspecaoEscolar.ACERVO
                                        SET    
                                            SITUACAO = @SITUACAO,
                                            ATO = @ATO,
                                            VOLUME = @VOLUME,
                                            MEDIDAID = @MEDIDAID,                    
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  ACERVOID = @ACERVOID  ";

            contextQuery.Parameters.Add("@ACERVOID", SqlDbType.Int, acervo.AcervoId);
            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, acervo.Situacao);
            contextQuery.Parameters.Add("@ATO", SqlDbType.VarChar, acervo.Ato);
            contextQuery.Parameters.Add("@VOLUME", SqlDbType.Int, acervo.Volume);
            contextQuery.Parameters.Add("@MEDIDAID", SqlDbType.Int, acervo.MedidaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, acervo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int acervoId, string usuario)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (acervoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
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

        public void Remove(int acervoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE InspecaoEscolar.ACERVO
                            WHERE  ACERVOID = @ACERVOID  ";

                contextQuery.Parameters.Add("@ACERVOID", SqlDbType.Int, acervoId);

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
    }
}
