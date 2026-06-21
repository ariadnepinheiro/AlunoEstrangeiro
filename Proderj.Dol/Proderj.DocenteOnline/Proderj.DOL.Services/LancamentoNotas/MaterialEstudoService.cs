using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.DOL.Service;

namespace Proderj.DOL.Service
{
    public class MaterialEstudoService : IMaterialEstudoService
    {
        private readonly IMaterialEstudoRepository repositorioMaterialEstudo;


        public MaterialEstudoService(IMaterialEstudoRepository repositorioMaterialEstudo)
		{
            this.repositorioMaterialEstudo = repositorioMaterialEstudo;			
		}


        public List<DTOMaterialEstudo> ObtemPor(string identificador)
        {
            var materialEstudo = repositorioMaterialEstudo.ObtemPor(identificador);

            var listaConvertida = materialEstudo as List<MaterialEstudo>;

            var dtoMat = new List<DTOMaterialEstudo>();
            foreach (var item in materialEstudo)
            {
                dtoMat.Add(new DTOMaterialEstudo { MaterialEstudoId = item.MaterialEstudoId , Ativo = item.Ativo, Descricao = item.Descricao });
            }


            return dtoMat;
        }

        public List<DTOMaterialEstudo> ObtemIds()
        {
            var materialEstudo = repositorioMaterialEstudo.ObtemIds();

            var listaConvertida = materialEstudo as List<MaterialEstudo>;

            var dtoMat = new List<DTOMaterialEstudo>();//
            foreach (var item in materialEstudo)
            {
                dtoMat.Add(new DTOMaterialEstudo { MaterialEstudoId = item.MaterialEstudoId });
            }
            
            return dtoMat;
        }

        public void Gravar(string identificador, List<DTOMaterialEstudo> lstMaterialEstudo)
        {
            var materialEstudo = repositorioMaterialEstudo.ObtemPor(identificador);

            var listaConvertida = materialEstudo as List<MaterialEstudo>;

            var dtoMat = new List<DTOMaterialEstudo>(); 
            foreach (var item in materialEstudo)
            {
                dtoMat.Add(new DTOMaterialEstudo { MaterialEstudoId = item.MaterialEstudoId, Ativo = item.Ativo, Descricao = item.Descricao });
            }
                                    
        }


        #region IMaterialEstudoService Members


        //public List<DTOMaterialEstudo> ObtemIds()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
