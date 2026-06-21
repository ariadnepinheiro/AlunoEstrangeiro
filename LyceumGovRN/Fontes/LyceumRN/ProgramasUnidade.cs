using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class ProgramasUnidade : RNBase
    {
        public static QueryTable ConsultarAgencias()
        {
            return Consultar("Select distinct agencia, nome_agencia from LY_AGENCIA_PROGRAMA");
        }

        public static QueryTable ConsultarProgramas(string agencia)
        {
            return Consultar("Select distinct programa, nome_programa from LY_AGENCIA_PROGRAMA where agencia = ?", agencia);
        }

        public static QueryTable ConsultarTipoBeneficio()
        {
            return Consultar("Select distinct tipo_beneficio, descricao from ly_tipo_beneficio");
        }

        public static bool ExisteUnidadeEnsinoPrograma(string unidadeEns, object agencia, object programa, object tipoBeneficio, object anoValidade, params object[] ID)
        {
            if(ID.Length > 0)
            {
                return ExecutarFuncao("select count(*) from Ly_unidade_ensino_programas where UNIDADE_ENS = ? and AGENCIA = ? and PROGRAMA = ? and TIPO_BENEFICIO = ? and ANO_VALIDADE = ? and ID_UNIDADE_ENSINO_PROGRAMAS <> ?",
                    unidadeEns, agencia.ToString(), programa.ToString(), tipoBeneficio.ToString(), Convert.ToInt32(anoValidade), Convert.ToInt32(ID[0])) > 0;
            }
            else
            {
                return ExecutarFuncao("select count(*) from Ly_unidade_ensino_programas where UNIDADE_ENS = ? and AGENCIA = ? and PROGRAMA = ? and TIPO_BENEFICIO = ? and ANO_VALIDADE = ?",
                    unidadeEns, agencia.ToString(), programa.ToString(), tipoBeneficio.ToString(), Convert.ToInt32(anoValidade)) > 0;
            }
        }

        public static string ConsultarDescricaoPrograma(string programa)
        {
            return Consultar("Select nome_programa from LY_AGENCIA_PROGRAMA where programa = ?", programa).Rows[0][0].ToString();
        }
    }
}
