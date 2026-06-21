using System.IO;
using System.Drawing;
using System;
using System.Drawing.Imaging;

namespace Techne.Lyceum.RN.Util
{
    public class Imagem
    {
        public byte[] RedimencionaImagemPor(byte[] imagem, int altura, int largura)
        {
            // Monta a imagem original.
            Stream objStream = new MemoryStream(imagem);
            BinaryReader objBinaryReader = new BinaryReader(objStream);
            int i = (int)objStream.Length;
            byte[] novaImagem = objBinaryReader.ReadBytes(i);

            // Monta a imagem redimensionada.
            if (altura > 0 && largura > 0)
            {
                // Lê os bytes da imagem original.
                MemoryStream objMemoryStream = new MemoryStream(novaImagem);

                // Monta uma nova imagem.
                System.Drawing.Bitmap imageBitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(objMemoryStream);

                // Atribui o tamanho à nova imagem.
                System.Drawing.Bitmap imageModificada = new System.Drawing.Bitmap((int)(altura), (int)(largura));

                // Define o desenho da nova imagem.
                Graphics graphic = Graphics.FromImage(imageModificada);
                graphic.DrawImage(imageBitmap, new System.Drawing.Rectangle(0, 0, imageModificada.Width, imageModificada.Height), 0, 0, imageBitmap.Width, imageBitmap.Height, System.Drawing.GraphicsUnit.Pixel);
                graphic.Dispose();
                MemoryStream objMemoryStreamModificado = new MemoryStream();

                // Salva a nova imagem no objeto de memória.
                imageModificada.Save(objMemoryStreamModificado, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Recupera os bytes da imagem modificada.
                novaImagem = objMemoryStreamModificado.GetBuffer();

                //Verifica tamanho da imagem
                int tamanhoByte = Buffer.ByteLength(novaImagem);
                long qualidade = 50L;

                //Enquanto a imagem estiver fora do padrao tenta redimensionar
                while ((tamanhoByte < 8192 || tamanhoByte > 32000)
                    && qualidade > 0L && qualidade < 101L)
                {
                    var encoderParameters = new EncoderParameters(1);

                    if (tamanhoByte < 8192)
                    {
                        qualidade = qualidade + 5L;
                    }
                    else if (tamanhoByte > 32768)
                    {
                        qualidade = qualidade - 5L;
                    }

                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualidade);

                    // Salva a nova imagem no objeto de memória.
                    imageModificada.Save(objMemoryStreamModificado, GetEncoder(ImageFormat.Jpeg), encoderParameters);

                    // Recupera os bytes da imagem modificada.
                    novaImagem = objMemoryStreamModificado.GetBuffer();
                    tamanhoByte = Buffer.ByteLength(novaImagem);
                }
            }

            return novaImagem;
        }

        public byte[] ComprimiImagemPor(byte[] imagem, int tamanhoByteMinimo, int tamanhoByteMaximo)
        {
            byte[] novaImagem = null;
            int tamanhoByte = Buffer.ByteLength(imagem);
            long qualidade = 100L;

            while ((tamanhoByte < tamanhoByteMinimo || tamanhoByte > tamanhoByteMaximo) && qualidade > 0L)
            {
                // Monta a imagem original.
                Stream objStream = new MemoryStream(imagem);
                BinaryReader objBinaryReader = new BinaryReader(objStream);
                int i = (int)objStream.Length;
                novaImagem = objBinaryReader.ReadBytes(i);

                // Lê os bytes da imagem original.
                MemoryStream objMemoryStream = new MemoryStream(novaImagem);

                // Monta uma nova imagem.
                System.Drawing.Bitmap imageBitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(objMemoryStream);

                System.Drawing.Bitmap imageModificada = new System.Drawing.Bitmap(imageBitmap);
                MemoryStream objMemoryStreamModificado = new MemoryStream();
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualidade);

                // Salva a nova imagem no objeto de memória.
                imageModificada.Save(objMemoryStreamModificado, GetEncoder(ImageFormat.Jpeg), encoderParameters);

                // Recupera os bytes da imagem modificada.
                novaImagem = objMemoryStreamModificado.GetBuffer();
                tamanhoByte = Buffer.ByteLength(novaImagem);
                if (tamanhoByte >= tamanhoByteMinimo && tamanhoByte <= tamanhoByteMaximo)
                {
                    return novaImagem;
                }
                else
                {
                    qualidade = qualidade - 1L;
                }
            }

            return imagem;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}