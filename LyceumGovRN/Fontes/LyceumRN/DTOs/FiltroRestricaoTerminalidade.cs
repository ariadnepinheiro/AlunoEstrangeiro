namespace Techne.Lyceum.RN.DTOs
{
    public class FiltroRestricaoTerminalidade
    {
        public int Ano { get; set; }

        public int Periodo { get; set; }

        public bool PorRegional { get; set; }

        public string Regional { get; set; }

        public bool PorMunicipio { get; set; }

        public string Municipio { get; set; }

        public bool PorUnidadeEnsino { get; set; }

        public string UnidadeEnsino { get; set; }

        public bool PorCurso { get; set; }

        public string Curso { get; set; }

        public bool PorSerie { get; set; }

        public int Serie { get; set; }      

        public bool Terminalidade { get; set; }
    }
}
