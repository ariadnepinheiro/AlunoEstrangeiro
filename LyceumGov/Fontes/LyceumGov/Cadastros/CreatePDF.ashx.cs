using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using QRCoder;
using System.Drawing;
using iTextSharp.text.html;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace Techne.Lyceum.Net.Cadastros
{
    public class CreatePDF : IHttpHandler, IRequiresSessionState
    {
        private IList<PDFSigner> _PDFSigners
        {
            get
            {
                IList<PDFSigner> result = HttpContext.Current.Session["Techne.Lyceum.Net.Cadastros.CreatePDF.PDFSigners"] as IList<PDFSigner>;
                result = result ?? new List<PDFSigner>();
                HttpContext.Current.Session["Techne.Lyceum.Net.Cadastros.CreatePDF.PDFSigners"] = result;
                return result;
            }
        }

        private string Certificado
        {
            get
            {
                return (HttpContext.Current.Request.Form["Certificado"] as string);
            }
        }

        private int MaeInscricaoId
        {
            get
            {
                var result = 0;
                int.TryParse(HttpContext.Current.Request.Form["MaeInscricaoId"] ?? "0", out result);
                return result;
            }
        }

        private string Nome
        {
            get
            {
                return (HttpContext.Current.Request.Form["Nome"] as string);
            }
        }

        private string RG
        {
            get
            {
                return (HttpContext.Current.Request.Form["RG"] as string);
            }
        }

        private string CPF
        {
            get
            {
                return (HttpContext.Current.Request.Form["CPF"] as string);
            }
        }
        
        private string URLBusca
        {
            get
            {
                return ConfigurationManager.AppSettings["MaeURLBusca"];
            }
        }

        private string URLQRCode
        {
            get
            {
                return ConfigurationManager.AppSettings["MaeQRCode"];
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (Certificado.IsNullOrEmptyOrWhiteSpace())
            {
                context.Response.Write("É necessário fornecer o parâmetro \"Certificado\" na query string");
                context.Response.End();
                return;
            }
            
            if (Nome.IsNullOrEmptyOrWhiteSpace())
            {
                context.Response.Write("É necessário fornecer o parâmetro \"Nome\" na query string");
                context.Response.End();
                return;
            }
            
            if (RG.IsNullOrEmptyOrWhiteSpace())
            {
                context.Response.Write("É necessário fornecer o parâmetro \"RG\" na query string");
                context.Response.End();
                return;
            }
            
            if (CPF.IsNullOrEmptyOrWhiteSpace())
            {
                context.Response.Write("É necessário fornecer o parâmetro \"CPF\" na query string");
                context.Response.End();
                return;
            }

            MemoryStream outputStream = new MemoryStream();

            iTextSharp.text.Document docPdf = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            PdfWriter.GetInstance(docPdf, outputStream);
            docPdf.Open();

            var imgpath = (context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath + "/Images/logo_seeduc_new.png").Replace("//Images/logo_seeduc_new.png", "/Images/logo_seeduc_new.png");
            var now = DateTime.Now;

            docPdf.AddHtmlToDocument("<p><img src='" + imgpath + "' width='85%'></p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_CENTER);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p style='text-align: right;'>Rio de Janeiro, {{Dia}} de {{Mes}} de {{Ano}}</p>"
                .Replace("{{Dia}}", now.Day.ToString())
                .Replace("{{Mes}}", new string[] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" }.GetValue(now.Month - 1) as string)
                .Replace("{{Ano}}", now.Year.ToString())
                , elem => ((Paragraph)elem).Alignment = Element.ALIGN_RIGHT);
            docPdf.AddHtmlToDocument("<p style='text-align: left;'>Ao<br /><b>Banco Bradesco</b><br />Att. Sr. Gerente</p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_LEFT);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p>Assunto: Servidor/cotista/pensionista – Abertura de conta para <b><u>Crédito de Salário</u></b></p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_LEFT);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p>Prezado Senhor,</p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_LEFT);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument(@"
                <p>
                    Apresentamos o (a) Sr.(a) {{Nome}}, portador(a) do RG nº 
                    {{RG}} e CPF nº {{CPF}}, com o propósito de ser creditado mensalmente 
                    os vencimentos da Universidade do Estado do Rio de Janeiro – UERJ, através do 
                    Projeto Escola Criativa e de Oportunidades – Eco Escola.
                </p>
            "
                .Replace("{{Nome}}", Nome)
                .Replace("{{RG}}", RG)
                .Replace("{{CPF}}", CPF)
                , elem => ((Paragraph)elem).Alignment = Element.ALIGN_JUSTIFIED);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument(@"
                <p>
                    Os servidores/cotistas/pensionistas do Governo do Estado do Rio de Janeiro já possuem 
                    aprovado um Pacote de Benefícios exclusivo, com direito a tarifas diferenciadas, razão 
                    pela qual não deverá cadastrar nenhuma cesta de serviço, pois o mesmo será efetuado 
                    automaticamente após o primeiro crédito salarial.
                </p>
            ", elem => ((Paragraph)elem).Alignment = Element.ALIGN_JUSTIFIED);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p>Sem mais, agradecemos.</p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_LEFT);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p>Atenciosamente,</p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_LEFT);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<br />", null);
            docPdf.AddHtmlToDocument("<p>__________________________________________________________________________</p>", elem => ((Paragraph)elem).Alignment = Element.ALIGN_JUSTIFIED);
            docPdf.AddHtmlToDocument(@"
                <p>
                    Para utilização do Bradesco: NESTE ESPAÇO COLAR A ETIQUETA COM O NÚMERO DA AGÊNCIA E CONTA PARA RECEBIMENTO DO SALÁRIO. DEVOLVER A CARTA AO CLIENTE.
                </p>
            ", elem => ((Paragraph)elem).Alignment = Element.ALIGN_JUSTIFIED);
            docPdf.AddHtmlToDocument("<br />", null);

            var tags = new HTMLTagProcessors();
            tags[HtmlTags.IMG] = new CustomImageHTMLTagProcessor();
            
            //var uniqueId = ((DateTime.Now.Ticks) - (new DateTime(2016, 1, 1).Ticks)).ToString("x");
            //var crc16 = new Crc16().ComputeChecksum(outputStream.ToArray());
            //var crc32 = new DamienG.Security.Cryptography.Crc32().ComputeHashHex(outputStream.ToArray());
            var verificador = ScrambleNumber(MaeInscricaoId);
            var crc = new DamienG.Security.Cryptography.Crc32().ComputeHashHex(MaeInscricaoId.ToString("00000000").ToByteArray());

            docPdf.AddHtmlToDocument(string.Format(@"
                <p>
                    <table cellspacing='0' cellpadding='0' border='0'>
                        <tr>
                            <td width='130' height='100' align='left'><img src='data:image/png;base64,{0}' width='90px' height='90px' /></td>
                            <td width='570' height='100' align='left'>
                                <font size='1'>
                                A autenticidade deste documento pode ser conferida escaneando o QRCode ou acessando o link:<br />
                                <u>{1}</u><br />
                                Código Verificador: {2} - Código CRC: {3}<br /><br />
                                Verifique se as informações de assinatura são idênticas ao que aparece na tela.
                                </font>
                            </td>
                        </tr>
                    </table>
                </p>", Convert.ToBase64String(QRCode(URLQRCode.Replace("{{verificador}}", verificador).Replace("{{crc}}", crc))), URLBusca, verificador, crc), tags, null);

            docPdf.Close();

            PDFSigner pdfSigner = new PDFSigner(outputStream.ToArray()) { Certificate = Certificado, Tag = verificador };
            _PDFSigners.Add(pdfSigner);
            var result = new JavaScriptSerializer().Serialize(new { 
                Id = pdfSigner.GetHashCode(),
                Hash = pdfSigner.GenerateHash(),
            });

            context.Response.ContentType = "application/json";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static byte[] QRCode(string urlDestaPagina)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(urlDestaPagina, QRCodeGenerator.ECCLevel.Q, false, false);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = new Bitmap(qrCode.GetGraphic(2), new Size(90, 90));
            //Bitmap qrCodeImage = qrCode.GetGraphic(2);

            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public string PasswordEncrypt(string inText, string key)
        {
            var generateSalt = new Func<byte[]>(() =>
            {
                var randomBytes = new byte[8];
                var rngCsp = new RNGCryptoServiceProvider();
                rngCsp.GetBytes(randomBytes);
                return randomBytes;
            });

            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
            using (Aes aes = Aes.Create())
            {
                var salt = generateSalt();
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, salt);
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    inText = Convert.ToBase64String(mStream.ToArray().Concat(salt).ToArray());
                }
            }
            return inText;
        }

        public string ScrambleNumber(int unscrambledNumber)
        {
            var number = unscrambledNumber.ToString("00000000");
            var scrambledNumber = "";
            for (int i = 0; i < number.Length; i++) 
            {
                var tmp = int.Parse(number.Substring(i, 1));
                scrambledNumber += (i % 2 == 1 ? 9 - tmp : tmp).ToString();
            }
            return scrambledNumber;
        }

        public int UnscrambleNumber(string scrambledNumber)
        {
            var number = scrambledNumber;
            var unscrambledNumber = "";
            for (int i = 0; i < number.Length; i++)
            {
                var tmp = int.Parse(number.Substring(i, 1));
                unscrambledNumber = unscrambledNumber + (i % 2 == 1 ? 9 - tmp : tmp).ToString();
            }
            return int.Parse(unscrambledNumber);
        }
    }

    public class CustomImageHTMLTagProcessor : IHTMLTagProcessor
    {
        /// <summary>
        /// Tells the HTMLWorker what to do when a close tag is encountered.
        /// </summary>
        public void EndElement(HTMLWorker worker, string tag)
        {
        }

        /// <summary>
        /// Tells the HTMLWorker what to do when an open tag is encountered.
        /// </summary>
        public void StartElement(HTMLWorker worker, string tag, IDictionary<string, string> attrs)
        {
            iTextSharp.text.Image image;
            var src = attrs["src"];

            if (src.StartsWith("data:image/"))
            {
                // data:[<MIME-type>][;charset=<encoding>][;base64],<data>
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                image = iTextSharp.text.Image.GetInstance(imagedata);
            }
            else
            {
                image = iTextSharp.text.Image.GetInstance(src);
            }

            worker.UpdateChain(tag, attrs);
            worker.ProcessImage(image, attrs);
            worker.UpdateChain(tag);
        }
    }

    public static class PdfExtension
    {
        public static void AddHtmlToDocument(this iTextSharp.text.Document docPdf, string html, Action<IElement> config)
        {
            AddHtmlToDocument(docPdf, html, null, config);
        }

        public static void AddHtmlToDocument(this iTextSharp.text.Document docPdf, string html, HTMLTagProcessors tags, Action<IElement> config)
        {
            var lista = HTMLWorker.ParseToList(new StringReader(html), null, tags, null);
            foreach (var elem in lista)
            {
                if (config != null)
                    config(elem as IElement);
                docPdf.Add(elem as IElement);
            }
        }
    }

    public class Crc16
    {
        const ushort polynomial = 0xA001;
        static readonly ushort[] table = new ushort[256];

        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }

        public Crc16()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }
}

namespace DamienG.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    /// <summary>
    /// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
    /// </summary>
    /// <remarks>
    /// Crc32 should only be used for backward compatibility with older file formats
    /// and algorithms. It is not secure enough for new applications.
    /// If you need to call multiple times for the same data either use the HashAlgorithm
    /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
    /// being passed in as the seed for the next Compute call.
    /// </remarks>
    public sealed class Crc32 : HashAlgorithm
    {
        public const UInt32 DefaultPolynomial = 0xedb88320u;
        public const UInt32 DefaultSeed = 0xffffffffu;

        static UInt32[] defaultTable;

        readonly UInt32 seed;
        readonly UInt32[] table;
        UInt32 hash;

        public Crc32()
            : this(DefaultPolynomial, DefaultSeed)
        {
        }

        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("Not supported on Big Endian processors");

            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = CalculateHash(table, hash, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32ToBigEndianBytes(~hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 32; } }

        public static UInt32 Compute(byte[] buffer)
        {
            return Compute(DefaultSeed, buffer);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return Compute(DefaultPolynomial, seed, buffer);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
                return defaultTable;

            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry >>= 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                defaultTable = createTable;

            return createTable;
        }

        static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
        {
            var hash = seed;
            for (var i = start; i < start + size; i++)
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            return hash;
        }

        static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        public string ComputeHashHex(byte[] buffer)
        {
            byte[] hash = ComputeHash(buffer);
            string result = "";
            foreach (byte b in hash)
                result += b.ToString("x");
            return result;
        }
    }
}
