using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;
using Techne.Data;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_prova_discipCustom : Ly_prova_discip.CustomBase
    {
        public override string PreInsert(Ly_prova_discip.Row row, TConnectionWritable cn)
        {
            if (String.IsNullOrEmpty(row.Disciplina)) return "Disciplina: Preenchimento obrigatório.";
            if (String.IsNullOrEmpty(row.Prova)) return "Instrumento: Preenchimento obrigatório.";
            if (String.IsNullOrEmpty(row.Descricao)) return "Descrição: Preenchimento obrigatório.";            
            if (String.IsNullOrEmpty(row.On_line)) return "Online: Preenchimento obrigatório.";
            if (!row.Ordem.HasValue) return "Ordem: Preenchimento obrigatório.";

            return string.Empty;
        }

        public override string PreUpdate(Ly_prova_discip.Row row, TConnectionWritable cn)
        {
            if (String.IsNullOrEmpty(row.Disciplina)) return "Disciplina: Preenchimento obrigatório.";
            if (String.IsNullOrEmpty(row.Prova)) return "Instrumento: Preenchimento obrigatório.";
            if (String.IsNullOrEmpty(row.Descricao)) return "Descrição: Preenchimento obrigatório.";
            if (String.IsNullOrEmpty(row.On_line)) return "Online: Preenchimento obrigatório.";
            if (!row.Ordem.HasValue) return "Ordem: Preenchimento obrigatório.";

            return string.Empty;
        }

        public override string PreDelete(Ly_prova_discip.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }        
    }
}
