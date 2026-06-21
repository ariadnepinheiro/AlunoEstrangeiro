using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EItemDespesa
    {
        public decimal? ID                  { get; set; }
        public decimal  IDDespesa           { get; set; }
        public decimal? IDItem              { get; set; }
        public String   Marca               { get; set; }
        public String   Unidade             { get; set; }
        public decimal? ValorUnitario       { get; set; }
        public decimal? ValorTotal          { get; set; }
        public decimal? Desconto            { get; set; }
        public decimal? Quantidade          { get; set; }
        public String   Descricao           { get; set; }
        public decimal? InicioInventario    { get; set; }
        public decimal? FimInventario       { get; set; }
        public String   DescricaoItem       { get; set; }
        public String   CodigoItem          { get; set; }
        public decimal? IDFinalidade        { get; set; }

        public void OnValidateFieldValue(bool newItem)
        {
            if (!ID.HasValue && !newItem)
                throw new Exception("O ID deve deve ser informado");
            if (IDDespesa <= 0)
                throw new Exception("Deve-se informar o código da despesa");
            if (!Quantidade.HasValue || Quantidade == 0)
                throw new Exception("A Quantidade deve ser informada");
            if (!ValorUnitario.HasValue)
                throw new Exception("O Valor Unitário deve ser informado");
            if (!ValorTotal.HasValue)
                throw new Exception("O Valor Total deve ser informado");
            if (string.IsNullOrEmpty(CodigoItem) || CodigoItem.Trim().Length==0)
                throw new Exception("O Item deve ser informado");
        }
    }
}
