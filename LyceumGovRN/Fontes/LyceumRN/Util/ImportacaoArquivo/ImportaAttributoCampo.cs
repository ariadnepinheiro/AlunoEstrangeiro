using System;
using System.Collections.Generic;
using System.Text;

namespace Techne.Lyceum.RN.Util.ImportacaoArquivo
{
    public enum DataType
    {
        String,
        Integer,
        DateTime
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ImportaAtributoCampo : Attribute
    {
        /// <summary>
        /// Descrição do Campo
        /// </summary>
        public string NomeCampo { get; set; }

        /// <summary>
        /// Posição do campo no arquivo
        /// </summary>
        public int Posicao { get; set; }

        /// <summary>
        /// Setar para true se a validação for requirida
        /// </summary>
        public bool ValidacaoRequirida { get; set; }

        /// <summary>
        /// Expressão de validação para o campo
        /// Regra só será aplicada caso o atributo ValidacaoRequirida for setada para true
        /// </summary>
        public string ValidacaoRegExp { get; set; }

        /// <summary>
        /// Setar para true caso o campo aceite '.'
        /// </summary>
        public bool PermitePontos { get; set; }

        /// <summary>
        /// Número de casas decimais do campo
        /// </summary>
        public int NumeroCasasDecimais { get; set; }

        /// <summary>
        /// Setar para true caso o campo seja requirido
        /// </summary>
        public bool Requirido { get; set; }

        /// <summary>
        /// Tamanho exato do campo
        /// </summary>
        public int TamanhoCampo { get; set; }

        /// <summary>
        /// Tamanho máximo do campo
        /// </summary>
        public int TamanhoMaximoCampo { get; set; }

        /// <summary>
        /// Determina quando o campo deve ter seus caracteres recortados
        /// </summary>
        public bool RetiraEspacos { get; set; }

        /// <summary>
        /// Tipo de dado do campo
        /// </summary>
        public DataType TipoDado { get; set; }

        public ImportaAtributoCampo(int posicao)
        {
            this.Posicao = posicao;
            this.TipoDado = DataType.String;
            this.PermitePontos = true;
        }

    }

}