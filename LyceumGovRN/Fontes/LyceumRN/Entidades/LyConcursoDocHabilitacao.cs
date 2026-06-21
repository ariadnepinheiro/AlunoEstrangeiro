using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyConcursoDocHabilitacao
    {
        private string concurso;
        
        public string Concurso
        {
          get { return concurso; }
          set { concurso = value; }
        }

        private string nucleo;

        public string Nucleo
        {
          get { return nucleo; }
          set { nucleo = value; }
        }

        private string categoria;

        public string Categoria
        {
          get { return categoria; }
          set { categoria = value; }
        }
         
        private string agrupamento;

        public string Agrupamento
        {
          get { return agrupamento; }
          set { agrupamento = value; }
        }
        
        private int vagas;

        public int Vagas
        {
          get { return vagas; }
          set { vagas = value; }
        }

        private int? carga_horaria;

        public int? Carga_horaria
        {
          get { return carga_horaria; }
          set { carga_horaria = value; }
        }

        private string municipio_proc;

        public string Municipio_proc
        {
          get { return municipio_proc; }
          set { municipio_proc = value; }
        }

        private int regionalId;

        public int RegionalId
        {
            get { return regionalId; }
            set { regionalId = value; }
        }
         
    }
}
