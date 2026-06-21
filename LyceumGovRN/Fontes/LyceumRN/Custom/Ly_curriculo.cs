using System;
using System.Globalization;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_curriculoCustom : Ly_curriculo.CustomBase
    {
        public override string PreInsert(Ly_curriculo.Row row, TConnectionWritable cn)
        {
            if (!Validacao.ValidouDataPodeHoje(row.Dt_extincao, Validacao.Tipo.data)) return "Data de extinção inválida.<br>A data de extinção deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            if (!Validacao.ValidouDataPodeHoje(row.Dt_homolog, Validacao.Tipo.data)) return "Data de homologação inválida.<br>A data de homologação deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            if (!Validacao.ValidouDuasDatas(row.Dt_homolog, row.Dt_extincao)) return "Data homologação/extinção inválidas.<br>A data de homologação deve ser inferior a data de extinção.";
            return string.Empty;
        }

        public override string PreUpdate(Ly_curriculo.Row row, TConnectionWritable cn)
        {
            if (!Validacao.ValidouDataPodeHoje(row.Dt_extincao, Validacao.Tipo.data)) return "Data de extinção inválida.<br>A data de extinção deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            if (!Validacao.ValidouDataPodeHoje(row.Dt_homolog, Validacao.Tipo.data)) return "Data de homologação inválida.<br>A data de homologação deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            if (!Validacao.ValidouDuasDatas(row.Dt_homolog, row.Dt_extincao)) return "Data homologação/extinção inválidas.<br>A data de homologação deve ser inferior a data de extinção.";
            return string.Empty;
        }

        public override string PreDelete(Ly_curriculo.Row row, TConnectionWritable cn)
        {

            return string.Empty;
        }


    }

}