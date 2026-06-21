using System;
using System.Globalization;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_serieCustom : Ly_serie.CustomBase
    {
        public override string PreInsert(Ly_serie.Row row, TConnectionWritable tconnw)
        {
            if (!Validacao.ValidouDataPodeHoje(row.Dt_extincao, Validacao.Tipo.data)) 
            {
                return "Data de extinção inválida.<br>" +
                    "A data de extinção deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            }

            return string.Empty;
        }

        public override string PreUpdate(Ly_serie.Row row, TConnectionWritable tconnw)
        {
            if (!Validacao.ValidouDataPodeHoje(row.Dt_extincao, Validacao.Tipo.data))
            {
                return "Data de extinção inválida.<br>" +
                    "A data de extinção deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            }

            if (row.Dt_extincao != null)
            {
                bool alunoAtivo = RN.Serie.VerificarSerieAlunoAtivo(tconnw,
                    row.Curso.ToString(), row.Turno.ToString(), row.Curriculo.ToString(),
                    Convert.ToDecimal(row.Serie), Convert.ToDateTime(row.Dt_extincao));
                if (alunoAtivo != false)
                {
                    return "Escolaridade com aluno ativo.";
                }

                bool turmaDataTermino = RN.Serie.VerificarTurmaDataTermino(tconnw,
                    row.Curso.ToString(), row.Turno.ToString(), row.Curriculo.ToString(),
                    Convert.ToDecimal(row.Serie), Convert.ToDateTime(row.Dt_extincao));
                if (turmaDataTermino != false)
                {
                    return "Escolaridade possui turma com data de término maior que a data de extinção.";
                }
            }

            return string.Empty;
        }

        public override string PreDelete(Ly_serie.Row row, TConnectionWritable tconnw)
        {
            return string.Empty;
        }
    }
}
