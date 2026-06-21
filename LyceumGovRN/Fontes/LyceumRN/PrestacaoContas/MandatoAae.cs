using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class MandatoAae
    {
        public DataTable ListaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT M.MANDATOAAEID,
		                                    CONVERT(VARCHAR(20), M.DATAINICIOMANDATO, 103) AS DATAINICIOMANDATO,
		                                    CONVERT(VARCHAR(20), M.DATAFIMMANDATO, 103) AS DATAFIMMANDATO,
		                                    M.MANDATO,
		                                    ISNULL(TE.NOME, PTE.NOME_COMPL) AS TESOUREIRO,
		                                    ISNULL(TE.CPF, PTE.CPF) AS CPFTESOUREIRO,
		                                    A.ARQUIVOAAEID,
		                                    A.NOMEARQUIVO,
		                                    A.TIPOARQUIVO,
		                                    A.ARQUIVO
                                    FROM PRESTACAOCONTAS.MANDATOAAE M
	                                    LEFT JOIN PRESTACAOCONTAS.ARQUIVOAAE A ON A.MANDATOAAEID = M.MANDATOAAEID
	                                    LEFT JOIN PRESTACAOCONTAS.TESOUREIRO TE ON M.TESOUREIROID = TE.TESOUREIROID
	                                    LEFT JOIN LY_PESSOA PTE ON M.PESSOATESOUREIRO = PTE.PESSOA
                                    WHERE CENSO = @CENSO
                                    order by m.DATAINICIOMANDATO desc, m.DATAFIMMANDATO desc, m.MANDATOAAEID ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public bool PossuiTesoureiroPor(DataContext contexto, int tesoureiroId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.MANDATOAAE (NOLOCK)
                                    WHERE TESOUREIROID = @TESOUREIROID ";

            contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiroId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DTOs.DadosUnidadeAae ObtemDadosUnidadeAaePor(string censo)
        {
            DTOs.DadosUnidadeAae dadosUnidadeAae = new DTOs.DadosUnidadeAae();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            ArquivoAae rnArquivoAae = new ArquivoAae();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            RN.DTOs.DadosDiretor dadosDiretor = new Techne.Lyceum.RN.DTOs.DadosDiretor();
            DTOs.DadosTesoureiro dadosTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosTesoureiro();
            UnidadeEnsinoImpedida rnUnidadeEnsinoImpedida = new UnidadeEnsinoImpedida();
            Entidades.UnidadeEnsinoImpedida unidadeImpedida = new Techne.Lyceum.RN.PrestacaoContas.Entidades.UnidadeEnsinoImpedida();
            MotivoImpedimento rnMotivoImpedimento = new MotivoImpedimento();
            ContaCorrente rnContaCorrente = new ContaCorrente();
            DTOs.DadosContaCorrente dadosContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosContaCorrente();

            try
            {
                //Busca Dados da Escola
                dadosUnidadeAae = rnUnidadeEnsino.ObtemDadosUnidadePor(contexto, censo);

                //Busca quantidade Alunos
                dadosUnidadeAae.NumeroAluno = Convert.ToString(rnAluno.RetornaQuantiadeAlunosAtivosPor(contexto, censo));

                //Busca dados do diretor
                dadosDiretor = rnUnidadeEnsino.ObtemDiretorPor(contexto, censo);

                //Caso exista diretor alimenta campos
                if (!dadosDiretor.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Atualiza campos do diretor
                    dadosUnidadeAae.Diretor = dadosDiretor.Nome;
                    dadosUnidadeAae.PresidenteNome = dadosDiretor.Nome;
                    dadosUnidadeAae.PresidenteRg = dadosDiretor.Rg;
                    dadosUnidadeAae.PresidenteCpf = dadosDiretor.Cpf;
                    dadosUnidadeAae.PresidenteEndereco = dadosDiretor.Endereco;
                    dadosUnidadeAae.PresidenteNumero = dadosDiretor.Numero;
                    dadosUnidadeAae.PresidenteComplemento = dadosDiretor.Complemento;
                    dadosUnidadeAae.PresidenteBairro = dadosDiretor.Bairro;
                    dadosUnidadeAae.PresidenteMunicipio = dadosDiretor.Municipio;
                    dadosUnidadeAae.PresidenteEmail = dadosDiretor.Email;
                    dadosUnidadeAae.PresidenteTelefone = dadosDiretor.Telefone;
                    dadosUnidadeAae.PresidenteIdFuncional = dadosDiretor.IdFuncional;
                    dadosUnidadeAae.PresidenteMatricula = dadosDiretor.Matricula;
                }

                //Busca dados do mandato ativo
                var mandatoAae = this.ObtemMandatoAtivoPor(contexto, censo);

                //Verifica se já tem mandato cadstrado
                if (mandatoAae.MandatoAaeId > 0)
                {
                    //Atualiza campos da mandato
                    dadosUnidadeAae.MandatoAaeId = mandatoAae.MandatoAaeId;
                    dadosUnidadeAae.Mandato = mandatoAae.Mandato;
                    dadosUnidadeAae.InicioMandato = mandatoAae.DataInicioMandato;
                    dadosUnidadeAae.FimMandato = mandatoAae.DataFimMandato;
                    dadosUnidadeAae.TesoureiroId = mandatoAae.TesoureiroId;
                    dadosUnidadeAae.TesoureiroPessoa = mandatoAae.PessoaTesoureiro;

                    //Busca id do Arquivo
                    string tipoArquivo = string.Empty;
                    string nomeArquivo = string.Empty;
                    dadosUnidadeAae.AtaMandatoArquivoId = rnArquivoAae.RetornaArquivoAaeIdPor(contexto, dadosUnidadeAae.MandatoAaeId, out tipoArquivo, out nomeArquivo);
                    dadosUnidadeAae.TipoArquivo = tipoArquivo;
                    dadosUnidadeAae.NomeArquivo = nomeArquivo;

                    //Verifica se tesoureiro é um servidor
                    if (mandatoAae.TesoureiroId != null && mandatoAae.TesoureiroId > 0)
                    {
                        ////Busca dados do servidor
                        //dadosTesoureiro = this.ObtemTesoureiroServidorPor(contexto, dadosUnidadeAae.MandatoAaeId);
                        //Busca dados do tesoureito
                        dadosTesoureiro = this.ObtemTesoureiroExternoPor(contexto, dadosUnidadeAae.MandatoAaeId);
                    }
                    else
                    {
                        ////Busca dados do tesoureito
                        //dadosTesoureiro = this.ObtemTesoureiroExternoPor(contexto, dadosUnidadeAae.MandatoAaeId);
                        //Busca dados do servidor
                        dadosTesoureiro = this.ObtemTesoureiroServidorPor(contexto, dadosUnidadeAae.MandatoAaeId);
                    }

                    //Atualiza dados do tesoureiro
                    dadosUnidadeAae.TesoureiroPossuiIdFuncional = dadosTesoureiro.PossuiIdFuncional;
                    dadosUnidadeAae.TesoureiroPessoa = dadosTesoureiro.TesoureiroPessoa;
                    dadosUnidadeAae.TesoureiroId = dadosTesoureiro.TesoureiroId;
                    dadosUnidadeAae.TesoureiroNome = dadosTesoureiro.Nome;
                    dadosUnidadeAae.TesoureiroRg = dadosTesoureiro.Rg;
                    dadosUnidadeAae.TesoureiroCpf = dadosTesoureiro.Cpf;
                    dadosUnidadeAae.TesoureiroEndereco = dadosTesoureiro.Endereco;
                    dadosUnidadeAae.TesoureiroNumero = dadosTesoureiro.Numero;
                    dadosUnidadeAae.TesoureiroComplemento = dadosTesoureiro.Complemento;
                    dadosUnidadeAae.TesoureiroBairro = dadosTesoureiro.Bairro;
                    dadosUnidadeAae.TesoureiroMunicipio = dadosTesoureiro.Municipio;
                    dadosUnidadeAae.TesoureiroTelefone = dadosTesoureiro.Telefone;
                    dadosUnidadeAae.TesoureiroEmail = dadosTesoureiro.Email;
                    dadosUnidadeAae.TesoureiroIdFuncional = dadosTesoureiro.IdFuncional;
                }

                //Busca dados impedimento
                unidadeImpedida = rnUnidadeEnsinoImpedida.ObtemImpedimentoAtivoPor(contexto, censo);

                //Verifica se escola está impedida
                if (unidadeImpedida.UnidadeEnsinoImpedidaId > 0)
                {
                    dadosUnidadeAae.Impedida = true;
                    dadosUnidadeAae.MotivoImpedimento = rnMotivoImpedimento.ObtemDescricaoPor(contexto, unidadeImpedida.MotivoImpedimentoId);
                }
                else
                {
                    dadosUnidadeAae.Impedida = false;
                    dadosUnidadeAae.MotivoImpedimento = null;                    
                }

                //Busca dados conta escola
                dadosContaCorrente = rnContaCorrente.ObtemContaAtivaUnidadePor(contexto, censo);

                //Atualiza dados da conta
                if (!dadosContaCorrente.Conta.IsNullOrEmptyOrWhiteSpace())
                {
                    dadosUnidadeAae.Banco = dadosContaCorrente.BancoNome;
                    dadosUnidadeAae.Agencia = dadosContaCorrente.Agencia;
                    dadosUnidadeAae.ContaCorrente = dadosContaCorrente.Conta;
                }

                return dadosUnidadeAae;
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

        private Entidades.MandatoAae ObtemMandatoAtivoPor(DataContext contexto, string censo)
        {
            return this.ObtemMandatoAtivoPor(contexto, censo, DateTime.Now.Date);
        }

        public Entidades.MandatoAae ObtemMandatoAtivoPor(DataContext contexto, string censo, DateTime dataConsulta)
        {
            Entidades.MandatoAae mandato = new Entidades.MandatoAae();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT TOP 1 *
                                     FROM    PrestacaoContas.MANDATOAAE AAE
                                     WHERE  AAE.CENSO = @CENSO 
                                           AND AAE.DATAINICIOMANDATO <= @DATACONSULTA
                                           AND (AAE.DATAFIMMANDATO IS NULL OR AAE.DATAFIMMANDATO >= @DATACONSULTA)
                                    ORDER BY DATAINICIOMANDATO desc, DATAFIMMANDATO desc, MANDATOAAEID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.DateTime, dataConsulta);

            mandato = contexto.TryToBindEntity<Entidades.MandatoAae>(contextQuery);

            return mandato;
        }

        public DTOs.DadosTesoureiro ObtemTesoureiroServidorPor(DataContext contexto, int mandatoAaeId)
        {
            DTOs.DadosTesoureiro dadosTesoureiro = new DTOs.DadosTesoureiro();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  M.CENSO,
		                                        M.PESSOATESOUREIRO,
		                                        P.NOME_COMPL AS NOME,
		                                        P.RG_NUM AS RG,
		                                        P.CPF,
		                                        P.ENDERECO,
		                                        P.END_NUM AS NUMERO,
		                                        P.END_COMPL AS COMPLEMENTO,
		                                        P.BAIRRO,
		                                        Mu.NOME AS MUNICIPIO,
		                                        P.E_MAIL_INTERNO AS EMAIL,
		                                        ISNULL(P.CELULAR, P.FONE) AS TELEFONE,
		                                        P.IDFUNCIONAL
                                        FROM    PrestacaoContas.MANDATOAAE m 
		                                        INNER JOIN LY_PESSOA P ON M.PESSOATESOUREIRO = P.PESSOA
		                                        LEFT JOIN MUNICIPIO MU ON P.END_MUNICIPIO = MU.CODIGO
                                        WHERE  m.MANDATOAAEID = @MANDATOAAEID ";

                contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTesoureiro.PossuiIdFuncional = true;
                    dadosTesoureiro.TesoureiroPessoa = Convert.ToDecimal(reader["PESSOATESOUREIRO"]);
                    dadosTesoureiro.TesoureiroId = null;
                    dadosTesoureiro.Censo = Convert.ToString(reader["CENSO"]);
                    dadosTesoureiro.Nome = Convert.ToString(reader["NOME"]);
                    dadosTesoureiro.Rg = Convert.ToString(reader["RG"]);
                    dadosTesoureiro.Cpf = Convert.ToString(reader["CPF"]);
                    dadosTesoureiro.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosTesoureiro.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosTesoureiro.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosTesoureiro.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosTesoureiro.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosTesoureiro.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosTesoureiro.Email = Convert.ToString(reader["EMAIL"]);
                    dadosTesoureiro.IdFuncional = Convert.ToString(reader["IDFUNCIONAL"]);
                }

                return dadosTesoureiro;
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

        public DTOs.DadosTesoureiro ObtemDadosTesoureiroPor(decimal? pessoa, int? tesoureiroId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DTOs.DadosTesoureiro dadosTesoureiro = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosTesoureiro();

            try
            {
                if (pessoa != null && pessoa > 0)
                {
                    dadosTesoureiro = this.ObtemDadosTesoureiroPessoaPor(contexto, Convert.ToDecimal(pessoa));
                }
                else if (tesoureiroId != null && tesoureiroId > 0)
                {
                    dadosTesoureiro = this.ObtemDadosTesoureiroExternoPor(contexto, Convert.ToInt32(tesoureiroId));
                }

                return dadosTesoureiro;
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

        private DTOs.DadosTesoureiro ObtemDadosTesoureiroExternoPor(DataContext contexto, int tesoureiroId)
        {
            DTOs.DadosTesoureiro dadosTesoureiro = new DTOs.DadosTesoureiro();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT T.TESOUREIROID,
		                                        T.NOME,
		                                        T. RG,
		                                        T.CPF,
		                                        T.ENDERECO,
		                                        T.NUMERO,
		                                        T.COMPLEMENTO,
		                                        T.BAIRRO,
		                                        Mu.NOME AS MUNICIPIO,
		                                        T.EMAIL,
		                                        T.TELEFONE
                                        FROM    PrestacaoContas.TESOUREIRO t 
		                                        LEFT JOIN MUNICIPIO MU ON t.MUNICIPIOID = MU.CODIGO
                                        WHERE  t.TESOUREIROID = @TESOUREIROID ";

                contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiroId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTesoureiro.PossuiIdFuncional = false;
                    dadosTesoureiro.TesoureiroPessoa = null;
                    dadosTesoureiro.TesoureiroId = Convert.ToInt32(reader["TESOUREIROID"]);
                    dadosTesoureiro.Nome = Convert.ToString(reader["NOME"]);
                    dadosTesoureiro.Rg = Convert.ToString(reader["RG"]);
                    dadosTesoureiro.Cpf = Convert.ToString(reader["CPF"]);
                    dadosTesoureiro.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosTesoureiro.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosTesoureiro.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosTesoureiro.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosTesoureiro.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosTesoureiro.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosTesoureiro.Email = Convert.ToString(reader["EMAIL"]);
                    dadosTesoureiro.IdFuncional = null;
                }

                return dadosTesoureiro;
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

        private DTOs.DadosTesoureiro ObtemDadosTesoureiroPessoaPor(DataContext contexto, decimal pessoa)
        {
            DTOs.DadosTesoureiro dadosTesoureiro = new DTOs.DadosTesoureiro();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT    P.PESSOA,
		                                            P.NOME_COMPL AS NOME,
		                                            P.RG_NUM AS RG,
		                                            P.CPF,
		                                            P.ENDERECO,
		                                            P.END_NUM AS NUMERO,
		                                            P.END_COMPL AS COMPLEMENTO,
		                                            P.BAIRRO,
		                                            MU.NOME AS MUNICIPIO,
		                                            P.E_MAIL_INTERNO AS EMAIL,
		                                            ISNULL(P.CELULAR, P.FONE) AS TELEFONE,
		                                            P.IDFUNCIONAL
                                            FROM LY_PESSOA P (NOLOCK)
                                            LEFT JOIN MUNICIPIO MU ON P.END_MUNICIPIO = MU.CODIGO
                                            WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTesoureiro.PossuiIdFuncional = true;
                    dadosTesoureiro.TesoureiroPessoa = Convert.ToDecimal(reader["PESSOA"]);
                    dadosTesoureiro.TesoureiroId = null;
                    dadosTesoureiro.Nome = Convert.ToString(reader["NOME"]);
                    dadosTesoureiro.Rg = Convert.ToString(reader["RG"]);
                    dadosTesoureiro.Cpf = Convert.ToString(reader["CPF"]);
                    dadosTesoureiro.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosTesoureiro.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosTesoureiro.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosTesoureiro.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosTesoureiro.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosTesoureiro.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosTesoureiro.Email = Convert.ToString(reader["EMAIL"]);
                    dadosTesoureiro.IdFuncional = Convert.ToString(reader["IDFUNCIONAL"]);
                }

                return dadosTesoureiro;
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

        public DTOs.DadosTesoureiro ObtemTesoureiroExternoPor(DataContext contexto, int mandatoAaeId)
        {
            DTOs.DadosTesoureiro dadosTesoureiro = new DTOs.DadosTesoureiro();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  M.CENSO,
		                                        M.TESOUREIROID,
		                                        T.NOME,
		                                        T. RG,
		                                        T.CPF,
		                                        T.ENDERECO,
		                                        T.NUMERO,
		                                        T.COMPLEMENTO,
		                                        T.BAIRRO,
		                                        Mu.NOME AS MUNICIPIO,
		                                        T.EMAIL,
		                                        T.TELEFONE
                                        FROM    PrestacaoContas.MANDATOAAE m 
		                                        INNER JOIN PrestacaoContas.TESOUREIRO t on m.TESOUREIROID = t.TESOUREIROID
		                                        LEFT JOIN MUNICIPIO MU ON t.MUNICIPIOID = MU.CODIGO
                                        WHERE  m.MANDATOAAEID = @MANDATOAAEID ";

                contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTesoureiro.PossuiIdFuncional = false;
                    dadosTesoureiro.TesoureiroPessoa = null;
                    dadosTesoureiro.TesoureiroId = Convert.ToInt32(reader["TESOUREIROID"]);
                    dadosTesoureiro.Censo = Convert.ToString(reader["CENSO"]);
                    dadosTesoureiro.Nome = Convert.ToString(reader["NOME"]);
                    dadosTesoureiro.Rg = Convert.ToString(reader["RG"]);
                    dadosTesoureiro.Cpf = Convert.ToString(reader["CPF"]);
                    dadosTesoureiro.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosTesoureiro.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosTesoureiro.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosTesoureiro.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosTesoureiro.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosTesoureiro.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosTesoureiro.Email = Convert.ToString(reader["EMAIL"]);
                    dadosTesoureiro.IdFuncional = null;
                }

                return dadosTesoureiro;
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

        public void Atualiza(Entidades.MandatoAae mandatoAae, Entidades.ArquivoAae arquivoAae, bool atualizaArquivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.ArquivoAae rnArquivoAae = new RN.PrestacaoContas.ArquivoAae();
            string operacao = "ALTERADO";

            try
            {
                //Atualiza mandndo
                this.Atualiza(contexto, mandatoAae);

                if (atualizaArquivo)
                {
                    arquivoAae.MandatoAaeId = mandatoAae.MandatoAaeId;

                    //Verifica se já existe um arquivo
                    if (rnArquivoAae.ExistePor(contexto, arquivoAae.MandatoAaeId))
                    {
                        //Atualiza arquivo
                        rnArquivoAae.Atualiza(contexto, arquivoAae);
                    }
                    else
                    {
                        //Insere arquivo
                        rnArquivoAae.Insere(contexto, arquivoAae);
                        operacao = "CADASTRADO";
                    }

                    //Insere auditoria
                    rnArquivoAae.InsereAuditoria(contexto, arquivoAae, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);
                }
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private void Atualiza(DataContext contexto, Entidades.MandatoAae mandatoAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.MANDATOAAE
                                       SET MANDATOAAEID = @MANDATOAAEID
										,TESOUREIROID = @TESOUREIROID
										,PESSOATESOUREIRO = @PESSOATESOUREIRO
										,MANDATO = @MANDATO
										,DATAFIMMANDATO = @DATAFIMMANDATO 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                        ,DATAINICIOMANDATO = @DATAINICIOMANDATO
                                     WHERE MANDATOAAEID = @MANDATOAAEID ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAae.MandatoAaeId);
            contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, mandatoAae.TesoureiroId == null || mandatoAae.TesoureiroId <= 0 ? (object)DBNull.Value : mandatoAae.TesoureiroId);
            contextQuery.Parameters.Add("@PESSOATESOUREIRO", SqlDbType.Int, mandatoAae.PessoaTesoureiro == null || mandatoAae.PessoaTesoureiro <= 0 ? (object)DBNull.Value : mandatoAae.PessoaTesoureiro);
            contextQuery.Parameters.Add("@MANDATO", SqlDbType.VarChar, mandatoAae.Mandato);
            contextQuery.Parameters.Add("@DATAINICIOMANDATO", SqlDbType.DateTime, mandatoAae.DataInicioMandato);
            contextQuery.Parameters.Add("@DATAFIMMANDATO", SqlDbType.DateTime, mandatoAae.DataFimMandato);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, mandatoAae.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(Entidades.MandatoAae mandatoAae, Entidades.ArquivoAae arquivoAae, bool atualizaArquivo)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoDatas = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (mandatoAae == null)
            {
                return validacaoDados;
            }

            if (mandatoAae.DataInicioMandato == null || mandatoAae.DataInicioMandato == DateTime.MinValue || mandatoAae.DataInicioMandato.Year <= 1800)
            {
                mensagens.Add("O campo obrigatório Data do início da vigência do mandato da AAE não foi preenchido com data válida.");
            }

            if (mandatoAae.DataInicioMandato != null)
            {
                if (mandatoAae.DataInicioMandato > DateTime.Now)
                {
                    mensagens.Add("A Data do início da vigência do mandato da AAE não pode ser superior a data atual.");
                }
            }

            if (mandatoAae.Mandato < 0)
            {
                mensagens.Add("O campo obrigatório Quantidade de meses do Mandato não foi preenchido");
            }

            if (mandatoAae.Mandato == 0)
            {
                mensagens.Add("O campo  Quantidade de meses do Mandato deve ser maior que zero.");
            }
            else if (mandatoAae.Mandato > 9999)
            {
                mensagens.Add("O campo  Quantidade de meses do Mandato conter no máximo 4 digitos.");
            }

            if (mandatoAae.DataFimMandato == null || mandatoAae.DataFimMandato == DateTime.MinValue || mandatoAae.DataFimMandato.Year <= 1800)
            {
                mensagens.Add("Data do fim da vigência do mandato da AAE não foi preenchido com data válida.");
            }

            if (mandatoAae.TesoureiroId == null && mandatoAae.PessoaTesoureiro == null)
            {
                mensagens.Add("Campo Tesoureiro da AAE é obrigatório.");
            }

            if (mandatoAae.TesoureiroId == null && mandatoAae.TesoureiroId <= 0
                && mandatoAae.PessoaTesoureiro == null && mandatoAae.PessoaTesoureiro <= 0)
            {
                mensagens.Add("Campo Nome Tesoureiro é obrigatório.");
            }

            if (atualizaArquivo)
            {

                if (arquivoAae.Arquivo == null || arquivoAae.Arquivo.Count() <= 0)
                {
                    mensagens.Add("Campo ARQUIVO é obrigatório.");
                }
                else
                {
                    if (arquivoAae.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                    }
                    else
                    {
                        //Apenas aceitar pdf e imagem 
                        if (arquivoAae.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                            && arquivoAae.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                        {
                            mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                        }
                    }

                    if (arquivoAae.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe outro mandato no intervalo
                    validacaoDatas = this.ValidaIntercalacaoDatas(contexto, mandatoAae);
                    if (validacaoDatas.Count > 0)
                    {
                        mensagens.AddRange(validacaoDatas);
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

        private List<string> ValidaIntercalacaoDatas(DataContext contexto, Entidades.MandatoAae mandatoAae)
        {
            List<string> mensagens = new List<string>();

            if (this.PossuiDataEmOutroIntervaloPor(contexto, mandatoAae.Censo, mandatoAae.MandatoAaeId, mandatoAae.DataInicioMandato, mandatoAae.DataInicioMandato))
            {
                mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro mandato.");
            }

            if (this.PossuiDataEmOutroIntervaloPor(contexto, mandatoAae.Censo, mandatoAae.MandatoAaeId, mandatoAae.DataInicioMandato, mandatoAae.DataFimMandato))
            {
                mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro mandato.");
            }

            if (this.PossuiOutraIntercaladaPor(contexto, mandatoAae.Censo, mandatoAae.MandatoAaeId, mandatoAae.DataInicioMandato, mandatoAae.DataFimMandato))
            {
                mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outro mandato.");
            }

            return mensagens;
        }

        private bool PossuiDataEmOutroIntervaloPor(DataContext ctx, string censo, int mandatoAaeId, DateTime dataInicio, DateTime dataAnalisar)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   PRESTACAOCONTAS.MANDATOAAE (NOLOCK) 
                                        WHERE  CENSO = @CENSO 
                                               AND MANDATOAAEID <> @MANDATOAAEID 
                                               AND CONVERT(DATE, DATAINICIOMANDATO) <> CONVERT(DATE, @DTINI) 
                                               AND @DATA BETWEEN CONVERT(DATE, DATAINICIOMANDATO) AND CONVERT(DATE, DATAFIMMANDATO) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);
            contextQuery.Parameters.Add("@DTINI", SqlDbType.DateTime, dataInicio);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, dataAnalisar);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext ctx, string censo, int mandatoAaeId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   PRESTACAOCONTAS.MANDATOAAE   (NOLOCK) 
                             WHERE  CENSO = @CENSO 
                                    AND MANDATOAAEID <> @MANDATOAAEID 
                                    AND CONVERT(DATE, DATAINICIOMANDATO) <> CONVERT(DATE, @DATA_INICIO)
                                    AND @DATA_INICIO <= CONVERT(DATE, DATAINICIOMANDATO) 
                                    AND @DATA_FIM >= CONVERT(DATE, DATAFIMMANDATO) ";

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@MANDATOAAEID", mandatoAaeId);
            contextQuery.Parameters.Add("@DATA_INICIO", dataInicio);
            contextQuery.Parameters.Add("@DATA_FIM", dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void Insere(Entidades.MandatoAae mandatoAae, Entidades.ArquivoAae arquivoAae)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.ArquivoAae rnArquivoAae = new RN.PrestacaoContas.ArquivoAae();

            try
            {
                //Inserre mandato
                this.Insere(contexto, mandatoAae);

                //Insere Arquivo
                mandatoAae.MandatoAaeId = mandatoAae.MandatoAaeId;
                arquivoAae.MandatoAaeId = mandatoAae.MandatoAaeId;

                if (arquivoAae.MandatoAaeId > 0)
                {
                    rnArquivoAae.Insere(contexto, arquivoAae);
                    
                    //Insere auditoria
                    rnArquivoAae.InsereAuditoria(contexto, arquivoAae, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                }
                else
                {
                    throw new Exception("ERRO: Mandado não inserido. Favor verificar os campos/arquivo informado.");
                }
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private void Insere(DataContext contexto, Entidades.MandatoAae mandatoAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   INSERT INTO PrestacaoContas.MANDATOAAE
                                           (CENSO 
                                           ,TESOUREIROID
                                           ,PESSOATESOUREIRO
                                           ,MANDATO
                                           ,DATAINICIOMANDATO
                                           ,DATAFIMMANDATO                                           
                                           ,USUARIOID
                                           ,DATACADASTRO )
                                     VALUES
	                                       (@CENSO 
                                           ,@TESOUREIROID
                                           ,@PESSOATESOUREIRO
                                           ,@MANDATO
                                           ,@DATAINICIOMANDATO
                                           ,@DATAFIMMANDATO 
                                           ,@USUARIOID
                                           ,@DATACADASTRO
										   ) 

                         SELECT IDENT_CURRENT('PrestacaoContas.MANDATOAAE') ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAae.MandatoAaeId);
            contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.VarChar, mandatoAae.TesoureiroId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, mandatoAae.Censo);
            contextQuery.Parameters.Add("@PESSOATESOUREIRO", SqlDbType.VarChar, mandatoAae.PessoaTesoureiro);
            contextQuery.Parameters.Add("@MANDATO", SqlDbType.Int, mandatoAae.Mandato);
            contextQuery.Parameters.Add("@DATAINICIOMANDATO", SqlDbType.DateTime, mandatoAae.DataInicioMandato);
            contextQuery.Parameters.Add("@DATAFIMMANDATO", SqlDbType.DateTime, mandatoAae.DataFimMandato.Date <= SqlDateTime.MinValue.Value ? (object)null : mandatoAae.DataFimMandato);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, mandatoAae.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            mandatoAae.MandatoAaeId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados ValidaRemocao(int mandatoAaeId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (mandatoAaeId <= 0)
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

        public void Remove(int mandatoAaeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.ArquivoAae rnArquivoAae = new ArquivoAae();

            try
            {
                //Remove arquivos
                rnArquivoAae.RemovePorMandato(ctx, mandatoAaeId);

                //Remove mandato
                this.Remove(ctx, mandatoAaeId);
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

        private void Remove(DataContext ctx, int mandatoAaeId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PrestacaoContas.MANDATOAAE
                            WHERE  MANDATOAAEID = @MANDATOAAEID  ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);

            ctx.ApplyModifications(contextQuery);
        }
    }
}
