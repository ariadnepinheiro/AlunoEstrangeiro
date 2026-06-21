using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class RespostaAssunto
    {
        public List<InspecaoEscolar.Entidades.RespostaAssunto> ListaRespostaDemaisDependenciasPor(int campanhaId, string unidadeEnsino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaRespostaPor(contexto, campanhaId, unidadeEnsino, false, true).ToList();
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

        public List<InspecaoEscolar.Entidades.RespostaAssunto> ListaRespostaConsideracoesFinaisPor(int campanhaId, string unidadeEnsino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaRespostaPor(contexto, campanhaId, unidadeEnsino, true, false).ToList();
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

        private ICollection<InspecaoEscolar.Entidades.RespostaAssunto> ListaRespostaPor(DataContext contexto, int campanhaId, string unidadeEnsino, bool consideracoesFinais, bool demais)
        {
            return this.ListaRespostaPor(contexto, campanhaId, unidadeEnsino, consideracoesFinais, demais, null, null, null);
        }

        private ICollection<InspecaoEscolar.Entidades.RespostaAssunto> ListaRespostaPor(DataContext contexto, int campanhaId, string unidadeEnsino, bool consideracoesFinais, bool demais, int? ordemGrupo, int? ordemAssuntoInicio, int? ordemAssuntoFim)
        {
            ICollection<InspecaoEscolar.Entidades.RespostaAssunto> respostas = new List<InspecaoEscolar.Entidades.RespostaAssunto>();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT R.* 
                                        FROM   INSPECAOESCOLAR.RESPOSTAASSUNTO R (NOLOCK) 
                                               INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                       ON R.ASSUNTOID = A.ASSUNTOID 
                                               INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                                       ON A.GRUPOID = G.GRUPOID 
                                               INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
                                                       ON R.CAMPANHAESCOLAID = CE.CAMPANHAESCOLAID 
                                        WHERE  G.CAMPANHAID = @CAMPANHAID 
                                               AND CE.UNIDADE_ENS = @CENSO  
                                                ");

            if (consideracoesFinais)
            {
                sql.Append(@" AND A.TIPOASSUNTOID IN ( 8, 9, 10, 11 ) ");
            }
            if (demais)
            {
                sql.Append(@" AND A.TIPOASSUNTOID IN ( 2, 3, 4, 5 ) ");
            }

            if (ordemGrupo != null && ordemGrupo > 0)
            {
                sql.Append(@" AND G.ORDEM = @ORDEMGRUPO
                                         ");

                contextQuery.Parameters.Add("@ORDEMGRUPO", SqlDbType.Int, ordemGrupo);
            }

            if (ordemAssuntoInicio != null && ordemAssuntoInicio > 0)
            {
                sql.Append(@" AND A.ORDEM >= @ORDEMINICIO
                                         ");

                contextQuery.Parameters.Add("@ORDEMINICIO", SqlDbType.Int, ordemAssuntoInicio);
            }

            if (ordemAssuntoFim != null && ordemAssuntoFim > 0)
            {
                sql.Append(@" AND A.ORDEM <= @ORDEMFIM
                                         ");

                contextQuery.Parameters.Add("@ORDEMFIM", SqlDbType.Int, ordemAssuntoFim);
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, unidadeEnsino);

            respostas = contexto.TryToBindEntities<InspecaoEscolar.Entidades.RespostaAssunto>(contextQuery);

            return respostas;
        }

        public ValidacaoDados Valida(List<InspecaoEscolar.Entidades.RespostaAssunto> listaRespostas, int campanhaId, string unidadeEnsino)
        {
            List<string> mensagens = new List<string>();
            Entidades.Assunto assunto = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            Assunto rnAssunto = new Assunto();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (listaRespostas == null)
            {
                return validacaoDados;
            }

            if (unidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (campanhaId <= 0)
            {
                mensagens.Add("Campo CAMPANHA é obrigatório.");
            }

            if (listaRespostas.Count == 0)
            {
                mensagens.Add("Responda ao menos um dos itens antes de salvar.");
            }

            foreach (InspecaoEscolar.Entidades.RespostaAssunto resposta in listaRespostas)
            {
                if (resposta.AssuntoId <= 0)
                {
                    mensagens.Add("Campo ASSUNTO é obrigatório.");
                }

                if (resposta.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo USUARIO é obrigatório.");
                }
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
                    else
                    {
                        foreach (InspecaoEscolar.Entidades.RespostaAssunto resposta in listaRespostas)
                        {
                            //Busca informçoes do assunto
                            assunto = rnAssunto.ObtemAssuntoPor(contexto, resposta.AssuntoId);

                            //Verifica se a tela está passando apenas as perguntas que foram respondidas
                            if (resposta.Descricao.IsNullOrEmptyOrWhiteSpace() && (resposta.OpcoesAssuntoId == null || resposta.OpcoesAssuntoId <= 0))
                            {
                                mensagens.Add(string.Format("O item {0}-{1} está sem resposta.", assunto.AssuntoId.ToString(), assunto.Descricao));
                            }
                            else
                            {
                                switch (assunto.TipoAssuntoId)
                                {
                                    case 2:  //2	MULTIPLA ESCOLHA COM 1 OPÇÃO
                                    case 10: //10	CONSIDERAÇÕES FINAIS - MULTIPLA ESCOLHA COM 1 OPÇÃO
                                        {
                                            if (resposta.OpcoesAssuntoId == null && resposta.OpcoesAssuntoId <= 0)
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} a resposta não foi informada.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} não é permitido informar a resposta descritiva.", assunto.AssuntoId.ToString(), assunto.Descricao));

                                            }

                                            if (resposta.AcaoDirecaoId != null)
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} não é permitido informar ação e direção.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            break;
                                        }
                                    case 3: //3	MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
                                    case 9: //9	CONSIDERAÇÕES FINAIS - MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
                                        {
                                            if (resposta.OpcoesAssuntoId == null && resposta.OpcoesAssuntoId <= 0)
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} a resposta não foi informada.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} não é permitido informar a resposta descritiva.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            //Verifica se é necessario ação e direção
                                            if (assunto.AcaodeDirecao)
                                            {
                                                if (resposta.AcaoDirecaoId == null && resposta.AcaoDirecaoId <= 0)
                                                {
                                                    mensagens.Add(string.Format("Para o item {0}-{1} é necessario ação e direção.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                                }
                                            }
                                            else
                                            {
                                                if (resposta.AcaoDirecaoId != null)
                                                {
                                                    mensagens.Add(string.Format("Para o item {0}-{1} não é permitido informar ação e direção.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                                }
                                            }
                                            break;
                                        }
                                    case 4: //4	DESCRITIVA
                                    case 8: //8	CONSIDERAÇÕES FINAIS - DESCRITIVA
                                        {
                                            if (resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} a resposta não foi informada.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            if (resposta.Descricao.Length > 500)
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} a resposta, ultrapassou 500 caracteres.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            if ((resposta.OpcoesAssuntoId != null || resposta.AcaoDirecaoId != null))
                                            {
                                                mensagens.Add(string.Format("Para o item {0}-{1} apenas é permitido informar a descrição.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                            }

                                            //Verifica se tem alguma restrição do tipo de descrição
                                            if (assunto.Restricao > 0)
                                            {
                                                if (assunto.Restricao == 1)// Apenas número
                                                {
                                                   long resultado;
                                                    if (!long.TryParse(resposta.Descricao, out resultado))
                                                    {
                                                         mensagens.Add(string.Format("Para o item {0}-{1} a resposta deve ser composta por apenas números.", assunto.AssuntoId.ToString(), assunto.Descricao));
                                                       
                                                    }
                                                }
                                                else if (assunto.Restricao == 2)// Data
                                                {
                                                    DateTime dataResultado;
                                                    if (!DateTime.TryParse(resposta.Descricao, out dataResultado))
                                                    {
                                                        mensagens.Add(string.Format("Para o item {0}-{1} a resposta deve ser uma data válida.", assunto.AssuntoId.ToString(), assunto.Descricao));

                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    case 5:     //5	SEM RESPOSTA               
                                    case 6:     //6	DEPENDÊNCIAS - SALA DE AULA
                                    case 7:     //7	DEPENDÊNCIAS - BANHEIRO
                                    case 11:    //11 CONSIDERAÇÕES FINAIS - SEM RESPOSTA  
                                        {
                                            mensagens.Add(string.Format("Assuntos de Sala de Aula /Banheiro / Sem reposta não devem ser enviados."));
                                            break;
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Salva(List<InspecaoEscolar.Entidades.RespostaAssunto> listaRespostas, int campanhaId, string unidadeEnsino, bool? possuiAcervo)
        {
            this.Salva(listaRespostas, campanhaId, unidadeEnsino, null, null, null, possuiAcervo);
        }

        public void Salva(List<InspecaoEscolar.Entidades.RespostaAssunto> listaRespostas, int campanhaId, string unidadeEnsino, int? ordemGrupo, int? ordemAssuntoInicio, int? ordemAssuntoFim, bool? possuiAcervo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            //Entidades.Assunto assunto = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            //Assunto rnAssunto = new Assunto();
            Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
            //ICollection<Entidades.RespostaAssunto> listaRespostaCadastradas = new List<Entidades.RespostaAssunto>();
            //bool consideracoesFinais = false;
            //bool demais = true;
            try
            {
                //Verifica se existe campanha cadastrada para a escola               
                campanhaEscola = rnCampanhaEscola.ObtemPor(contexto, campanhaId, unidadeEnsino);

                if (campanhaEscola.CampanhaEscolaId > 0)
                {
                    //Deleta as respostas anteriores por assunto
                    foreach (int assuntoId in listaRespostas.Select(x => x.AssuntoId).Distinct())
                    {
                        //Deleta as respostas anteriores por assunto
                        this.RemovePorAssunto(contexto, assuntoId, campanhaEscola.CampanhaEscolaId);
                    }

                    //Insere respostas
                    foreach (InspecaoEscolar.Entidades.RespostaAssunto resposta in listaRespostas)
                    {
                        //Atualiza id da campanha e Insere
                        resposta.CampanhaEscolaId = campanhaEscola.CampanhaEscolaId;
                        this.Insere(contexto, resposta);
                    }

                    //FOI TROCADO PARA DELETAR TUDO E INSERIR
                    ////Busca id de um dos assuntos
                    //int idAssunto = listaRespostas.Select(x => x.AssuntoId).FirstOrDefault();

                    ////Busca informçoes do assunto
                    //assunto = rnAssunto.ObtemAssuntoPor(contexto, idAssunto);

                    ////Verifica se é de consideraçoes finais
                    //if (assunto.TipoAssuntoId == 8 || assunto.TipoAssuntoId == 9 || assunto.TipoAssuntoId == 10 || assunto.TipoAssuntoId == 11)
                    //{
                    //    consideracoesFinais = true;
                    //    demais = false;
                    //}


                    ////Busca respostas que já estão cadastradas na base
                    //listaRespostaCadastradas = this.ListaRespostaPor(contexto, campanhaId, unidadeEnsino, consideracoesFinais, demais, ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim);

                    //if (listaRespostaCadastradas.Count > 0)
                    //{
                    //    foreach (Entidades.RespostaAssunto item in listaRespostaCadastradas)
                    //    {
                    //        //Verifica se o assunto foi deletado na alteração
                    //        if (listaRespostas.Where(x => x.AssuntoId == item.AssuntoId).Count() <= 0)
                    //        {
                    //            //Remove
                    //            this.Remove(contexto, item.RespostaAssuntoId);
                    //        }
                    //    }
                    //}

                    //foreach (InspecaoEscolar.Entidades.RespostaAssunto resposta in listaRespostas)
                    //{

                    //    //Atualiza id da campanha
                    //    resposta.CampanhaEscolaId = campanhaEscola.CampanhaEscolaId;

                    //    int? respostaId = null;

                    //    //Busca informçoes do assunto
                    //    assunto = rnAssunto.ObtemAssuntoPor(contexto, resposta.AssuntoId);

                    //    /*
                    //    *** GAMBIARRA ***
                    //    Feita por Felipe R. Gomes em 29/05/2023
                    //    Deleta todas as respostas relacionadas a assuntos que são de múltipla escolha com N opções.
                    //    O código abaixo não está prevendo esse tipo de comportamento, então fiz esse pedaço antes de
                    //    o código abaixo rodar, para que siga fluxo normal como se fosse a primeira inserção das respostas.
                    //    */
                    //    if (assunto.TipoAssuntoId == 3)
                    //    {
                    //        var respostasASeremDeletadas = listaRespostaCadastradas.Where(q => q.AssuntoId == assunto.AssuntoId).ToList();
                    //        foreach (var rd in respostasASeremDeletadas)
                    //        {
                    //            this.Remove(contexto, rd.RespostaAssuntoId);
                    //            listaRespostaCadastradas.Remove(listaRespostaCadastradas.Single(q => q.RespostaAssuntoId == rd.RespostaAssuntoId));
                    //        }
                    //    }
                    //    /*
                    //    *** FIM DA GAMBIARRA *** 
                    //    */

                    //    //2	MULTIPLA ESCOLHA COM 1 OPÇÃO
                    //    if (assunto.TipoAssuntoId == 2 || assunto.TipoAssuntoId == 10)
                    //    {
                    //        respostaId = listaRespostaCadastradas.Where(x => x.AssuntoId == resposta.AssuntoId).Select(x => x.RespostaAssuntoId).FirstOrDefault();
                    //    }
                    //    else
                    //    {
                    //        respostaId = listaRespostaCadastradas.Where(x => x.AssuntoId == resposta.AssuntoId && x.OpcoesAssuntoId == resposta.OpcoesAssuntoId).Select(x => x.RespostaAssuntoId).FirstOrDefault();
                    //    }

                    //    if (respostaId == null || respostaId <= 0)
                    //    {
                    //        this.Insere(contexto, resposta);
                    //    }
                    //    else
                    //    {
                    //        resposta.RespostaAssuntoId = Convert.ToInt32(respostaId);
                    //        this.Atualiza(contexto, resposta);
                    //    }


                    //}
                }
                else
                {
                    //Monta campanha 
                    campanhaEscola.CampanhaId = campanhaId;
                    campanhaEscola.Unidade_Ens = unidadeEnsino;
                    campanhaEscola.Finalizado = false;
                    campanhaEscola.DataFinalizacao = null;
                    campanhaEscola.UsuarioId = listaRespostas.Select(x => x.UsuarioId).First();
                    campanhaEscola.PossuiAcervo = possuiAcervo;

                    //Insere a campanha para a escola      
                    rnCampanhaEscola.Insere(contexto, campanhaEscola);

                    foreach (InspecaoEscolar.Entidades.RespostaAssunto resposta in listaRespostas)
                    {
                        //Atualiza id da campanha e Insere
                        resposta.CampanhaEscolaId = campanhaEscola.CampanhaEscolaId;
                        this.Insere(contexto, resposta);
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

        private void Insere(DataContext contexto, InspecaoEscolar.Entidades.RespostaAssunto respostaAssunto)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO InspecaoEscolar.RESPOSTAASSUNTO
                                                   (ASSUNTOID
                                                   ,CAMPANHAESCOLAID
                                                   ,DESCRICAO
                                                   ,OPCOESASSUNTOID
                                                   ,ACAODIRECAOID
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@ASSUNTOID, 
                                                   @CAMPANHAESCOLAID, 
                                                   @DESCRICAO, 
                                                   @OPCOESASSUNTOID,
                                                   @ACAODIRECAOID, 
                                                   @USUARIOID,
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, respostaAssunto.AssuntoId);
            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, respostaAssunto.CampanhaEscolaId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, respostaAssunto.Descricao);
            contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, respostaAssunto.OpcoesAssuntoId);
            contextQuery.Parameters.Add("@ACAODIRECAOID", SqlDbType.Int, respostaAssunto.AcaoDirecaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaAssunto.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void Atualiza(DataContext contexto, InspecaoEscolar.Entidades.RespostaAssunto respostaAssunto)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" uPDATE InspecaoEscolar.RESPOSTAASSUNTO
                                             SET DESCRICAO = @DESCRICAO,
                                                 OPCOESASSUNTOID = @OPCOESASSUNTOID,
                                                 ACAODIRECAOID = @ACAODIRECAOID, 
                                                 USUARIOID = @USUARIOID,  
                                                 DATAALTERACAO = @DATAALTERACAO
                                             WHERE RESPOSTAASSUNTOID = @RESPOSTAASSUNTOID ";

            contextQuery.Parameters.Add("@RESPOSTAASSUNTOID", SqlDbType.Int, respostaAssunto.RespostaAssuntoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, respostaAssunto.Descricao);
            contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, respostaAssunto.OpcoesAssuntoId);
            contextQuery.Parameters.Add("@ACAODIRECAOID", SqlDbType.Int, respostaAssunto.AcaoDirecaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaAssunto.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext contexto, int respostaDependenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE [INSPECAOESCOLAR].[RESPOSTAASSUNTO]
                                     WHERE RESPOSTAASSUNTOID = @RESPOSTAASSUNTOID ";

            contextQuery.Parameters.Add("@RESPOSTAASSUNTOID", SqlDbType.Int, respostaDependenciaId);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemovePorAssunto(DataContext contexto, int assuntoId, int campanhaEscolaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE [INSPECAOESCOLAR].[RESPOSTAASSUNTO]
                                     WHERE ASSUNTOID = @ASSUNTOID
                                        AND CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);
            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
