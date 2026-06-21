using System;
using System.Text;
using System.Collections.Generic;


namespace Proderj.DOL.Domain {
    
    public class LY_TURNO {
        public LY_TURNO() {
            //LY_CENTROCUSTO_ASSOCIACAO = new List<LyCentrocustoAssociacao>();
            //LY_CURRICULO = new List<LyCurriculo>();
            //LY_FERIADO = new List<LyFeriado>();
            //LY_HOR_OPER = new List<LyHorOper>();
            //LY_JUR_EQUIPE = new List<LyJurEquipe>();
            //LY_LOTACAO = new List<LyLotacao>();
            //LY_SALA_SERIE = new List<LySalaSerie>();
            //LY_TURMA = new List<LyTurma>();
            //LY_UNIDADE_ENSINO_CURSOS = new List<LyUnidadeEnsinoCursos>();
            //RENOVACAO = new List<Renovacao>();
            //TCE_CVT_CONFIRMACAO_INICIAL_ESCOLA = new List<TceCvtConfirmacaoInicialEscola>();
            //TCE_CVT_CONFIRMACAO_TURNO_VAGA = new List<TceCvtConfirmacaoTurnoVaga>();
        }
        public virtual string TURNO { get; set; }
        public virtual string MNEMONICO { get; set; }
        public virtual string DESCRICAO { get; set; }
        public virtual DateTime? STAMP_ATUALIZACAO { get; set; }
        public virtual DateTime HORAINICIO { get; set; }
        public virtual DateTime HORAFIM { get; set; }
        //public virtual ICollection<LyCentrocustoAssociacao> LY_CENTROCUSTO_ASSOCIACAO { get; set; }
        //public virtual ICollection<LyCurriculo> LY_CURRICULO { get; set; }
        //public virtual ICollection<LyFeriado> LY_FERIADO { get; set; }
        //public virtual ICollection<LyHorOper> LY_HOR_OPER { get; set; }
        //public virtual ICollection<LyJurEquipe> LY_JUR_EQUIPE { get; set; }
        //public virtual ICollection<LyLotacao> LY_LOTACAO { get; set; }
        //public virtual ICollection<LySalaSerie> LY_SALA_SERIE { get; set; }
        //public virtual ICollection<LyTurma> LY_TURMA { get; set; }
        //public virtual ICollection<LyUnidadeEnsinoCursos> LY_UNIDADE_ENSINO_CURSOS { get; set; }
        //public virtual ICollection<Renovacao> RENOVACAO { get; set; }
        //public virtual ICollection<TceCvtConfirmacaoInicialEscola> TCE_CVT_CONFIRMACAO_INICIAL_ESCOLA { get; set; }
        //public virtual ICollection<TceCvtConfirmacaoTurnoVaga> TCE_CVT_CONFIRMACAO_TURNO_VAGA { get; set; }
    }
}
