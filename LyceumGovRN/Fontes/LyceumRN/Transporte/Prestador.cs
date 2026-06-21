using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class Prestador
    {
        public Entidades.Prestador ObtemPor(int prestadorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Prestador prestador = new Entidades.Prestador();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                          FROM  [Transporte].[PRESTADOR] (NOLOCK) 
                                          WHERE PRESTADORID = @PRESTADORID ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

                prestador = contexto.TryToBindEntity<Entidades.Prestador>(contextQuery);

                return prestador;
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

        public bool EhAtivoPor(DataContext contexto, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PRESTADOR (NOLOCK)
                                    WHERE PRESTADORID = @PRESTADORID
										  AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaInsercao(Entidades.Prestador prestador, bool pessoaJuridica, Entidades.PrestadorVigencia prestadorVigencia)
        {
            return this.Valida(prestador, pessoaJuridica, prestadorVigencia, true);
        }

        public ValidacaoDados ValidaAlteracao(Entidades.Prestador prestador, bool pessoaJuridica)
        {
            return this.Valida(prestador, pessoaJuridica, null, false);
        }

        private ValidacaoDados Valida(Entidades.Prestador prestador, bool pessoaJuridica, Entidades.PrestadorVigencia prestadorVigencia, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestador == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (prestador.PrestadorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }

                prestadorVigencia = null;
            }           

            if (pessoaJuridica)
            {
                prestador.Cpf = null;

                if (prestador.Cnpj.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CNPJ é obrigatório.");
                }
                else
                {
                    prestador.Cnpj = Utils.RetirarMascara(prestador.Cnpj);

                    if (!Validacao.ValidaCnpj(prestador.Cnpj))
                    {
                        mensagens.Add("O CNPJ informado não é válido!");
                    }
                }
            }
            else
            {
                prestador.Cnpj = null;

                if (prestador.Cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CPF é obrigatório.");
                }
                else
                {
                    prestador.Cpf = Utils.RetirarMascara(prestador.Cpf);

                    if (!Validacao.ValidaCpf(prestador.Cpf))
                    {
                        mensagens.Add("O CPF informado não é válido!");
                    }
                }
            }

            if (prestador.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else if (prestador.Nome.Length > 255)
            {
                mensagens.Add("Campo NOME deve conter no máximo 255 caracteres.");
            }

            if (prestador.Telefone.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TELEFONE é obrigatório.");
            }
            else
            {
                prestador.Telefone = Utils.RetirarMascara(prestador.Telefone);

                if (!Validacao.ValidaTelefoneComDDD(prestador.Telefone) && !Validacao.ValidaCelularComDDD(prestador.Telefone))
                {
                    mensagens.Add("Campo TELEFONE OU CELULAR é inválido.");
                }
            }

            if (cadastro)
            {
                if (prestadorVigencia.Censo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CENSO é obrigatório.");
                }

                if (prestadorVigencia.DataInicio == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA INICIO é obrigatório.");
                }

                if (prestadorVigencia.DataFim == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA FIM é obrigatório.");
                }
                else
                {
                    if (prestadorVigencia.DataInicio != DateTime.MinValue && prestadorVigencia.DataInicio.Date > prestadorVigencia.DataFim.Date)
                    {
                        mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
                    }
                }
            }

            if (prestador.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o cnpj
                    if (!prestador.Cnpj.IsNullOrEmptyOrWhiteSpace() && this.PossuiOutroCnpjCadastradoPor(contexto, prestador.Cnpj, prestador.PrestadorId))
                    {
                        mensagens.Add("Já existe um prestador com este CNPJ.");
                    }

                    // Verifica se já existe o cpf
                    if (!prestador.Cpf.IsNullOrEmptyOrWhiteSpace() && this.PossuiOutroCpfCadastradoPor(contexto, prestador.Cpf, prestador.PrestadorId))
                    {
                        mensagens.Add("Já existe um prestador com este CPF.");
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

        private bool PossuiOutroCnpjCadastradoPor(DataContext ctx, string cnpj, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADOR] (NOLOCK)
                                WHERE CNPJ = @CNPJ
	                                AND PRESTADORID <> @PRESTADORID ";

            contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, cnpj);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCpfCadastradoPor(DataContext ctx, string cpf, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADOR] (NOLOCK)
                                WHERE CPF = @CPF
	                                AND PRESTADORID <> @PRESTADORID ";

            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Prestador prestador, Entidades.PrestadorVigencia prestadorVigencia)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            PrestadorVigencia rnPrestadorVigencia = new PrestadorVigencia();

            try
            {                
                //Insere Prestador
                this.Insere(contexto, prestador);

                prestadorVigencia.PrestadorId = prestador.PrestadorId;
                prestadorVigencia.UsuarioId = prestador.UsuarioId;

                //Insere vigencia inicial
                rnPrestadorVigencia.Insere(contexto, prestadorVigencia);
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

        private void Insere(DataContext contexto, Entidades.Prestador prestador)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.PRESTADOR 
                                                (CNPJ, 
                                                 CPF, 
                                                 NOME, 
                                                 TELEFONE, 
                                                 ATIVO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@CNPJ, 
                                                 @CPF, 
                                                 @NOME, 
                                                 @TELEFONE, 
                                                 @ATIVO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) 

                                SELECT IDENT_CURRENT('Transporte.PRESTADOR') ";

            contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, prestador.Cnpj);
            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, prestador.Cpf);
            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, prestador.Nome);
            contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, prestador.Telefone);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, true);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prestador.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            prestador.PrestadorId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.Prestador prestador)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.PRESTADOR
                                               SET CNPJ = @CNPJ,
                                                  CPF = @CPF, 
                                                  NOME = @NOME, 
                                                  TELEFONE = @TELEFONE, 
                                                  USUARIOID = @USUARIOID, 
                                                  DATAALTERACAO = @DATAALTERACAO
                                             WHERE PRESTADORID = @PRESTADORID ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestador.PrestadorId);
                contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, prestador.Cnpj);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, prestador.Cpf);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, prestador.Nome);
                contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, prestador.Telefone);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prestador.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int prestadorId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PrestadorVigencia rnPrestadorVigencia = new PrestadorVigencia();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe vigencia
                    if (rnPrestadorVigencia.PossuiPrestadorVigenciaPor(contexto, prestadorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois possui vigências cadastradas.");
                    }

                    //Verifica se ja foi utilizado tabelas PRESTADORCONDUTORVEICULO e PRESTADORCONDUTOR
                    if (this.PossuiPrestadorCondutorPorPrestador(contexto, prestadorId) || this.PossuiPrestadorCondutorVeiculoPorPrestador(contexto, prestadorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi associado a um condutor / veículo.");
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

        public void Remove(int prestadorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.PRESTADOR
                            WHERE  PRESTADORID = @PRESTADORID  ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

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

        public DataTable ListaPrestadorCondutorPor(int prestadorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PC.PRESTADORID, 
                                                   PC.CONDUTORID, 
                                                   C.CPF, 
                                                   C.NOME, 
                                                   C.NUMEROCNH, 
                                                   C.DATAVALIDADECNH, 
                                                   PC.ATIVO, 
                                                   PC.USUARIOID, 
                                                   PC.DATACADASTRO, 
                                                   PC.DATAALTERACAO 
                                            FROM   [Transporte].[PRESTADORCONDUTOR] PC (NOLOCK) 
                                                   INNER JOIN [TRANSPORTE].[CONDUTOR] C (NOLOCK) 
                                                           ON PC.CONDUTORID = C.CONDUTORID 
                                            WHERE  PRESTADORID = @PRESTADORID 
                                                   AND PC.ATIVO = 1 ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

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

        public ValidacaoDados ValidaPrestadorCondutor(int prestadorId, int condutorId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Condutor rnCondutor = new Condutor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorId <= 0)
            {
                mensagens.Add("Campo PRESTADOR é obrigatório.");
            }

            if (condutorId <= 0)
            {
                mensagens.Add("Campo CONDUTOR é obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi existe associação ativa
                    if (this.PossuiPrestadorCondutorAtivoPor(contexto, prestadorId, condutorId, true))
                    {
                        mensagens.Add("Já existe a associação deste prestador / condutor");
                    }

                    //Deve permitir a edição da associação, se todos os envolvidos estiverem ativos
                    if (!this.EhAtivoPor(contexto, prestadorId))
                    {
                        mensagens.Add("Este prestador não está ativo.");
                    }

                    if (!rnCondutor.EhAtivoPor(contexto, condutorId))
                    {
                        mensagens.Add("Este condutor não está ativo.");
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

        private bool PossuiPrestadorCondutorAtivoPor(DataContext ctx, int prestadorId, int condutorId, bool ativo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTOR] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID
	                                AND PRESTADORID = @PRESTADORID 
                                    AND ATIVO = @ATIVO ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }       

        public void InserePrestadorCondutor(int prestadorId, int condutorId, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Verifica se já existe cancelador
                if (this.PossuiPrestadorCondutorAtivoPor(contexto, prestadorId, condutorId, false))
                {
                    this.AtualizarPrestadorCondutor(contexto, prestadorId, condutorId, true, usuarioResponsavel);
                }
                else
                {
                    this.InserePrestadorCondutor(contexto, prestadorId, condutorId, usuarioResponsavel);
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

        private void InserePrestadorCondutor(DataContext contexto, int prestadorId, int condutorId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.PRESTADORCONDUTOR 
                                                    (PRESTADORID, 
                                                     CONDUTORID, 
                                                     ATIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@PRESTADORID, 
                                                     @CONDUTORID, 
                                                     @ATIVO, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, true);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizarPrestadorCondutor(DataContext contexto, int prestadorId, int condutorId, bool ativo, string usuarioResponsavel)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Transporte.PRESTADORCONDUTOR
                                    SET ATIVO = @ATIVO,
	                                    USUARIOID = @USUARIOID,
	                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE PRESTADORID = @PRESTADORID
	                                    AND CONDUTORID = @CONDUTORID ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocaoPrestadorCondutor(int prestadorId, int condutorId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorId <= 0)
            {
                mensagens.Add("Campo PRESTADOR é obrigatório.");
            }

            if (condutorId <= 0)
            {
                mensagens.Add("Campo CONDUTOR é obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi existe associação com veiculo 
                    if (this.PossuiPrestadorCondutorVeiculoPorCondutorPrestador(contexto, condutorId, prestadorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi associado a um veiculo");
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

        public void RemovePrestadorCondutor(int prestadorId, int condutorId, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.AtualizarPrestadorCondutor(contexto, prestadorId, condutorId, false, usuarioResponsavel);
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

        private bool PossuiPrestadorCondutorPorPrestador(DataContext ctx, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTOR] (NOLOCK)
                                WHERE PRESTADORID = @PRESTADORID ";


            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiPrestadorCondutorVeiculoPorPrestador(DataContext ctx, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTORVEICULO] (NOLOCK)
                                WHERE PRESTADORID = @PRESTADORID ";


            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        } 

        private bool PossuiPrestadorCondutorVeiculoPorCondutorPrestador(DataContext ctx, int condutorId, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTORVEICULO] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID
                                      AND PRESTADORID = @PRESTADORID ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }       
    }
}