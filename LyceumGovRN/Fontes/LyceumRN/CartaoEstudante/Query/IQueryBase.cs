using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    interface IQueryBase<T> where T: IEntity
    {
        void Insere(T obj);
        void Altera(T obj);
        void Remove(T obj);
        T ObtemPor(int id);
        List<T> Lista();
        long ObtemMAX();
    }
}
