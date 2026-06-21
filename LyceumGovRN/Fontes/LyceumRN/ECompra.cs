using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ECompra
    {
        public decimal      ID                  { get; set; }
        public DateTime?    Data                { get; set; }
        public decimal      IDPrestacaoContas   { get; set; }
        public decimal?     IDFornecedor        { get; set; }
        public decimal      Valor               { get; set; }
        public decimal?     Desconto            { get; set; }        

        public String       TipoOperacao        { get; set; }
        public String       PequenaDespesa      { get; set; }
        public String       NomeFornecedor      { get; set; }

        public decimal?     IDCheque            { get; set; }
        public decimal?     IDContaBanco        { get; set; }
        public decimal?     NumeroCheque        { get; set; }
        public decimal?     ValorCheque         { get; set; }
        public DateTime?    DataEmissao         { get; set; }
        public DateTime?    DataApresentacao    { get; set; }
        public decimal?     Banco               { get; set; }
        public string       Agencia             { get; set; }
        public string       Conta               { get; set; }
    }
}