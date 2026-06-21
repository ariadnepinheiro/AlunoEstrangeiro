using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.InspecaoEscolar.DTOs;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class RespostaDependencia
    {
        private int Insere(DataContext contexto, Entidades.RespostaDependencia respostaDependencia)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" INSERT INTO InspecaoEscolar.RESPOSTADEPENDENCIA
                                               (CAMPANHAESCOLAID
                                               ,DEPENDENCIA
                                               ,FACULDADE
                                               ,PLACAIDENTIFICACAO
                                               ,IDENTIFICACAODEPENDENCIAID
                                              ,USUARIOID
                                                ,DATACADASTRO
                                                ,DATAALTERACAO) 
                                    VALUES      (@CAMPANHAESCOLAID
                                                ,@DEPENDENCIA
                                                ,@FACULDADE
                                                ,@PLACAIDENTIFICACAO
                                                ,@IDENTIFICACAODEPENDENCIAID
                                                ,@USUARIOID
                                                ,@DATACADASTRO
                                                ,@DATAALTERACAO)
                    
                    SELECT IDENT_CURRENT('InspecaoEscolar.RESPOSTADEPENDENCIA') ";

            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, respostaDependencia.CampanhaEscolaId);
            contextQuery.Parameters.Add("@DEPENDENCIA", SqlDbType.VarChar, respostaDependencia.Dependencia);
            contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, respostaDependencia.Faculdade);
            contextQuery.Parameters.Add("@PLACAIDENTIFICACAO", SqlDbType.Bit, respostaDependencia.PlacaIdentificacao);
            contextQuery.Parameters.Add("@IDENTIFICACAODEPENDENCIAID", SqlDbType.Int, respostaDependencia.IdentificacaoDependenciaId);         
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaDependencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            respostaDependencia.RespostaDependenciaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            return respostaDependencia.RespostaDependenciaId;
        }

        private void Atualiza(DataContext contexto, Entidades.RespostaDependencia respostaDependencia, bool salaAula, bool Banheiro)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" UPDATE InspecaoEscolar.RESPOSTADEPENDENCIA
                          SET USUARIOID = @USUARIOID, 
                              ");

            if (salaAula)
            {
                sql.Append(@" PLACAIDENTIFICACAO = @PLACAIDENTIFICACAO,
                              ");
            }

            if (Banheiro)
            {
                sql.Append(@" IDENTIFICACAODEPENDENCIAID = @IDENTIFICACAODEPENDENCIAID,
                              ");
            }

            sql.Append(@"     DATAALTERACAO = @DATAALTERACAO
                            WHERE RESPOSTADEPENDENCIAid = @RESPOSTADEPENDENCIAID ");

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependencia.RespostaDependenciaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaDependencia.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (salaAula)
            {
                contextQuery.Parameters.Add("@PLACAIDENTIFICACAO", SqlDbType.Bit, respostaDependencia.PlacaIdentificacao);
            }
            if (Banheiro)
            {
                contextQuery.Parameters.Add("@IDENTIFICACAODEPENDENCIAID", SqlDbType.Int, respostaDependencia.IdentificacaoDependenciaId);
            }            

            contexto.ApplyModifications(contextQuery);
        }

        public int RetornaQuantidadePor(DataContext contexto, int campanhaEscolaId, bool salaAula, bool Banheiro)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            StringBuilder sql = new StringBuilder();
            int retorno = 0;
            try
            {
                sql.Append(@" SELECT COUNT(*) AS QUANTIDADE
                                            FROM INSPECAOESCOLAR.RESPOSTADEPENDENCIA (NOLOCK)
                                            WHERE CAMPANHAESCOLAID = @CAMPANHAESCOLAID 
                                            ");

                if (salaAula)
                {
                    sql.Append(@" AND PLACAIDENTIFICACAO IS NOT NULL ");
                }

                if (Banheiro)
                {
                    sql.Append(@" AND IDENTIFICACAODEPENDENCIAID IS NOT NULL ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
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

        private void Remove(DataContext contexto, int respostaDependenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE InspecaoEscolar.RESPOSTADEPENDENCIA
                                     WHERE RESPOSTADEPENDENCIAID = @RESPOSTADEPENDENCIAID ";

            contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependenciaId);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListarRespostaDependencia(string faculdade, int campanhaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {//


                contextQuery.Command = @"    
                                        SELECT CE.CAMPANHAESCOLAID, 
                                               RD.RESPOSTADEPENDENCIAID, 
                                               RD.DEPENDENCIA,
                                               RD.PLACAIDENTIFICACAO,
                                               RD.FACULDADE,
                                               RD.IDENTIFICACAODEPENDENCIAID,
                                               RD.USUARIOID,
                                               RD.DATACADASTRO,
                                               RD.DATAALTERACAO
                                        FROM   INSPECAOESCOLAR.CAMPANHAESCOLA CE 
                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD 
                                                      ON ( CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID ) 
                                        WHERE  RD.FACULDADE = @FACULDADE 
                                               AND CE.CAMPANHAID = @campanhaId ";

                contextQuery.Parameters.Add("@campanhaId", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, faculdade);


                retorno = contexto.GetDataTable(contextQuery);
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

            return retorno;
        }

        public List<string> ListaSalasSemRespostaPor(DataContext contexto, int campanhaId, string censo)
        {
            List<string> lista = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT DEP.DEPENDENCIA										    
                                            FROM   LY_DEPENDENCIA DEP (NOLOCK) 
                                                    LEFT JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
                                                            ON DEP.FACULDADE = CE.UNIDADE_ENS 
                                                                AND CE.CAMPANHAID = @CAMPANHAID 
                                                    LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK) 
                                                            ON DEP.DEPENDENCIA = RD.DEPENDENCIA 
                                                                AND CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID 
                                            WHERE  CAD_SALA_AULA = 'S' 
                                                    AND ATIVA = 'S' 
                                                    AND TIPO_DEPEND = 'SALA' 
                                                    AND DEP.FACULDADE = @CENSO  
		                                            AND (RD.RESPOSTADEPENDENCIAID IS NULL or PLACAIDENTIFICACAO IS NULL)
                                            ORDER  BY DEP.DEPENDENCIA  ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string sala = Convert.ToString(reader["DEPENDENCIA"]);

                    lista.Add(sala);
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

        public List<string> ListaBanheirosSemRespostaPor(DataContext contexto, int campanhaId, string censo)
        {
            List<string> lista = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DEP.DEPENDENCIA 
                                        FROM   LY_DEPENDENCIA DEP (NOLOCK) 
                                               LEFT JOIN LY_TIPO_DEPENDENCIA TP ON DEP.TIPO_DEPEND=TP.TIPO_DEPEND
                                               LEFT JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
                                                      ON DEP.FACULDADE = CE.UNIDADE_ENS 
                                                         AND CE.CAMPANHAID = @CAMPANHAID 
                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK) 
                                                      ON DEP.DEPENDENCIA = RD.DEPENDENCIA 
                                                         AND CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID 
                                        WHERE  ATIVA = 'S' 
                                               AND DEP.TIPO_DEPEND LIKE 'BANHEIRO%'                                               
                                               AND DEP.FACULDADE = @CENSO   
											  AND (RD.RESPOSTADEPENDENCIAID IS NULL or IDENTIFICACAODEPENDENCIAID IS NULL)
                                        ORDER  BY DEP.DEPENDENCIA   ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string sala = Convert.ToString(reader["DEPENDENCIA"]);

                    lista.Add(sala);
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

        private ICollection<Entidades.RespostaDependencia> ListaRespostaDependenciaPor(DataContext contexto, int campanhaEscolaId)
        {
            ICollection<Entidades.RespostaDependencia> lista = new List<Entidades.RespostaDependencia>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                          FROM [INSPECAOESCOLAR].[RESPOSTADEPENDENCIA]
                                          WHERE [CAMPANHAESCOLAID] = @CAMPANHAESCOLAID ";

            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

            lista = contexto.TryToBindEntities<Entidades.RespostaDependencia>(contextQuery);

            return lista;
        }

        public ValidacaoDados ValidaSalaAula(DadosCampanhaEscola dados)
        {
            return this.Valida(dados, true, false);
        }

        public ValidacaoDados ValidaBanheiro(DadosCampanhaEscola dados)
        {
            return this.Valida(dados, false, true);
        }

        private ValidacaoDados Valida(DadosCampanhaEscola dados, bool salaAula, bool banheiro)
        {
            List<string> mensagens = new List<string>();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados == null)
            {
                return validacaoDados;
            }

            if (dados.UNIDADE_ENS.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (dados.CAMPANHAID <= 0)
            {
                mensagens.Add("Campo CAMPANHA é obrigatório.");
            }

            if (dados.USUARIOID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (dados.RespostasDependencias.Count == 0)
            {
                mensagens.Add("Preencha ao menos um dos itens antes de salvar.");
            }

            foreach (DTOs.DadosCampanhaEscola.DadosRespostaDependencia resposta in dados.RespostasDependencias)
            {
                if (resposta.DEPENDENCIA.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo DEPENDENCIA é obrigatório.");
                }

                else
                {
                    if (salaAula)
                    {
                        if (resposta.PLACAIDENTIFICACAO == null)
                        {
                            mensagens.Add(string.Format("Campo PLACA DE IDENTIFICAÇÃO do item {0} é obrigatório.", resposta.DEPENDENCIA));
                        }

                        resposta.IDENTIFICACAODEPENDENCIAID = null;
                    }
                    else if (banheiro)
                    {
                        if (resposta.IDENTIFICACAODEPENDENCIAID == null || resposta.IDENTIFICACAODEPENDENCIAID <= 0)
                        {
                            mensagens.Add(string.Format("Campo IDENTIFICACAO DE DEPENDENCIA do item {0} é obrigatório.", resposta.DEPENDENCIA));
                        }
                        resposta.PLACAIDENTIFICACAO = null;
                    }

                    foreach (DTOs.DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao opcao in resposta.RespostasDependenciasOpcoes)
                    {
                        if (opcao.OPCOESASSUNTOID <= 0)
                        {
                            mensagens.Add(string.Format("Campo OPCOES ASSUNTO do item {0} é obrigatório.", resposta.DEPENDENCIA));
                        }

                        if (opcao.ACAODIRECAOID <= 0)
                        {
                            mensagens.Add(string.Format("Campo ACAO DIRECAO do item {0} é obrigatório.", resposta.DEPENDENCIA));
                        }
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a campanha ja foi finalizada pela escola
                    if (rnCampanhaEscola.EhCampanhaEscolaFinalizadaPor(contexto, dados.CAMPANHAID, dados.UNIDADE_ENS))
                    {
                        mensagens.Add("Esta CAMPANHA / ESCOLA já foi finalizada.");
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
        
        public void SalvaBanheiro(DadosCampanhaEscola dados, bool? possuiAcervo)
        {
            this.Salva(dados, false, true, possuiAcervo);
        }

        public void SalvaSalaAula(DadosCampanhaEscola dados, bool? possuiAcervo)
        {
            this.Salva(dados, true, false, possuiAcervo);
        }

        private void Salva(DadosCampanhaEscola dados, bool salaAula, bool banheiro, bool? possuiAcervo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            CampanhaEscola rnCampanhaEscola = new CampanhaEscola();
            Entidades.RespostaDependencia respostaDependencia = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaDependencia();
            Entidades.RespostaDependenciaOpcao respostaDependenciaOpcao = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaDependenciaOpcao();
            Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
            RespostaDependenciaOpcao rnRespostaDependenciaOpcao = new RespostaDependenciaOpcao();
            ICollection<Entidades.RespostaDependencia> listaRespostaCadastradas = new List<Entidades.RespostaDependencia>();
            ICollection<Entidades.RespostaDependencia> listaRespostaCadastradas_filtradas = new List<Entidades.RespostaDependencia>();
            ICollection<Entidades.RespostaDependenciaOpcao> listaOpcoesCadastradas = new List<Entidades.RespostaDependenciaOpcao>();
            bool cadastro = false;

            try
            {
                //Verifica se existe campanha cadastrada para a escola               
                campanhaEscola = rnCampanhaEscola.ObtemPor(contexto, dados.CAMPANHAID, dados.UNIDADE_ENS);

                //Caso não exista insere
                if (campanhaEscola.CampanhaEscolaId <= 0)
                {
                    cadastro = true;

                    //Monta campanha 
                    campanhaEscola.CampanhaId = dados.CAMPANHAID;
                    campanhaEscola.Unidade_Ens = dados.UNIDADE_ENS;
                    campanhaEscola.Finalizado = false;
                    campanhaEscola.DataFinalizacao = null;
                    campanhaEscola.UsuarioId = dados.USUARIOID;
                    campanhaEscola.PossuiAcervo = possuiAcervo;

                    //Insere a campanha para a escola      
                    rnCampanhaEscola.Insere(contexto, campanhaEscola);
                }

                //Atualiza com id criado ou encontrado
                dados.CAMPANHAESCOLAID = campanhaEscola.CampanhaEscolaId;

                //Verifica se campanhaEscola ja existia
                if (!cadastro)
                {

                    //Busca respostas que já estavam cadastradas
                    listaRespostaCadastradas_filtradas = this.ListaRespostaDependenciaPor(contexto, campanhaEscola.CampanhaEscolaId);

                    //Busca respostas que já estavam cadastradas

                    if (banheiro)
                        //placa de indentificação é para banheiro
                        listaRespostaCadastradas = listaRespostaCadastradas_filtradas.Where(x => x.PlacaIdentificacao == null).ToList();
                    else
                        listaRespostaCadastradas = listaRespostaCadastradas_filtradas.Where(x => x.PlacaIdentificacao != null).ToList();
                        
                    
                    if (listaRespostaCadastradas.Count > 0)
                    {
                        foreach (Entidades.RespostaDependencia item in listaRespostaCadastradas)
                        {
                            //Verifica se a resposta foi deletada na alteração
                            if (dados.RespostasDependencias.Where(x => x.DEPENDENCIA == item.Dependencia).Count() <= 0)
                            {
                                //Remove todas as Opção
                                rnRespostaDependenciaOpcao.RemovePorDependencia(contexto, item.RespostaDependenciaId);

                                //Remove
                                this.Remove(contexto, item.RespostaDependenciaId);
                            }
                            else
                            {
                                //Busca opções que já estavam cadastradas
                                listaOpcoesCadastradas = rnRespostaDependenciaOpcao.ListaRespostaDependenciaOpcaoPor(contexto, item.RespostaDependenciaId);
                                
                                //Busca informaçoes da reposta na tela
                                DadosCampanhaEscola.DadosRespostaDependencia resposta = dados.RespostasDependencias.Where(x => x.DEPENDENCIA == item.Dependencia).FirstOrDefault();

                                foreach (Entidades.RespostaDependenciaOpcao opcao in listaOpcoesCadastradas)
                                {
                                    if (resposta.RespostasDependenciasOpcoes.Where(x => x.OPCOESASSUNTOID == opcao.OpcoesAssuntoId).Count() <= 0)
                                    {
                                        //Remove Opção
                                        rnRespostaDependenciaOpcao.Remove(contexto, opcao.RespostaDependenciaOpcaoId);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (InspecaoEscolar.DTOs.DadosCampanhaEscola.DadosRespostaDependencia resposta in dados.RespostasDependencias)
                {
                    //Monta resposta dependencia
                    respostaDependencia = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaDependencia();
                    respostaDependencia.CampanhaEscolaId = campanhaEscola.CampanhaEscolaId;
                    respostaDependencia.Dependencia = resposta.DEPENDENCIA;
                    respostaDependencia.Faculdade = dados.UNIDADE_ENS;
                    respostaDependencia.PlacaIdentificacao = resposta.PLACAIDENTIFICACAO;
                    respostaDependencia.IdentificacaoDependenciaId = resposta.IDENTIFICACAODEPENDENCIAID;
                    respostaDependencia.UsuarioId = dados.USUARIOID;

                    if (cadastro)
                    {
                        this.Insere(contexto, respostaDependencia);
                    }
                    else
                    {
                        //Verifica se já existe a resposta para a dependencia na escola
                        int? respostaDependenciaId = listaRespostaCadastradas.Where(x => x.Dependencia == resposta.DEPENDENCIA).Select(x => x.RespostaDependenciaId).FirstOrDefault();

                        if (respostaDependenciaId == null || respostaDependenciaId <= 0)
                        {
                            this.Insere(contexto, respostaDependencia);
                        }
                        else
                        {
                            respostaDependencia.RespostaDependenciaId = respostaDependenciaId.Value;
                            this.Atualiza(contexto, respostaDependencia, salaAula, banheiro);
                        }
                    }

                    foreach (DTOs.DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao opcao in resposta.RespostasDependenciasOpcoes)
                    {
                        //Monta resposta dependencia opcao
                        respostaDependenciaOpcao = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaDependenciaOpcao();
                        respostaDependenciaOpcao.RespostaDependenciaId = respostaDependencia.RespostaDependenciaId;
                        respostaDependenciaOpcao.OpcoesAssuntoId = opcao.OPCOESASSUNTOID;
                        respostaDependenciaOpcao.AcaoDirecaoId = opcao.ACAODIRECAOID;
                        respostaDependenciaOpcao.UsuarioId = dados.USUARIOID;

                        if (cadastro)
                        {
                            rnRespostaDependenciaOpcao.Insere(contexto, respostaDependenciaOpcao);
                        }
                        else
                        {
                            //Verifica se já existe a resposta para a dependencia na escola
                            int respostaDependenciaOpcaoId = rnRespostaDependenciaOpcao.RetornaRespostaDependenciaOpcaoIdPor(contexto, respostaDependencia.RespostaDependenciaId, opcao.OPCOESASSUNTOID);

                            if (respostaDependenciaOpcaoId == null || respostaDependenciaOpcaoId <= 0)
                            {
                                rnRespostaDependenciaOpcao.Insere(contexto, respostaDependenciaOpcao);
                            }
                            else
                            {
                                respostaDependenciaOpcao.RespostaDependenciaOpcaoId = respostaDependenciaOpcaoId;
                                rnRespostaDependenciaOpcao.Atualiza(contexto, respostaDependenciaOpcao);
                            }
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
        }
    }
}

