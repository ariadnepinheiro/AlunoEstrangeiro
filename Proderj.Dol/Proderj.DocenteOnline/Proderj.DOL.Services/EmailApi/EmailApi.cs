using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace Proderj.DOL.Service
{
    public class EmailApi
    {
        public class EmailDTO
        {
            public EmailDTO()
            {
                Smtp = new SmtpDTO();
                Message = new MessageDTO();
            }

            public string File { get; set; }
            public SmtpDTO Smtp { get; set; }
            public MessageDTO Message { get; set; }

            public class SmtpDTO
            {
                public string Host { get; set; }
                public int Port { get; set; }
                public string UserName { get; set; }
                public string DisplayName { get; set; }
                public string Password { get; set; }
                public bool EnableSSL { get; set; }
            }

            public class MessageDTO
            {
                public MessageDTO()
                {
                    To = new HashSet<MailAddressDTO>();
                    Cc = new HashSet<MailAddressDTO>();
                    Bcc = new HashSet<MailAddressDTO>();
                }

                public MailAddressDTO From { get; set; }
                public ICollection<MailAddressDTO> To { get; set; }
                public ICollection<MailAddressDTO> Cc { get; set; }
                public ICollection<MailAddressDTO> Bcc { get; set; }
                public string Subject { get; set; }
                public string Body { get; set; }
                public bool IsBodyHtml { get; set; }

                public class MailAddressDTO
                {
                    public string Address { get; set; }
                    public string Name { get; set; }
                }
            }
        }

        public string EmailApiSend(EmailDTO email)
        {
            var result = new List<string>();
            try
            {
                var authUrl = System.Configuration.ConfigurationManager.AppSettings["EmailApiAuthUrl"];
                var newEmailUrl = System.Configuration.ConfigurationManager.AppSettings["EmailApiNewEmailUrl"];
                var runOutboxUrl = System.Configuration.ConfigurationManager.AppSettings["EmailApiRunOutboxUrl"];
                var login = System.Configuration.ConfigurationManager.AppSettings["EmailApiLogin"];
                var senha = System.Configuration.ConfigurationManager.AppSettings["EmailApiSenha"];

                var authRequest = new JavaScriptSerializer().Serialize(new { Login = login, Senha = senha });
                var authResponse = PostWebApi(authRequest, null, authUrl);
                result.Add("authResponse: " + authResponse);

                var accessToken = string.Empty;
                try
                {
                    var authObject = new JavaScriptSerializer().Deserialize<TokenResult>(authResponse);
                    accessToken = authObject.AccessToken;
                }
                catch
                {
                    throw new AuthResponseException();
                }

                var newEmailRequest = new JavaScriptSerializer().Serialize(email);
                var newEmailResponse = PostWebApi(newEmailRequest, accessToken, newEmailUrl);
                result.Add("newEmailResponse: " + newEmailResponse);

                if (!newEmailResponse.Contains("Email criado com sucesso:"))
                    throw new NewEmailResponseException();

                var runOutboxResponse = PostWebApi(null, accessToken, runOutboxUrl);
                result.Add("runOutboxResponse: " + runOutboxResponse);

                if (!runOutboxResponse.Contains("e-mail(s) enviado(s) com sucesso.") && !runOutboxResponse.Contains("Não havia(m) e-mail(s) para ser(em) enviado(s)"))
                    throw new RunOutboxResponseException(runOutboxResponse);

                return result.Aggregate((current, next) => current + Environment.NewLine + next);
            }
            catch (AuthResponseException)
            {
                throw;
            }
            catch (NewEmailResponseException)
            {
                throw;
            }
            catch (RunOutboxResponseException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Erro ao enviar e-mail: " + ex.Message);
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

            if (!string.IsNullOrWhiteSpace(bearerToken))
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

        private class TokenResult
        {
            public string Login { get; set; }
            public string AccessToken { get; set; }
        }        

        private class AuthResponseException : System.Exception
        {
            public AuthResponseException()
                : base("Erro ao enviar e-mail: Erro na autenticação do EmailApi.")
            {
            }
        }

        private class NewEmailResponseException : System.Exception
        {
            public NewEmailResponseException()
                : base("Erro ao enviar e-mail: A criação da mensagem de e-mail deu erro.")
            {
            }
        }

        private class RunOutboxResponseException : System.Exception
        {
            public RunOutboxResponseException()
                : base("Erro ao enviar e-mail: O disparo da mensagem de e-mail deu erro.")
            {
            }

            public RunOutboxResponseException(string message)
                : base(message)
            {
            }
        }
    }
}
