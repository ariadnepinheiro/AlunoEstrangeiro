using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp
{
	public class NinjectControllerFactory : DefaultControllerFactory
	{
		private IKernel ninjectKernel;

		public NinjectControllerFactory()
		{
			var modulosDeAssociacao = new INinjectModule[]
                                        {
                                            new NinjectModuloController(), 
                                            new NinjectModuloServico()
                                        };

			ninjectKernel = new StandardKernel(modulosDeAssociacao);

		}

		protected override IController GetControllerInstance(RequestContext contextoDoRequest, Type tipoDeController)
		{
			IController controllerFabricado;
			if (tipoDeController == null)
			{
				controllerFabricado = null;
			}
			else
			{
				controllerFabricado = (IController)ninjectKernel.Get(tipoDeController);
			}
			return controllerFabricado;
		}

		public IController CriaController(Type tipoDeController)
		{
			return GetControllerInstance(null, tipoDeController);
		}

	}
}
