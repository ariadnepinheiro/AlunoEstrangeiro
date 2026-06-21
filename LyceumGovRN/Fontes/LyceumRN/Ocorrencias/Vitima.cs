using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class Vitima : RNBase
    {
        public enum Tipo
        {
            [StringValue("Aluno")]
            Aluno = 1,
            [StringValue("Servidor")]
            Servidor = 2,
            [StringValue("Unidade de Ensino")]
            Unidade = 3,
            [StringValue("Outros")]
            Outros = 4
        }

        public bool PossuiVitimaPor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Ocorrencias.VITIMA (NOLOCK) 
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
                contextQuery.Command = @" SELECT VITIMAID,
									CASE
										WHEN V.TIPO = 1 THEN 'Aluno'
										WHEN V.TIPO = 2 THEN 'Servidor'
										WHEN V.TIPO = 3 THEN 'Unidade de Ensino'
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
                                FROM Ocorrencias.VITIMA V
									INNER JOIN Ocorrencias.OCORRENCIA O ON V.OCORRENCIAID = O.OCORRENCIAID
									LEFT JOIN LY_PESSOA P ON V.PESSOAID = P.PESSOA
									LEFT JOIN LY_FUNCAO F ON V.FUNCAO = F.FUNCAO
									LEFT JOIN Matricula.PESSOAALUNO PA ON PA.PESSOAID = V.PESSOAID
                                WHERE V.OCORRENCIAID = @OCORRENCIAID ";

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

        public ValidacaoDados Valida(Entidades.Vitima vitima)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (vitima == null)
            {
                return validacaoDados;
            }

            if (vitima.OcorrenciaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (vitima.Tipo != (int)Vitima.Tipo.Aluno
                 && vitima.Tipo != (int)Vitima.Tipo.Servidor
                 && vitima.Tipo != (int)Vitima.Tipo.Unidade
                 && vitima.Tipo != (int)Vitima.Tipo.Outros)
            {
                mensagens.Add("Campo ALVO é inválido.");
            }

            //Verifica se é um aluno
            if (vitima.Tipo == (int)Vitima.Tipo.Aluno)
            {
                vitima.Nome = null;
                vitima.CPF = null;
                vitima.DataNascimento = null;
                vitima.Cargo = null;
                vitima.Funcao = null;
                vitima.RgNumero = null;
                vitima.RgTipo = null;
                vitima.RgEmissor = null;
                vitima.RgUF = null;
                vitima.RgDataExp = null;
                vitima.Vinculo = null;

                if (!vitima.Desconhecido && (vitima.PessoaId == null || vitima.PessoaId <= 0))
                {
                    mensagens.Add("Campo ALUNO ALVO é obrigatório.");
                }
            }
            else if (vitima.Tipo == (int)Vitima.Tipo.Servidor)
            {
                vitima.Nome = null;
                vitima.CPF = null;
                vitima.DataNascimento = null;
                vitima.RgNumero = null;
                vitima.RgTipo = null;
                vitima.RgEmissor = null;
                vitima.RgUF = null;
                vitima.RgDataExp = null;

                if (!vitima.Desconhecido)
                {
                    if (vitima.PessoaId == null || vitima.PessoaId <= 0)
                    {
                        mensagens.Add("Campo ALVO SERVIDOR é obrigatório.");
                    }
                    else if (vitima.Vinculo == null || vitima.Vinculo <= 0)
                    {
                        mensagens.Add("O SERVIDOR está sem Id Funcional / Vínculo cadastrado, seu cadastro deve ser atualizado antes do registro da ocorrência.");
                    }

                    if (vitima.Cargo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo CARGO DO SERVIDOR é obrigatório.");
                    }

                    if (vitima.Funcao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo FUNÇÃO DO SERVIDOR é obrigatório.");
                    }
                }
            }
            else if (vitima.Tipo == (int)Vitima.Tipo.Unidade)
            {
                vitima.Nome = null;
                vitima.CPF = null;
                vitima.DataNascimento = null;
                vitima.Cargo = null;
                vitima.Funcao = null;
                vitima.RgNumero = null;
                vitima.RgTipo = null;
                vitima.RgEmissor = null;
                vitima.RgUF = null;
                vitima.RgDataExp = null;
                vitima.PessoaId = null;
                vitima.Vinculo = null;
            }
            else
            {
                vitima.PessoaId = null;
                vitima.Vinculo = null;
                vitima.Cargo = null;
                vitima.Funcao = null;

                if (!vitima.Desconhecido)
                {
                    if (vitima.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME DO ALVO é obrigatório.");
                    }

                    if (!vitima.CPF.IsNullOrEmptyOrWhiteSpace()
                        && !Utils.ValidarCpf(vitima.CPF))
                    {
                        mensagens.Add("Campo CPF DO ALVO é inválido.");
                    }

                    //Verifica se foi preenchido o tipo do documento
                    if (!vitima.RgTipo.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (Convert.ToString(vitima.RgNumero).IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo NÚMERO DOCUMENTO DO ALVO é obrigatório.");
                        }
                        else
                        {
                            var rg = Utils.RetirarMascara(vitima.RgNumero);

                            if (rg.Length < 5)
                            {
                                mensagens.Add("O NÚMERO DO DOCUMENTO DO ALVO deve conter no mínimo cinco dígitos!");
                            }

                            if (vitima.RgTipo == "RG" && vitima.RgEmissor == "DETRAN" && vitima.RgUF == "RJ")
                            {
                                if (!Validacao.ValidaNumerosInteirosPositivos(vitima.RgNumero))
                                {
                                    mensagens.Add("O número de documento do RG Detran deve conter só números inteiros.");
                                }
                            }
                        }

                        if (vitima.RgTipo == "RG")
                        {
                            if (Convert.ToString(vitima.RgUF).IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo ESTADO DO DOCUMENTO DO ALVO é obrigatório.");
                            }

                            if (Convert.ToString(vitima.RgEmissor).IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo ÓRGÃO EMISSOR DO DOCUMENTO DO ALVO é obrigatório.");
                            }

                            if (vitima.RgDataExp == null || vitima.RgDataExp == DateTime.MinValue)
                            {
                                mensagens.Add("Campo Data de Expedição DO DOCUMENTO DO ALVO é obrigatório.");
                            }
                            else if (Convert.ToDateTime(vitima.RgDataExp).Date > DateTime.Now.Date)
                            {
                                mensagens.Add("Campo Data de Expedição DO DOCUMENTO DO ALVO não pode ser maior que a data atual.");
                            }
                        }
                    }
                }
            }

            if (vitima.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a vitima com pessoa já foi cadastrada para o ocorrencia
                    if (vitima.PessoaId != null && vitima.PessoaId > 0 && this.PossuiPessoaIdPor(contexto, vitima.OcorrenciaId, Convert.ToDecimal(vitima.PessoaId)))
                    {
                        mensagens.Add("Este ALVO já foi cadastrado para esta ocorrência.");
                    }

                    //Verifica se a vitima com cpf já foi cadastrada para o ocorrencia
                    if (!vitima.CPF.IsNullOrEmptyOrWhiteSpace() && this.PossuiCpfPor(contexto, vitima.OcorrenciaId, vitima.CPF))
                    {
                        mensagens.Add("Este ALVO já foi cadastrado para esta ocorrência.");
                    }

                    //Verifica se a vitima é do tipo Unidade
                    if (vitima.Tipo == (int)Vitima.Tipo.Unidade && this.PossuiTipoUnidadePor(contexto, vitima.OcorrenciaId))
                    {
                        //Para Tipo Unidade so pode inserir um registro
                        mensagens.Add("A unidade já foi cadastrada como ALVO para esta ocorrência.");
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

        private bool PossuiTipoUnidadePor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   OCORRENCIAS.VITIMA (NOLOCK) 
                                        WHERE  OCORRENCIAID = @OCORRENCIAID
											AND TIPO = 3 --Unidade";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiPessoaIdPor(DataContext contexto, int ocorrenciaId, decimal pessoaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   OCORRENCIAS.VITIMA (NOLOCK) 
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
                                        FROM   OCORRENCIAS.VITIMA (NOLOCK) 
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

        public void Insere(Entidades.Vitima vitima)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Ocorrencias.VITIMA
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

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, vitima.OcorrenciaId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, vitima.Tipo);
                contextQuery.Parameters.Add("@DESCONHECIDO", SqlDbType.Bit, vitima.Desconhecido);
                contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, vitima.PessoaId != null && Convert.ToInt32(vitima.PessoaId) > 0 ? (int?)vitima.PessoaId : (int?)null);
                contextQuery.Parameters.Add("@VINCULO", SqlDbType.Decimal, vitima.Vinculo != null && Convert.ToInt32(vitima.Vinculo) > 0 ? (int?)vitima.Vinculo : (int?)null);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, vitima.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, vitima.CPF);
                contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, vitima.DataNascimento != null && vitima.DataNascimento != DateTime.MinValue ? (DateTime?)vitima.DataNascimento : (DateTime?)null);
                contextQuery.Parameters.Add("@CARGO", SqlDbType.VarChar, vitima.Cargo);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, vitima.Funcao);
                contextQuery.Parameters.Add("@RGNUMERO", SqlDbType.VarChar, vitima.RgNumero);
                contextQuery.Parameters.Add("@RGTIPO", SqlDbType.VarChar, vitima.RgTipo);
                contextQuery.Parameters.Add("@RGEMISSOR", SqlDbType.VarChar, vitima.RgEmissor);
                contextQuery.Parameters.Add("@RGUF", SqlDbType.VarChar, vitima.RgUF);
                contextQuery.Parameters.Add("@RGDATAEXP", SqlDbType.VarChar, vitima.RgDataExp);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, vitima.UsuarioId);
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

        public void Remove(int vitimaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Ocorrencias.VITIMA
                            WHERE  VITIMAID = @VITIMAID ";

                contextQuery.Parameters.Add("@VITIMAID", SqlDbType.Int, vitimaId);

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


        public bool PossuiVitimaPessoaCorretaPor(DataContext contexto, decimal pessoaCorreta)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                        FROM  Ocorrencias.VITIMA
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
                                        FROM Ocorrencias.VITIMA A	                                 
                                        WHERE PESSOAID = @PESSOAERRADA ";

            contextQuery.Parameters.Add("@PESSOAERRADA", SqlDbType.Int, pessoaErrada);
            contextQuery.Parameters.Add("@PESSOACORRETA", SqlDbType.Int, pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
