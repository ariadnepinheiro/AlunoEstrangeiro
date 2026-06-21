using System.Collections.Generic;

namespace Proderj.DOL.Service
{
	public interface IItemTabelaService
	{
		List<DTOItemTabela> ListaPor(ItemTabelaService.NomeTabelaEnum nomeTabela);
	}
}