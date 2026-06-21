using System;
using Ninject;
using Ninject.Modules;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    //TODO: COLOCAR A LÓGICA DO NINJECT NO FRAMEWORK
    public class NinjectServiceFactory
    {
        private IKernel ninjectKernel;

        public NinjectServiceFactory()
        {
            ninjectKernel = new StandardKernel(new INinjectModule[] { new NinjectModuloServico() });
        }

        public T Obtem<T>()
        {
            return ninjectKernel.Get<T>();
        }
    }
}
