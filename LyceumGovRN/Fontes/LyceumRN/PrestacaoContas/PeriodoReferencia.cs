using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PeriodoReferencia
    {
        public Entidades.PeriodoReferencia ObtemPor(int periodoReferenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemPor(contexto, periodoReferenciaId);
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

        public Entidades.PeriodoReferencia ObtemPor(DataContext contexto, int periodoReferenciaId)
        {
            Entidades.PeriodoReferencia periodoReferencia = new Entidades.PeriodoReferencia();
            ContextQuery contextQuery = new ContextQuery();


            contextQuery.Command = @"   SELECT *
                                            FROM PRESTACAOCONTAS.PERIODOREFERENCIA
                                            WHERE PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

            periodoReferencia = contexto.TryToBindEntity<Entidades.PeriodoReferencia>(contextQuery);

            return periodoReferencia;
        }

        public DateTime ObtemUltimoDiaPor(DataContext contexto, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime retorno = DateTime.MinValue;
            int ano = 0;
            int mesFim = 0;

            try
            {
                contextQuery.Command = @" SELECT ANO, 
                                                MESFINAL
                                        FROM PrestacaoContas.PERIODOREFERENCIA
                                        WHERE PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ano = Convert.ToInt32(reader["ANO"]);
                    mesFim = Convert.ToInt32(reader["MESFINAL"]);
                }

                retorno = new DateTime(ano, mesFim, DateTime.DaysInMonth(ano, mesFim));
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

        public DateTime ObtemPrimeiroDiaPor(DataContext contexto, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime retorno = DateTime.MinValue;
            int ano = 0;
            int mesInicio = 0;

            try
            {
                contextQuery.Command = @" SELECT ANO, 
                                                MESINICIAL
                                        FROM PrestacaoContas.PERIODOREFERENCIA
                                        WHERE PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ano = Convert.ToInt32(reader["ANO"]);
                    mesInicio = Convert.ToInt32(reader["MESINICIAL"]);
                }

                retorno = new DateTime(ano, mesInicio, 1);
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
                contextQuery.Command = @" SELECT  PERIODOREFERENCIAID
                                                  ,ANO
                                                  ,MESINICIAL
                                                  ,MESFINAL
                                                  ,REFERENCIA
                                                  ,ANOANTERIOR
                                                  ,MESANTERIOR
                                                  ,DATALIMITEPRESTACAOCONTAS
                                                  ,DATALIMITEANALISE
                                                  ,DATALIMITEDESPESAS
                                                  ,USUARIOID
                                                  ,DATACADASTRO
                                                  ,DATAALTERACAO
                    FROM   PrestacaoContas.PeriodoReferencia (NOLOCK) 
                    ORDER BY ANO, MESINICIAL";

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

        public DataTable ListaPeriodoPor(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOREFERENCIAID, REFERENCIA + ' - ' +
	                                        CASE
		                                        WHEN MESINICIAL = 1 THEN 'Janeiro'
		                                        WHEN MESINICIAL = 2 THEN 'Fevereiro'
		                                        WHEN MESINICIAL = 3 THEN 'Março'
		                                        WHEN MESINICIAL = 4 THEN 'Abril'
		                                        WHEN MESINICIAL = 5 THEN 'Maio'
		                                        WHEN MESINICIAL = 6 THEN 'Junho'
		                                        WHEN MESINICIAL = 7 THEN 'Julho'
		                                        WHEN MESINICIAL = 8 THEN 'Agosto'
		                                        WHEN MESINICIAL = 9 THEN 'Setembro'
		                                        WHEN MESINICIAL = 10 THEN 'Outubro'
		                                        WHEN MESINICIAL = 11 THEN 'Novembro'
		                                        WHEN MESINICIAL = 12 THEN 'Dezembro'
		                                        ELSE ''
	                                        END + 
	                                        CASE
												WHEN REFERENCIA = 'Mensal' then ''
		                                        WHEN MESFINAL = 1 THEN ' / Janeiro'
		                                        WHEN MESFINAL = 2 THEN ' / Fevereiro'
		                                        WHEN MESFINAL = 3 THEN ' / Março'
		                                        WHEN MESFINAL = 4 THEN ' / Abril'
		                                        WHEN MESFINAL = 5 THEN ' / Maio'
		                                        WHEN MESFINAL = 6 THEN ' / Junho'
		                                        WHEN MESFINAL = 7 THEN ' / Julho'
		                                        WHEN MESFINAL = 8 THEN ' / Agosto'
		                                        WHEN MESFINAL = 9 THEN ' / Setembro'
		                                        WHEN MESFINAL = 10 THEN ' / Outubro'
		                                        WHEN MESFINAL = 11 THEN ' / Novembro'
		                                        WHEN MESFINAL = 12 THEN ' / Dezembro'
		                                        ELSE ''
	                                        END DESCRICAO
                                        FROM PrestacaoContas.PERIODOREFERENCIA
                                        WHERE ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

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

        public DataTable ListaAno()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT ANO
                                FROM PrestacaoContas.PERIODOREFERENCIA ";

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

        public ValidacaoDados Valida(Entidades.PeriodoReferencia periodoReferencia, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoReferencia == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoReferencia.PeriodoReferenciaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoReferencia.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoReferencia.MesInicial <= 0)
            {
                mensagens.Add("Campo MÊS INICIAL é obrigatório.");
            }

            if (periodoReferencia.MesFinal <= 0)
            {
                mensagens.Add("Campo MÊS FINAL é obrigatório.");
            }

            if (periodoReferencia.Referencia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo REFERÊNCIA é obrigatório.");
            }
            else
            {
                switch (periodoReferencia.Referencia.ToUpper())
                {
                    case "MENSAL":
                        //Verifica se é o mesmo mes
                        if (periodoReferencia.MesFinal != periodoReferencia.MesInicial)
                        {
                            mensagens.Add("Para periodos com referência MENSAL mês inicial deve ser igual ao mês final.");
                        }
                        break;
                    case "BIMESTRAL":
                        //Verifica a diferença entre Final e inicial
                        if (periodoReferencia.MesFinal - periodoReferencia.MesInicial != 1)
                        {
                            mensagens.Add("Para periodos com referência BIMESTRAL devem ser 2 meses entre inicial e final.");
                        }
                        break;
                    case "TRIMESTRAL":
                        //Verifica a diferença entre Final e inicial
                        if (periodoReferencia.MesFinal - periodoReferencia.MesInicial != 2)
                        {
                            mensagens.Add("Para periodos com referência TRIMESTRAL devem ser 3 meses entre inicial e final.");
                        }
                        break;
                    case "SEMESTRAL":
                        //Verifica a diferença entre Final e inicial
                        if (periodoReferencia.MesFinal - periodoReferencia.MesInicial != 5)
                        {
                            mensagens.Add("Para periodos com referência SEMESTRAL devem ser 6 meses entre inicial e final.");
                        }
                        break;
                    case "ANUAL":
                        //Verifica se inicial é 1 e final é 12
                        if (periodoReferencia.MesInicial != 1 || periodoReferencia.MesFinal != 12)
                        {
                            mensagens.Add("Para periodos com referência ANUAL o mês inicial deve ser Janeiro e final deve ser Dezembro.");
                        }
                        break;
                    default:
                        mensagens.Add("Campo REFERÊNCIA invalido.");
                        break;
                }
            }
            if (periodoReferencia.AnoAnterior <= 0)
            {
                mensagens.Add("Campo ANO ANTERIOR é obrigatório.");
            }

            if (periodoReferencia.MesAnterior <= 0)
            {
                mensagens.Add("Campo MÊS ANTERIOR é obrigatório.");
            }
            else
            {
                if (periodoReferencia.AnoAnterior == periodoReferencia.Ano)
                {
                    if (periodoReferencia.MesInicial <= periodoReferencia.MesAnterior)
                    {
                        mensagens.Add("Campo MÊS ANTERIOR não pode ser maior ou igual ao mes inicio caso o ano anterior seja igual ao ano.");
                    }                    
                }                
            }

            if (periodoReferencia.Ano > 0 && periodoReferencia.AnoAnterior > 0)
            {
                if (periodoReferencia.AnoAnterior > periodoReferencia.Ano)
                {
                    mensagens.Add("Campo ANO ANTERIOR não pode ser maior que o ANO.");
                }
            }

            if (periodoReferencia.DataLimitePrestacaoContas != DateTime.MinValue)
            {
                if (periodoReferencia.DataLimitePrestacaoContas.Date < DateTime.Now.Date)
                {
                    mensagens.Add("Campo DATA LIMITE DA PRESTAÇÃO não pode ser menor ou igual a data atual.");
                }
                else
                {
                    DateTime dataMesInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                    if (periodoReferencia.DataLimitePrestacaoContas.Date < dataMesInicio)
                    {
                        mensagens.Add("Campo DATA LIMITE DA PRESTAÇÃO não pode ser menor que o inicio do PERIODO REFERÊNCIA.");
                    }
                }
            }
            else
            {
                mensagens.Add("Campo DATA LIMITE DA PRESTAÇÃO é obrigatório.");
            }

            if (periodoReferencia.DataLimitePrestacaoContas != DateTime.MinValue && periodoReferencia.DataLimiteAnalise != DateTime.MinValue)
            {
                if (periodoReferencia.DataLimiteAnalise.Date < periodoReferencia.DataLimitePrestacaoContas.Date)
                {
                    mensagens.Add("Campo DATA LIMITE DA ANÁLISE não pode ser inferior a DATA LIMITE DA PRESTAÇÃO.");
                }
                else
                {
                    DateTime dataMesInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                    if (periodoReferencia.DataLimiteAnalise.Date < dataMesInicio)
                    {
                        mensagens.Add("Campo DATA LIMITE DA ANÁLISE não pode ser menor que o inicio do PERIODO REFERÊNCIA.");
                    }
                }

                if (periodoReferencia.Ano > periodoReferencia.DataLimiteAnalise.Year || periodoReferencia.Ano > periodoReferencia.DataLimitePrestacaoContas.Year)
                {
                    mensagens.Add("DATA LIMITE DA ANÁLISE E DA PRESTAÇÃO não pode ser inferior ao ANO do PERÍODO DE REFERÊNCIA");
                }
            }
            else
            {
                mensagens.Add("Campo DATA LIMITE DA ANÁLISE é obrigatório.");
            }

            //VERIFICAÇÃO DATA LIMITE DESPESAS
            if (periodoReferencia.DataLimiteDespesas.Date < DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA LIMITE DESPESA não pode ser menor que a data atual.");
            }

            if (periodoReferencia.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Para cada mes do intervalo
                    for (int i = periodoReferencia.MesInicial; i <= periodoReferencia.MesFinal; i++)
                    {
                        // Verifica se já existe um periodo referencia cadastrado que utilize aquele mes e ano
                        if (this.PossuiOutroPeriodoReferenciaCadastradoPor(contexto, periodoReferencia.PeriodoReferenciaId, periodoReferencia.Ano, i))
                        {
                            mensagens.Add(string.Format("Já existe um periodo PERÍODO DE REFÊRENCIA que utiliza o mes {0} neste ano.", i.ToString()));
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

        private bool PossuiOutroPeriodoReferenciaCadastradoPor(DataContext ctx, int PeriodoReferenciaId, int Ano, int Mes)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PERIODOREFERENCIA (NOLOCK)
                                WHERE ANO = @ANO
                                    AND @MES BETWEEN MESINICIAL AND MESFINAL
	                                AND PERIODOREFERENCIAID <> @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, Ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, Mes);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferenciaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }        

        public void Insere(Entidades.PeriodoReferencia PeriodoReferencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO PrestacaoContas.PERIODOREFERENCIA
			                                    (ANO, 
                                                 MESINICIAL,
                                                 MESFINAL,
                                                 REFERENCIA,
                                                 ANOANTERIOR,
                                                 MESANTERIOR,
                                                 DATALIMITEPRESTACAOCONTAS,
                                                 DATALIMITEANALISE, 
                                                 DATALIMITEDESPESAS, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@ANO, 
                                                 @MESINICIAL,
                                                 @MESFINAL,
                                                 @REFERENCIA,
                                                 @ANOANTERIOR,
                                                 @MESANTERIOR,
                                                 @DATALIMITEPRESTACAOCONTAS,
                                                 @DATALIMITEANALISE,
                                                 @DATALIMITEDESPESAS,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO)   ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, PeriodoReferencia.Ano);
                contextQuery.Parameters.Add("@MESINICIAL", SqlDbType.Int, PeriodoReferencia.MesInicial);
                contextQuery.Parameters.Add("@MESFINAL", SqlDbType.Int, PeriodoReferencia.MesFinal);
                contextQuery.Parameters.Add("@REFERENCIA", SqlDbType.VarChar, PeriodoReferencia.Referencia);
                contextQuery.Parameters.Add("@ANOANTERIOR", SqlDbType.Int, PeriodoReferencia.AnoAnterior);
                contextQuery.Parameters.Add("@MESANTERIOR", SqlDbType.Int, PeriodoReferencia.MesAnterior);
                contextQuery.Parameters.Add("@DATALIMITEPRESTACAOCONTAS", SqlDbType.DateTime, PeriodoReferencia.DataLimitePrestacaoContas);
                contextQuery.Parameters.Add("@DATALIMITEANALISE", SqlDbType.DateTime, PeriodoReferencia.DataLimiteAnalise);
                contextQuery.Parameters.Add("@DATALIMITEDESPESAS", SqlDbType.DateTime, PeriodoReferencia.DataLimiteDespesas);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, PeriodoReferencia.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoReferencia PeriodoReferencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.PERIODOREFERENCIA
                                        SET     MESINICIAL = @MESINICIAL,
                                                MESFINAL = @MESFINAL,
                                                REFERENCIA = @REFERENCIA,
                                                ANOANTERIOR = @ANOANTERIOR,
                                                MESANTERIOR = @MESANTERIOR,
                                                DATALIMITEPRESTACAOCONTAS = @DATALIMITEPRESTACAOCONTAS,
                                                DATALIMITEANALISE = @DATALIMITEANALISE,
                                                DATALIMITEDESPESAS = @DATALIMITEDESPESAS,
                                                USUARIOID = @USUARIOID, 
                                                DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOREFERENCIAID = @PERIODOREFERENCIAID ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferencia.PeriodoReferenciaId);
                contextQuery.Parameters.Add("@MESINICIAL", SqlDbType.Int, PeriodoReferencia.MesInicial);
                contextQuery.Parameters.Add("@MESFINAL", SqlDbType.Int, PeriodoReferencia.MesFinal);
                contextQuery.Parameters.Add("@REFERENCIA", SqlDbType.VarChar, PeriodoReferencia.Referencia);
                contextQuery.Parameters.Add("@ANOANTERIOR", SqlDbType.Int, PeriodoReferencia.AnoAnterior);
                contextQuery.Parameters.Add("@MESANTERIOR", SqlDbType.Int, PeriodoReferencia.MesAnterior);
                contextQuery.Parameters.Add("@DATALIMITEPRESTACAOCONTAS", SqlDbType.DateTime, PeriodoReferencia.DataLimitePrestacaoContas);
                contextQuery.Parameters.Add("@DATALIMITEANALISE", SqlDbType.DateTime, PeriodoReferencia.DataLimiteAnalise);
                contextQuery.Parameters.Add("@DATALIMITEDESPESAS", SqlDbType.DateTime, PeriodoReferencia.DataLimiteDespesas);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, PeriodoReferencia.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int PeriodoReferenciaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ImportacaoSei rnImportacaoSei = new ImportacaoSei();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (PeriodoReferenciaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnImportacaoSei.PossuiPeriodoReferenciaPor(contexto, PeriodoReferenciaId))
                    {
                        mensagens.Add("Esta Periodo de Referencia não pode ser excluida pois foi utilizado.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int PeriodoReferenciaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.PERIODOREFERENCIA
                            WHERE  PERIODOREFERENCIAID = @PERIODOREFERENCIAID  ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferenciaId);

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

        public int ObtemPeriodoReferenciaPor(DataContext contexto, int mes, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime retorno = DateTime.MinValue;
           
            int periodoId = 0;

            try
            {
                contextQuery.Command = @" SELECT PERIODOREFERENCIAID
                                        FROM PrestacaoContas.PERIODOREFERENCIA
                                        WHERE ANO = @ANO
                                            AND @MES BETWEEN MESINICIAL AND MESFINAL ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    periodoId = Convert.ToInt32(reader["PERIODOREFERENCIAID"]);
                   
                }                
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return periodoId;
        }
    }
}
