using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ProgramaAluno : RNBase
    {

        public static bool ExisteAlunoPrograma(string aluno, object id_unidade_ensino_programas, params object[] ID)
        {
            if (ID.Length > 0)
            {
                return ExecutarFuncao("select count(*) from Ly_aluno_programas where ALUNO = ? and ID_UNIDADE_ENSINO_PROGRAMAS = ? and id_aluno_programas <> ?",
                    aluno, Convert.ToInt32(id_unidade_ensino_programas), Convert.ToInt32(ID[0])) > 0;
            }
            else
            {
                return ExecutarFuncao("select count(*) from Ly_aluno_programas where ALUNO = ? and ID_UNIDADE_ENSINO_PROGRAMAS = ?",
                    aluno, Convert.ToInt32(id_unidade_ensino_programas)) > 0;
            }
        }
    }
}
