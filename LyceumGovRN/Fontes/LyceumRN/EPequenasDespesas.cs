using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EPequenasDespesas
    {
        public decimal?     ID                  { get; set; }
        public decimal?     IDCompra            { get; set; }
        public decimal?     IDFinalidade        { get; set; }
        public DateTime?    Data                { get; set; }
        public decimal?     Valor               { get; set; }
        public decimal?     Desconto            { get; set; }
        public String       Matricula           { get; set; }
        public String       Observacoes         { get; set; } 

        public decimal      IDPrestacaoContas   { get; set; }
        public String       TipoOperacao        { get; set; }

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