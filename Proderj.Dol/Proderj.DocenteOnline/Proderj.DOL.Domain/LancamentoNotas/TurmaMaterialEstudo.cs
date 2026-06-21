using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class TurmaMaterialEstudo
    {
        public TurmaMaterialEstudo() 
        {
		}

        public virtual int Turma_MaterialEstudoId { get; set; }

        public virtual int MaterialEstudoId { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Turma { get; set; }

        public virtual int Ano { get; set; }

        public virtual int Semestre { get; set; }

        public virtual string Disciplina { get; set; }

        public virtual decimal SubPeriodo { get; set; }

        public virtual int Turma_MaterialEstudo { get; set; }

        public virtual bool Ativo { get; set; }

        public virtual string UsuarioId { get; set; }

        public virtual DateTime? DataCadastro { get; set; }

        public virtual DateTime? DataAlteracao { get; set; }
    }
}
