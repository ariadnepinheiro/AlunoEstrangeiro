using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.Pkcs;
using System.IO;

namespace Techne.Lyceum.RN.Certificacao
{
    /// <summary>
    /// Classe que encapsula a assinatura digital de arquivos, utilizando-se da API criptográfica do Windows, com código gerenciado (sem usar a DLL do GPID)
    /// </summary>
    public class SignWrappers
    {
        public static byte[] SignFile(X509Certificate2Collection certs, byte[] data)
        {
            try
            {
                ContentInfo content = new ContentInfo(data);
                SignedCms signedCms = new SignedCms(content, false);
                if (VerifySign(data))
                {
                    signedCms.Decode(data); // Se o arquivo está assinado, coloca o mesmo no objeto signedCms para ser co-assinado (mantém as assinaturas atuais e assina com os certificados do parâmetro certs.
                }
                foreach (X509Certificate2 cert in certs)
                {
                    CmsSigner signer = new CmsSigner(cert);
                    signer.IncludeOption = X509IncludeOption.WholeChain;
                    signedCms.ComputeSignature(signer,false);
                }
                return signedCms.Encode();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao assinar arquivo. A mensagem retornada foi: " + ex.Message);
            }
        }

        public static byte[] SignFile(string CertFile, string CertPass, byte[] data)
        {
            FileStream fs = new FileStream(CertFile, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            X509Certificate2 cert = new X509Certificate2(buffer, CertPass);
            fs.Close();
            fs.Dispose();
            return SignFile(cert, data);
        }

        public static byte[] SignFile(string CertFile, string CertPass, string FileName)
        {
            FileStream fsArq = new FileStream(FileName, FileMode.Open);
            byte[] bufArq = new byte[fsArq.Length];
            fsArq.Read(bufArq, 0, bufArq.Length);
            fsArq.Close();
            fsArq.Dispose();
            return SignFile(CertFile, CertPass, bufArq);
        }

        public static byte[] Assinardocumento(X509Certificate2 Certificado, string senha, byte[] Documento, string FileNameSigned)
        {

            // aqui ele recebe o certf, a senha, o arquivo

            //FileStream fs = new FileStream(Certificado, FileMode.Open);
            //byte[] buffer = new byte[fs.Length];
            //fs.Read(buffer, 0, buffer.Length);
            //X509Certificate2 cert = new X509Certificate2(buffer, senha);
            //fs.Close();
            //fs.Dispose();
            //return SignFile(cert, Documento);










            //assinatura do arquivo

            ContentInfo content = new ContentInfo(Documento);
            SignedCms signedCms = new SignedCms(content, false);

            if (VerifySign(Documento)) //verifica se o documento já está assinado...
            {
                signedCms.Decode(Documento); // Se o arquivo está assinado, coloca o mesmo no objeto signedCms para ser co-assinado (mantém as assinaturas atuais e assina com os certificados do parâmetro certs.
            }
        
            //uso apenas um certificado

            //foreach (X509Certificate2 cert in certs)
            //{
                CmsSigner signer = new CmsSigner(Certificado);
                signer.IncludeOption = X509IncludeOption.WholeChain;
                signedCms.ComputeSignature(signer);// assina o documento
            //}
            return signedCms.Encode();//retorna documento assinado


         
           
            //return SignFile(Certificado, Documento);
        }

        public static byte[] SignFile(X509Certificate2 cert, byte[] data)
        {
            X509Certificate2Collection certs = new X509Certificate2Collection(cert);
            return SignFile(certs, data);
        }

        public static byte[] SignFile(X509Certificate2 cert, string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            fs.Dispose();
            return SignFile(cert, buffer);
        }

        public static bool VerifySign(byte[] data)
        {
            try
            {
                SignedCms signed = new SignedCms();
                signed.Decode(data);
            }
            catch
            {
                return false; // Arquivo não assinado
            }
            return true;
        }

        private static X509Certificate2 FindCertOnStore(X509Certificate2 cert)
        {
            X509Store st = new X509Store(StoreLocation.CurrentUser);
            st.Open(OpenFlags.ReadOnly);
            X509Certificate2 ret = null;
            foreach (X509Certificate2 c in st.Certificates)
            {
                if (c.Subject.Equals(cert.Subject))
                {
                    ret = c;
                    break;
                }
            }
            return ret;
        }

        public static X509Certificate2 EscolherCertificado_()
        {
            
            X509Certificate2 x509 = null;
                        
            X509Certificate2Collection lcerts;
            X509Store lStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            // Abre o Store
            lStore.Open(OpenFlags.ReadOnly);

            // Lista os certificados
            lcerts = lStore.Certificates;

            foreach (X509Certificate2 cert in lcerts)
            {
                if (cert.HasPrivateKey && cert.NotAfter > DateTime.Now && cert.NotBefore < DateTime.Now)
                {
                     x509 = cert;
                }
            }
            lStore.Close();


            return x509;



        }

        public static RSACryptoServiceProvider LerDispositivo(RSACryptoServiceProvider key, string PIN)
        {
            CspParameters csp = new CspParameters(key.CspKeyContainerInfo.ProviderType, key.CspKeyContainerInfo.ProviderName);
            SecureString ss = new SecureString();
            foreach (char a in PIN)
            {
                ss.AppendChar(a);
            }
            csp.ProviderName = key.CspKeyContainerInfo.ProviderName;
            csp.ProviderType = key.CspKeyContainerInfo.ProviderType;
            csp.KeyNumber = key.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2;
            csp.KeyContainerName = key.CspKeyContainerInfo.KeyContainerName;
            csp.KeyPassword = ss;
            csp.Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseDefaultKeyContainer;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
            return rsa;
        }
    }
}
