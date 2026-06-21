using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosExclusaoFuncaoLotacao
    {
        public decimal Pessoa { get; set; }

        public string Matricula { get; set; }

        public decimal NumFunc { get; set; }

        public string UsuarioResponsavel { get; set; }

        //Alimentados internamente pelo ValidaAlteracaoLotacaoSituacaoServidor:      

        public LyLotacao LotacaoParaDesativar { get; set; }

        public LyLotacao Lotacao { get; set; }     
    }
}
