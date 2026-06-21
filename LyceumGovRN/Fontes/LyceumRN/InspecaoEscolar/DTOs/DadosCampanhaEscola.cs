using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.DTOs
{
    public class DadosCampanhaEscola
    {
        public DadosCampanhaEscola()
        {
            RespostasDependencias = new HashSet<DadosRespostaDependencia>();
        }

        public int CAMPANHAESCOLAID { get; set; }
        public int CAMPANHAID { get; set; }
        public string UNIDADE_ENS { get; set; }
        public string USUARIOID { get; set; }
        public DateTime DATACADASTRO { get; set; }
        public DateTime DATAALTERACAO { get; set; }
        public bool? FINALIZADO { get; set; }
        public DateTime? DATAFINALIZACAO { get; set; }

        public ICollection<DadosRespostaDependencia> RespostasDependencias { get; set; }

        public class DadosRespostaDependencia
        {
            public DadosRespostaDependencia()
            {
                RespostasDependenciasOpcoes = new HashSet<DadosRespostaDependenciaOpcao>();
            }

            public int RESPOSTADEPENDENCIAID { get; set; }
            public int CAMPANHAESCOLAID { get; set; }
            public string DEPENDENCIA { get; set; }
            public string FACULDADE { get; set; }
            public bool? PLACAIDENTIFICACAO { get; set; }
            public int? IDENTIFICACAODEPENDENCIAID { get; set; }
            public string USUARIOID { get; set; }
            public DateTime DATACADASTRO { get; set; }
            public DateTime DATAALTERACAO { get; set; }

            public ICollection<DadosRespostaDependenciaOpcao> RespostasDependenciasOpcoes { get; set; }

            public class DadosRespostaDependenciaOpcao
            {
                public int RESPOSTADEPENDENCIAOPCAOID { get; set; }
                public int RESPOSTADEPENDENCIAID { get; set; }
                public int OPCOESASSUNTOID { get; set; }
                public int ACAODIRECAOID { get; set; }
                public string USUARIOID { get; set; }
                public DateTime DATACADASTRO { get; set; }
                public DateTime DATAALTERACAO { get; set; }
            }
        }
    }
}




