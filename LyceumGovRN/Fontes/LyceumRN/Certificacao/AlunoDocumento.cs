using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Certificacao.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Certificacao;
using System.ComponentModel;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Servicos;
using Seeduc.Infra.Extensions;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.RN.Certificacao
{
    public class AlunoDocumento
    {
        public int ObtemSequencialPor(string unidade, int tipoDocumento, int alunoDocumentoId)
        {
            int sequencial = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                //Verifica se ja tem sequencial criado
                sequencial = this.ObtemSequencialPor(ctx, unidade, tipoDocumento, alunoDocumentoId);

                //Caso não tenha busca o proximo
                if (sequencial == 0)
                {
                    sequencial = this.ObtemSequencialPor(ctx, unidade, tipoDocumento);
                }

                return sequencial;
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
        }

        public int ObtemSequencialPor(DataContext contexto, string unidade, int tipoDocumento, int alunoDocumentoId)
        {
            int sequencial = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select isnull(SEQUENCIAL, 0) SEQUENCIAL
                                            from [CertificacaoEscolar].[ALUNODOCUMENTO] AD                                           
                                            where ALUNODOCUMENTOID = @ALUNODOCUMENTOID
                                                and UNIDADEENSINO = @UNIDADEENSINO
                                                and DOCUMENTOID = @TIPODOCUMENTO ";

                contextQuery.Parameters.Add("@ALUNODOCUMENTOID", alunoDocumentoId);
                contextQuery.Parameters.Add("@UNIDADEENSINO", unidade);
                contextQuery.Parameters.Add("@TIPODOCUMENTO", tipoDocumento);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    sequencial = Convert.ToInt32(reader["SEQUENCIAL"]);
                }

                return sequencial;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int ObtemSequencialPor(DataContext ctx, string unidade, int tipoDocumento)
        {
            int quantidade = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"select isnull(MAX(SEQUENCIAL), 0) CONTADOR
                                            from [CertificacaoEscolar].[ALUNODOCUMENTO] AD                                            
                                            where UNIDADEENSINO = @UNIDADEENSINO
                                            and DOCUMENTOID = @TIPODOCUMENTO ";

                contextQuery.Parameters.Add("@UNIDADEENSINO", unidade);
                contextQuery.Parameters.Add("@TIPODOCUMENTO", tipoDocumento);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidade = Convert.ToInt32(reader["CONTADOR"]) + 1;
                }

                return quantidade;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable ListaPor(int alunoCertificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ALUNOCERTIFICACAOID,
	                                        ALUNODOCUMENTOID,
	                                        UNIDADEENSINO,
	                                        UE.NOME_COMP AS ESCOLA,
											AD.TIPOCONCLUSAOID,
											TC.DESCRICAO AS DESCRICAOTIPOCONCLUSAO,
											AD.DOCUMENTOID,
											D.DESCRICAO AS DESCRICAODOCUMENTO,
	                                        CASE 
		                                        WHEN MODALIDADE = 'EFR' THEN 'Ensino Fundamental Regular'
		                                        WHEN MODALIDADE = 'EMR' THEN 'Ensino Médio Regular'
		                                        WHEN MODALIDADE = 'EFE' THEN 'Ensino Fundamental EJA'
		                                        WHEN MODALIDADE = 'EME' THEN 'Ensino Médio EJA'
		                                        WHEN MODALIDADE = 'EPI' THEN 'Educação Profissional Integrada'
		                                        WHEN MODALIDADE = 'EPC' THEN 'Educação Profissional Concomitante'
		                                        WHEN MODALIDADE = 'EPS' THEN 'Educação Profissional Subsequente'
		                                        WHEN MODALIDADE = 'CN' THEN 'Curso Normal'
	                                        END DESCRICAOMODALIDADE,
                                            MODALIDADE,
	                                        NOMECURSO,
	                                        ATOAUTORIZA,
	                                        DATAAUTORIZA,
	                                        TOTALHORASAULA,
	                                        TOTALHORASRELOGIO,
	                                        DATACONCLUSAO,
	                                        NUMEROLIVRO,
	                                        LIVRO,
                                            FOLHALIVRO,        
	                                        OBSERVACAO
                                        FROM CertificacaoEscolar.ALUNODOCUMENTO ad
	                                        INNER JOIN LY_UNIDADE_ENSINO ue on ad.UNIDADEENSINO = ue.UNIDADE_ENS
											INNER JOIN CertificacaoEscolar.TIPOCONCLUSAO tc on ad.TIPOCONCLUSAOID = tc.TIPOCONCLUSAOID
											INNER JOIN CertificacaoEscolar.TIPODOCUMENTOCERTIFICA d on d.DOCUMENTOID = ad.DOCUMENTOID
                                        WHERE ALUNOCERTIFICACAOID = @ALUNOCERTIFICACAOID ";

                contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", SqlDbType.Int, alunoCertificacaoId);

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

        public ValidacaoDados Valida(Entidades.AlunoDocumento alunoDocumento, DateTime dataNasc, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\s{2,}");
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunoDocumento == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (alunoDocumento.AlunoDocumentoId <= 0)
                {
                    mensagens.Add("Campo CODIGO DOCUMENTO é obrigatório.");
                }
            }

            if (alunoDocumento.AlunoCertificacaoId <= 0)
            {
                mensagens.Add("Campo CODIGO ALUNO CERTIFICAÇÃO é obrigatório.");
            }

            if (alunoDocumento.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }


            if (alunoDocumento.DocumentoId <= 0)
            {
                mensagens.Add("Campo TIPO DOCUMENTO é obrigatório.");
            }

            if (alunoDocumento.TipoConclusaoId <= 0)
            {
                mensagens.Add("Campo NÍVEL é obrigatório.");
            }

            if (alunoDocumento.Modalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MODALIDADE é obrigatório.");
            }

            if (alunoDocumento.TipoConclusaoId > 0 && !alunoDocumento.Modalidade.IsNullOrEmptyOrWhiteSpace())
            {
                if (alunoDocumento.TipoConclusaoId == 1 && (alunoDocumento.Modalidade != "EFR" && alunoDocumento.Modalidade != "EFE"))
                {
                    mensagens.Add("Esta modalidade não é permitida para o TIPO CONCLUSÂO FUNDAMENTAL.");

                }
                
                if (alunoDocumento.TipoConclusaoId == 2 && (alunoDocumento.Modalidade != "EMR" && alunoDocumento.Modalidade != "EME" && alunoDocumento.Modalidade != "CN"))
                {
                    mensagens.Add("Esta modalidade não é permitida para o TIPO CONCLUSÂO MÉDIO.");

                }
                if (alunoDocumento.TipoConclusaoId == 3 && (alunoDocumento.Modalidade != "EPI" && alunoDocumento.Modalidade != "EPC" && alunoDocumento.Modalidade != "EPS"))
                {
                    mensagens.Add("Esta modalidade não é permitida para o TIPO CONCLUSÂO PROFISSIONALIZANTE.");

                }

            }

            if (alunoDocumento.NomeCurso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO CURSO é obrigatório.");
            }

            if (alunoDocumento.AtoAutoriza.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ATO AUTORIZATIVO é obrigatório.");
            }

            if (alunoDocumento.DataAutoriza == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO ATO é obrigatório.");
            }

            if (alunoDocumento.TotalHorasAula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TOTAL HORA-AULA é obrigatório.");
            }
            else
            {
                int resultado;

                if (!int.TryParse(alunoDocumento.TotalHorasAula, out resultado))
                {
                    mensagens.Add("Campo TOTAL HORA-AULA aceita somente números.");
                }
                else
                {
                    if (Convert.ToInt32(alunoDocumento.TotalHorasAula) <= 0)
                    {
                        mensagens.Add("Campo TOTAL HORA-AULA tem que ser maior que zero(0).");
                    }
                }
            }

            if (alunoDocumento.TotalHorasRelogio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TOTAL HORA-RELÓGIO é obrigatório.");
            }
            else
            {
                int resultado;

                if (!int.TryParse(alunoDocumento.TotalHorasRelogio, out resultado))
                {
                    mensagens.Add("Campo TOTAL HORA-RELÓGIO aceita somente números.");
                }
                else
                {

                    if (Convert.ToInt32(alunoDocumento.TotalHorasRelogio) <= 0)
                    {
                        mensagens.Add("Campo TOTAL HORA-RELÓGIO tem que ser maior que zero(0).");
                    }
                }
            }

            if (alunoDocumento.DataConclusao == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE CONCLUSÃO é obrigatório.");
            }
            else
            {
                if (alunoDocumento.DataConclusao > DateTime.Now.Date)
                {
                    mensagens.Add("Campo DATA DE CONCLUSÃO não pode ser maior que a data atual.");
                }

                if (alunoDocumento.DataConclusao < dataNasc)
                {
                    mensagens.Add("Campo DATA DE CONCLUSÃO não pode ser menor que a data nascimento.");
                }

            }

            if (alunoDocumento.NumeroLivro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }
            else
            {
                int resultado;

                if (int.TryParse(alunoDocumento.NumeroLivro, out resultado))
                {
                    if (resultado == 0)
                    {
                        mensagens.Add("Campo NÚMERO não pode ser igual a zero(0).");
                    }
                }

            }

            if (alunoDocumento.Livro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LIVRO é obrigatório.");
            }
            else
            {
                int resultado;

                if (int.TryParse(alunoDocumento.Livro, out resultado))
                {
                    if (resultado == 0)
                    {
                        mensagens.Add("Campo LIVRO não pode ser igual a zero(0).");
                    }
                }

            }

            if (alunoDocumento.FolhaLivro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FOLHA é obrigatório.");
            }
            else
            {
                int resultado;

                if (int.TryParse(alunoDocumento.FolhaLivro, out resultado))
                {
                    if (resultado == 0)
                    {
                        mensagens.Add("Campo FOLHA não pode ser igual a zero(0).");
                    }
                }

            }

            if (alunoDocumento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida se já tem a documento e tipo conclusao para o aluno
                    if (PossuiOutroDocumentoPor(contexto, alunoDocumento.AlunoCertificacaoId, alunoDocumento.DocumentoId, alunoDocumento.TipoConclusaoId, alunoDocumento.AlunoDocumentoId))
                    {
                        mensagens.Add("Este aluno já possui cadastro para esse TIPO DOCUMENTO e NÍVEL.");
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



        private bool PossuiOutroDocumentoPor(DataContext ctx, int alunoCertificacaoId, int documentoId, int tipoConclusaoId, int alunoDocumentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM CertificacaoEscolar.ALUNODOCUMENTO
                                         WHERE ALUNOCERTIFICACAOID = @ALUNOCERTIFICACAOID
                                            AND DOCUMENTOID = @DOCUMENTOID
                                            AND TIPOCONCLUSAOID = @TIPOCONCLUSAOID
                                            AND ALUNODOCUMENTOID <> @ALUNODOCUMENTOID ";

            contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", SqlDbType.Int, alunoCertificacaoId);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, documentoId);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipoConclusaoId);
            contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumentoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;

        }

        public void Insere(Entidades.AlunoDocumento alunoDocumento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere documento
                this.Insere(contexto, alunoDocumento);
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

        private void Insere(DataContext contexto, Entidades.AlunoDocumento alunoDocumento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO CertificacaoEscolar.ALUNODOCUMENTO
                                                        (
                                                         ALUNOCERTIFICACAOID,
                                                         DOCUMENTOID,
                                                         TIPOCONCLUSAOID,
                                                         UNIDADEENSINO,
		                                                 MODALIDADE,
		                                                 NOMECURSO,
		                                                 ATOAUTORIZA,
		                                                 DATAAUTORIZA,
		                                                 TOTALHORASAULA,
		                                                 TOTALHORASRELOGIO,
		                                                 DATACONCLUSAO,
		                                                 NUMEROLIVRO,
		                                                 FOLHALIVRO,
		                                                 LIVRO, 
		                                                 OBSERVACAO,
		                                                 USUARIOID,
                                                         DATACADASTRO,
		                                                 DATAALTERACAO)
                                            VALUES      (@ALUNOCERTIFICACAOID,
                                                         @DOCUMENTOID,
                                                         @TIPOCONCLUSAOID,
                                                         @UNIDADEENSINO,                                                         
		                                                 @MODALIDADE,
		                                                 @NOMECURSO,
		                                                 @ATOAUTORIZA,
		                                                 @DATAAUTORIZA,
		                                                 @TOTALHORASAULA,
		                                                 @TOTALHORASRELOGIO,
		                                                 @DATACONCLUSAO,
		                                                 @NUMEROLIVRO,
		                                                 @FOLHALIVRO,
		                                                 @LIVRO, 
		                                                 @OBSERVACAO,
		                                                 @USUARIOID,
                                                         @DATACADASTRO,
		                                                 @DATAALTERACAO)    

                                SELECT IDENT_CURRENT('CertificacaoEscolar.ALUNODOCUMENTO') ";

            contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", SqlDbType.Int, alunoDocumento.AlunoCertificacaoId);
            contextQuery.Parameters.Add("@DOCUMENTOID", SqlDbType.Int, alunoDocumento.DocumentoId);
            contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, alunoDocumento.TipoConclusaoId);
            contextQuery.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, alunoDocumento.UnidadeEnsino);
            contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, alunoDocumento.Modalidade);
            contextQuery.Parameters.Add("@NOMECURSO", SqlDbType.VarChar, alunoDocumento.NomeCurso);
            contextQuery.Parameters.Add("@ATOAUTORIZA", SqlDbType.VarChar, alunoDocumento.AtoAutoriza);
            contextQuery.Parameters.Add("@DATAAUTORIZA", SqlDbType.DateTime, alunoDocumento.DataAutoriza);
            contextQuery.Parameters.Add("@TOTALHORASAULA", SqlDbType.Int, alunoDocumento.TotalHorasAula);
            contextQuery.Parameters.Add("@TOTALHORASRELOGIO", SqlDbType.Int, alunoDocumento.TotalHorasRelogio);
            contextQuery.Parameters.Add("@DATACONCLUSAO", SqlDbType.DateTime, alunoDocumento.DataConclusao);
            contextQuery.Parameters.Add("@NUMEROLIVRO", SqlDbType.VarChar, alunoDocumento.NumeroLivro);
            contextQuery.Parameters.Add("@FOLHALIVRO", SqlDbType.VarChar, alunoDocumento.FolhaLivro);
            contextQuery.Parameters.Add("@LIVRO", SqlDbType.VarChar, alunoDocumento.Livro);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, alunoDocumento.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", alunoDocumento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            alunoDocumento.AlunoDocumentoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.AlunoDocumento alunoDocumento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza documento
                this.Atualiza(contexto, alunoDocumento);
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

        private void Atualiza(DataContext contexto, Entidades.AlunoDocumento alunoDocumento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CertificacaoEscolar.ALUNODOCUMENTO
                                        SET    UNIDADEENSINO = @UNIDADEENSINO,
		                                       MODALIDADE = @MODALIDADE,
		                                       NOMECURSO = @NOMECURSO, 
		                                       ATOAUTORIZA = @ATOAUTORIZA, 
		                                       DATAAUTORIZA = @DATAAUTORIZA, 
		                                       TOTALHORASAULA = @TOTALHORASAULA, 
		                                       TOTALHORASRELOGIO = @TOTALHORASRELOGIO, 
		                                       DATACONCLUSAO = @DATACONCLUSAO, 
		                                       NUMEROLIVRO = @NUMEROLIVRO, 
		                                       FOLHALIVRO = @FOLHALIVRO, 
		                                       LIVRO = @LIVRO, 
		                                       OBSERVACAO = @OBSERVACAO, 
		                                       USUARIOID = @USUARIOID, 
		                                       DATAALTERACAO = @DATAALTERACAO
                                        WHERE  ALUNODOCUMENTOID = @ALUNODOCUMENTOID ";

            contextQuery.Parameters.Add("@ALUNODOCUMENTOID", SqlDbType.Int, alunoDocumento.AlunoDocumentoId);
            contextQuery.Parameters.Add("@UNIDADEENSINO", TechneDbType.T_CODIGO, alunoDocumento.UnidadeEnsino);
            contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, alunoDocumento.Modalidade);
            contextQuery.Parameters.Add("@NOMECURSO", SqlDbType.VarChar, alunoDocumento.NomeCurso);
            contextQuery.Parameters.Add("@ATOAUTORIZA", SqlDbType.VarChar, alunoDocumento.AtoAutoriza);
            contextQuery.Parameters.Add("@DATAAUTORIZA", SqlDbType.DateTime, alunoDocumento.DataAutoriza);
            contextQuery.Parameters.Add("@TOTALHORASAULA", SqlDbType.Int, alunoDocumento.TotalHorasAula);
            contextQuery.Parameters.Add("@TOTALHORASRELOGIO", SqlDbType.Int, alunoDocumento.TotalHorasRelogio);
            contextQuery.Parameters.Add("@DATACONCLUSAO", SqlDbType.DateTime, alunoDocumento.DataConclusao);
            contextQuery.Parameters.Add("@NUMEROLIVRO", SqlDbType.VarChar, alunoDocumento.NumeroLivro);
            contextQuery.Parameters.Add("@FOLHALIVRO", SqlDbType.VarChar, alunoDocumento.FolhaLivro);
            contextQuery.Parameters.Add("@LIVRO", SqlDbType.VarChar, alunoDocumento.Livro);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, alunoDocumento.Observacao);
            contextQuery.Parameters.Add("@USUARIOID", alunoDocumento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
