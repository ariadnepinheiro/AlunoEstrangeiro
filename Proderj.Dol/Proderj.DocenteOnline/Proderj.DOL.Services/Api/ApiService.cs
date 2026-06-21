using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Script.Serialization;

namespace Proderj.DOL.Service
{
	public class ApiService : IApiService
	{

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("d$s&T!20%@22@*V`");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12"); 
        private IApiRepository repositorioApi;
        private readonly ILoginService loginService;
        public ApiService(IApiRepository repositorioApi,ILoginService loginService )
        { 
            this.repositorioApi = repositorioApi;
            this.loginService = loginService;          
        }

        public DTODocenteLogado ValidaUsuario(string crp)
        {
            DTODocenteLogado dtoDocenteLogado = null;
            //this.repositorioApi = repositorioApi;
            //this.loginService = loginService;
            string xmlaux = Decrypt(HttpContext.Current.Request.QueryString["crp"]);
            string vaipara = HttpContext.Current.Request.QueryString["sit"];
            XmlSerializer serializer = new XmlSerializer(typeof(UsuarioXml));
            UsuarioXml usuario;

            using (StringReader reader = new StringReader(xmlaux))
            {
                usuario = (UsuarioXml)serializer.Deserialize(reader);
            }

            string aux = usuario.UsuarioNome;
            var palavras = aux.Split('/');
            var mtr = loginService.VerificaNumFunc(palavras[1], Convert.ToInt64(palavras[0]));
            dtoDocenteLogado = loginService.VerificaLoginApi(mtr, usuario.Senha, palavras[0], palavras[1]);
            return dtoDocenteLogado;
        }

        public String EnviaUsuario(string Usuario, string Senha)
        {
            var datahoraatual = DateTime.Now;

            var usuarioenvia = new DadosUsuario
            {
                usuario = Usuario,
                senha = Senha,
                dataValidade = datahoraatual.AddSeconds(1)
            };
            string xmlaux = Encrypt(MySerializer<DadosUsuario>.Serialize(usuarioenvia));

            return xmlaux;
        }

        public string EnviaUsuario(string usuario, string senha, int duracaoDoTokenEmSegundos)
        {
            var datahoraatual = DateTime.Now;

            var usuarioenvia = new DadosUsuario
            {
                usuario = usuario,
                senha = senha,
                dataValidade = datahoraatual.AddSeconds(duracaoDoTokenEmSegundos)
            };
            string xmlaux = Encrypt(MySerializer<DadosUsuario>.Serialize(usuarioenvia));

            return xmlaux;
        }

        public class MySerializer<T> where T : class
        {
            public static string Serialize(T obj)
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
                using (var sww = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented })
                    {
                        xsSubmit.Serialize(writer, obj);
                        return sww.ToString();
                    }
                }
            }
        }
        public class DadosUsuario
        {
            public string usuario { get; set; }
            public string senha { get; set; }
            public DateTime dataValidade { get; set; }
        }
        [XmlRoot("DadosUsuario")]
        public class UsuarioXml
        {
            [XmlElement("usuario")]
            public string UsuarioNome { get; set; }

            [XmlElement("senha")]
            public string Senha { get; set; }

            [XmlElement("dataValidade")]
            public DateTime DataValidade { get; set; }
        }

        public static string Decrypt(string cipherText)
        {
            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText.Replace(' ', '+'))))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        public DTOApi ReceberDados(string idFuncional)
		{

      		return new DTOApi
			{
				Chr  = idFuncional
			};			
		}

    

	}
}
