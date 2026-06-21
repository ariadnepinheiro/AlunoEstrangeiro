using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class Fornecedor
    {
        public string ObtemCNPJPor(DataContext contexto, int fornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT CNPJ 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID
                                        AND TIPO = 'Pessoa Jurídica'";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemCnpjOuCpfPor(DataContext contexto, int fornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT CNPJ 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemEmailPor(DataContext contexto, int fornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT EMAIL 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool EhFornecedorFinalizado(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0) 
                                        FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                        WHERE FORNECEDORID = @FORNECEDORID 
                                            AND FINALIZADO = 1 ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        public List<DTOs.DadosFormularioFornecedor> ObtemDadosFormularioFornecedorPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim)
        {
            List<DTOs.DadosFormularioFornecedor> lista = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormularioFornecedor>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT F.FORNECEDORID,
	                                        RE.REGIONAL,
	                                        'TODOS' AS MUNICIPIOSATENDIDOS,
                                            F.TIPO,
	                                        CNPJ,
	                                        --R.DESCRICAO AS RAZAOSOCIAL,
                                            [PrestacaoContas].[fn_ObterRazaoSocialDoFornecedorNaDtPgtoDaDespesa](E.EVENTOID) as RAZAOSOCIAL,
	                                        INSCRICAOESTADUAL,
	                                        F.INSCRICAOMUNICIPAL,
	                                        F.ENDERECO,
	                                        ISNULL(F.NUMERO, '') AS NUMERO,
	                                        F.COMPLEMENTO,
	                                        F.BAIRRO,
	                                        F.MUNICIPIOID,
	                                        MU.NOME AS MUNICIPIO,
	                                        MU.UF_SIGLA,
	                                        F.CEP,
	                                        F.TELEFONE,
	                                        F.EMAIL,
	                                        --RL.NOME,
	                                        --RL.CPF
                                            [PrestacaoContas].[fn_ObterRepresentanteLegalDoFornecedorNaDtPgtoDaDespesa](E.EVENTOID, 'NOME') as NOME,
	                                        [PrestacaoContas].[fn_ObterRepresentanteLegalDoFornecedorNaDtPgtoDaDespesa](E.EVENTOID, 'CPF') as CPF
                                        FROM PRESTACAOCONTAS.EVENTO E
	                                        INNER JOIN LY_UNIDADE_ENSINO UE ON E.CENSO = UE.UNIDADE_ENS
	                                        INNER JOIN TCE_REGIONAL RE ON UE.ID_REGIONAL = RE.ID_REGIONAL
	                                        INNER JOIN PRESTACAOCONTAS.FORNECEDOR F ON E.FORNECEDORID = F.FORNECEDORID
	                                        --LEFT JOIN PRESTACAOCONTAS.FORNECEDORRAZAOSOCIAL R
				                            --            ON F.FORNECEDORID = R.FORNECEDORID
				                            --             AND @DATAFIM  BETWEEN R.DATAINICIO AND ISNULL(R.DATAFIM, GETDATE())
	                                        INNER JOIN MUNICIPIO MU ON F.MUNICIPIOID = MU.CODIGO
	                                        --LEFT JOIN PRESTACAOCONTAS.FORNECEDORREPRESENTANTELEGAL RL
				                            --            ON F.FORNECEDORID = RL.FORNECEDORID
				                            --             AND @DATAFIM BETWEEN RL.DATAINICIO AND ISNULL(RL.DATAFIM, GETDATE())
                                        WHERE E.DATAPAGAMENTO BETWEEN @DATAINICIO AND @DATAFIM
	                                        AND E.CENSO = @CENSO
	                                        AND E.APROVADO = 1 ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DTOs.DadosFormularioFornecedor fornecedor = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormularioFornecedor();

                    fornecedor.DiretoriaRegional = Convert.ToString(reader["REGIONAL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["REGIONAL"]);
                    fornecedor.MunicipioAtendidos = Convert.ToString(reader["MUNICIPIOSATENDIDOS"]);
                    fornecedor.Cnpj = Convert.ToString(reader["CNPJ"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["CNPJ"]);
                    fornecedor.RazaoSocial = Convert.ToString(reader["RAZAOSOCIAL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["RAZAOSOCIAL"]);
                    fornecedor.InscricaoEstadual = Convert.ToString(reader["INSCRICAOESTADUAL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["INSCRICAOESTADUAL"]);
                    fornecedor.InscricaoMunicipal = Convert.ToString(reader["INSCRICAOMUNICIPAL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["INSCRICAOMUNICIPAL"]);
                    fornecedor.Endereco = Convert.ToString(reader["ENDERECO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["ENDERECO"]) + ", " + Convert.ToString(reader["NUMERO"]);
                    fornecedor.ComplementoEndereco = Convert.ToString(reader["COMPLEMENTO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["COMPLEMENTO"]);
                    fornecedor.Bairro = Convert.ToString(reader["BAIRRO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["BAIRRO"]);
                    fornecedor.UF = Convert.ToString(reader["UF_SIGLA"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["UF_SIGLA"]);
                    fornecedor.Municipio = Convert.ToString(reader["MUNICIPIO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["MUNICIPIO"]);
                    fornecedor.Cep = Convert.ToString(reader["CEP"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["CEP"]);
                    fornecedor.CaixaPostal = " - ";
                    fornecedor.Email = Convert.ToString(reader["EMAIL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["EMAIL"]);
                    fornecedor.Nome = Convert.ToString(reader["NOME"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["NOME"]);
                    fornecedor.Cpf = Convert.ToString(reader["CPF"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["CPF"]);

                    string telefone = Convert.ToString(reader["TELEFONE"]).RetirarMascaraTelefone();
                    fornecedor.DDD = Convert.ToString(reader["TELEFONE"]).IsNullOrEmptyOrWhiteSpace() ? " - " : telefone.Substring(0, 2);
                    fornecedor.Telefone = Convert.ToString(reader["TELEFONE"]).IsNullOrEmptyOrWhiteSpace() ? " - " : telefone.Substring(2, telefone.Length - 2);

                    lista.Add(fornecedor);
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

        public DTOs.DadosFornecedor ObtemPor(int fornecedorId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT  f.FORNECEDORID
                                            ,frs.FORNECEDORRAZAOSOCIALID
					                        ,f.TIPO
                                            ,f.CNPJ
                                            ,frs.DESCRICAO as RAZAOSOCIAL
                                            ,frs.DATAINICIO
                                            ,frs.DATAFIM
                                            ,f.INSCRICAOESTADUAL
                                            ,f.INSCRICAOMUNICIPAL
                                            ,f.ENDERECO
                                            ,f.NUMERO
                                            ,f.COMPLEMENTO
                                            ,f.BAIRRO
                                            ,f.MUNICIPIOID
                                            ,mun.NOME as MUNICIPIODESCRICAO
                                            ,mun.UF_SIGLA as UF
                                            ,f.CEP
                                            ,f.EMAIL
                                            ,f.TELEFONE
					                        ,f.ENVIADO
					                        ,f.GRANDEPORTE
					                        ,f.EVENTUAL
                                            ,f.FINALIZADO
                                            ,f.USUARIOID
                                            ,f.DATACADASTRO
                                            ,f.DATAALTERACAO
					                        ,CASE
	                                            when ENVIADO = 0 then 'Aguardando envio para análise'
	                                            when FINALIZADO is null then 'Em análise'
	                                            when FINALIZADO = 1 then 'Aprovado'
	                                            when FINALIZADO = 0 then 'Reprovado:' + char(13) + char(10) + (
		                                            select STRING_AGG(DESCRICAO, char(13) + char(10)) as DESCRICAO 
		                                            from PrestacaoContas.MOTIVOREPROVACAOFORNECEDOR mrf (nolock)
		                                            inner join PrestacaoContas.FORNECEDORANALISE__MOTIVOREPROVACAOFORNECEDOR famrf (nolock) on famrf.MOTIVOREPROVACAOFORNECEDORID = mrf.MOTIVOREPROVACAOFORNECEDORID
		                                            inner join (
			                                            select top 1 fa2.FORNECEDORANALISEID, fa2.FORNECEDORID
			                                            from PrestacaoContas.FORNECEDORANALISE (nolock) fa2 
			                                            inner join PrestacaoContas.FORNECEDOR (nolock) f2 on fa2.FORNECEDORID = f2.FORNECEDORID
			                                            where f2.FORNECEDORID = @FORNECEDORID
			                                            and f2.ENVIADO = 1
			                                            and f2.FINALIZADO = 0
			                                            and fa2.APROVADA = 0
			                                            order by fa2.DATACADASTRO desc
		                                            ) fa on fa.FORNECEDORANALISEID = famrf.FORNECEDORANALISEID
	                                            )
                                            END SITUACAO
                                            FROM PrestacaoContas.FORNECEDOR f
						                        left join ( select FORNECEDORID, MAX(DATACADASTRO) DATACADASTRO
									                        from PrestacaoContas.FORNECEDORRAZAOSOCIAL frs (nolock)
									                        where frs.DATAFIM is null
									                        group by FORNECEDORID ) frs_group on frs_group.FORNECEDORID = f.FORNECEDORID
						                        left join PrestacaoContas.FORNECEDORRAZAOSOCIAL frs on frs.FORNECEDORID = frs_group.FORNECEDORID and frs.DATACADASTRO = frs_group.DATACADASTRO AND frs.DATAFIM is null
						                        inner join MUNICIPIO mun on mun.CODIGO = f.MUNICIPIOID
                                            WHERE f.FORNECEDORID = @FORNECEDORID  ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                using (var reader = contexto.GetDataReader(contextQuery))
                    while (reader.Read())
                        return new DTOs.DadosFornecedor
                        {
                            FornecedorId = Convert.ToInt32(reader["FORNECEDORID"]),
                            Situacao = Convert.ToString(reader["SITUACAO"]),
                            FornecedorRazaoSocialId = reader["FORNECEDORRAZAOSOCIALID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["FORNECEDORRAZAOSOCIALID"]) : null,
                            Tipo = Convert.ToString(reader["TIPO"]),
                            Cnpj = Convert.ToString(reader["CNPJ"]),
                            RazaoSocial = reader["RAZAOSOCIAL"] != DBNull.Value ? Convert.ToString(reader["RAZAOSOCIAL"]) : null,
                            DataInicio = reader["DATAINICIO"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["DATAINICIO"]) : null,
                            DataFim = reader["DATAFIM"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["DATAFIM"]) : null,
                            InscricaoEstadual = Convert.ToString(reader["INSCRICAOESTADUAL"]),
                            InscricaoMunicipal = Convert.ToString(reader["INSCRICAOMUNICIPAL"]),
                            Endereco = Convert.ToString(reader["ENDERECO"]),
                            Numero = Convert.ToString(reader["NUMERO"]),
                            Complemento = Convert.ToString(reader["COMPLEMENTO"]),
                            Bairro = Convert.ToString(reader["BAIRRO"]),
                            MunicipioId = Convert.ToString(reader["MUNICIPIOID"]),
                            MunicipioDescricao = Convert.ToString(reader["MUNICIPIODESCRICAO"]),
                            Uf = Convert.ToString(reader["UF"]),
                            Cep = Convert.ToString(reader["CEP"]),
                            Email = Convert.ToString(reader["EMAIL"]),
                            Telefone = Convert.ToString(reader["TELEFONE"]),
                            Enviado = Convert.ToBoolean(reader["ENVIADO"]),
                            GrandePorte = Convert.ToBoolean(reader["GRANDEPORTE"]),
                            Eventual = Convert.ToBoolean(reader["EVENTUAL"]),
                            Finalizado = reader["FINALIZADO"] != DBNull.Value ? (bool?)Convert.ToBoolean(reader["FINALIZADO"]) : null,
                            UsuarioId = Convert.ToString(reader["USUARIOID"]),
                            DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]),
                            DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]),
                        };

                return null;
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
        }

        public bool EhEnviadoAnalisePor(DataContext ctx, int fornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  SELECT COUNT(0) 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID
                                          AND ENVIADO = 1 ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool ExistePor(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  SELECT COUNT(0) 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE FORNECEDORID = @FORNECEDORID ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        public bool ExistePor(DataContext ctx, string cnpj)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  SELECT COUNT(0) 
                                    FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                    WHERE CNPJ = @CNPJ ";

                contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, cnpj.RetirarMascaraCNPJ());

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        public bool ExistePor(string cnpj)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                return ExistePor(contexto, cnpj);
            }
            catch (Exception ex)
            {
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
        }

        private bool PossuiOutroCnpjPor(DataContext ctx, string cnpj, int? fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0) 
                                        FROM PrestacaoContas.FORNECEDOR (NOLOCK)
                                        WHERE CNPJ = @CNPJ
                                            AND (FORNECEDORID <> @FORNECEDORID OR @FORNECEDORID IS NULL) ";

                contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, cnpj);
                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId ?? (object)DBNull.Value);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
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
        }

        /// <summary>
        /// Valida os dados da DTO, para que esteja apta a ser recebida pelo Insere() e Atualiza()
        /// TABELAS: PrestacaoContas.FORNECEDOR e PrestacaoContas.FORNECEDORRAZAOSOCIAL
        /// </summary>
        /// <param name="dadosFornecedor">DTO que contém os campos necessários para inserir e atualizar o fornecedor</param>
        /// <param name="cadastro"></param>
        /// <returns></returns>
        public ValidacaoDados Valida(DTOs.DadosFornecedor dadosFornecedor, bool cadastro)
        {
            RN.PrestacaoContas.FornecedorRazaoSocial rnFornecedorRazaoSocial = new FornecedorRazaoSocial();
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            if (!cadastro)
            {
                if (dadosFornecedor.FornecedorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (dadosFornecedor.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO FORNECEDOR é obrigatório.");
            }
            else if (dadosFornecedor.Tipo != "Pessoa Física" && dadosFornecedor.Tipo != "Pessoa Jurídica")
            {
                mensagens.Add("Campo TIPO FORNECEDOR deve ser 'Pessoa Física' ou 'Pessoa Jurídica'.");
            }

            if (dadosFornecedor.Cnpj.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("CNPJ / CPF: Preenchimento obrigatório");
            }
            else
            {               
                if (dadosFornecedor.Tipo == "Pessoa Jurídica")
                {
                    string cnpj = dadosFornecedor.Cnpj.RetirarMascaraCNPJ();
                    if (!RN.Validacao.ValidaCnpj(cnpj))
                    {
                        mensagens.Add("O CNPJ é inválido.");
                    }

                    //Valida inscricao estadual / municipal para empresa que nao seja grande porte(cadastro simplificado) ou eventual
                    if (!dadosFornecedor.GrandePorte && !dadosFornecedor.Eventual)
                    {
                        if (dadosFornecedor.InscricaoEstadual.IsNullOrEmptyOrWhiteSpace() && dadosFornecedor.InscricaoMunicipal.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("INSCRIÇÃO ESTADUAL / MUNICIPAL: Preenchimento obrigatório");
                        }
                        else
                        {
                            if (!dadosFornecedor.InscricaoEstadual.IsNullOrEmptyOrWhiteSpace() && dadosFornecedor.InscricaoEstadual.Length > 50)
                            {
                                mensagens.Add("INSCRIÇÃO ESTADUAL: Não pode ter mais do que 50 caracteres");
                            }

                            if (!dadosFornecedor.InscricaoMunicipal.IsNullOrEmptyOrWhiteSpace() && dadosFornecedor.InscricaoMunicipal.Length > 50)
                            {
                                mensagens.Add("INSCRIÇÃO MUNICIPAL: Não pode ter mais do que 50 caracteres");
                            }
                        }
                    }
                }
                else
                {
                    string cpf = dadosFornecedor.Cnpj.RetirarMascaraCPF();
                    if (!RN.Validacao.ValidaCpf(cpf))
                    {
                        mensagens.Add("O CPF é inválido.");
                    }

                    if (dadosFornecedor.GrandePorte )
                    {
                        mensagens.Add("Apenas fornecedores do tipo Pessoa Jurídica podem ser marcados como Cadastro Simplificado.");                        
                    }
                }
            }

            if (dadosFornecedor.RazaoSocial.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("RAZÃO SOCIAL: Preenchimento obrigatório");
            }
            else
            {
                if (dadosFornecedor.RazaoSocial.Length > 500)
                {
                    mensagens.Add("RAZÃO SOCIAL: Não pode ter mais do que 500 caracteres");
                }
            }

            if (dadosFornecedor.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("CEP: Preenchimento obrigatório");
            }
            else
            {
                if (dadosFornecedor.Cep.Length > 8)
                {
                    mensagens.Add("CEP: Não pode ter mais do que 8 números");
                }
            }

            if (dadosFornecedor.MunicipioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("MUNICÍPIO: Preenchimento obrigatório");
            }

            if (dadosFornecedor.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("ENDEREÇO: Preenchimento obrigatório");
            }
            else
            {
                if (dadosFornecedor.Endereco.Length > 255)
                {
                    mensagens.Add("ENDEREÇO: Não pode ter mais do que 255 caracteres");
                }
            }
            if (dadosFornecedor.Numero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("NÚMERO: Preenchimento obrigatório");
            }
            else
            {
                if (dadosFornecedor.Numero.Length > 50)
                {
                    mensagens.Add("NÚMERO: Não pode ter mais do que 50 caracteres");
                }
            }

            if (dadosFornecedor.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("BAIRRO: Preenchimento obrigatório");
            }
            else
            {
                if (dadosFornecedor.Bairro.Length > 255)
                {
                    mensagens.Add("BAIRRO: Não pode ter mais do que 255 caracteres");
                }
            }

            // Valida o e-mail apenas se foi informado
            if (!dadosFornecedor.Email.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosFornecedor.Email.Length > 255)
                {
                    mensagens.Add("E-MAIL: Não pode ter mais do que 255 caracteres");
                }

                if (!RN.Validacao.ValidaEmail(dadosFornecedor.Email))
                {
                    mensagens.Add("Campo E-MAIL inválido.");
                }
            }


            // Valida o telefone apenas se foi informado
            if (!dadosFornecedor.Telefone.IsNullOrEmptyOrWhiteSpace())
            {
                var telefone = dadosFornecedor.Telefone.RetirarMascaraTelefone();

                if (telefone.Length > 11)
                {
                    mensagens.Add("TELEFONE: Não pode ter mais do que 11 números");
                }

                if (!Validacao.ValidaTelefoneComDDD(telefone) && !Validacao.ValidaCelularComDDD(telefone))
                {
                    mensagens.Add("Campo TELEFONE é inválido.");
                }
            }
        

            if (dadosFornecedor.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiOutroCnpjPor(contexto, dadosFornecedor.Cnpj, dadosFornecedor.FornecedorId))
                    {
                        mensagens.Add("CNPJ / CPF: Já existe cadastrado para outro fornecedor");
                    }

                    if (!RN.Municipio.ValidarMunicipio(dadosFornecedor.MunicipioId))
                    {
                        mensagens.Add("MUNICÍPIO: Este ID não existe cadastrado no banco de dados");
                    }

                    //if (!cadastro)
                    //{
                    //    DateTime fim = DateTime.Now.AddDays(-1);

                    //    DateTime? inicio = rnFornecedorRazaoSocial.ObtemInicioRazaoSocialAtivaPor(contexto, dadosFornecedor.FornecedorId.Value);

                    //    if (fim < inicio)
                    //    {
                    //        mensagens.Add("A DATA FIM da razão social ativa não pode ser menor que a DATA INÍCIO.");
                    //    }
                    //}
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

        public ValidacaoDados ValidaEnvioAnalise(int fornecedorId, string usuarioId, string tipo, bool grandePorte, bool eventual)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            FornecedorRazaoSocial rnFornecedorRazaoSocial = new FornecedorRazaoSocial();
            FornecedorRepresentanteLegal rnFornecedorRepresentanteLegal = new FornecedorRepresentanteLegal();
            DocumentosFornecedor rnDocumentosFornecedor = new DocumentosFornecedor();

            if (fornecedorId <= 0)
            {
                mensagens.Add("Campo FORNECEDOR é obrigatório.");
            }

            if (tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO FORNECEDOR é obrigatório.");
            }
            else if (tipo != "Pessoa Física" && tipo != "Pessoa Jurídica")
            {
                mensagens.Add("Campo TIPO FORNECEDOR deve ser 'Pessoa Física' ou 'Pessoa Jurídica'.");

            }
            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi enviado para analise
                    if (this.EhEnviadoAnalisePor(contexto, fornecedorId))
                    {
                        mensagens.Add("Este fornecedor já foi enviado para análise.");
                    }
                    else
                    {
                        //Valida habilitação apenas para empresas que não sejam de grande porte(cadastro simplificado)
                        if (!grandePorte && !eventual && !this.PossuiHabilitacaoPor(contexto, fornecedorId))
                        {
                            mensagens.Add("HABILITAÇÃO: tem que ter pelo menos 1 habilitação vigente para poder enviar o fornecedor para análise.");
                        }

                        if (!rnFornecedorRazaoSocial.PossuiRazaoSocialVigentePor(contexto, fornecedorId, DateTime.Now))
                        {
                            mensagens.Add("RAZÃO SOCIAL: tem que ter pelo menos 1 razão social vigente para poder enviar o fornecedor para análise.");
                        }

                        //Valida representante legal apenas para empresas que não sejam de grande porte(cadastro simplificado)
                        if (!grandePorte && !eventual && !rnFornecedorRepresentanteLegal.TemExatamenteUmRepresentantesLegaisVigentes(contexto, fornecedorId))
                        {
                            mensagens.Add("REPRESENTANTE LEGAL: tem que ter exatamente 1 representante legal vigente para poder enviar o fornecedor para análise.");
                        }

                        //Valida documentos anexados apenas para empresas que não sejam de grande porte(cadastro simplificado)
                        if (!grandePorte && !eventual)
                        {
                            //Verifica se algum documento não foi enviado
                            if (rnDocumentosFornecedor.PossuiDocumentosObrigatoriosPendentes(contexto, fornecedorId, tipo))
                            {
                                mensagens.Add("DOCUMENTOS: todos os arquivos dos documentos obrigatórios precisam estar cadastrados para poder enviar o fornecedor para análise.");
                            }
                            else if(rnDocumentosFornecedor.PossuiDocumentosForaDataAtual(contexto, fornecedorId))
                            {
                                mensagens.Add("DOCUMENTOS: todos os documentos obrigatórios precisam estar vigentes para poder enviar o fornecedor para análise.");
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

        private bool PossuiHabilitacaoPor(DataContext ctx, int fornecedorId)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0)
                                          FROM PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO
                                          WHERE FORNECEDORID = @FORNECEDORID ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                return ctx.GetReturnValue<int>(contextQuery) >= 1;
            }
            catch (Exception ex)
            {
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
        }

        /// <summary>
        /// Insere os dados do fornecedor.
        /// TABELAS: PrestacaoContas.FORNECEDOR e PrestacaoContas.FORNECEDORRAZAOSOCIAL
        /// </summary>
        /// <param name="dadosFornecedor">DTO que contém os campos necessários para inserir o fornecedor</param>
        public void Insere(DTOs.DadosFornecedor dadosFornecedor)
        {
            DataContext contexto = null;
            Entidades.Fornecedor fornecedor = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Fornecedor();
            Entidades.FornecedorRazaoSocial fornecedorRazaoSocial = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorRazaoSocial();
            FornecedorRazaoSocial rnFornecedorRazaoSocial = new FornecedorRazaoSocial();
            Entidades.FornecedorRepresentanteLegal fornecedorRepresentanteLegal = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorRepresentanteLegal();
            FornecedorRepresentanteLegal rnFornecedorRepresentanteLegal = new FornecedorRepresentanteLegal();

            try
            {
                /*
                Ao incluir o fornecedor o sistema irá cadastrar a primeira razão social do fornecedor com a 
                Data de Inicio da razão social igual a data do cadastro do fornecedor e a data de fim da 
                razão social como nula. Tabela RAZAOSOCIALFORNECEDOR  campos DATAINICIO = GETDATE() e DATAFIM = NULL
                */

                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Monta a entidade do fornecedor
                fornecedor.Tipo = dadosFornecedor.Tipo;
                fornecedor.Cnpj = dadosFornecedor.Cnpj;
                fornecedor.InscricaoEstadual = dadosFornecedor.InscricaoEstadual;
                fornecedor.InscricaoMunicipal = dadosFornecedor.InscricaoMunicipal;
                fornecedor.Endereco = dadosFornecedor.Endereco;
                fornecedor.Numero = dadosFornecedor.Numero;
                fornecedor.Complemento = dadosFornecedor.Complemento;
                fornecedor.Bairro = dadosFornecedor.Bairro;
                fornecedor.MunicipioId = dadosFornecedor.MunicipioId;
                fornecedor.Cep = dadosFornecedor.Cep;
                fornecedor.Email = dadosFornecedor.Email;
                fornecedor.Telefone = dadosFornecedor.Telefone;
                fornecedor.GrandePorte = dadosFornecedor.GrandePorte;
                fornecedor.Eventual = dadosFornecedor.Eventual;
                fornecedor.Enviado = false; //Inicia sempre como false
                fornecedor.Finalizado = null; //inicia sempre como null
                fornecedor.UsuarioId = dadosFornecedor.UsuarioId;

                //Insere fornecedor
                this.Insere(contexto, fornecedor);
                dadosFornecedor.FornecedorId = fornecedor.FornecedorId;

                //Monta razão social
                fornecedorRazaoSocial.FornecedorId = Convert.ToInt32(dadosFornecedor.FornecedorId);
                fornecedorRazaoSocial.Descricao = dadosFornecedor.RazaoSocial;
                fornecedorRazaoSocial.UsuarioId = dadosFornecedor.UsuarioId;

                //Insere razão social
                rnFornecedorRazaoSocial.Insere(contexto, fornecedorRazaoSocial);

                //Se for pessoa física, monta e insere o representante legal
                if (fornecedor.Tipo == "Pessoa Física") 
                {
                    fornecedorRepresentanteLegal.FornecedorId = Convert.ToInt32(dadosFornecedor.FornecedorId);
                    fornecedorRepresentanteLegal.Nome = dadosFornecedor.RazaoSocial;
                    fornecedorRepresentanteLegal.Cpf = dadosFornecedor.Cnpj;
                    fornecedorRepresentanteLegal.DataInicio = DateTime.Now;
                    fornecedorRepresentanteLegal.UsuarioId = dadosFornecedor.UsuarioId;

                    rnFornecedorRepresentanteLegal.Insere(contexto, fornecedorRepresentanteLegal);
                }
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
        }

        private void Insere(DataContext contexto, Entidades.Fornecedor fornecedor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"
                insert into HADES.SiteFornecedor.AspNetUsers (
                    Id
                    ,UserName
                    ,NormalizedUserName
                    ,Email
                    ,NormalizedEmail
                    ,EmailConfirmed
                    ,SecurityStamp
                    ,PhoneNumberConfirmed
                    ,TwoFactorEnabled
                    ,LockoutEnabled
                    ,AccessFailedCount
                )
                values (
                    newid()
                    ,@CNPJ
                    ,upper(@CNPJ)
                    ,@EMAIL
                    ,upper(@EMAIL)
                    ,0
                    ,newid()
                    ,0
                    ,0
                    ,1
                    ,0
                )
 
                INSERT INTO PrestacaoContas.FORNECEDOR
                        (
                        TIPO
                        ,CNPJ
                        ,INSCRICAOESTADUAL
                        ,INSCRICAOMUNICIPAL
                        ,ENDERECO
                        ,NUMERO
                        ,COMPLEMENTO
                        ,BAIRRO
                        ,MUNICIPIOID
                        ,CEP
                        ,EMAIL
                        ,TELEFONE
                        ,ENVIADO
                        ,FINALIZADO
                        ,GRANDEPORTE
                        ,EVENTUAL
                        ,USUARIOID
                        ,DATACADASTRO
                        ,DATAALTERACAO
                        )
                    VALUES
                        (
                        @TIPO
                        ,@CNPJ
                        ,@INSCRICAOESTADUAL
                        ,@INSCRICAOMUNICIPAL
                        ,@ENDERECO
                        ,@NUMERO
                        ,@COMPLEMENTO
                        ,@BAIRRO
                        ,@MUNICIPIOID
                        ,@CEP
                        ,@EMAIL
                        ,@TELEFONE
                        ,@ENVIADO
                        ,@FINALIZADO
                        ,@GRANDEPORTE
                        ,@EVENTUAL
                        ,@USUARIOID
                        ,@DATACADASTRO
                        ,@DATAALTERACAO
                        )

               SELECT FORNECEDORID 
               from PrestacaoContas.FORNECEDOR 
               where CNPJ = @CNPJ 
                ";

            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, fornecedor.Tipo);
            contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, fornecedor.Cnpj);
            contextQuery.Parameters.Add("@INSCRICAOESTADUAL", SqlDbType.VarChar, fornecedor.InscricaoEstadual);
            contextQuery.Parameters.Add("@INSCRICAOMUNICIPAL", SqlDbType.VarChar, fornecedor.InscricaoMunicipal);
            contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, fornecedor.Endereco);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, fornecedor.Numero);
            contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, fornecedor.Complemento);
            contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, fornecedor.Bairro);
            contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, fornecedor.MunicipioId);
            contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, fornecedor.Cep);
            contextQuery.Parameters.Add("@EMAIL", SqlDbType.VarChar, fornecedor.Email);
            contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, fornecedor.Telefone);
            contextQuery.Parameters.Add("@ENVIADO", SqlDbType.Bit, fornecedor.Enviado);
            contextQuery.Parameters.Add("@GRANDEPORTE", SqlDbType.Bit, fornecedor.GrandePorte);
            contextQuery.Parameters.Add("@EVENTUAL", SqlDbType.Bit, fornecedor.Eventual);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, fornecedor.Finalizado == null ? (object)DBNull.Value : fornecedor.Finalizado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedor.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            fornecedor.FornecedorId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        /*
         Não existe forma de enviar e-mail de confirmação e e-mail de reset de senha
         pelo Conexão Gestão. O motivo é que, por questões de segurança, estes e-mails
         dependem da geração do token feita pelo ASP.NET Identity, mecanismo incompatível
         com o .NET Framework 3.5.
         
         A forma correta de se fazer isso é acionar o mecanismo de geração destes e-mails
         no Site do Fornecedor, que é o responsável pela autenticação do usuário e usa
         o ASP.NET Identity. Isso pode ser feito expondo um endpoint do Site do Fornecedor
         para que o Conexão Gestão "faça o disparo" destes e-mails.
         
         Dessa forma, o mecanismo de geração dos e-mails fica concentrado em apenas
         1 sistema responsável por isso, tonando ambos os sistemas mais manteníveis.
         */

        //public void EnviaEmailConfirmacao(string email, string razaoSocial)
        //{
        //    EmailApi rnEmailApi = new EmailApi();

        //    try
        //    {
        //        //Dados Gerais de email
        //        var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
        //        var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);

        //        //Dados Email Fornecedor
        //        var from = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_From"];
        //        var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_FromName"];
        //        //var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Bcc"];
        //        //var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_BccName"];                
        //        var userName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_UserName"];
        //        var password = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Password"];
        //        var subject = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Subject"];
        //        var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_UrlRoot"];

        //        string emailMessage = @" FORNECEDOR CADASTRADO - EMAIL DE CONFIRMACAO - VER CONTEUDO  ";               

        //        var emailObject = new RN.Util.EmailApi.EmailDTO
        //        {
        //            Smtp = new EmailApi.EmailDTO.SmtpDTO
        //            {
        //                Host = host,
        //                Port = port,
        //                UserName = userName,
        //                Password = password,
        //                EnableSSL = true,
        //            },
        //            Message = new EmailApi.EmailDTO.MessageDTO
        //            {
        //                From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
        //                {
        //                    Address = from,
        //                    Name = fromName
        //                },
        //                To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
        //                {
        //                    new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = razaoSocial },
        //                },
        //                Subject = string.Format("{0} - Confirmação", subject),
        //                Body = emailMessage,
        //                IsBodyHtml = true,
        //            },
        //        };

        //        //if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
        //        //    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

        //        var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void EnviaEmailAlteracaoSenha(string email, string razaoSocial)
        //{
        //    EmailApi rnEmailApi = new EmailApi();

        //    try
        //    {
        //        //Dados Gerais de email
        //        var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
        //        var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);

        //        //Dados Email Fornecedor
        //        var from = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_From"];
        //        var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_FromName"];
        //        //var bcc = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Bcc"];
        //        //var bccName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_BccName"];                
        //        var userName = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_UserName"];
        //        var password = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Password"];
        //        var subject = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_Subject"];
        //        var urlRoot = System.Configuration.ConfigurationManager.AppSettings["EmailFornecedor_UrlRoot"];

        //        string emailMessage = @" FORNECEDOR CADASTRADO - ALTERACAO DE SENHA - VER CONTEUDO  ";

        //        var emailObject = new RN.Util.EmailApi.EmailDTO
        //        {
        //            Smtp = new EmailApi.EmailDTO.SmtpDTO
        //            {
        //                Host = host,
        //                Port = port,
        //                UserName = userName,
        //                Password = password,
        //                EnableSSL = true,
        //            },
        //            Message = new EmailApi.EmailDTO.MessageDTO
        //            {
        //                From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
        //                {
        //                    Address = from,
        //                    Name = fromName
        //                },
        //                To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
        //                {
        //                    new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = razaoSocial },
        //                },
        //                Subject = string.Format("{0} - Alteração de Senha", subject),
        //                Body = emailMessage,
        //                IsBodyHtml = true,
        //            },
        //        };

        //        //if (!bcc.IsNullOrEmptyOrWhiteSpace() && !bccName.IsNullOrEmptyOrWhiteSpace())
        //        //    emailObject.Message.Bcc.Add(new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = bcc, Name = bccName });

        //        var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Atualiza os dados do fornecedor
        /// TABELAS: PrestacaoContas.FORNECEDOR e PrestacaoContas.FORNECEDORRAZAOSOCIAL
        /// </summary>
        /// <param name="dadosFornecedor">DTO que contém os campos necessários para atualizar o fornecedor</param>
        public void Atualiza(DTOs.DadosFornecedor dadosFornecedor)
        {
            Entidades.Fornecedor fornecedor = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Fornecedor();
            Entidades.FornecedorRazaoSocial fornecedorRazaoSocial = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorRazaoSocial();
            FornecedorRazaoSocial rnFornecedorRazaoSocial = new FornecedorRazaoSocial();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Monta a entidade do fornecedor
                fornecedor.FornecedorId = Convert.ToInt32(dadosFornecedor.FornecedorId);
                fornecedor.Tipo = dadosFornecedor.Tipo;
                fornecedor.Cnpj = dadosFornecedor.Cnpj;
                fornecedor.InscricaoEstadual = dadosFornecedor.InscricaoEstadual;
                fornecedor.InscricaoMunicipal = dadosFornecedor.InscricaoMunicipal;
                fornecedor.Endereco = dadosFornecedor.Endereco;
                fornecedor.Numero = dadosFornecedor.Numero;
                fornecedor.Complemento = dadosFornecedor.Complemento;
                fornecedor.Bairro = dadosFornecedor.Bairro;
                fornecedor.MunicipioId = dadosFornecedor.MunicipioId;
                fornecedor.Cep = dadosFornecedor.Cep;
                fornecedor.Email = dadosFornecedor.Email;
                fornecedor.Telefone = dadosFornecedor.Telefone;
                fornecedor.GrandePorte = dadosFornecedor.GrandePorte;
                fornecedor.Eventual = dadosFornecedor.Eventual;
                fornecedor.Enviado = false; //Quando é alterado volta para false e tem q enviar novamente para analise
                fornecedor.Finalizado = null; //Quando é alterado volta para null 
                fornecedor.UsuarioId = dadosFornecedor.UsuarioId;

                //pega o e-mail original, isto é, antes de qualquer update do registro
                var emailOriginal = ObtemEmailPor(contexto, fornecedor.FornecedorId);
                var cnpjOriginal = ObtemCnpjOuCpfPor(contexto, fornecedor.FornecedorId);

                //Atualiza fornecedor
                this.Atualiza(contexto, fornecedor);

                //Verifica se houve alteração de CNPJ
                if (cnpjOriginal != fornecedor.Cnpj)
                {
                    //verifica se está como "Aguardando envio para análise"
                    if (this.ObtemSituacaoPor(fornecedor.FornecedorId) == "Aguardando envio para análise")
                    {
                        //alterar CNPJ
                        this.AtualizaCnpj(contexto, fornecedor.FornecedorId, fornecedor.Cnpj, fornecedor.UsuarioId);

                        //alterar o CNPJ no SiteFornecedor
                        this.AtualizaIdentityUserName(cnpjOriginal, fornecedor.Cnpj);
                    }
                }

                //Verificar se houve alteração de e-mail
                if (emailOriginal != fornecedor.Email)
                {
                    if (!fornecedor.Email.IsNullOrEmptyOrWhiteSpace() && !emailOriginal.IsNullOrEmptyOrWhiteSpace())
                    {
                        //solicitar alteração do e-mail ao ASP.Net identity do Site Fornecedor
                        string resultadoTrocaEmail = TrocaEmailNoIdentityDoSiteFornecedor(fornecedor.Cnpj, fornecedor.Email);

                        //Se der erro, transformar em Exception e abortar a transação
                        if (new string[] { "E-mail não encontrado", "Parâmetros incorretos" }.Contains(resultadoTrocaEmail))
                            throw new Exception(resultadoTrocaEmail);
                    }
                }    

                //Monta razão social
                fornecedorRazaoSocial.FornecedorId = Convert.ToInt32(dadosFornecedor.FornecedorId);
                fornecedorRazaoSocial.Descricao = dadosFornecedor.RazaoSocial;
                fornecedorRazaoSocial.UsuarioId = dadosFornecedor.UsuarioId;
                fornecedorRazaoSocial.DataInicio = DateTime.Now;

                //Insere razão social
                rnFornecedorRazaoSocial.AtualizaFornecedor(contexto, fornecedorRazaoSocial);
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

        private void Atualiza(DataContext contexto, Entidades.Fornecedor fornecedor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  
                UPDATE PrestacaoContas.FORNECEDOR 
                SET
                    TIPO = @TIPO
                    ,INSCRICAOESTADUAL = @INSCRICAOESTADUAL
                    ,INSCRICAOMUNICIPAL = @INSCRICAOMUNICIPAL
                    ,ENDERECO = @ENDERECO
                    ,NUMERO = @NUMERO
                    ,COMPLEMENTO = @COMPLEMENTO
                    ,BAIRRO = @BAIRRO
                    ,MUNICIPIOID = @MUNICIPIOID
                    ,CEP = @CEP
                    ,EMAIL = @EMAIL
                    ,TELEFONE = @TELEFONE
                    ,ENVIADO = @ENVIADO
                    ,FINALIZADO = @FINALIZADO
                    ,GRANDEPORTE = @GRANDEPORTE
                    ,EVENTUAL = @EVENTUAL
                    ,USUARIOID = @USUARIOID
                    ,DATAALTERACAO = @DATAALTERACAO
                WHERE FORNECEDORID = @FORNECEDORID
            ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedor.FornecedorId);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, fornecedor.Tipo);
            contextQuery.Parameters.Add("@CNPJ", SqlDbType.VarChar, fornecedor.Cnpj);
            contextQuery.Parameters.Add("@INSCRICAOESTADUAL", SqlDbType.VarChar, fornecedor.InscricaoEstadual);
            contextQuery.Parameters.Add("@INSCRICAOMUNICIPAL", SqlDbType.VarChar, fornecedor.InscricaoMunicipal);
            contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, fornecedor.Endereco);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, fornecedor.Numero);
            contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, fornecedor.Complemento);
            contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, fornecedor.Bairro);
            contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, fornecedor.MunicipioId);
            contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, fornecedor.Cep);
            contextQuery.Parameters.Add("@EMAIL", SqlDbType.VarChar, fornecedor.Email);
            contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, fornecedor.Telefone);
            contextQuery.Parameters.Add("@ENVIADO", SqlDbType.Bit, fornecedor.Enviado);
            contextQuery.Parameters.Add("@GRANDEPORTE", SqlDbType.Bit, fornecedor.GrandePorte);
            contextQuery.Parameters.Add("@EVENTUAL", SqlDbType.Bit, fornecedor.Eventual);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, fornecedor.Finalizado == null ? (object)DBNull.Value : fornecedor.Finalizado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedor.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizaCnpj(DataContext contexto, int fornecedorId, string cnpjNovo, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  
                UPDATE LYCEUM.PrestacaoContas.FORNECEDOR SET CNPJ = @CNPJ_NOVO, USUARIOID = @USUARIOID, DATAALTERACAO = @DATAALTERACAO WHERE FORNECEDORID = @FORNECEDORID
            ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
            contextQuery.Parameters.Add("@CNPJ_NOVO", SqlDbType.VarChar, cnpjNovo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizaIdentityUserName(string cnpjOriginal, string cnpjNovo)
        {
            /*
             * Precisa rodar em uma transação exclusiva, pois foram observados problemas referente
             * ao registro preso por transação na hora de fazer um UPDATE na tabela AspNetUsers do 
             * lado do SiteFornecedor. Esse registro preso estava sendo causado pela transação de
             * atualização da tela do fornecedor no Gestão.
             */

            DataContext contexto = DataContextBuilder.FromHades.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  
                UPDATE HADES.SiteFornecedor.AspNetUsers set UserName = @CNPJ_NOVO, NormalizedUserName = UPPER(@CNPJ_NOVO) WHERE UserName = @CNPJ_ORIGINAL
                ";

                contextQuery.Parameters.Add("@CNPJ_ORIGINAL", SqlDbType.VarChar, cnpjOriginal);
                contextQuery.Parameters.Add("@CNPJ_NOVO", SqlDbType.VarChar, cnpjNovo);

                contexto.ApplyModifications(contextQuery);
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

        public void AtualizaSituacao(DataContext contexto, int fornecedorId, bool aprovado, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE PrestacaoContas.FORNECEDOR 
                                        SET FINALIZADO = @FINALIZADO
                                            ,USUARIOID = @USUARIOID
                                            ,DATAALTERACAO = @DATAALTERACAO
                                        WHERE FORNECEDORID = @FORNECEDORID ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, aprovado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void EnviaAnalise(int fornecedorId, string usuarioId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE PrestacaoContas.FORNECEDOR 
                                            set ENVIADO = 1,
                                                FINALIZADO = NULL,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO = GETDATE()
                                        WHERE FORNECEDORID = @FORNECEDORID ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

                contexto.ApplyModifications(contextQuery);
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

        public string ObtemSituacaoPor(int fornecedorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" 
                SELECT CASE
	                when ENVIADO = 0 then 'Aguardando envio para análise'
	                when FINALIZADO is null then 'Em análise'
	                when FINALIZADO = 1 then 'Aprovado'
	                when FINALIZADO = 0 then 'Reprovado:' + char(13) + char(10) + (
		                select STRING_AGG(DESCRICAO, char(13) + char(10)) as DESCRICAO 
		                from PrestacaoContas.MOTIVOREPROVACAOFORNECEDOR mrf (nolock)
		                inner join PrestacaoContas.FORNECEDORANALISE__MOTIVOREPROVACAOFORNECEDOR famrf (nolock) on famrf.MOTIVOREPROVACAOFORNECEDORID = mrf.MOTIVOREPROVACAOFORNECEDORID
		                inner join (
			                select top 1 fa2.FORNECEDORANALISEID, fa2.FORNECEDORID
			                from PrestacaoContas.FORNECEDORANALISE (nolock) fa2 
			                inner join PrestacaoContas.FORNECEDOR (nolock) f2 on fa2.FORNECEDORID = f2.FORNECEDORID
			                where f2.FORNECEDORID = @FORNECEDORID
			                and f2.ENVIADO = 1
			                and f2.FINALIZADO = 0
			                and fa2.APROVADA = 0
			                order by fa2.DATACADASTRO desc
		                ) fa on fa.FORNECEDORANALISEID = famrf.FORNECEDORANALISEID
	                )
                END SITUACAO
                FROM PrestacaoContas.FORNECEDOR F (NOLOCK)
                where FORNECEDORID = @FORNECEDORID
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public bool PossuiProdutoServicoGrupoPor(DataContext contexto, int produtoServicoGrupoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.FORNECEDOR__PRODUTOSERVICOGRUPO  (NOLOCK)
                                    WHERE PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        #region Disparo de Email de Confirmação e Email de Redefinição de Senha

        public string EnviaEmailConfirmacao(string userName)
        {
            var result = new List<string>();
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["SiteFornecedorEmailConfirmacaoUrl"];

                var response = PostWebApi(null, null, url + "?userName=" + userName);
                result.Add("response: " + response);

                return result.Aggregate((current, next) => current + Environment.NewLine + next);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404)"))
                    throw new Exception("O e-mail já havia sido confirmado.");

                throw new Exception("Erro ao enviar e-mail: " + ex.Message);
            }
        }

        public string TrocaEmailNoIdentityDoSiteFornecedor(string userName, string novoEmail)
        {
            var result = new List<string>();
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["SiteFornecedorTrocarEmailUrl"];

                var response = PostWebApi(null, null, url + "?userName=" + userName + "&novoEmail=" + novoEmail + "&senha=2LxW5QNlsA1LHl%!m92g03Jn%@Q67964f%26krCGr54S@Z9RDixs");
                result.Add("response: " + response);

                return result.Aggregate((current, next) => current + Environment.NewLine + next);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404)"))
                    throw new Exception("E-mail não encontrado no Fornecedor Seeduc");

                if (ex.Message.Contains("(400)"))
                    throw new Exception("Parâmetros incorretos na hora de trocar o e-mail no Fornecedor Seeduc");

                throw new Exception("Erro ao enviar e-mail: " + ex.Message);
            }
        }

        public string EnviaEmailRedefinicaoSenha(string userName)
        {
            var result = new List<string>();
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["SiteFornecedorEmailRedefinicaoSenhaUrl"];

                var response = PostWebApi(null, null, url + "?userName=" + userName);
                result.Add("response: " + response);

                return result.Aggregate((current, next) => current + Environment.NewLine + next);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail: " + ex.Message);
            }
        }

        private string PostWebApi(string data, string bearerToken, string url)
        {
            // Create a request using a URL that can receive a post.
            WebRequest request = WebRequest.Create(url);

            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(data ?? string.Empty);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";

            if (!bearerToken.IsNullOrEmptyOrWhiteSpace())
                request.Headers.Add("Authorization", "Bearer " + bearerToken);

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            string responseFromServer = string.Empty;
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.
                responseFromServer = reader.ReadToEnd();
            }

            // Close the response.
            response.Close();

            return responseFromServer;
        }

        #endregion
    }
}
