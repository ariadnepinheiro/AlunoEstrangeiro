using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Transporte
{
    public class Veiculo
    {
        public Entidades.Veiculo ObtemPor(int veiculoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Veiculo veiculo = new Entidades.Veiculo();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                          FROM  [Transporte].[VEICULO] (NOLOCK) 
                                          WHERE VEICULOID = @VEICULOID ";

                contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

                veiculo = contexto.TryToBindEntity<Entidades.Veiculo>(contextQuery);

                return veiculo;
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

        public bool PossuiTipoVeiculoPor(DataContext contexto, int tipoVeiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.VEICULO (NOLOCK)
                                    WHERE TIPOVEICULOID = @TIPOVEICULOID ";

            contextQuery.Parameters.Add("@TIPOVEICULOID", SqlDbType.Int, tipoVeiculoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemQuantidadeAssentosPor(DataContext contexto, int veiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT QUANTIDADEASSENTOS
                                        FROM   Transporte.VEICULO
				                        WHERE VEICULOID = @VEICULOID";

                contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADEASSENTOS"]);
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

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT VEICULOID, 
                                           V.TIPOVEICULOID, 
                                           T.DESCRICAO AS DESCRICAOTIPOVEICULO, 
                                           PLACA, 
                                           ANOLICENCIAMENTO, 
                                           NOME, 
                                           ANOMODELO, 
                                           QUANTIDADEASSENTOS, 
                                           OBSERVACAO, 
                                           V.ATIVO, 
                                           V.USUARIOID, 
                                           V.DATACADASTRO, 
                                           V.DATAALTERACAO 
                                    FROM   [Transporte].[VEICULO] V (NOLOCK) 
                                           INNER JOIN [Transporte].[TIPOVEICULO] T (NOLOCK) 
                                                   ON V.TIPOVEICULOID = T.TIPOVEICULOID  ";

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

        public DataTable ListaPrestadorCondutorVeiculoPor(int prestadorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PCV.PRESTADORID, 
                                           PCV.CONDUTORID, 
                                           PCV.VEICULOID, 
                                           C.NOME AS CONDUTOR, 
                                           C.CPF, 
                                           V.PLACA, 
                                           V.NOME AS VEICULO 
                                    FROM   [TRANSPORTE].[PRESTADORCONDUTORVEICULO] PCV (NOLOCK)
                                           INNER JOIN [TRANSPORTE].[CONDUTOR] C  (NOLOCK)
                                                   ON PCV.CONDUTORID = C.CONDUTORID 
                                           INNER JOIN [TRANSPORTE].[VEICULO] V  (NOLOCK)
                                                   ON PCV.VEICULOID = V.VEICULOID 
                                    WHERE  PCV.ATIVO = 1 
                                           AND PRESTADORID = @PRESTADORID ";

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

        public ValidacaoDados Valida(Entidades.Veiculo veiculo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (veiculo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (veiculo.VeiculoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (veiculo.TipoVeiculoId <= 0)
            {
                mensagens.Add("Campo TIPO DE VEÍCULO é obrigatório.");
            }

            if (veiculo.Placa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PLACA é obrigatório.");
            }
            else 
            {
                if (veiculo.TipoVeiculoId > 0)
                {
                    if (veiculo.TipoVeiculoId != 1)
                    {
                        if (veiculo.Placa.Length != 7)
                        {
                            mensagens.Add("Campo PLACA deve conter 7 dígitos.");
                        }

                        //Retirar o caracter - que será permitido
                        string placa = veiculo.Placa.Replace("-", string.Empty);

                        //Apenas aceitar letras e numeros
                        if (!Validacao.SomenteLetrasNumeros(placa))
                        {
                            mensagens.Add("Campo PLACA deve conter apenas números e letras.");
                        }
                    }
                    else
                    {
                        if (veiculo.Placa.Length != 10)
                        {
                            mensagens.Add("Campo PLACA deve conter 10 dígitos.");
                        }
                    }
                }
            }

            if (veiculo.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else if (veiculo.Nome.Length > 255)
            {
                mensagens.Add("Campo NOME deve conter no máximo 255 caracteres.");
            }

            if (veiculo.AnoLicenciamento <= 0)
            {
                mensagens.Add("Campo ANO DE LICENCIAMENTO é obrigatório.");
            }
            else
            {
                if (veiculo.AnoLicenciamento <= DateTime.MinValue.Year)
                {
                    mensagens.Add("Campo ANO DE LICENCIAMENTO inválido.");
                }
                else
                {
                    if (veiculo.AnoLicenciamento > DateTime.Now.Year)
                    {
                        mensagens.Add("Campo ANO DE LICENCIAMENTO não pode ser maior que o ano atual.");
                    }

                    if (veiculo.AnoLicenciamento < DateTime.Now.Year - 1)
                    {
                        mensagens.Add("Campo ANO DE LICENCIAMENTO pode ser apenas o ano anterior e o ano atual.");
                    }
                }
            }

            if (veiculo.AnoModelo <= 0)
            {
                mensagens.Add("Campo ANO MODELO é obrigatório.");
            }
            else
            {
                if (veiculo.AnoModelo <= DateTime.MinValue.Year)
                {
                    mensagens.Add("Campo ANO MODELO inválido.");
                }
                else if (veiculo.AnoModelo > DateTime.Now.Year + 1)
                {
                    mensagens.Add("Campo ANO MODELO não pode ser maior que o próximo ano.");
                }
            }

            if (veiculo.QuantidadeAssentos <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE ASSENTOS é obrigatório.");
            }
            else
            {
                if (veiculo.QuantidadeAssentos < 4)
                {
                    mensagens.Add("Campo QUANTIDADE DE ASSENTOS não pode ser menor que 4.");
                }
                else if (veiculo.QuantidadeAssentos > 70)
                {
                    mensagens.Add("Campo QUANTIDADE DE ASSENTOS não pode ser maior que 70.");
                }
            }

            if (!veiculo.Observacao.IsNullOrEmptyOrWhiteSpace() && veiculo.Observacao.Length > 500)
            {
                mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo 500 caracteres.");
            }

            if (veiculo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a placa cadastrada
                    if (this.PossuiOutraPlacaCadastradaPor(contexto, veiculo.Placa, veiculo.VeiculoId))
                    {
                        mensagens.Add("Já existe um veículo com esta PLACA.");
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

        private bool PossuiOutraPlacaCadastradaPor(DataContext ctx, string placa, int veiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[VEICULO] (NOLOCK)
                                WHERE PLACA = @PLACA
	                                AND VEICULOID <> @VEICULOID ";

            contextQuery.Parameters.Add("@PLACA", SqlDbType.VarChar, placa);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Veiculo veiculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.VEICULO 
                                                    (TIPOVEICULOID, 
                                                     PLACA, 
                                                     ANOLICENCIAMENTO, 
                                                     NOME, 
                                                     ANOMODELO, 
                                                     QUANTIDADEASSENTOS, 
                                                     OBSERVACAO, 
                                                     ATIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@TIPOVEICULOID, 
                                                     @PLACA, 
                                                     @ANOLICENCIAMENTO, 
                                                     @NOME, 
                                                     @ANOMODELO, 
                                                     @QUANTIDADEASSENTOS, 
                                                     @OBSERVACAO, 
                                                     @ATIVO, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO)  ";

                contextQuery.Parameters.Add("@TIPOVEICULOID", SqlDbType.Int, veiculo.TipoVeiculoId);
                contextQuery.Parameters.Add("@PLACA", SqlDbType.VarChar, veiculo.Placa);
                contextQuery.Parameters.Add("@ANOLICENCIAMENTO", SqlDbType.Int, veiculo.AnoLicenciamento);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, veiculo.Nome);
                contextQuery.Parameters.Add("@ANOMODELO", SqlDbType.Int, veiculo.AnoModelo);
                contextQuery.Parameters.Add("@QUANTIDADEASSENTOS", SqlDbType.Int, veiculo.QuantidadeAssentos);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, veiculo.Observacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, veiculo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, veiculo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
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

        public void Atualiza(Entidades.Veiculo veiculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.VEICULO 
                                        SET    TIPOVEICULOID = @TIPOVEICULOID, 
                                               PLACA = @PLACA, 
                                               ANOLICENCIAMENTO = @ANOLICENCIAMENTO, 
                                               NOME = @NOME, 
                                               ANOMODELO = @ANOMODELO, 
                                               QUANTIDADEASSENTOS = @QUANTIDADEASSENTOS, 
                                               OBSERVACAO = @OBSERVACAO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  VEICULOID = @VEICULOID ";

                contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculo.VeiculoId);
                contextQuery.Parameters.Add("@TIPOVEICULOID", SqlDbType.Int, veiculo.TipoVeiculoId);
                contextQuery.Parameters.Add("@PLACA", SqlDbType.VarChar, veiculo.Placa);
                contextQuery.Parameters.Add("@ANOLICENCIAMENTO", SqlDbType.Int, veiculo.AnoLicenciamento);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, veiculo.Nome);
                contextQuery.Parameters.Add("@ANOMODELO", SqlDbType.Int, veiculo.AnoModelo);
                contextQuery.Parameters.Add("@QUANTIDADEASSENTOS", SqlDbType.Int, veiculo.QuantidadeAssentos);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, veiculo.Observacao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, veiculo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, veiculo.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int veiculoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (veiculoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado na tabelas PRESTADORCONDUTORVEICULO
                    if (this.PossuiPrestadorCondutorVeiculoPorVeiculo(contexto, veiculoId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi associado a um prestador / condutor");
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

        public void Remove(int veiculoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.VEICULO
                            WHERE  VEICULOID = @VEICULOID  ";

                contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

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

        private bool PossuiPrestadorCondutorVeiculoPorVeiculo(DataContext ctx, int veiculoid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTORVEICULO] (NOLOCK)
                                WHERE VEICULOID = @VEICULOID ";

            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoid);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InserePrestadorCondutorVeiculo(int prestadorId, int condutorId, int veiculoId, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Verifica se já existe cancelador
                if (this.PossuiPrestadorCondutorVeiculoPor(contexto, prestadorId, condutorId, veiculoId, false))
                {
                    this.AtualizarPrestadorCondutorVeiculo(contexto, prestadorId, condutorId, veiculoId, true, usuarioResponsavel);
                }
                else
                {
                    this.InserePrestadorCondutorVeiculo(contexto, prestadorId, condutorId, veiculoId, usuarioResponsavel);
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

        private void InserePrestadorCondutorVeiculo(DataContext contexto, int prestadorId, int condutorId, int veiculoId, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.PRESTADORCONDUTORVEICULO 
                                                    (PRESTADORID, 
                                                     CONDUTORID, 
                                                     VEICULOID,
                                                     ATIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@PRESTADORID, 
                                                     @CONDUTORID, 
                                                     @VEICULOID,
                                                     @ATIVO, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, true);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizarPrestadorCondutorVeiculo(DataContext contexto, int prestadorId, int condutorId, int veiculoId, bool ativo, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Transporte.PRESTADORCONDUTORVEICULO
                                    SET ATIVO = @ATIVO,
	                                    USUARIOID = @USUARIOID,
	                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE PRESTADORID = @PRESTADORID
	                                    AND CONDUTORID = @CONDUTORID
	                                    AND VEICULOID = @VEICULOID ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocaoPrestadorCondutorVeiculo(int prestadorId, int condutorId, int veiculoId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();
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

            if (veiculoId <= 0)
            {
                mensagens.Add("Campo VEICULO é obrigatório.");
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

                    //Uma associação entre prestador, condutor e veículo só pode ser excluída se não houver rota cadastrada atendida por essa associação.
                    if (rnRotaTrajeto.PossuiRotaAtivaPor(contexto, prestadorId, condutorId, veiculoId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem Rotas ativas atendidas por essa associação.");
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

        public void RemovePrestadorCondutorVeiculo(int prestadorId, int condutorId, int veiculoId, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Cancela
                this.AtualizarPrestadorCondutorVeiculo(contexto, prestadorId, condutorId, veiculoId, false, usuarioResponsavel);
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

        public bool EhAtivoPor(DataContext contexto, int veiculoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.VEICULO (NOLOCK)
                                    WHERE VEICULOID = @VEICULOID
										  AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPrestadorCondutorVeiculoPor(DataContext ctx, int prestadorId, int condutorId, int veiculoId, bool ativo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORCONDUTORVEICULO] (NOLOCK)
                                WHERE CONDUTORID = @CONDUTORID
	                                AND PRESTADORID = @PRESTADORID 
                                    AND VEICULOID = @VEICULOID
                                    AND ATIVO = @ATIVO ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }        

        public ValidacaoDados ValidaPrestadorCondutorVeiculo(int prestadorId, int condutorId, int veiculoId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Prestador rnPrestador = new Prestador();
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

            if (veiculoId <= 0)
            {
                mensagens.Add("Campo VEICULO é obrigatório.");
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
                    if (this.PossuiPrestadorCondutorVeiculoPor(contexto, prestadorId, condutorId, veiculoId, true))
                    {
                        mensagens.Add("Já existe a associação deste prestador / condutor / veículo.");
                    }

                    //Deve permitir a edição da associação, se todos os envolvidos estiverem ativos
                    if(!rnPrestador.EhAtivoPor(contexto, prestadorId))
                    {
                        mensagens.Add("Este prestador não está ativo.");
                    }

                    if (!rnCondutor.EhAtivoPor(contexto, condutorId))
                    {
                        mensagens.Add("Este condutor não está ativo.");
                    }

                    if (!this.EhAtivoPor(contexto, veiculoId))
                    {
                        mensagens.Add("Este veículo não está ativo.");
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
    }
}