using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PequenaDespesaServidor
    {
        public ICollection<DTOs.DadosPequenaDespesaServidor> ObtemDadosPequenaDespesaServidorPor(int pequenaDespesaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemDadosPequenaDespesaServidorPor(contexto, pequenaDespesaId);
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
        }

        public ICollection<DTOs.DadosPequenaDespesaServidor> ObtemDadosPequenaDespesaServidorPor(DataContext contexto, int pequenaDespesaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            IList<DTOs.DadosPequenaDespesaServidor> dados = new List<DTOs.DadosPequenaDespesaServidor>();

            try
            {
                contextQuery.Command = @" 
                    select pds.*, vwf.NOME_COMPL, vwf.MATRICULA 
                    from PrestacaoContas.PEQUENADESPESASERVIDOR pds (nolock)
                    inner join (select distinct PESSOA, NOME_COMPL, MATRICULA from VW_FUNCIONARIOS (nolock)) vwf on vwf.PESSOA = pds.PESSOA
                    where PEQUENADESPESAID = @PEQUENADESPESAID ";
                contextQuery.Parameters.Add("@PEQUENADESPESAID", SqlDbType.Int, pequenaDespesaId);
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Add(new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor {
                        PequenaDespesaServidorId = Convert.ToInt32(reader["PEQUENADESPESASERVIDORID"]),
                        PequenaDespesaId = Convert.ToInt32(reader["PEQUENADESPESAID"]),
                        Pessoa = Convert.ToInt32(reader["PESSOA"]),
                        NomeCompl = Convert.ToString(reader["NOME_COMPL"]),
                        IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]),
                        Matricula = Convert.ToString(reader["MATRICULA"]),
                        UsuarioId = Convert.ToString(reader["USUARIOID"]),
                        DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]),
                        DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]),
                    });
                }

                return dados;
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
                if (reader != null)
                    reader.Dispose();
            }
        }

        public ICollection<DTOs.DadosPequenaDespesaServidor> ObtemDadosPequenaDespesaServidorPorEventoId(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemDadosPequenaDespesaServidorPorEventoId(contexto, eventoId);
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
        }

        public ICollection<DTOs.DadosPequenaDespesaServidor> ObtemDadosPequenaDespesaServidorPorEventoId(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            IList<DTOs.DadosPequenaDespesaServidor> dados = new List<DTOs.DadosPequenaDespesaServidor>();

            try
            {
                contextQuery.Command = @" 
                    select pds.*, vwf.NOME_COMPL, vwf.MATRICULA 
                    from PrestacaoContas.PEQUENADESPESASERVIDOR pds (nolock)
                    inner join PrestacaoContas.PEQUENADESPESA pd (nolock) on pd.PEQUENADESPESAID = pds.PEQUENADESPESAID
                    inner join (select distinct PESSOA, NOME_COMPL, MATRICULA from VW_FUNCIONARIOS (nolock)) vwf on vwf.PESSOA = pds.PESSOA
                    where EVENTOID = @EVENTOID ";
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Add(new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor
                    {
                        PequenaDespesaServidorId = Convert.ToInt32(reader["PEQUENADESPESASERVIDORID"]),
                        PequenaDespesaId = Convert.ToInt32(reader["PEQUENADESPESAID"]),
                        Pessoa = Convert.ToInt32(reader["PESSOA"]),
                        NomeCompl = Convert.ToString(reader["NOME_COMPL"]),
                        IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]),
                        Matricula = Convert.ToString(reader["MATRICULA"]),
                        UsuarioId = Convert.ToString(reader["USUARIOID"]),
                        DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]),
                        DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]),
                    });
                }

                return dados;
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
                if (reader != null)
                    reader.Dispose();
            }
        }
        
        public ICollection<Entidades.PequenaDespesaServidor> ObtemPor(DataContext contexto, int pequenaDespesaId) 
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select * from PrestacaoContas.PEQUENADESPESASERVIDOR (nolock) where PEQUENADESPESAID = @PEQUENADESPESAID ";
                contextQuery.Parameters.Add("@PEQUENADESPESAID", SqlDbType.Int, pequenaDespesaId);
                return contexto.TryToBindEntities<Entidades.PequenaDespesaServidor>(contextQuery);
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
        }

        public void Insere(DataContext contexto, DTOs.DadosPequenaDespesaServidor dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.PEQUENADESPESASERVIDOR    
                                               (PEQUENADESPESAID
                                               ,PESSOA
                                               ,IDFUNCIONAL
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PEQUENADESPESAID, 
                                               @PESSOA, 
                                               @IDFUNCIONAL,
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PEQUENADESPESAID", SqlDbType.Int, dados.PequenaDespesaId);
                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Int, dados.Pessoa);
                contextQuery.Parameters.Add("@IDFUNCIONAL", SqlDbType.Int, dados.IdFuncional);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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
        }

        public void Remove(DataContext contexto, int pequenaDespesaServidorId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.PEQUENADESPESASERVIDOR                                    
                                    WHERE PEQUENADESPESASERVIDORID = @PEQUENADESPESASERVIDORID ";

                contextQuery.Parameters.Add("@PEQUENADESPESASERVIDORID", SqlDbType.Int, pequenaDespesaServidorId);

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
        }

        public void Salva(DataContext contexto, IEnumerable<DTOs.DadosPequenaDespesaServidor> dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                var listaNova = dados ?? new List<DTOs.DadosPequenaDespesaServidor>();
                                
                var pequenaDespesaId = listaNova.Select(s => s.PequenaDespesaId).FirstOrDefault();
                if (pequenaDespesaId == null)
                    return;

                if (!listaNova.All(q => q.PequenaDespesaId == pequenaDespesaId))
                    return;

                var listaAntiga = ObtemPor(contexto, pequenaDespesaId);

                var listaDeInsercao = listaNova.Where(lN => !listaAntiga.Any(lA => lA.IdFuncional == lN.IdFuncional && lA.Pessoa == lN.Pessoa));
                var listaDeExclusao = listaAntiga.Where(lA => !listaNova.Any(lN => lN.IdFuncional == lA.IdFuncional && lN.Pessoa == lA.Pessoa));

                foreach (var i in listaDeExclusao)
                    Remove(contexto, i.PequenaDespesaServidorId);

                foreach (var i in listaDeInsercao)
                    Insere(contexto, i);
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


        }

        public ValidacaoDados ValidaRemocao(int pequenaDespesaServidorId)
        {
            List<string> mensagens = new List<string>();
            //AnaliseContaCorrente rnAnaliseContaCorrente = new AnaliseContaCorrente();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pequenaDespesaServidorId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();


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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void RemoveTodosPorEvento(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" 
            insert into PrestacaoContas.PEQUENADESPESASERVIDOR_EXCLUIDO  
            SELECT *,GETDATE() FROM PrestacaoContas.PEQUENADESPESASERVIDOR                                    
                where PEQUENADESPESAID = (
                    select PEQUENADESPESAID from PrestacaoContas.PEQUENADESPESA (nolock)
                    where EVENTOID = @EVENTOID )

            delete from PrestacaoContas.PEQUENADESPESASERVIDOR                                    
                where PEQUENADESPESAID = (
                    select PEQUENADESPESAID from PrestacaoContas.PEQUENADESPESA (nolock)
                    where EVENTOID = @EVENTOID
                )
                ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

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
        }
    }
}
