using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.Matriculas
{
    public class DiasNaoLetivos
    {
        public ICollection<Entidades.DiasNaoLetivos> ListaPor(DataContext contexto, int ano, string municipio)
        {
            ICollection<Entidades.DiasNaoLetivos> lista = new List<Techne.Lyceum.RN.Matriculas.Entidades.DiasNaoLetivos>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                            FROM MATRICULA.DIASNAOLETIVOS
                                            WHERE YEAR(DIA) = @ANO
                                                  AND (MUNICIPIOID IS NULL OR MUNICIPIOID = @MUNICIPIOID) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);

            lista = contexto.TryToBindEntities<Entidades.DiasNaoLetivos>(contextQuery);

            return lista;
        }

        public ICollection<Entidades.DiasNaoLetivos> ListaPor(string municipio, DateTime dataInicio, DateTime dataFim)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = new List<Entidades.DiasNaoLetivos>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                            FROM MATRICULA.DIASNAOLETIVOS (NOLOCK)
                                            WHERE (MUNICIPIOID IS NULL OR MUNICIPIOID = @MUNICIPIOID)
												  AND CONVERT(DATE, DIA) BETWEEN @DATAINICIO AND @DATAFIM ";

                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim.Date);

                diasNaoLetivos = contexto.TryToBindEntities<Entidades.DiasNaoLetivos>(contextQuery);

                return diasNaoLetivos;
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

        private ICollection<Entidades.DiasNaoLetivos> ListaPor(DataContext contexto, string municipio, DateTime dataInicio)
        {
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = new List<Entidades.DiasNaoLetivos>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                            FROM MATRICULA.DIASNAOLETIVOS (NOLOCK)
                                            WHERE (MUNICIPIOID IS NULL OR MUNICIPIOID = @MUNICIPIOID)
												  AND CONVERT(DATE, DIA) >= @DATAINICIO ";

            contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipio);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);

            diasNaoLetivos = contexto.TryToBindEntities<Entidades.DiasNaoLetivos>(contextQuery);

            return diasNaoLetivos;
        }

        public DataTable ListaPor(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DN.DIASNAOLETIVOSID, 
												 DN.DIA, DN.MUNICIPIOID, 
												 DN.USUARIOID, 
												 DN.DATACADASTRO, 
												 M.NOME AS MUNICIPIO 
                                            FROM MATRICULA.DIASNAOLETIVOS DN (NOLOCK)
												 LEFT JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
														ON DN.MUNICIPIOID = M.ID_MUNICIPIO
                                            WHERE YEAR(DIA) = @ANO
											ORDER BY DIA DESC  ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

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

        public ValidacaoDados Valida(Entidades.DiasNaoLetivos diasNaoLetivos, bool porMunicipio)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (diasNaoLetivos == null)
            {
                return validacaoDados;
            }

            //Verifica se é por municipio
            if (porMunicipio)
            {
                if (diasNaoLetivos.MunicipioId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo MUNICÍPIO é obrigatório.");
                }
            }
            else
            {
                diasNaoLetivos.MunicipioId = null;
            }

            if (diasNaoLetivos.Dia == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA é obrigatório.");
            }
            else if (diasNaoLetivos.Dia.Date < DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA não pode ser inferior a data atual.");
            }

            if (diasNaoLetivos.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDataCadastradaPor(contexto, diasNaoLetivos.Dia, diasNaoLetivos.MunicipioId, diasNaoLetivos.DiasNaoLetivosId))
                    {
                        mensagens.Add(string.Format("Esta DATA já foi cadastrada{0}.", porMunicipio ? " para este municipio" : string.Empty));
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

        private bool PossuiOutraDataCadastradaPor(DataContext ctx, DateTime dia, string municipioId, int diasNaoLetivosId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Matricula.DIASNAOLETIVOS (NOLOCK)
                                    WHERE DIA = @DIA                                       
	                                    AND DIASNAOLETIVOSID <> @DIASNAOLETIVOSID ";

            if (municipioId.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command = contextQuery.Command + "  AND MUNICIPIOID IS NULL ";
            }
            else
            {
                contextQuery.Command = contextQuery.Command + "  AND MUNICIPIOID = @MUNICIPIOID ";
                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipioId);
            }

            contextQuery.Parameters.Add("@DIA", SqlDbType.Date, dia.Date);
            contextQuery.Parameters.Add("@DIASNAOLETIVOSID", SqlDbType.Int, diasNaoLetivosId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiDiaNaoLetivoPor(DataContext ctx, DateTime dia, string municipioId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Matricula.DIASNAOLETIVOS (NOLOCK)
                                    WHERE DIA = @DIA
                                        AND (MUNICIPIOID IS NULL OR MUNICIPIOID = @MUNICIPIOID) ";

            contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, municipioId);
            contextQuery.Parameters.Add("@DIA", SqlDbType.Date, dia.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.DiasNaoLetivos diasNaoLetivos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            List<DadosConvocados> listaConvocados = new List<DadosConvocados>();
            RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new RN.Matriculas.OpcaoInscricao();
            Matriculas.Entidades.ConvocacaoSemEmail convocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.Entidades.ConvocacaoSemEmail();
            Matriculas.ConvocacaoSemEmail rnConvocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.ConvocacaoSemEmail();
            bool emailEnviado = false;
            RN.Matricula rnMatricula = new Matricula();

            try
            {
                this.Insere(ctx, diasNaoLetivos);

                //Lista as oções com prazo de resposta para serem recalculadas apenas fase 2 pois fase 1 tem data fixa
                listaConvocados = rnOpcaoInscricao.ListaConvocadosPor(ctx, diasNaoLetivos.Dia.Year, 2);

                foreach (DadosConvocados convocado in listaConvocados)
                {
                    //Caso a opção tenha sido retornada a data inicio do prazo é a data do retorno, caso contrario é a data de convocação
                    DateTime dataInicio = convocado.DataRetorno == null || convocado.DataRetorno == DateTime.MinValue ? convocado.DataConvocacao : Convert.ToDateTime(convocado.DataRetorno);

                    //Busca novo prazo de resposta para o aluno
                    DateTime novoPrazoResposta = rnOpcaoInscricao.RetornaPrazoResposta(ctx, dataInicio, convocado.MunicipioEscola);

                    //Verifica se o prazo mudou
                    if (novoPrazoResposta > convocado.PrazoFinal && novoPrazoResposta > DateTime.Now)
                    {
                        //Atualiza novo prazo
                        rnOpcaoInscricao.AtualizaPrazoResposta(ctx, convocado.OpcaoInscricaoId, novoPrazoResposta, diasNaoLetivos.UsuarioId);

                        //Monta mensagem do email
                        string mensagem = string.Format(@"<br />{0}
                                            <br />Informamos que sua vaga está reservada na série {1} do Ensino {2} do {3}. Compareça de {4} a {5} na escola onde foi alocado para confirmar a sua matrícula.
                                            <br />{6}
                                           ", convocado.Nome, convocado.Serie, convocado.Segmento, convocado.Escola, dataInicio.ToString("dd/MM/yyyy"), novoPrazoResposta.ToString("dd/MM/yyyy"), rnMatricula.RetornaTextoEmailConvocacaoPor(diasNaoLetivos.Dia.Year));


                        //Tentar Enviar e-mail
                        try
                        {
                            //Envia e-mail
                            this.EnviaEmailConvocacao(convocado.Nome, convocado.Email, mensagem);
                            emailEnviado = true;
                        }
                        catch (Exception)
                        {
                            emailEnviado = false;
                        }

                        //Verifica se não foi possivel enviar o e-mail
                        if (!emailEnviado)
                        {
                            //Monta entidade de email não enviado
                            convocacaoSemEmail.InscricaoAlunoId = convocado.InscricaoAlunoId;
                            convocacaoSemEmail.OpcaoInscricaoId = convocado.OpcaoInscricaoId;
                            convocacaoSemEmail.UsuarioResponsavel = diasNaoLetivos.UsuarioId;
                            convocacaoSemEmail.DataAviso = DateTime.Now;

                            rnConvocacaoSemEmail.Insere(ctx, convocacaoSemEmail);
                        }
                    }
                }
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

        protected void EnviaEmailConvocacao(string nome, string email, string mensagem)
        {
            EmailApi rnEmailApi = new EmailApi();

            try
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["EmailApi_Host"];
                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailApi_Port"]);
                var from = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFacil_From"];
                var fromName = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFacil_FromName"];
                var userName = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFacil_UserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFacil_Password"];
                var subject = System.Configuration.ConfigurationManager.AppSettings["EmailMatriculaFacil_Subject"];

                string emailMessage = mensagem;

                var emailObject = new RN.Util.EmailApi.EmailDTO
                {
                    Smtp = new EmailApi.EmailDTO.SmtpDTO
                    {
                        Host = host,
                        Port = port,
                        UserName = userName,
                        Password = password,
                        EnableSSL = true,
                    },
                    Message = new EmailApi.EmailDTO.MessageDTO
                    {
                        From = new EmailApi.EmailDTO.MessageDTO.MailAddressDTO
                        {
                            Address = from,
                            Name = fromName
                        },
                        To = new List<EmailApi.EmailDTO.MessageDTO.MailAddressDTO>
                        {
                            new EmailApi.EmailDTO.MessageDTO.MailAddressDTO { Address = email, Name = nome },
                        },
                        Subject = subject,
                        Body = emailMessage,
                        IsBodyHtml = true,
                    },
                };

                var emailApiResult = rnEmailApi.EmailApiSend(emailObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insere(DataContext ctx, Entidades.DiasNaoLetivos diasNaoLetivos)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.DIASNAOLETIVOS 
                                                        (DIA, 
                                                         MUNICIPIOID, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DIA, 
                                                         @MUNICIPIOID, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@DIA", SqlDbType.DateTime, diasNaoLetivos.Dia.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, diasNaoLetivos.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            if (diasNaoLetivos.MunicipioId.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, null);
            }
            else
            {
                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, diasNaoLetivos.MunicipioId);
            }

            ctx.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int diasNaoLetivosId, DateTime dia)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (diasNaoLetivosId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (dia == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA é obrigatório.");
            }
            else if (dia.Date < DateTime.Now.Date)
            {
                mensagens.Add("Esta DATA não pode ser excluida, pois é inferior a data atual.");
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

        public void Remove(int diasNaoLetivosId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Matricula.diasNaoLetivos
                            WHERE  diasNaoLetivosID = @diasNaoLetivosID  ";

                contextQuery.Parameters.Add("@diasNaoLetivosID", SqlDbType.Int, diasNaoLetivosId);

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

        public DateTime RetornaProximaDataFinalPor(DataContext contexto, DateTime dataInicio, int quantidadeDiasUteis, string municipio)
        {
            DateTime prazoFinal = dataInicio.Date;
            int diasUteis = 0;

            //Busca Dias nao letivos
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = this.ListaPor(contexto, prazoFinal.Year, municipio);

            //Calcula prazo final
            while (diasUteis < quantidadeDiasUteis)
            {
                prazoFinal = prazoFinal.AddDays(1);

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)prazoFinal.DayOfWeek != 0 && (int)prazoFinal.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == prazoFinal.Date).Count() == 0)
                    {
                        diasUteis++;
                    }
                }
            }

            return prazoFinal.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public DateTime RetornaDataFinalPor(DataContext contexto, DateTime dataInicio, int quantidadeDiasUteis, string municipio)
        {
            DateTime prazoFinal = dataInicio.Date;
            int diasUteis = 1;

            //Busca Dias nao letivos
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = this.ListaPor(contexto, municipio, dataInicio);

            //Calcula prazo final
            while (diasUteis <= quantidadeDiasUteis)
            {
                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)prazoFinal.DayOfWeek != 0 && (int)prazoFinal.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == prazoFinal.Date).Count() == 0)
                    {
                        diasUteis++;
                    }
                }

                prazoFinal = prazoFinal.AddDays(1);
            }

            return prazoFinal.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public DateTime RetornaDataFinalPor(DateTime dataInicio, int quantidadeDiasUteis, string municipio)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaDataFinalPor(contexto, dataInicio, quantidadeDiasUteis, municipio);
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
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public List<DateTime> RetornaDiasLetivosPor(DataContext contexto, string municipio, DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> diasLetivos = new List<DateTime>();

            //Busca Dias nao letivos
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = this.ListaPor(municipio, dataInicio, dataFim);

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)data.DayOfWeek != 0 && (int)data.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == data.Date).Count() == 0)
                    {
                        diasLetivos.Add(i);
                    }
                }
            }

            return diasLetivos;
        }

        public int RetornaDiasUteisPor(string municipio, DateTime dataInicio, DateTime dataFim)
        {
            int diasUteis = 0;

            //Busca Dias nao letivos
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = this.ListaPor(municipio, dataInicio, dataFim);

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)data.DayOfWeek != 0 && (int)data.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == data.Date).Count() == 0)
                    {
                        diasUteis++;
                    }
                }
            }

            return diasUteis;
        }

        public List<DateTime> RetornaDiasLetivosPor(string municipio, int mes, int ano, int periodo)
        {
            List<DateTime> diasLetivos = new List<DateTime>();

            int diaInicial = 1;
            if (mes == 2)
            {
                diaInicial = 5;
            }
            DateTime dataInicio = new DateTime(ano, mes, diaInicial);
            DateTime dataFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            //Busca Dias nao letivos
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = this.ListaPor(municipio, dataInicio, dataFim);

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)data.DayOfWeek != 0 && (int)data.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == data.Date).Count() == 0)
                    {
                        diasLetivos.Add(i);
                    }
                }
            }

            //Datas solicitadas para seram exibidas como dias não letivos

            if (periodo == 0)
            {
                //Os dias a serem apresentados na tela como não letivos para os períodos 0 e 2 seguem abaixo :
                //'2024-02-01', '2024-02-02', '2024-07-08', '2024-07-09', '2024-07-10', '2024-07-11', '2024-07-12', '2024-07-15', 
                //'2024-07-16', '2024-07-17', '2024-07-18', '2024-07-19'
                diasLetivos.Remove(Convert.ToDateTime("2024-02-01"));
                diasLetivos.Remove(Convert.ToDateTime("2024-02-02"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-08"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-09"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-10"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-11"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-12"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-15"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-16"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-17"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-18"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-19"));
            }

            if (periodo == 1)
            {
                //Se for possível fazer sem alterar a tela, considerar como não letivos apenas para o período 1 os dias abaixo:
                diasLetivos.Remove(Convert.ToDateTime("2025-07-28"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-29"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-30"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-31"));
            }            

            if (periodo == 2)
            {
                //Os dias a serem apresentados na tela como não letivos para os períodos 0 e 2 seguem abaixo :
                //'2024-02-01', '2024-02-02', '2024-07-08', '2024-07-09', '2024-07-10', '2024-07-11', '2024-07-12', '2024-07-15', 
                //'2024-07-16', '2024-07-17', '2024-07-18', '2024-07-19'
                //Se for possível fazer sem alterar a tela, considerar como não letivos apenas para o período 2 os dias abaixo:
                //'2024-07-01', '2024-07-02', '2024-07-03, '2024-07-04', '2024-07-05'
                diasLetivos.Remove(Convert.ToDateTime("2024-07-01"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-02"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-03"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-04"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-05"));
                diasLetivos.Remove(Convert.ToDateTime("2024-02-01"));
                diasLetivos.Remove(Convert.ToDateTime("2024-02-02"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-08"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-09"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-10"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-11"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-12"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-15"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-16"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-17"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-18"));
                diasLetivos.Remove(Convert.ToDateTime("2024-07-19"));


                diasLetivos.Remove(Convert.ToDateTime("2025-07-01"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-02"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-03"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-04"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-07"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-08"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-09"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-10"));
                diasLetivos.Remove(Convert.ToDateTime("2025-07-11"));
            }

            return diasLetivos;
        }
    }
}
