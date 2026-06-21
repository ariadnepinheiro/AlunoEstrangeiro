using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_FUNCAO {
        public LY_FUNCAO() {
            //FUNCAO = new List<Funcao>();
			//LY_CATEGORIA_DOCENTE = new List<LY_CATEGORIA_DOCENTE>();
			//LY_CH_CATEGORIA = new List<LY_CH_CATEGORIA>();
            //LY_CONCURSO_DOCENTE = new List<LyConcursoDocente>();
            //LY_DOCENTE_FUNCAO_GLP = new List<LY_DOCENTE_FUNCAO_GLP>();
            //LY_ESTAGIO_EMPRESA = new List<LyEstagioEmpresa>();
            //LY_EXTRA_CLASSE = new List<LyExtraClasse>();
            //LY_HISTESTAGIO_EMPRESA = new List<LyHistestagioEmpresa>();
			LY_LOTACAO = new List<LY_LOTACAO>();
            //LY_PADACES_FUNCAO = new List<LyPadacesFuncao>();
            //LY_VALOR_GLP = new List<LyValorGlp>();
        }
        public virtual string FUNCAO { get; set; }
        public virtual string DESCRICAO { get; set; }
        public virtual string FUNCAOBB { get; set; }
        public virtual string TIPO { get; set; }
        public virtual string CAMPO_01 { get; set; }
        public virtual string CAMPO_02 { get; set; }
        public virtual string CAMPO_03 { get; set; }
        public virtual string CAMPO_04 { get; set; }
        public virtual string CAMPO_05 { get; set; }
        public virtual string CAMPO_06 { get; set; }
        public virtual string CAMPO_07 { get; set; }
        public virtual string CAMPO_08 { get; set; }
        public virtual string CAMPO_09 { get; set; }
        public virtual string CAMPO_10 { get; set; }
        public virtual string SEMCARGAHORARIAEFETIVA { get; set; }
        //public virtual ICollection<Funcao> FUNCAO { get; set; }
        //public virtual ICollection<LY_CATEGORIA_DOCENTE> LY_CATEGORIA_DOCENTE { get; set; }
        //public virtual ICollection<LY_CH_CATEGORIA> LY_CH_CATEGORIA { get; set; }
        //public virtual ICollection<LyConcursoDocente> LY_CONCURSO_DOCENTE { get; set; }
        //public virtual ICollection<LY_DOCENTE_FUNCAO_GLP> LY_DOCENTE_FUNCAO_GLP { get; set; }
        //public virtual ICollection<LyEstagioEmpresa> LY_ESTAGIO_EMPRESA { get; set; }
        //public virtual ICollection<LyExtraClasse> LY_EXTRA_CLASSE { get; set; }
        //public virtual ICollection<LyHistestagioEmpresa> LY_HISTESTAGIO_EMPRESA { get; set; }
        public virtual ICollection<LY_LOTACAO> LY_LOTACAO { get; set; }
        //public virtual ICollection<LyPadacesFuncao> LY_PADACES_FUNCAO { get; set; }
        //public virtual ICollection<LyValorGlp> LY_VALOR_GLP { get; set; }
    }
}
