using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;


namespace Techne.Lyceum.RN.Certificacao
{
    /// <summary>
    /// This class works only for apply digital signature using eToken (Certificate Type: A3)
    /// </summary>
    public class AssinaturaDigitalA3
    {
        #region Properties
        private string PathFileName { get; set; }
        private string CertificateThumbPrint { get; set; }
        #endregion

        public AssinaturaDigitalA3(string certificateThumbPrint)
        {
            this.CertificateThumbPrint = GetCertThumbprint(certificateThumbPrint);
        }

        /// <summary>
        /// Methods to apply digital signature from eToken
        /// </summary>
        public void ApplySignatureToPdf(string pathFileName)
        {
            //this.PathFileName = pathFileName;

            //Get certificate
            //Open the currently logged-in user certificate store
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            //Select a certificate from the certificate store
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, this.CertificateThumbPrint, true);
            store.Close();

            //Verify that a certificate exists 
            if (certs.Count == 0)
            {
                throw new Exception("Certificado não instalado na máquina. Por favor, entrar em contato com a Arqdigital");
            }

            //Open Pdf document
            byte[] pdfData = File.ReadAllBytes(pathFileName);

            //Sign the PDF document
            byte[] signedData = SignDocument(pdfData, certs[0]);

            File.WriteAllBytes(pathFileName, signedData);
        }

        public byte[] ApplySignatureToPdf(byte[] pdfData)
        {
            //this.PathFileName = pathFileName;

            //Get certificate
            //Open the currently logged-in user certificate store
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            //Select a certificate from the certificate store
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, this.CertificateThumbPrint, true);
            store.Close();

            //Verify that a certificate exists 
            if (certs.Count == 0)
            {
                throw new Exception("Certificado não encontrado na máquina.");
            }

            //Sign the PDF document
            byte[] signedData = SignDocument(pdfData, certs[0]);

            return signedData;
        }

        /// <summary>
        /// Remove all white spaces in ThumbPrint and return it
        /// </summary>
        /// <param name="certThumbprint"></param>
        /// <returns></returns>
        private string GetCertThumbprint(string certThumbprint)
        {
            string thumbprint = certThumbprint.Replace(" ", "").ToUpperInvariant();
            if (thumbprint[0] == 8206)
            {
                thumbprint = thumbprint.Substring(1);
            }

            return thumbprint;
        }
        private byte[] SignDocument(byte[] pdfData, X509Certificate2 cert)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var reader = new PdfReader(pdfData);
                var stp = PdfStamper.CreateSignature(reader, stream, '\0');
                var sap = stp.SignatureAppearance;

                //Protect certain features of the document 
                stp.SetEncryption(null,
                    Guid.NewGuid().ToByteArray(), //random password 
                    PdfWriter.ALLOW_PRINTING | PdfWriter.ALLOW_COPY | PdfWriter.ALLOW_SCREENREADERS,
                    PdfWriter.ENCRYPTION_AES_256);

                //Get certificate chain
                var cp = new Org.BouncyCastle.X509.X509CertificateParser();
                var certChain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };

                //Set signature appearance
                BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font font = new Font(helvetica, 12, iTextSharp.text.Font.NORMAL);
                sap.Layer2Font = font;
                sap.SetVisibleSignature(new iTextSharp.text.Rectangle(415, 100, 585, 40), 1, null);


                //Drawing text, shape, and image into the signature appearance.

                var dic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                //Set some stuff in the signature dictionary.
                dic.Date = new PdfDate(sap.SignDate);
                dic.Name = cert.Subject;    //Certificate name

                if (sap.Reason != null)
                {
                    dic.Reason = sap.Reason;
                }
                if (sap.Location != null)
                {
                    dic.Location = sap.Location;
                }

                //Set the crypto dictionary 
                sap.CryptoDictionary = dic;

                //Set the size of the certificates and signature. 
                int csize = 8192; //Size of the signature - 4K

                //Reserve some space for certs and signatures
                var reservedSpace = new Dictionary<PdfName, int>();
                reservedSpace[PdfName.CONTENTS] = csize * 2 + 2; //*2 because binary data is stored as hex strings. +2 for end of field
                sap.PreClose(reservedSpace);    //Actually reserve it 

                //Build the signature 
                HashAlgorithm sha = new SHA1CryptoServiceProvider();

                var sapStream = sap.GetRangeStream();
                int read = 0;
                byte[] buff = new byte[8192];

                while ((read = sapStream.Read(buff, 0, 8192)) > 0)
                {
                    sha.TransformBlock(buff, 0, read, buff, 0);
                }

                sha.TransformFinalBlock(buff, 0, 0);

                byte[] pk = SignMsg(sha.Hash, cert, false);

                //Put the certs and signature into the reserved buffer 
                byte[] outc = new byte[csize];
                Array.Copy(pk, 0, outc, 0, pk.Length);

                //Put the reserved buffer into the reserved space 
                PdfDictionary certificateDictionary = new PdfDictionary();
                certificateDictionary.Put(PdfName.CONTENTS, new PdfString(outc).SetHexWriting(true));

                //Write the signature 
                sap.Close(certificateDictionary);
                //Close the stamper and save it 
                stp.Close();

                reader.Close();

                //Return the saved pdf 
                return stream.GetBuffer();
            }
        }
       
            
        /// <summary>
        /// Calculates the correct hash from eToken based on the PIN code
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cert"></param>
        /// <param name="detached"></param>
        /// <returns></returns>
        private byte[] SignMsg(Byte[] msg, X509Certificate2 cert, bool detached)
        {
            //Place message in a ContentInfo object. This is required to build a SignedCms object. 
            ContentInfo contentInfo = new ContentInfo(msg);

            //Instantiate SignedCms object with the ContentInfo above. 
            //Has default SubjectIdentifierType IssuerAndSerialNumber. 
            SignedCms signedCms = new SignedCms(contentInfo, detached);

            //Formulate a CmsSigner object for the signer. 
            CmsSigner cmsSigner = new CmsSigner(cert);  //First cert in the chain is the signer cert

            //Do the whole certificate chain. This way intermediate certificates get sent across as well.
            cmsSigner.IncludeOption = X509IncludeOption.ExcludeRoot;

            //Sign the CMS/PKCS #7 message. The second argument is needed to ask for the pin.
            signedCms.ComputeSignature(cmsSigner, false);

            //Encode the CMS/PKCS #7 message. 
            return signedCms.Encode();
        }

        public static X509Certificate2 BuscarCertificado()
        {

            X509Certificate2 x509 = null;

            X509Certificate2Collection lcerts;
            X509Store lStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
           // X509Store lStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
           
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



        //---------------------métodos para converter pdf em fileStream

        public byte[] ObtemArquivoPor(int fornecedorDocumentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO (NOLOCK) 
                                            WHERE FORNECEDORDOCUMENTOID = @FORNECEDORDOCUMENTOID ";

                contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOID", SqlDbType.Int, fornecedorDocumentoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

//        public void Insere(DataContext contexto, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
//        {
//            ContextQuery contextQuery = new ContextQuery();

//            contextQuery.Command = @"  INSERT INTO PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO
//                                               (FORNECEDORDOCUMENTOID 
//                                               ,CHAVEARQUIVO
//                                               ,ARQUIVO
//                                               ,TIPOARQUIVO
//                                               ,NOMEARQUIVO
//                                               ,USUARIOID
//                                               ,DATACADASTRO
//                                               ,DATAALTERACAO)
//                                         VALUES
//                                               (@FORNECEDORDOCUMENTOID, 
//                                               NEWID(), 
//                                               @ARQUIVO,
//                                               @TIPOARQUIVO, 
//                                               @NOMEARQUIVO, 
//                                               @USUARIOID, 
//                                               @DATACADASTRO, 
//                                               @DATAALTERACAO) 
//
//                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTOARQUIVO') ";

//            contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOID", SqlDbType.Int, fornecedorDocumentoArquivo.FornecedorDocumentoId);
//            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
//            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
//            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
//            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
//            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
//            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

//            fornecedorDocumentoArquivo.FornecedorDocumentoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
//        }

//        public void Atualiza(DataContext ctx, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo)
//        {
//            ContextQuery contextQuery = new ContextQuery();

//            contextQuery.Command = @"  UPDATE prestacaocontas.FORNECEDORDOCUMENTOARQUIVO 
//                                            SET    ARQUIVO = @ARQUIVO, 
//                                                   TIPOARQUIVO = @TIPOARQUIVO, 
//                                                   NOMEARQUIVO = @NOMEARQUIVO, 
//                                                   USUARIOID = @USUARIOID, 
//                                                   DATAALTERACAO = @DATAALTERACAO 
//                                            WHERE  FORNECEDORDOCUMENTOID = @FORNECEDORDOCUMENTOID ";

//            contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOID", SqlDbType.Int, fornecedorDocumentoArquivo.FornecedorDocumentoId);
//            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
//            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
//            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
//            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
//            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

//            ctx.ApplyModifications(contextQuery);
//        }

//        public void InsereAuditoria(DataContext contexto, Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo, string operacao, string estacao)
//        {
//            ContextQuery contextQuery = new ContextQuery();

//            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.FORNECEDORDOCUMENTOARQUIVO
//                                               (FORNECEDORDOCUMENTOARQUIVOID
//                                               ,FORNECEDORDOCUMENTOID
//                                               ,CHAVEARQUIVO
//                                               ,ARQUIVO
//                                               ,TIPOARQUIVO
//                                               ,NOMEARQUIVO
//                                               ,USUARIOID
//                                               ,DATACADASTRO
//                                               ,DATAALTERACAO
//                                               ,OPERACAO
//                                               ,ESTACAO )
//                                         VALUES
//                                               (@FORNECEDORDOCUMENTOARQUIVOID, 
//                                               @FORNECEDORDOCUMENTOID,
//                                               NEWID(), 
//                                               @ARQUIVO,
//                                               @TIPOARQUIVO, 
//                                               @NOMEARQUIVO, 
//                                               @USUARIOID, 
//                                               @DATACADASTRO, 
//                                               @DATAALTERACAO,
//                                               @OPERACAO,
//                                               @ESTACAO) 
//
//                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTO') ";

//            contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOARQUIVOID", SqlDbType.Int, fornecedorDocumentoArquivo.FornecedorDocumentoArquivoId);
//            contextQuery.Parameters.Add("@FORNECEDORDOCUMENTOID", SqlDbType.Int, fornecedorDocumentoArquivo.FornecedorDocumentoId);
//            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, fornecedorDocumentoArquivo.Arquivo);
//            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.TipoArquivo);
//            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, fornecedorDocumentoArquivo.NomeArquivo);
//            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorDocumentoArquivo.UsuarioId);
//            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
//            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
//            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
//            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

//            contexto.ApplyModifications(contextQuery);
//        }
    }
}