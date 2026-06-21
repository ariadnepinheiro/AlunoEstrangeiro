using System.Collections.Generic;
using System.Configuration;
using System;
using System.Linq;
using Techne.Lyceum.RN.DTOs;
using System.Net.Mail;
using Proderj.Framework.Common;
using System.Text;
using System.Net;

namespace Techne.Lyceum.RN.Util
{
    public class Email : RNBase
    {       

        public static void Envia(DadosEmail email)
        {
            List<string> destinatarios = new List<string>();
            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
            destinatarios.Add(email.Destinatario);
            
            if (!string.IsNullOrEmpty(email.Login) && !string.IsNullOrEmpty(email.Senha))
                Envia(email.Remetente, email.Login, email.Senha, email.Assunto, email.Texto, encoding, destinatarios);
            else
                Envia(email.Remetente, email.Assunto, email.Texto, encoding, destinatarios);
        }

        /// <summary>
        /// Envia um e-mail utilizando as configurações de e-mail do web.config (mailSettings).
        /// </summary>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Corpo do e-mail</param>
        /// <param name="encoding">Encoding a ser utilizado</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        private static void Envia(string remetente, string assunto, string mensagem, Encoding encoding, List<string> destinatarios)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(remetente);
                email.BodyEncoding = encoding;
                email.IsBodyHtml = true;

                foreach (var destinatario in destinatarios)
                {
                    email.To.Add(destinatario);
                }

                email.Body = mensagem;
                email.Subject = assunto;        

                smtpClient.EnableSsl = true;
                smtpClient.Send(email);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void Envia(string remetente, string login, string senha, string assunto, string mensagem, Encoding encoding, List<string> destinatarios)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage email = new MailMessage();
                email.From = new MailAddress(remetente);
                email.BodyEncoding = encoding;
                email.IsBodyHtml = true;

                foreach (var destinatario in destinatarios)
                {
                    email.To.Add(destinatario);
                }

                email.Body = mensagem;
                email.Subject = assunto;

                smtpClient.Credentials = new NetworkCredential(login, senha);
                smtpClient.EnableSsl = true;
                smtpClient.Send(email);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DadosEmail MontarEmailReset(string matricula, string senha, string emailDocente)
        {
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(emailDocente))
            {
                return new DadosEmail();
            }

            var texto = string.Format(@"Prezado(a),
                            <br><br>Conforme solicitado às {0}h do dia {1}, foi realizado a reinicialização da sua senha.
                            <br>Sua nova senha para o Docente On-line (matrícula: {2}) é: <b>{3}</b>
                            <br>Acesse o Docente On-line e troque a sua senha.
                            <br><br><b><FONT COLOR='#FF0000'>Email automático por favor NÃO RESPONDA!</FONT></b>", DateTime.Now.ToString("HH:mm"), DateTime.Today.ToString("dd/MM/yyyy"), matricula, senha);

            var email = new RN.DTOs.DadosEmail
            {
                Assunto = "Senha Docente On-line",
                Destinatario = emailDocente,
                Remetente = Convert.ToString(ConfigurationManager.AppSettings["EMailReset"]),
                Texto = texto
            };

            return email;
        }

        public static DadosEmail MontaEmailMatriculaFacil(ConsolidaTurnosDoMatriculaFacil obj)
        {
            string strCursosEnv = string.Empty;
            string strUnidadeEnv = string.Empty;
            string strTurnosMigrados = string.Empty;
            string strTurnosNaoMigrados = string.Empty;

            string texto = string.Empty;
            try
            {
                if (obj == null)
                {
                    return new DadosEmail();
                }

                var cursosEnv = new List<string>();
                cursosEnv.Add("<tr><td colspan='4'>Nenhum</td></tr>");

                var unidadeEnv = new List<string>();
                unidadeEnv.Add("<tr><td colspan='2'>Nenhum</td></tr>");

                var turnosMigrados = new List<string>();
                turnosMigrados.Add("<tr><td colspan='8'>Nenhum</td></tr>");

                var turnosNaoMigrados = new List<string>();
                turnosNaoMigrados.Add("<tr><td colspan='8'>Nenhum</td></tr>");

                if (obj != null)
                {
                    unidadeEnv.Clear();
                    unidadeEnv.Add(@" <tr><td> " + obj.UnidadeEnsino + @"</td> <td> " + obj.NomeUnidadeEnsino + @" </td></tr>");
                }

                strUnidadeEnv = unidadeEnv.Aggregate((x, y) => x + Environment.NewLine + y);

                if (obj.DetalhesConsolidaTurnosDoMatriculaFacil.Count > 0)
                {
                    cursosEnv.Clear();
                    turnosMigrados.Clear();
                    turnosNaoMigrados.Clear();
                    IList<DetalhesConsolidaTurnosDoMatriculaFacil> det 
                        = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                    for (int i = 0; i < obj.DetalhesConsolidaTurnosDoMatriculaFacil.Count; i++)
                    {
                        det.Add( 
                            new DetalhesConsolidaTurnosDoMatriculaFacil
                            { 
                                Curso = obj.DetalhesConsolidaTurnosDoMatriculaFacil[i].Curso, 
                                NomeCurso = obj.DetalhesConsolidaTurnosDoMatriculaFacil[i].NomeCurso, 
                                Serie = obj.DetalhesConsolidaTurnosDoMatriculaFacil[i].Serie,
                                TipoCurso = obj.DetalhesConsolidaTurnosDoMatriculaFacil[i].TipoCurso,
                                Turno = null,
                                ModalidadeCurso = null,
                                TipoOperacao = null
                            });
                    }

                    IEnumerable<DetalhesConsolidaTurnosDoMatriculaFacil> distinct = det.Distinct();

                    foreach (var item in distinct)
                    {
                        cursosEnv.Add(@"<tr> <td> " + item.Curso.ToString() + @" </td> <td> " + item.NomeCurso.ToString() + @" </td> <td>
                        " + item.Serie.ToString() + @" </td> <td> " + item.TipoCurso.ToString() + @" </td> </tr>    ");
                    }

                    if (cursosEnv.Count > 0)
                    {
                        strCursosEnv = cursosEnv.Aggregate((x, y) => x + Environment.NewLine + y);
                    }

                    foreach (var item in obj.DetalhesConsolidaTurnosDoMatriculaFacil)
                    {
                        if (item.TipoRetorno.Equals("00"))
                        {
                            turnosMigrados.Add(@"<tr> <td> " + obj.UnidadeEnsino + @" </td><td> " + item.Curso + @" </td><td> " + item.Serie + @" </td><td> " + obj.Ano + @" </td>
                            <td> " + obj.Periodo + @" </td><td> " + item.Turno + @" </td> <td> " + item.TipoOperacao + @" </td><td> " + item.DescricaoRetorno + @" </td></tr>");
                        }
                        else
                        {
                            turnosNaoMigrados.Add(@"<tr> <td> " + obj.UnidadeEnsino + @" </td><td> " + item.Curso + @" </td><td> " + item.Serie + @" </td><td> " + obj.Ano + @" </td>
                            <td> " + obj.Periodo + @" </td><td> " + item.Turno + @" </td> <td> " + item.TipoOperacao + @" </td><td> " + item.DescricaoRetorno + @" </td></tr>");
                        }
                    }

                    if (turnosMigrados.Count > 0)
                    {
                        strTurnosMigrados = turnosMigrados.Aggregate((x, y) => x + Environment.NewLine + y);
                    }

                    if (turnosNaoMigrados.Count > 0)
                    {
                        strTurnosNaoMigrados = turnosNaoMigrados.Aggregate((x, y) => x + Environment.NewLine + y);
                    }
                }

                #region Corpo do Email

                texto = @"
                <div style='width:600; padding:0;margin:0;'>
                    <h2> Sistema Conexão Educação - Integração Turnos SisMati </h2>

                    <br />
                    <br />

                    <table border='1' width='550px'>
	                    <tr>
		                    <td colspan='2'> <b>Unidades de Ensino Envolvidas no Processo:</b> </td>
	                    </tr>
	                    <tr>
		                    <td> CENSO </td>
		                    <td> NOME UNIDADE </td>
	                    </tr> "
                            + strUnidadeEnv.ToString() +

                        @"
                    </table>

                    <br />
                    <br />

                    <table border='1' width='550px'>
	                    <tr>
		                    <td colspan ='4'> <b>Cursos Envolvidos no Processo:</b> </td>
	                    </tr>
	                    <tr>
		                    <td> CURSO </td>
		                    <td> NOME CURSO </td>
		                    <td> SÉRIE </td>
		                    <td> TIPO </td>                        
	                    </tr>
                        " + strCursosEnv.ToString() + @"
                    </table>

                    <br /><br />

                    <table border='1' width='550px'>
	                    <tr>
		                    <td colspan='8'><b>Turnos Migrados:</b></td>
	                    </tr>
	                    <tr>
		                    <td>CENSO</td> 	
		                    <td>CURSO</td> 	
		                    <td>SÉRIE</td> 
		                    <td>ANO</td> 
		                    <td>PERÍODO</td> 
		                    <td>TURNO</td> 	
                            <td>OPERAÇÃO</td>
		                    <td>RESPOSTA</td> 
	                    </tr>
                        " + strTurnosMigrados.ToString() + @"
                    </table>

                    <br /><br />

                    <table border='1' width='550px'>
	                    <tr>
		                    <td colspan='8'><b>Turnos NÃO Migrados (erro/falha):</b></td>
	                    </tr>
	                    <tr>
		                    <td>CENSO</td> 	
		                    <td>CURSO</td> 	
		                    <td>SÉRIE</td> 
		                    <td>ANO</td> 
		                    <td>PERÍODO</td> 
		                    <td>TURNO</td> 
                            <td>OPERAÇÃO</td>	
		                    <td>RESPOSTA</td> 
	                    </tr>
                        " + strTurnosNaoMigrados.ToString() + @"
                    </table>
                    <br><br>
                    <b><FONT COLOR='#FF0000'>Email automático. Por favor NÃO RESPONDA!</FONT></b>
                </div>";

                #endregion

                var email = new RN.DTOs.DadosEmail
                {
                    Assunto = @"Sistema Conexão Educação - Integração Turnos SisMati",
                    Destinatario = Convert.ToString(ConfigurationManager.AppSettings["EMailsSisMat"]),
                    Remetente = @"reset.prof@educacao.rj.gov.br",
                    Texto = texto
                };

                return email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool EnviarMail(DadosEmail email)
        {
            try
            {
                //Proderj.Framework.Common.Email.Enviar(email.Remetente, email.Assunto, email.Texto, email.Destinatario);
                //Proderj.Framework.Common.Email mail = new Proderj.Framework.Common.Email();
                //mail.Enviar(email.Remetente, email.Assunto, email.Texto, email.Destinatario);
                List<string> destinatarios = new List<string>();
                Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
                destinatarios.Add(email.Destinatario);

                if (!string.IsNullOrEmpty(email.Login) && !string.IsNullOrEmpty(email.Senha))
                    Envia(email.Remetente, email.Login, email.Senha, email.Assunto, email.Texto, encoding, destinatarios);
                else
                    Envia(email.Remetente, email.Assunto, email.Texto, encoding, destinatarios);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;

        }
    }
}
