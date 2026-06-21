using System;
using System.Globalization;

using Techne.Data;

using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_disciplinaCustom : Ly_disciplina.CustomBase
    {
        public override string PreInsert(Ly_disciplina.Row row, TConnectionWritable cn)
        {
            if (string.IsNullOrEmpty(row.Disciplina.ToString())) return "Disciplina: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nome.ToString())) return "Nome: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Faculdade.ToString())) return "Unidade de Ensino: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Depto.ToString())) return "Departamento: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nota_max.ToString())) return "Nota Máxima: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nota_max_media.ToString())) return "Nota Máxima Média: Preenchimento obrigatório.";

            if (!string.IsNullOrEmpty(row.Horas_aula.ToString()) && !Validacao.Validou(row.Horas_aula.ToString(), Validacao.Tipo.decimal8)) return "'Horas de aula' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_ativ.ToString()) && !Validacao.Validou(row.Horas_ativ.ToString(), Validacao.Tipo.decimal8)) return "'Horas de atividades' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_estagio.ToString()) && !Validacao.Validou(row.Horas_estagio.ToString(), Validacao.Tipo.decimal8)) return "'Horas de estágio' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_lab.ToString()) && !Validacao.Validou(row.Horas_lab.ToString(), Validacao.Tipo.decimal8)) return "'Horas de laboratório' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";

            if (!string.IsNullOrEmpty(row.Aulas_sem_aula.ToString()) && !Validacao.Validou(row.Aulas_sem_aula.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Teórica' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";
            if (!string.IsNullOrEmpty(row.Aulas_sem_lab.ToString()) && !Validacao.Validou(row.Aulas_sem_lab.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Prática' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";
            if (!string.IsNullOrEmpty(row.Aulas_sem_ativ.ToString()) && !Validacao.Validou(row.Aulas_sem_ativ.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Atividades' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";

            return string.Empty;
        }

        public override string PreUpdate(Ly_disciplina.Row row, TConnectionWritable cn)
        {
            if (string.IsNullOrEmpty(row.Disciplina.ToString())) return "Disciplina: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nome.ToString())) return "Nome: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Faculdade.ToString())) return "Unidade de Ensino: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Depto.ToString())) return "Departamento: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nota_max.ToString())) return "Nota Máxima: Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(row.Nota_max_media.ToString())) return "Nota Máxima Média: Preenchimento obrigatório.";

            if (!string.IsNullOrEmpty(row.Horas_aula.ToString()) && !Validacao.Validou(row.Horas_aula.ToString(), Validacao.Tipo.decimal8)) return "'Horas de aula' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_ativ.ToString()) && !Validacao.Validou(row.Horas_ativ.ToString(), Validacao.Tipo.decimal8)) return "'Horas de atividades' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_estagio.ToString()) && !Validacao.Validou(row.Horas_estagio.ToString(), Validacao.Tipo.decimal8)) return "'Horas de estágio' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";
            if (!string.IsNullOrEmpty(row.Horas_lab.ToString()) && !Validacao.Validou(row.Horas_lab.ToString(), Validacao.Tipo.decimal8)) return "'Horas de laboratório' inválido.<br>O campo deve ter no máximo 8 números inteiros e 2 casas decimais.";

            if (!string.IsNullOrEmpty(row.Aulas_sem_aula.ToString()) && !Validacao.Validou(row.Aulas_sem_aula.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Teórica' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";
            if (!string.IsNullOrEmpty(row.Aulas_sem_lab.ToString()) && !Validacao.Validou(row.Aulas_sem_lab.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Prática' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";
            if (!string.IsNullOrEmpty(row.Aulas_sem_ativ.ToString()) && !Validacao.Validou(row.Aulas_sem_ativ.ToString(), Validacao.Tipo.decimal3)) return "Campo 'Atividades' inválido.<br>O campo deve ter no máximo 3 números inteiros e 4 casas decimais.";

            return string.Empty;
        }

        public override string PreDelete(Ly_disciplina.Row row, TConnectionWritable cn)
        {

            return string.Empty;
        }
    }
}
