using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosArquivoXml
    {
        public DadosArquivoXml()
        {
            itensXml = new List<DadosXmlItem>();
        }

        public Stream ArquivoXml { get; set; }

        public string NomeArquivo { get; set; }

        public string TipoArquivo { get; set; }

        public List<DadosXmlItem> itensXml { get; set; } //Será Alimentado no metodo Valida
    }
}
