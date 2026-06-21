using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;

namespace Techne.Lyceum.RN.Util
{
    public class PDFSigner : IDisposable
    {
        private PdfReader reader;
        private MemoryStream ms;
        private PdfStamper stamper;
        private PdfSignatureAppearance appearance;
        private const int RESERVED_SPACE_SIGNATURE = 8192;

        private byte[] originalBytes;

        public PDFSigner(byte[] Content) :
            this(new PdfReader(Content))
        {
            originalBytes = Content;
        }

        public PDFSigner(string fileName) :
            this(new PdfReader(fileName))
        {
        }

        public PDFSigner(PdfReader Reader)
        {
            reader = Reader;
            ms = new MemoryStream();
            stamper = PdfStamper.CreateSignature(reader, ms, '\0');
            appearance = stamper.SignatureAppearance;
        }

        public static void RemoveSignatures(string src, string dest)
        {
            using (var reader = new PdfReader(src))
            {
                var names = reader.AcroFields.GetSignatureNames();
                using (var memStream = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader, memStream, '\0', true))
                    {
                        foreach (var name in names)
                            stamper.AcroFields.RemoveField(name);

                        stamper.Close();
                        File.WriteAllBytes(dest, memStream.ToArray());
                    }
                }
            }
        }

        public string Certificate { get; set; }

        public string Tag { get; set; }

        public string GenerateHash()
        {
            if (appearance.IsPreClosed())
                return null;

            string Reason = "Assinatura Digital";
            string Location = "Secretaria de Estado da Educação do Rio de Janeiro";
            string Contact = "(21) 91234-4321";

            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Convert.FromBase64String(Certificate));

            var cn = certificate
                .SubjectName
                .Name
                .Split(',')
                .Select(i => i.Trim().Split('=')[1].Split(':')[0])
                .FirstOrDefault() ?? "";

            string signatureFieldName = null;
            appearance.SetVisibleSignature(new Rectangle(420, 275, 220, 325), 1, signatureFieldName);
            appearance.SignDate = DateTime.Now;
            appearance.Reason = Reason;
            appearance.Location = Location;
            appearance.Contact = Contact;
            StringBuilder buf = new StringBuilder();
            buf.Append("Assinado digitalmente por");
            buf.Append("\n");
            buf.Append(cn);
            buf.Append("\n");
            buf.Append("Data: " + appearance.SignDate);
            appearance.Layer2Text = buf.ToString();
            appearance.Acro6Layers = true;
            appearance.CertificationLevel = 0;

            PdfSignature dic = GeneratePdfSignature(certificate.IssuerName.Name);

            appearance.CryptoDictionary = dic;

            Dictionary<PdfName, int> exclusionSizes = new Dictionary<PdfName, int>();
            exclusionSizes.Add(PdfName.CONTENTS, (RESERVED_SPACE_SIGNATURE * 2) + 2);
            appearance.PreClose(exclusionSizes);
            
            HashAlgorithm sha = new SHA256CryptoServiceProvider();
            Stream s = appearance.GetRangeStream();
            int read = 0;
            byte[] buff = new byte[0x2000];
            while ((read = s.Read(buff, 0, 0x2000)) > 0)
            {
                sha.TransformBlock(buff, 0, read, buff, 0);
            }
            sha.TransformFinalBlock(buff, 0, 0);

            return Convert.ToBase64String(sha.Hash);
        }

        public byte[] GetOriginalBytes()
        {
            return originalBytes;
        }

        private PdfSignature GeneratePdfSignature(string userName)
        {
            PdfSignature dic = new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED)
            {
                Date = new PdfDate(appearance.SignDate),
                Name = userName
            };
            dic.Reason = appearance.Reason;
            dic.Location = appearance.Location;
            dic.Contact = appearance.Contact;
            return dic;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
        }

        public byte[] SignPDFToMemory(byte[] pk)
        {
            byte[] paddedSig = new byte[RESERVED_SPACE_SIGNATURE];
            System.Array.Copy(pk, 0, paddedSig, 0, pk.Length);

            PdfDictionary dic2 = new PdfDictionary();
            dic2.Put(PdfName.CONTENTS, new PdfString(paddedSig).SetHexWriting(true));
            appearance.Close(dic2);

            return ms.ToArray();
        }

        public void SignPDFToNewFile(byte[] pk, string destinyPDFSigned)
        {
            var pdfInMemory = SignPDFToMemory(pk);

            File.WriteAllBytes(destinyPDFSigned, pdfInMemory);
        }

        public bool IsValid(string fileName)
        {
            try
            {
                new PdfReader(fileName);
                return true;
            }
            catch (InvalidPdfException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (reader != null)
                reader.Dispose();
            if (ms != null)
                ms.Dispose();
            if (stamper != null)
                stamper.Dispose();
        }
    }
}