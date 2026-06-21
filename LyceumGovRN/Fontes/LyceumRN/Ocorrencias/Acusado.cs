using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class Acusado : RNBase
    {
        public enum Tipo
        {
            [StringValue("Aluno")]
            Aluno = 1,
            [StringValue("ervidor")]
            Servidor = 2,
            [StringValue("Outros")]
            Outros = 4
        }

        public bool PossuiAcusadoPor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Ocorrencias.ACUSADO (NOLOCK) 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(int ocorrenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ACUSADOID,
									CASE
										WHEN V.TIPO = 1 THEN 'Aluno'
										WHEN V.TIPO = 2 THEN 'Servidor'
										WHEN V.TIPO = 4 THEN 'Outros'
									END TIPODESCRICAO,
									V.PESSOAID,
									CASE 
										WHEN V.TIPO = 1 THEN PA.ALUNO
										ELSE NULL
									END ALUNO, 
									CASE 
										WHEN V.TIPO = 2 THEN P.IDFUNCIONAL
										ELSE NULL
									END IDFUNCIONAL,
									CASE
										WHEN V.DESCONHECIDO = 1 THEN 'Desconhecido'
										WHEN V.NOME IS NOT NULL THEN V.NOME
										WHEN P.NOME_COMPL IS NOT NULL THEN P.NOME_COMPL
									END NOME,
									ISNULL(V.DATANASCIMENTO, P.DT_NASC) as DATANASCIMENTO,
									CASE
										WHEN V.DATANASCIMENTO IS NOT NULL THEN [dbo].[CalcularIdadePorDataReferencia] (V.DATANASCIMENTO, O.DATAOCORRENCIA)
										WHEN P.DT_NASC IS NOT NULL THEN [dbo].[CalcularIdadePorDataReferencia] (P.DT_NASC, O.DATAOCORRENCIA)
									END IDADE,
									V.CARGO,
									V.FUNCAO,
									F.DESCRICAO AS DESCRICAOFUNCAO
                                FROM Ocorrencias.ACUSADO V
									INNER JOIN Ocorrencias.OCORRENCIA O ON V.OCORRENCIAID = O.OCORRENCIAID
									LEFT JOIN LY_PESSOA P ON V.PESSOAID = P.PESSOA
									LEFT JOIN LY_FUNCAO F ON V.FUNCAO = F.FUNCAO
									LEFT JOIN Matricula.PESSOAALUNO PA ON PA.PESSOAID = V.PESSOAID
                                WHERE V.OCORRENCIAID = @OCORRENCIAID  ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

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

        public ValidacaoDados Valida(Entidades.Acusado acusado, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (acusado == null)
            {
                return validacaoDados;
            }

            if (acusado.OcorrenciaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (acusado.Tipo != (int)Acusado.Tipo.Aluno
             && acusado.Tipo != (int)Acusado.Tipo.Servidor
             && acusado.Tipo != (int)Acusado.Tipo.Outros)
            {
                mensagens.Add("Campo AUTOR é inválido.");
            }
            else
            {
                if (acusado.Tipo == (int)Acusado.Tipo.Aluno)
                {
                    acusado.Nome = null;
                    acusado.CPF = null;
                    acusado.DataNascimento = null;
                    acusado.Cargo = null;
                    acusado.Funcao = null;
                    acusado.RgNumero = null;
                    acusado.RgTipo = null;
                    acusado.RgEmissor = null;
                    acusado.RgUF = null;
                    acusado.RgDataExp = null;
                    acusado.Vinculo = null;

                    if (!acusado.Desconhecido &&
                       (acusado.PessoaId == null || acusado.PessoaId <= 0))
                    {
                        mensagens.Add("Campo ALUNO AUTOR é obrigatório.");
                    }
                }
                else if (acusado.Tipo == (int)Acusado.Tipo.Servidor)
                {
                    acusado.Nome = null;
                    acusado.CPF = null;
                    acusado.DataNascimento = null;
                    acusado.RgNumero = null;
                    acusado.RgTipo = null;
                    acusado.RgEmissor = null;
                    acusado.RgUF = null;
                    acusado.RgDataExp = null;

                    if (!acusado.Desconhecido)
                    {
                        if (acusado.PessoaId == null || acusado.PessoaId <= 0)
                        {
                            mensagens.Add("Campo SERVIDOR AUTOR é obrigatório.");
                        }
                        else if (acusado.Vinculo == null || acusado.PessoaId <= 0)
                        {
                            mensagens.Add("O SERVIDOR AUTOR está sem Id Funcional / Vínculo cadastrado, seu cadastro deve ser atualizado antes do registro da ocorrência.");
                        }

                        if (acusado.Cargo.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo CARGO DO AUTOR é obrigatório.");
                        }

                        if (acusado.Funcao.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo FUNÇÃO DO AUTOR é obrigatório.");
                        }
                    }
                }
                else
                {
                    acusado.PessoaId = null;
                    acusado.Vinculo = null;
                    acusado.Cargo = null;
                    acusado.Funcao = null;

                    if (!acusado.Desconhecido)
                    {
                        if (acusado.Nome.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo NOME DO AUTOR é obrigatório.");
                        }

                        if (!acusado.CPF.IsNullOrEmptyOrWhiteSpace()
                            && !Utils.ValidarCpf(acusado.CPF))
                        {
                            mensagens.Add("Campo CPF DO AUTOR é inválido.");
                        }

                        //Verifica se foi preenchido o tipo do documento
                        if (!acusado.RgTipo.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (Convert.ToString(acusado.RgNumero).IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo NÚMERO DOCUMENTO DO AUTOR é obrigatório.");
                            }
                            else
                            {
                                var rg = Utils.RetirarMascara(acusado.RgNumero);

                                if (rg.Length < 5)
                                {
                                    mensagens.Add("O NÚMERO DO DOCUMENTO DO AUTOR deve conter no mínimo cinco dígitos!");
                                }

                                if (acusado.RgTipo == "RG" && acusado.RgEmissor == "DETRAN" && acusado.RgUF == "RJ")
                                {
                                    if (!Validacao.ValidaNumerosInteirosPositivos(acusado.RgNumero))
                                    {
                                        mensagens.Add("O número de documento do RG Detran deve conter só números inteiros.");
                                    }
                                }
                            }

                            if (acusado.RgTipo == "RG")
                            {
                                if (Convert.ToString(acusado.RgUF).IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo ESTADO DO DOCUMENTO DO AUTOR é obrigatório.");
                                }

                                if (Convert.ToString(acusado.RgEmissor).IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo ÓRGÃO EMISSOR DO DOCUMENTO DO AUTOR é obrigatório.");
                                }

                                if (acusado.RgDataExp == null || acusado.RgDataExp == DateTime.MinValue)
                                {
                                    mensagens.Add("Campo Data de Expedição DO DOCUMENTO DO AUTOR é obrigatório.");
                                }
                                else if (Convert.ToDateTime(acusado.RgDataExp).Date > DateTime.Now.Date)
                                {
                                    mensagens.Add("Campo Data de Expedição DO DOCUMENTO DO AUTOR não pode ser maior que a data atual.");
                                }
                            }
                        }
                    }
                }
            }

            if (acusado.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se acusado com pessoa já foi cadastrada para o ocorrencia
                    if (acusado.PessoaId != null && acusado.PessoaId > 0 && this.PossuiPessoaIdPor(contexto, acusado.OcorrenciaId, Convert.ToDecimal(acusado.PessoaId)))
                    {
                        mensagens.Add("Este AUTOR já foi cadastrado para esta ocorrncia.");
                    }

                    //Verifica se acusado com cpf já foi cadastrada para o ocorrencia
                    if (!acusado.CPF.IsNullOrEmptyOrWhiteSpace() && this.PossuiCpfPor(contexto, acusado.OcorrenciaId, acusado.CPF))
                    {
                        mensagens.Add("Este AUTOR já foi cadastrado para esta ocorrncia.");
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

        private bool PossuiPessoaIdPor(DataContext contexto, int ocorrenciaId, decimal pessoaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   OCORRENCIAS.ACUSADO (NOLOCK) 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID
											AND PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Int, pessoaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiCpfPor(DataContext contexto, int ocorrenciaId, string cpf)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   OCORRENCIAS.ACUSADO (NOLOCK) 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID
											AND CPF = @CPF ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Acusado acusado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Ocorrencias.ACUSADO
                                           (OCORRENCIAID,
                                            TIPO,
                                            DESCONHECIDO,
                                            PESSOAID,
                                            VINCULO,
                                            NOME,
                                            RGNUMERO,
                                            RGTIPO,
                                            RGEMISSOR,
                                            RGUF,
                                            RGDATAEXP,
                                            CPF,
                                            DATANASCIMENTO,
                                            CARGO,
                                            FUNCAO,
                                            USUARIOID,
                                            DATACADASTRO,
                                            DATAALTERACAO)
                                     VALUES
                                           (@OCORRENCIAID,
                                           @TIPO,
                                           @DESCONHECIDO,
                                           @PESSOAID, 
                                           @VINCULO,
                                           @NOME,
                                           @RGNUMERO,
                                           @RGTIPO,
                                           @RGEMISSOR,
                                           @RGUF,
                                           @RGDATAEXP, 
                                           @CPF,
                                           @DATANASCIMENTO, 
                                           @CARGO,
                                           @FUNCAO,
                                           @USUARIOID,
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, acusado.OcorrenciaId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, acusado.Tipo);
                contextQuery.Parameters.Add("@DESCONHECIDO", SqlDbType.Bit, acusado.Desconhecido);
                contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, acusado.PessoaId != null && Convert.ToInt32(acusado.PessoaId) > 0 ? (int?)acusado.PessoaId : (int?)null);
                contextQuery.Parameters.Add("@VINCULO", SqlDbType.Decimal, acusado.Vinculo != null && Convert.ToInt32(acusado.Vinculo) > 0 ? (int?)acusado.Vinculo : (int?)null);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, acusado.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, acusado.CPF);
                contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, acusado.DataNascimento != null && acusado.DataNascimento != DateTime.MinValue ? (DateTime?)acusado.DataNascimento : (DateTime?)null);
                contextQuery.Parameters.Add("@CARGO", SqlDbType.VarChar, acusado.Cargo);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, acusado.Funcao);
                contextQuery.Parameters.Add("@RGNUMERO", SqlDbType.VarChar, acusado.RgNumero);
                contextQuery.Parameters.Add("@RGTIPO", SqlDbType.VarChar, acusado.RgTipo);
                contextQuery.Parameters.Add("@RGEMISSOR", SqlDbType.VarChar, acusado.RgEmissor);
                contextQuery.Parameters.Add("@RGUF", SqlDbType.VarChar, acusado.RgUF);
                contextQuery.Parameters.Add("@RGDATAEXP", SqlDbType.VarChar, acusado.RgDataExp);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, acusado.UsuarioId);
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

        public void Remove(int acusadoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Ocorrencias.ACUSADO
                            WHERE  ACUSADOID = @ACUSADOID ";

                contextQuery.Parameters.Add("@ACUSADOID", SqlDbType.Int, acusadoId);

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

        public bool PossuiAcusadoPessoaCorretaPor(DataContext contexto, decimal pessoaCorreta)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                        FROM  Ocorrencias.ACUSADO
                                        WHERE PESSOAID = @PESSOACORRETA ";

            contextQuery.Parameters.Add("@PESSOACORRETA", pessoaCorreta);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void AtualizaPessoaCorreta(DataContext contexto, decimal pessoaCorreta, decimal pessoaErrada, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE A
                                        SET PESSOAID = @PESSOACORRETA,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        FROM Ocorrencias.ACUSADO A	                                 
                                        WHERE PESSOAID = @PESSOAERRADA ";

            contextQuery.Parameters.Add("@PESSOAERRADA", SqlDbType.Int, pessoaErrada);
            contextQuery.Parameters.Add("@PESSOACORRETA", SqlDbType.Int, pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

    }
}
