using System;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class Atividade : RNBase
    {
        /// <summary>
        /// Verifica se docente se docente possui atividade no horário.
        /// </summary>
        /// <param name="connection">conexão</param>
        /// <param name="ano">ano</param>
        /// <param name="semestre">semestre</param>
        /// <param name="diaSemana">dia da semana</param>
        /// <param name="num_func">número do funcionário</param>
        /// <param name="horaIni">hora de início</param>
        /// <param name="horaFim">hora de fim</param>
        /// <param name="aula">aula</param>
        /// <param name="disciplina">disciplina</param>
        /// <returns>lista de erros</returns>
        public static ErrorList VerificarDisponibilidadeAtividade(TConnection connection, decimal ano, decimal semestre, decimal diaSemana, decimal num_func, DateTime horaIni, DateTime horaFim, decimal aula, string disciplina)
        {
            RN.Docentes rnDocentes = new Docentes();

            string sql = " Select T.Descricao from LY_DOCENTE_ATIVIDADES_HORARIOS H, " +
                         " LY_DOCENTE_TIPOS_ATIVIDADES T, LY_PERIODO_LETIVO p " +
                         " where " +
                         " h.codativ_docen = t.codativ_docen " +
                         " and h.DIA_SEMANA = ? " + 
                         " and T.disponivel = 'N' " +
                         " and h.NUM_FUNC = ?" +
                         " and p.ANO = ? " + 
                         " and p.PERIODO = ? " + 
                         " and h.data_ini >= p.dt_inicio " +
                         " and h.data_fim <= p.dt_fim " +
                         " and (h.HORA_FIM >= ? or h.HORA_INICIO >= ?)";

            QueryTable qt = null;
            qt =  RNBase.Consultar(connection, sql, Convert.ToString(diaSemana), Convert.ToString(num_func), 
                    Convert.ToString(ano), Convert.ToString(semestre),
                    new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second),
                    new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second));

            if (qt != null)
            {
                if (qt.Rows.Count > 0)
                {
                    string matricula = rnDocentes.ObtemMatriculaPor(num_func);
                    string nomeDocente = rnDocentes.ObtemNomeDocentePorNumFunc(num_func); 
                    string nomeDisciplina = RN.Disciplina.ObterNomeDisciplina(connection, disciplina);

                    string mensagem = aula + "|" + diaSemana + "|O Docente " + Convert.ToString(num_func) + " , está com Horário Indisponível para dar aula. " +
                                      "|" + String.Format("{0:HH:mm}", horaIni) + "|" + String.Format("{0:HH:mm}", horaFim) + "|" + matricula + " - " + nomeDocente + "|" + nomeDisciplina;

                    ErrorList erro = new ErrorList();
                    erro.Add(mensagem, "ERRO_VALIDACAO");
                    return erro;
                }
            }
            return null;
        }


    }
}


