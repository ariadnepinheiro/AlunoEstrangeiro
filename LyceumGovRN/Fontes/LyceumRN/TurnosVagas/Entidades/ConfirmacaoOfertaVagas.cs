using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    [AtributoTabela("TurnosVagas.CONFIRMACAOOFERTAVAGAS", Nome = "TurnosVagas.CONFIRMACAOOFERTAVAGAS")]
    public class ConfirmacaoOfertaVagas
    {
        [AtributoCampo(Nome = "CONFIRMACAOOFERTAVAGASID")]
        public int ConfirmacaoOfertaVagasId { get; set; }

        [AtributoCampo(Nome = "CONFIRMACAOOFERTAID")]
        public int ConfirmacaoOfertaId { get; set; }

        [AtributoCampo(Nome = "CURSO")]
        public string Curso { get; set; }

        [AtributoCampo(Nome = "SERIE")]
        public int Serie { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEOPTANTES")]
        public int QuantidadeOptantes { get; set; }

        [AtributoCampo(Nome = "VAGASMANHA")]
        public int VagasManha { get; set; }

        [AtributoCampo(Nome = "VAGASTARDE")]
        public int VagasTarde { get; set; }

        [AtributoCampo(Nome = "VAGASNOITE")]
        public int VagasNoite { get; set; }

        [AtributoCampo(Nome = "VAGASINTEGRAL")]
        public int VagasIntegral { get; set; }
    }
}