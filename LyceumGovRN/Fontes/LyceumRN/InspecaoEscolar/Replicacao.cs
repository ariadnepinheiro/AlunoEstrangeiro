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
    public class Replicacao
    {
        public bool verificaSeCampanhaExiste( DataContext contexto, Entidades.Campanha campanhaDados)
        {
            ContextQuery ctxCampanha = new ContextQuery();
            bool achouCampanha = false;

            ctxCampanha.Command = @" select count(*)
                                      from InspecaoEscolar.CAMPANHA c (nolock)
                                      where c.ANO = @ano 
                                        and c.SEMESTRE = @semestre 
                                        and c.OBJETIVO = @objetivo 
                                        and c.PROCEDIMENTO = @procedimento
                                        and c.TITULO = @titulo 
                                        and c.CAMPANHAID <> @campanhaID ";

            ctxCampanha.Parameters.Add("@ano", campanhaDados.Ano);
            ctxCampanha.Parameters.Add("@semestre", campanhaDados.Semestre);
            ctxCampanha.Parameters.Add("@objetivo", campanhaDados.Objetivo);
            ctxCampanha.Parameters.Add("@procedimento", campanhaDados.Procedimento);
            ctxCampanha.Parameters.Add("@titulo", campanhaDados.Titulo);
            ctxCampanha.Parameters.Add("@campanhaID", campanhaDados.CampanhaId);

            if (contexto.GetReturnValue<int>(ctxCampanha) > 0) 
            {
                achouCampanha = true;
            }
            return achouCampanha;
        }
        
        public bool verificaSeExisteGrupo(DataContext contexto, int campanhaIdOrigem)
        {
            ContextQuery ctxGrupo = new ContextQuery();
            bool achouGrupo = false;

            ctxGrupo.Command = @" select count(*) from InspecaoEscolar.Grupo g (nolock)
                                     where g.campanhaid = @campanhaId ";

            ctxGrupo.Parameters.Add("@campanhaId", campanhaIdOrigem);

            if (contexto.GetReturnValue<int>(ctxGrupo) > 0)
            {
                achouGrupo = true;
            }
            return achouGrupo;
        }
        
        public bool verificaSeExisteAssunto(DataContext contexto, int campanhaIdOrigem)
        {
            ContextQuery ctxAssunto = new ContextQuery();
            bool achouAssunto = false;

            ctxAssunto.Command = @" select count(*) from InspecaoEscolar.Grupo g (nolock)
	                                 INNER JOIN INSPECAOESCOLAR.ASSUNTO A (nolock) ON (G.GRUPOID = A.GRUPOID)
                                     where g.campanhaid = @campanhaId ";

            ctxAssunto.Parameters.Add("@campanhaId", campanhaIdOrigem);

            if (contexto.GetReturnValue<int>(ctxAssunto) > 0)
            {
                achouAssunto = true;
            }
            return achouAssunto;
        }
        
        public bool verificaSeExisteOpcoesAssunto(DataContext contexto, int campanhaIdOrigem)
        {
            ContextQuery ctxOpAssunto = new ContextQuery();
            bool achouopAssunto = false;

            ctxOpAssunto.Command = @" select count(*)	
                                      from InspecaoEscolar.OPCOESASSUNTO op
	                                    left join InspecaoEscolar.ASSUNTO a on(op.ASSUNTOID=a.ASSUNTOID)
	                                    left join InspecaoEscolar.GRUPO g on(g.GRUPOID=a.GRUPOID)
	                                    left join InspecaoEscolar.CAMPANHA c on(c.CAMPANHAID=g.CAMPANHAID)
                                      where c.CAMPANHAID=@campanhaId ";

            ctxOpAssunto.Parameters.Add("@campanhaId", campanhaIdOrigem);

            if (contexto.GetReturnValue<int>(ctxOpAssunto) > 0)
            {
                achouopAssunto = true;
            }
            return achouopAssunto;
        }

        public ValidacaoDados ValidaReplicacao(Entidades.Campanha campanhaDados, int campanhaIdOrigem)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanhaDados == null)
                return validacaoDados;
            
            if( campanhaIdOrigem == -1)
            {
                mensagens.Add("Campo CAMPANHA DE ORIGEM é obrigatório");
            }

            if (campanhaDados.Ano == -1)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (campanhaDados.Semestre == -1)
            {
                mensagens.Add("Campo SEMESTRE é obrigatório.");
            }

            if (campanhaDados.Titulo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TÍTULO é obrigatório.");
            }

            if (campanhaDados.Objetivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo OBJETIVO é obrigatório.");
            }

            if (campanhaDados.Procedimento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROCEDIMENTO é obrigatório.");
            }            

            if (campanhaDados.ExibeInspecaoEscolar == null)
            {
                mensagens.Add("Campo EXIBE ABA INSPEÇÃO ESCOLA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.verificaSeCampanhaExiste(contexto, campanhaDados))
                        mensagens.Add("Já existe uma campanha cadastrada com estas mesmas informações de ANO, SEMESTRE, OBJETIVO, PROCEDIMENTO e TÍTULO    ...");                                      

                    if (!this.verificaSeExisteGrupo(contexto, campanhaIdOrigem))
                        mensagens.Add("A campanha de origem selecionada não possui grupos associados a ela ...");

                    if (!this.verificaSeExisteAssunto(contexto, campanhaIdOrigem))
                        mensagens.Add("A campanha de origem selecionada não possui assuntos(perguntas) associadas a ela ...");

                    if (!this.verificaSeExisteOpcoesAssunto(contexto, campanhaIdOrigem))
                        mensagens.Add("A campanha de origem selecionada não possui opções de assuntos(respostas) associadas a ela ...");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool replicar(Entidades.Campanha dadosCampanha, int campanhaOrigem)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            
            RN.InspecaoEscolar.Campanha rnCampanha = new Campanha();
            RN.InspecaoEscolar.Grupo rnGrupo = new Grupo();
            RN.InspecaoEscolar.Assunto rnAssunto = new Assunto();
            RN.InspecaoEscolar.OpcoesAssunto rnOpAssunto = new OpcoesAssunto();

            ICollection<RN.InspecaoEscolar.Entidades.Grupo> grupos = new List<RN.InspecaoEscolar.Entidades.Grupo>();
            ICollection<RN.InspecaoEscolar.Entidades.Assunto> assuntos = new List<RN.InspecaoEscolar.Entidades.Assunto>();
            ICollection<RN.InspecaoEscolar.Entidades.OpcoesAssunto> opAssuntos = new List<RN.InspecaoEscolar.Entidades.OpcoesAssunto>();

            bool retorno = false;
            
            try
            {
                grupos = rnGrupo.ObtemPor(contexto, campanhaOrigem);
                assuntos = rnAssunto.ObtemPor(contexto, campanhaOrigem);
                opAssuntos = rnOpAssunto.ObtemPor(contexto, campanhaOrigem);

                retorno = rnCampanha.Insere(contexto, dadosCampanha);

                foreach (RN.InspecaoEscolar.Entidades.Grupo grupoOrigem in grupos)
                {
                    RN.InspecaoEscolar.Entidades.Grupo grupoDestino = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Grupo();
                    
                    grupoDestino.Ordem = grupoOrigem.Ordem;
                    grupoDestino.Descricao = grupoOrigem.Descricao;
                    grupoDestino.CampanhaId = dadosCampanha.CampanhaId;
                    grupoDestino.UsuarioId = dadosCampanha.UsuarioId;
                    
                    retorno = rnGrupo.Insere(contexto, grupoDestino);
                    var grupowhere = assuntos.Where(x => x.GrupoId == grupoOrigem.GrupoId).ToList();

                    foreach (RN.InspecaoEscolar.Entidades.Assunto assuntoOrigem in grupowhere)
                    {
                        RN.InspecaoEscolar.Entidades.Assunto assuntoDestino = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();
                        
                        assuntoDestino.Descricao = assuntoOrigem.Descricao;
                        assuntoDestino.AcaodeDirecao = assuntoOrigem.AcaodeDirecao;
                        assuntoDestino.Ordem = assuntoOrigem.Ordem;
                        assuntoDestino.TipoAssuntoId = assuntoOrigem.TipoAssuntoId;
                        assuntoDestino.AssuntoId = assuntoOrigem.AssuntoId;
                        assuntoDestino.GrupoId = grupoDestino.GrupoId;
                        assuntoDestino.UsuarioId = dadosCampanha.UsuarioId;
                        
                        if (assuntoOrigem.IdPaiAssuntoId == null)
                        {
                            assuntoDestino.IdPaiAssuntoId = null;
                        }
                        else
                        {
                            assuntoDestino.IdPaiAssuntoId = rnAssunto.RetornaIdPaiPor(contexto, grupoDestino.GrupoId, Convert.ToInt32(assuntoOrigem.IdPaiAssuntoId) );
                        }
                        
                        retorno = rnAssunto.Insere(contexto, assuntoDestino);

                        var opAssuntoWhere = opAssuntos.Where( x => x.AssuntoId == assuntoOrigem.AssuntoId ).ToList();
 
                        foreach (RN.InspecaoEscolar.Entidades.OpcoesAssunto opAssuntoOrigem in opAssuntoWhere)
                        {
                            RN.InspecaoEscolar.Entidades.OpcoesAssunto opAssuntoDestino = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.OpcoesAssunto();                            
                            
                            opAssuntoDestino.Descricao = opAssuntoOrigem.Descricao;
                            opAssuntoDestino.AcaodeDirecao = opAssuntoOrigem.AcaodeDirecao;
                            opAssuntoDestino.Ordem = opAssuntoOrigem.Ordem;
                            opAssuntoDestino.OpcoesAssuntoId = opAssuntoOrigem.OpcoesAssuntoId;
                            opAssuntoDestino.AssuntoId = assuntoDestino.AssuntoId;
                            opAssuntoDestino.UsuarioId = dadosCampanha.UsuarioId;

                            retorno = rnOpAssunto.Insere(contexto, opAssuntoDestino);
                        }

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
            return retorno;
        }
    }
}
