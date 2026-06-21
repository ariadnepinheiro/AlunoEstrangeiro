using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosLotacaoDocenteFuncionario
    {
        //Alimentados pela Tela
        public string Matricula { get; set; }

        public string Funcao { get; set; }

        public string Setor { get; set; }

        public bool Readaptado { get; set; }

        public string UnidadeEnsino { get; set; }

        public string Regional { get; set; }

        public DateTime? DataInicioReadaptacao { get; set; }

        public DateTime? DataFimReadaptacao { get; set; }

        public string Situacao { get; set; }

        public DateTime? DataInicioSituacao { get; set; }

        public DateTime? DataFimSituacao { get; set; }

        public string Reducaoch { get; set; }

        public DateTime? dtinich { get; set; }

        public DateTime? dtfimch { get; set; }

        public string Usuario { get; set; }

        public decimal NumFunc { get; set; }

        public bool LicencaPossuiDataFim { get; set; }

        //Alimentados internamente pelo ValidaAlteracaoLotacaoSituacaoServidor:      

        public bool PossuiAulasAlocadas { get; set; }        

        public LyLotacao LotacaoParaDesativar { get; set; }

        public LyLotacao Lotacao { get; set; }

        public LyLotacao LotacaoFutura { get; set; }

        public LyLicencaDocente LicencaDocente { get; set; }

        public LyLicencaPessoa LicencaPessoa { get; set; }

        public string MotivoAntigo { get; set; }

        public DateTime SituacaoAntigaDataIni { get; set; }

        public string FuncaoAnterior { get; set; }

        public bool EraReadaptado { get; set; }

        public DateTime? DataInicioReadaptacaoAntiga { get; set; }

        public string Categoria { get; set; }
        public string Pessoa { get; set; }
    }
}
