using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Techne.Lyceum.RN.Util.ImportacaoArquivo
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImportaAtributoArquivo : Attribute
    {
        /// <summary>
        /// Indica o número de colunas no arquivo
        /// </summary>
        public int Colunas { get; set; }

        /// <summary>
        /// Indica se a primeira linha contém cabeçalho
        /// </summary>
        public bool CabecalhoPrimeiraLinha { get; set; }

        /// <summary>
        /// Indica qual o delimitador do arquivo
        /// </summary>
        public string CaracterDelimitador { get; set; }

        public ImportaAtributoArquivo()
        {
            //Default delimitador
            CaracterDelimitador = ";";
        }

    }
}