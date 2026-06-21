using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAlunoCertificacao
    {
        public decimal Pessoa { get; set; }

        public DateTime DataNascimento  { get; set; } //DT_NASC em pessoa

        public string Nome { get; set; } //NOME_COMPL em pessoa

        public string NomeMae { get; set; } //NOME_MAE em pessoa

        public string NomePai { get; set; } //NOME_PAI em pessoa

        public string RgTipo { get; set; } //RG_TIPO em pessoa -- Sempre "RG"

        public string RgNumero { get; set; } //RG_NUM em pessoa       

        public string RgEmissor { get; set; } //RG_EMISSOR em pessoa

        public string RgUf { get; set; } //RG_UF em pessoa

        public DateTime RgDataExpedicao  { get; set; } //RG_DTEXP em pessoa

        public string MunicipioNascimento { get; set; } //MUNICIPIO_NASC em pessoa

        public string PaisNascimento { get; set; } //PAIS_NASC em pessoa

        public string Nacionalidade { get; set; } //NACIONALIDADE em pessoa

        public string UsuarioResponsavel { get; set; }
    }
}
