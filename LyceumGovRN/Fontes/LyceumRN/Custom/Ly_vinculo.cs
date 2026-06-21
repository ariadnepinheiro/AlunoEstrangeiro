using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;
using Techne.Data;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_vinculoCustom : Ly_vinculo.CustomBase
    {
        public override string PreInsert(Ly_vinculo.Row row, TConnectionWritable cn)
        {
            RN.VinculoLy rnVinculo = new VinculoLy();

            row.Ordem = rnVinculo.GeraOrdemServidorPor(Convert.ToDecimal(row.Pessoa));

            if (RN.VinculoLy.VerificaDataDesativacao(Convert.ToDecimal(row.Pessoa), row.Ordem) && row.Data_desativacao == null)
            {
                return "Não é possível inserir mais de duas matrículas ativas.";
            }

            if (!string.IsNullOrEmpty(row.Matricula))
            {
                if (row.Matricula == "00000000" || row.Matricula == "11111111" || row.Matricula == "22222222" || row.Matricula == "44444444" || row.Matricula == "66666666" || row.Matricula == "88888888" || row.Matricula == "99999999")
                    return "Matrícula inválida. <br>Este número de matrícula é reservado.";
            }

            if (!RN.VinculoLy.ValidaMatricula(row.Pessoa, row.Matricula))
            {
                return "Não é possível inserir um número de matrícula já existente.";
            }

            if (RN.VinculoLy.ExisteMatricula(row.Matricula))
            {
                return "Número de matrícula já cadastrado para outro funcionário.";
            }
            return string.Empty;
        }

        public override string PreUpdate(Ly_vinculo.Row row, TConnectionWritable cn)
        {
            if (RN.VinculoLy.VerificaDataDesativacao(Convert.ToDecimal(row.Pessoa), row.Ordem) && row.Data_desativacao == null)
            {
                return "Não é possível inserir mais de duas matrículas ativas.";
            }
            return string.Empty;
        }       
    }
}
