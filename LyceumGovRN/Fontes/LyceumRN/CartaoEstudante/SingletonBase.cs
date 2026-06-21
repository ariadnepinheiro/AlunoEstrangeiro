using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante
{
    public abstract class SingletonBase<T>
    {
        private static T instancia;
        private static readonly object syncObj = new object();

        public static T Instancia
        {
            get
            {
                lock (syncObj)
                {
                    if (instancia == null)
                        instancia = (T)Activator.CreateInstance(typeof(T), true);

                    return instancia;
                }
            }
        }
    }
}
