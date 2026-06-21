using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System;

namespace Proderj.DOL.WebApp.Models.Captcha
{
    public class Captcha
    {
        private const int larguraImagem = 200;
        private const int alturaImagem = 60;
        private const int tamanhoFonte = 40;
        private const float V = 4.0f;

        private string chave;
        private Bitmap imagem;

        public string Chave
        {
            get { return this.chave; }
            set { chave = value; }
        }
        public Bitmap Imagem
        {
            get
            {
                if (!string.IsNullOrEmpty(chave) && alturaImagem > 0 && larguraImagem > 0)
                    GeraImagem();
                return this.imagem;
            }
        }

        public Captcha()
        { }

        ~Captcha()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.imagem.Dispose();
        }

        public void GeraImagem()
        {
            Random random = new Random();

            // Imagem
            Bitmap bitmap = new Bitmap(larguraImagem, alturaImagem, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectangle = new Rectangle(0, 0, larguraImagem, alturaImagem);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);

            graphics.FillRectangle(hatchBrush, rectangle);

            // Fonte configurada para ser usada no texto do captcha
            GraphicsPath graphicsPath = new GraphicsPath();
            Font font = new Font("Verdana", tamanhoFonte, FontStyle.Bold);

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphicsPath.AddString(this.chave, font.FontFamily, (int)font.Style, font.Size, rectangle, stringFormat);

            PointF[] points = new[]
                         {
                             new PointF(random.Next(rectangle.Width) / V, random.Next(rectangle.Height) / V), 
                             new PointF(rectangle.Width - (random.Next(rectangle.Width) / V), random.Next(rectangle.Height) / V), 
                             new PointF(random.Next(rectangle.Width) / V, rectangle.Height - (random.Next(rectangle.Height) / V)), 
                             new PointF(rectangle.Width - (random.Next(rectangle.Width) / V), rectangle.Height - (random.Next(rectangle.Height) / V))
                         };

            Matrix matrix = new Matrix();

            matrix.Translate(0.0F, 0.0F);
            graphicsPath.Warp(points, rectangle, matrix, WarpMode.Perspective, 0.0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Gray, Color.DarkGray);
            graphics.FillPath(hatchBrush, graphicsPath);

            // Add some random noise.
            int m = Math.Max(rectangle.Width, rectangle.Height);

            for (int i = 0; i <= (int)(rectangle.Width * rectangle.Height / 30.0f) - 1; i++)
            {
                int xx = random.Next(rectangle.Width);
                int y = random.Next(rectangle.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);

                graphics.FillEllipse(hatchBrush, xx, y, w, h);
            }

            this.imagem = bitmap;

            // Libera os objeto da memória pois os mesmos não são mais necessários
            font.Dispose();
            graphics.Dispose();
            hatchBrush.Dispose();
        }

        public static string GeraChaveSeguranca()
        {
            string chaveSeguranca = string.Empty;
            Random random = new Random();

            // Monta uma string de sete digitos aleatorios
            for (var i = 0; i < 6; i++)
            {
                chaveSeguranca = string.Concat(chaveSeguranca, random.Next(0, 9).ToString());
            }

            return chaveSeguranca;
        }
    }
}