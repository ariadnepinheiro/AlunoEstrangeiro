using System.Diagnostics;
using System.IO;

namespace Techne
{
    internal class FileLib
    {
        public static int CompareFileVersion(FileVersionInfo v1, FileVersionInfo v2)
        {
            var major = CompareInt(v1.FileMajorPart, v2.FileMajorPart);
            if (major != 0)
            {
                return major;
            }

            var minor = CompareInt(v1.FileMinorPart, v2.FileMinorPart);
            if (minor != 0)
            {
                return minor;
            }

            var build = CompareInt(v1.FileBuildPart, v2.FileBuildPart);
            if (build != 0)
            {
                return build;
            }

            var pvt = CompareInt(v1.FilePrivatePart, v2.FilePrivatePart);
            if (pvt != 0)
            {
                return pvt;
            }

            return 0;
        }

        /// <summary>
        ///   Obtém o conteúdo de um arquivo texto como string.
        /// </summary>
        /// <param name = "path">Nome e path do arquivo texto.</param>
        public static string GetTextFile(string path)
        {
            StreamReader reader = null;

            try
            {
                reader = File.OpenText(path);
                return reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        ///   Salva uma string como arquivo texto.
        /// </summary>
        /// <param name = "path">Nome e path do arquivo a ser criado.</param>
        /// <param name = "text">String a ser salva.</param>
        public static void SaveText(string path, string text)
        {
            StreamWriter writer = null;

            try
            {
                writer = File.CreateText(path);
                writer.Write(text);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private static int CompareInt(int i1, int i2)
        {
            if (i1 > i2)
            {
                return 1;
            }
            else if (i1 < i2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}