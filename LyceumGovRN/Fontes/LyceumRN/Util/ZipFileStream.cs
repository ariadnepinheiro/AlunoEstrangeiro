using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;

namespace Techne.Lyceum.RN.Util
{
    public class ZipFileStream
    {
        [Serializable]
        public class FileStream
        {
            public string NomeArquivo { get; set; }
            public Stream StreamArquivo { get; set; }
        }

        public List<FileStream> ListaItensStream;

        public ZipFileStream()
        {
            this.ListaItensStream = new List<FileStream>();
        }

        public void AdicionaFileStreamZip(string NomeArquivo, Stream StreamArquivo)
        {
            FileStream fileStream = new FileStream();
            fileStream.NomeArquivo = NomeArquivo;
            fileStream.StreamArquivo = StreamArquivo;
            this.ListaItensStream.Add(fileStream);
        }

        public byte[] retornaBytesZipados()
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);

            int countArquivos = 0;

            foreach (FileStream fi in ListaItensStream)
            {
                ZipEntry entry = new ZipEntry((fi.NomeArquivo));
                zipStream.PutNextEntry(entry);
                fi.StreamArquivo.Position = 0;
                StreamUtils.Copy(fi.StreamArquivo, zipStream, new byte[4096]);
                zipStream.CloseEntry();
                fi.StreamArquivo.Position = 0;
                countArquivos++;
            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }
    }
}
