using System;
using System.Globalization;

using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_nucleoCustom: Ly_nucleo.CustomBase
    {
        public override string PreInsert(Ly_nucleo.Row row, TConnectionWritable cn)
        {
            RN.Coordenadoria.GeraNumeroCoordenadoria(row);

            RN.RNBase.RetirarEspaco(row);
            if (!string.IsNullOrEmpty(row.End_num) && !Validacao.Validou(row.End_num, Validacao.Tipo.numerico)) return "Número inválido.<br>O número do endereço deve ter somente números.";
            
            if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();
            return string.Empty;
        }

        public override string PreUpdate(Ly_nucleo.Row row, TConnectionWritable cn)
        {
            RN.RNBase.RetirarEspaco(row);
            if (!string.IsNullOrEmpty(row.End_num) && !Validacao.Validou(row.End_num, Validacao.Tipo.numerico)) return "Número inválido.<br>O número do endereço deve ter somente números.";
            if (!string.IsNullOrEmpty(row.Fone)) row.Fone = row.Fone.RetirarMascaraTelefone();

            return string.Empty;
        }

        public override string PreDelete(Ly_nucleo.Row row, TConnectionWritable cn)
        {

            return string.Empty;
        }
    }
}
