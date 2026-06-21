using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class UnidadeDadosPedagogicos
    {
        public UnidadeDadosPedagogicos()
        {
            OrgaoColegiado = new List<int>();
            MaterialPedagogico = new List<int>();
        }

        //LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA
        public string Censo { get; set; }

        //IDs DA TABELA MATERIALPEDAGOGICO (UNIDADEFISICA_MATERIALPEDAGOGICO)
        public List<int> MaterialPedagogico { get; set; }

        //IDs DA TABELA ORGAOCOLEGIADO (UNIDADEFISICA_ORGAOCOLEGIADO)
        public List<int> OrgaoColegiado { get; set; }

        //LY_UNIDADE_ENSINO
        public string PossuiPaginaWeb { get; set; }

        //LY_UNIDADE_ENSINO
        public string PaginaWeb { get; set; }

        //LY_UNIDADE_ENSINO
        public string PossuiProjetoPedagogico  { get; set; }

        //LY_UNIDADE_ENSINO
        public string CumpriuProjetoPedagogico { get; set; }

        //LY_UNIDADE_FISICA
        public string EspacoEquipamentoEntorno { get; set; }

        //LY_UNIDADE_FISICA
        public string EspacoEscolaComunidade { get; set; }

        //LY_UNIDADE_FISICA
        public string Educacaoambiental { get; set; }
        //LY_UNIDADE_FISICA
        public string ConteudoComponentes { get; set; }
        //LY_UNIDADE_FISICA
        public string Componentecurricular { get; set; }
        //LY_UNIDADE_FISICA
        public string EixoEstuturante { get; set; }
        //LY_UNIDADE_FISICA
        public string EmEventos { get; set; }
        //LY_UNIDADE_FISICA
        public string ProjetosTransversais { get; set; }
        //LY_UNIDADE_FISICA
        public string NOL { get; set; }


        //MATRICULA (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string UsuarioResponsavel { get; set; }
    }
}

