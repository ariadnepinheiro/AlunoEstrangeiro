using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Models
{
	public abstract class ViewModelPadrao
	{
		public ViewModelPadrao(DocenteLogadoBindModel docenteLogadoModelo) {
			if (docenteLogadoModelo == null)
				throw new ArgumentException("O ViewModel de docente logado informado está nulo");

			CabecalhoModelo.DocenteLogadoModelo = docenteLogadoModelo;
		}

		public ViewModelPadrao() { }

		public string NomeDaPagina		{ get; set; }
		public string TituloDaPagina	{ get; set; }

		private CabecalhoViewModel cabecalhoModelo;
		public CabecalhoViewModel CabecalhoModelo
		{
			get
			{
				if (cabecalhoModelo == null)
				{
					cabecalhoModelo = new CabecalhoViewModel
							{
								BotaoAjudaHabilitado = true,
								BotaoInicioHabilitado = true,
								BotaoSairHabilitado = true
							};
				}

				return cabecalhoModelo;
			}
		}

		public SelectList ListaPaginasAte(int numeroPaginas, int? paginaAtual)
		{
			var lista = new List<DTOItemCombo>();

			for ( var contador = 1; contador <= numeroPaginas; contador++)
			{
				lista.Add(new DTOItemCombo{ 
				Codigo = contador.ToString(), 
				Descricao = String.Concat("Página ", contador, " de ",numeroPaginas)});
			}

			return new SelectList(lista, "Codigo", "Descricao", paginaAtual);
		}
	}
}
