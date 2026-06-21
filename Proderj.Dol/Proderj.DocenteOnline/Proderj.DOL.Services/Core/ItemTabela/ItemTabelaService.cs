using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
	public class ItemTabelaService : IItemTabelaService
	{
		private readonly IItemTabelaRepository repositorio;

		public enum NomeTabelaEnum
		{
			JustificativaNota
		}

		public  ItemTabelaService(IItemTabelaRepository repositorio)
		{
			this.repositorio = repositorio;
		}

		public List<DTOItemTabela> ListaPor(NomeTabelaEnum nomeTabela)
		{
			List<ItemTabela> listaItens = repositorio.EnumeraPor(nomeTabela.ToString()).ToList();




			List<DTOItemTabela> listaDtoItens = listaItens.ConvertAll(itemTabela => new DTOItemTabela
			                                    	{
			                                    		Codigo = itemTabela.Item,
			                                    		Descricao = itemTabela.Descricao
			                                    	});
			return listaDtoItens;
		}
	}
}
