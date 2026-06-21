using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public interface ITurmaMaterialEstudoService
    {
        List<DTOMaterialEstudo> ObtemPor(string identificador);
        void Gravar(string ids, DTOTurmaMaterialEstudo dtoTurmaMaterialEstudo, List<DTOMaterialEstudo> lstDtoMaterialEstudo, string usuario);
        DTOTurmaMaterialEstudo Obtem(int materialEstudoId, int subperiodo, string disciplina, string semestre, string ano, string turma);
        

        List<DTOTurmaMaterialEstudo> ObtemTurmaMatEstPor(string turma, int ano, int semestre, string disciplina, short subperiodo);
    }
}
