using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class PontoEmbarque
    {
        public DataTable ListaPor(int rotaTrajetoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  PONTOEMBARQUEID, 
                                            PE.ROTATRAJETOID, 
                                            RT.IDA,
                                            PRIMEIRO, 
                                            PE.CEP, 
                                            LOGRADOURO, 
                                            NUMERO, 
                                            BAIRRO, 
                                            PE.MUNICIPIO, 
                                            M.NOME AS DESCRICAOMUNICIPIO,
                                            LATITUDE, 
                                            LONGITUDE, 
                                            M.UF AS ESTADO,
                                            PE.USUARIOID, 
                                            PE.DATACADASTRO, 
                                            PE.DATAALTERACAO 
                                FROM   [TRANSPORTE].[PONTOEMBARQUE] PE (NOLOCK) 
								        INNER JOIN HADES.DBO.HD_MUNICIPIO M (NOLOCK) 
									          ON PE.MUNICIPIO = M.MUNICIPIO
	                                    INNER JOIN [TRANSPORTE].ROTATRAJETO RT  
				                                    ON RT.ROTATRAJETOID =  PE.ROTATRAJETOID
                                WHERE  PE.ROTATRAJETOID = @ROTATRAJETOID ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);

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

        public bool PossuiRotaSecundariaPor(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PONTOEMBARQUE PE (NOLOCK)
											INNER JOIN Transporte.ROTATRAJETO RT (NOLOCK) 
													ON PE.ROTATRAJETOID = RT.ROTATRAJETOID
                                    WHERE ROTAID = @ROTAID
                                          AND PRIMEIRO = 0 ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhPrimeiroPontoEmbarquePor(DataContext contexto, int pontoEmbarqueId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PONTOEMBARQUE (NOLOCK)
                                    WHERE PONTOEMBARQUEID = @PONTOEMBARQUEID
                                          AND PRIMEIRO = 1 ";

            contextQuery.Parameters.Add("@PONTOEMBARQUEID", SqlDbType.Int, pontoEmbarqueId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(DataContext contexto, Entidades.PontoEmbarque pontoEmbarque)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.PONTOEMBARQUE 
                                            (ROTATRAJETOID, 
                                             PRIMEIRO, 
                                             CEP, 
                                             LOGRADOURO, 
                                             NUMERO, 
                                             BAIRRO, 
                                             MUNICIPIO, 
                                             LATITUDE, 
                                             LONGITUDE, 
                                             USUARIOID, 
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                VALUES      (@ROTATRAJETOID, 
                                             @PRIMEIRO, 
                                             @CEP, 
                                             @LOGRADOURO, 
                                             @NUMERO, 
                                             @BAIRRO, 
                                             @MUNICIPIO, 
                                             @LATITUDE, 
                                             @LONGITUDE, 
                                             @USUARIOID, 
                                             @DATACADASTRO, 
                                             @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, pontoEmbarque.RotaTrajetoId);
            contextQuery.Parameters.Add("@PRIMEIRO", SqlDbType.Bit, pontoEmbarque.Primeiro);
            contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, pontoEmbarque.Cep);
            contextQuery.Parameters.Add("@LOGRADOURO", SqlDbType.VarChar, pontoEmbarque.Logradouro);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, pontoEmbarque.Numero);
            contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, pontoEmbarque.Bairro);
            contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, pontoEmbarque.Municipio);
            contextQuery.Parameters.Add("@LATITUDE", SqlDbType.VarChar, pontoEmbarque.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", SqlDbType.VarChar, pontoEmbarque.Longitude);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, pontoEmbarque.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void Atualiza(DataContext contexto, Entidades.PontoEmbarque pontoEmbarque)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TRANSPORTE.PONTOEMBARQUE 
                                      SET PRIMEIRO = @PRIMEIRO,
                                            CEP = @CEP,
                                            LOGRADOURO = @LOGRADOURO,
                                            NUMERO = @NUMERO,
                                            BAIRRO = @BAIRRO,
                                            MUNICIPIO = @MUNICIPIO,
                                            LATITUDE = @LATITUDE,
                                            LONGITUDE = @LONGITUDE,
                                            USUARIOID = @USUARIOID,
                                            DATAALTERACAO = @DATAALTERACAO
                                      WHERE PONTOEMBARQUEID = @PONTOEMBARQUEID ";

            contextQuery.Parameters.Add("@PONTOEMBARQUEID", SqlDbType.Int, pontoEmbarque.PontoEmbarqueId);
            contextQuery.Parameters.Add("@PRIMEIRO", SqlDbType.Bit, pontoEmbarque.Primeiro);
            contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, pontoEmbarque.Cep);
            contextQuery.Parameters.Add("@LOGRADOURO", SqlDbType.VarChar, pontoEmbarque.Logradouro);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, pontoEmbarque.Numero);
            contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, pontoEmbarque.Bairro);
            contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, pontoEmbarque.Municipio);
            contextQuery.Parameters.Add("@LATITUDE", SqlDbType.VarChar, pontoEmbarque.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", SqlDbType.VarChar, pontoEmbarque.Longitude);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, pontoEmbarque.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void DesmarcaPrimeiroPonto(DataContext contexto, int rotaTrajetoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TRANSPORTE.PONTOEMBARQUE 
                                      SET PRIMEIRO = 0
                                      WHERE ROTATRAJETOID = @ROTATRAJETOID ";

            contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(Entidades.PontoEmbarque pontoEmbarque, int rotaId, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            decimal numeroDecimalLat;
            decimal numeroDecimalLong;
            Rota rnRota = new Rota();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pontoEmbarque == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (pontoEmbarque.PontoEmbarqueId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (pontoEmbarque.RotaTrajetoId <= 0)
            {
                mensagens.Add("Campo ROTA TRAJETO é obrigatório.");
            }

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ROTA é obrigatório.");
            }

            if (pontoEmbarque.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }
            else
            {
                pontoEmbarque.Cep = Utils.RetirarMascara(pontoEmbarque.Cep);

                if (!Validacao.ValidarCEP(pontoEmbarque.Cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (pontoEmbarque.Logradouro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }
            else
            {
                if (pontoEmbarque.Logradouro.Length > 50)
                {
                    mensagens.Add("Campo LOGRADOURO deve conter no máximo 50 caracteres.");
                }
            }

            if (pontoEmbarque.Numero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (pontoEmbarque.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(pontoEmbarque.Bairro))
                {
                    mensagens.Add("Campo BAIRRO é inválido!");
                }
            }

            if (pontoEmbarque.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (!pontoEmbarque.Latitude.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(pontoEmbarque.Latitude.Replace(".", ","), out numeroDecimalLat))
                {
                    mensagens.Add("LATITUDE inválida.");
                }
                else
                {
                    var lat = Math.Abs(numeroDecimalLat);
                    if (lat < 20 || lat > 24)
                    {
                        mensagens.Add("Latitude fora do limite permitido. Favor verificar.");
                    }
                }
            }

            if (!pontoEmbarque.Longitude.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(pontoEmbarque.Longitude.Replace(".", ","), out numeroDecimalLong))
                {
                    mensagens.Add("LONGITUDE inválida.");
                }
                else
                {
                    var longi = Math.Abs(numeroDecimalLong);
                    if (longi < 40 || longi > 45)
                    {
                        mensagens.Add("Longitude fora do limite permitido. Favor verificar.");
                    }
                }
            }

            if (pontoEmbarque.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }


            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a rota pode ser alterada
                    if (!rnRota.PodeEditarPor(contexto, rotaId))
                    {
                        mensagens.Add("Os pontos de embarque não podem ser alterados, pois o período para edição terminou.");
                    }
                    else
                    {
                        //Verifica se está alterando e tirando a marcação de primeiro
                        if (!cadastro && !pontoEmbarque.Primeiro)
                        {
                            //Verifica se era o primeiro
                            if (this.EhPrimeiroPontoEmbarquePor(contexto, pontoEmbarque.PontoEmbarqueId))
                            {
                                mensagens.Add("A opção PRIMEIRO do ponto de embarque não pode ser retirada, caso necessário acrescente essa opção em outro ponto de embarque e este será desmarcado.");
                            }
                        }

                        //Verifica se é unico
                        if (this.PossuiOutroPontoCadastradoPor(contexto, pontoEmbarque.RotaTrajetoId, pontoEmbarque.Logradouro, pontoEmbarque.Numero, pontoEmbarque.Bairro, pontoEmbarque.Municipio, pontoEmbarque.PontoEmbarqueId))
                        {
                            mensagens.Add("Este ponto de embarque já foi cadastrado para este trajeto.");
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

        private bool PossuiOutroPontoCadastradoPor(DataContext ctx, int rotaTrajetoId, string logradouro, string numero, string bairro, string municipio, int pontoEmbarqueId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                     FROM TRANSPORTE.PONTOEMBARQUE (NOLOCK)
                                      WHERE PONTOEMBARQUEID <> @PONTOEMBARQUEID
											AND ROTATRAJETOID = @ROTATRAJETOID
											AND LOGRADOURO = @LOGRADOURO
											AND NUMERO = @NUMERO
											AND BAIRRO = @BAIRRO
											AND MUNICIPIO = @MUNICIPIO ";

            contextQuery.Parameters.Add("@PONTOEMBARQUEID", SqlDbType.Int, pontoEmbarqueId);
            contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);
            contextQuery.Parameters.Add("@LOGRADOURO", SqlDbType.VarChar, logradouro);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, numero);
            contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, bairro);
            contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.PontoEmbarque pontoEmbarque)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(ctx, pontoEmbarque);
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

        public void Atualiza(Entidades.PontoEmbarque pontoEmbarque)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            PontoEmbarque rnPontoEmbarque = new PontoEmbarque();

            try
            {
                //NOTA: Para alterar um ponto que não era primeiro e passa a ser desmarca o outro primeiro, emitir msg confirmacao na tela

                //Verifica se está alterando e adicionando a marcação de primeiro que nao existia
                if (pontoEmbarque.Primeiro && !this.EhPrimeiroPontoEmbarquePor(contexto, pontoEmbarque.PontoEmbarqueId))
                {
                    //Desmarca o opção de primeiro ponto que já existia para a rota
                    this.DesmarcaPrimeiroPonto(contexto, pontoEmbarque.RotaTrajetoId);
                }

                //Atualiza o ponto de embarque
                this.Atualiza(contexto, pontoEmbarque);
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

        public ValidacaoDados ValidaRemocao(int pontoEmbarqueId, int rotaId, bool primeiro)
        {
            List<string> mensagens = new List<string>();
            Rota rnRota = new Rota();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pontoEmbarqueId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (primeiro)
            {
                mensagens.Add("O primeiro ponto de embarque não pode ser excluído.");
            }

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ROTA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a rota pode ser alterada
                    if (!rnRota.PodeEditarPor(contexto, rotaId))
                    {
                        mensagens.Add("Os pontos de embarque não podem ser alterados, pois o periodo para edição terminou.");
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

        public void Remove(int pontoEmbarqueId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.PONTOEMBARQUE
                            WHERE  PONTOEMBARQUEID = @PONTOEMBARQUEID  ";

                contextQuery.Parameters.Add("@PONTOEMBARQUEID", SqlDbType.Int, pontoEmbarqueId);

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

        public void RemovePrimeiro(DataContext contexto, int rotaI)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE pe
									FROM Transporte.PONTOEMBARQUE pe (nolock)
										inner join Transporte.ROTATRAJETO rt (nolock) 
											on pe.ROTATRAJETOID = rt.ROTATRAJETOID
                                      WHERE  ROTAID = @ROTAID
                                             AND PRIMEIRO = 1 ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaI);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
