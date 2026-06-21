using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Aluno
	{
		public Aluno()
		{ 
		}

		public virtual string MatriculaAluno { get; set; }

		public virtual string NomeCompleto { get; set; }
		
	}	
}
