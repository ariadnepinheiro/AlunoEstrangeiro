using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class UnidadeDadosInternet
    {
        public UnidadeDadosInternet()
        {
            AcessoInternet = new List<int>();
        }

        //LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA
        public string Censo { get; set; }

        //IDs DA TABELA ACESSOINTERNET (UNIDADEFISICA_ACESSOINTERNET)
        public List<int> AcessoInternet { get; set; }

        //UNIDADEFISICA_REDEINTERNET
        public string BandaLarga { get; set; }

        //UNIDADEFISICA_REDEINTERNET
        public string DispositivoEscola { get; set; }

        //UNIDADEFISICA_REDEINTERNET
        public string DispositivoPessoal { get; set; }

        //UNIDADEFISICA_REDEINTERNET
        public string RedeCabo { get; set; }

        //UNIDADEFISICA_REDEINTERNET
        public string RedeWireless { get; set; }

        public string SemRedeComputador { get; set; }

        //MATRICULA 
        public string UsuarioResponsavel { get; set; }
    }
}

