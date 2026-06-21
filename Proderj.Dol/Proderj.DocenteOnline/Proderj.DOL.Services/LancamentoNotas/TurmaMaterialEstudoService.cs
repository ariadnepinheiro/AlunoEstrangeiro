using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;


namespace Proderj.DOL.Service
{
    public class TurmaMaterialEstudoService : ITurmaMaterialEstudoService
    {
        private readonly ITurmaMaterialEstudoRepository repositorioTurmaMaterialEstudo;


        public TurmaMaterialEstudoService(ITurmaMaterialEstudoRepository repositorioTurmaMaterialEstudo)
        {
            this.repositorioTurmaMaterialEstudo = repositorioTurmaMaterialEstudo;
        }



        public void Gravar(string ids, DTOTurmaMaterialEstudo dtoTurmaMaterialEstudo, List<DTOMaterialEstudo> lstDtoMaterialEstudo, string usuario)
        {
            var arrayIds = ids.Split(',').ToList();

            for (int i = 0; i < lstDtoMaterialEstudo.Count; i++)
            {
                for (int j = 0; j < arrayIds.Count; j++)
                {
                    if (lstDtoMaterialEstudo[i].MaterialEstudoId == Convert.ToInt32(arrayIds[j]))
                    {
                        lstDtoMaterialEstudo.RemoveAt(i);
                    }
                }
            }

            foreach (var item in lstDtoMaterialEstudo)
            {

                repositorioTurmaMaterialEstudo.Remove(item.MaterialEstudoId, dtoTurmaMaterialEstudo.Turma, Convert.ToInt32( dtoTurmaMaterialEstudo.Ano ), Convert.ToInt32( dtoTurmaMaterialEstudo.Semestre ), dtoTurmaMaterialEstudo.Disciplina, Convert.ToInt32( dtoTurmaMaterialEstudo.Subperiodo ));
            }


            foreach (var id in arrayIds)
            {
                var turmaMaterialEstudo = new TurmaMaterialEstudo
                {
                    Ativo = true,
                    DataAlteracao = DateTime.Now,
                    DataCadastro = DateTime.Now,
                    Descricao = "",

                    MaterialEstudoId = Convert.ToInt32(id.ToString()),
                    UsuarioId = usuario,
                    Turma = dtoTurmaMaterialEstudo.Turma,
                    Ano = Convert.ToInt32(dtoTurmaMaterialEstudo.Ano),
                    Semestre = Convert.ToInt32(dtoTurmaMaterialEstudo.Semestre),
                    Disciplina = dtoTurmaMaterialEstudo.Disciplina,
                    SubPeriodo = dtoTurmaMaterialEstudo.Subperiodo


                };
                repositorioTurmaMaterialEstudo.Remove(Convert.ToInt32(id.ToString()), dtoTurmaMaterialEstudo.Turma, Convert.ToInt32( dtoTurmaMaterialEstudo.Ano ), Convert.ToInt32( dtoTurmaMaterialEstudo.Semestre ), dtoTurmaMaterialEstudo.Disciplina, Convert.ToInt32( dtoTurmaMaterialEstudo.Subperiodo));
                repositorioTurmaMaterialEstudo.Insere(turmaMaterialEstudo);
            }





        }


        public DTOTurmaMaterialEstudo Obtem(int materialEstudoId, int subperiodo, string disciplina, string semestre, string ano, string turma)
        {

            DTOTurmaMaterialEstudo dtoTurmaMaterialEstudo = null;


            dtoTurmaMaterialEstudo = new DTOTurmaMaterialEstudo
            {
                Ano = ano,
                Disciplina = disciplina,
                MaterialEstudoId = materialEstudoId,
                Semestre = semestre,
                Subperiodo = subperiodo,
                Turma = turma


            };

            return dtoTurmaMaterialEstudo;


        }



        public List<DTOTurmaMaterialEstudo> ObtemTurmaMatEstPor(string turma, int ano, int semestre, string disciplina, short subperiodo)
        {

            var turmaMaterialEstudo = repositorioTurmaMaterialEstudo.ObtemTurmaMatEstPor(turma, ano, semestre, disciplina , subperiodo);

            var listaConvertida = turmaMaterialEstudo as List<TurmaMaterialEstudo>;

            var dtoMat = new List<DTOTurmaMaterialEstudo>();
            foreach (var item in turmaMaterialEstudo)
            {
                dtoMat.Add(new DTOTurmaMaterialEstudo { TurmaMaterialEstudoId = item.Turma_MaterialEstudoId, MaterialEstudoId = item.MaterialEstudoId, Subperiodo = item.SubPeriodo });
            }


            return dtoMat;


        }



        #region ITurmaMaterialEstudoService Members

        public List<DTOMaterialEstudo> ObtemPor(string identificador)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
