using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
using Proderj.DOL.Service;

namespace Proderj.DOL.Service
{
	public interface ILancamentoNotasService : IService
	{
		void VerificaPermissaoParaLancarNota(long numeroFuncionarioDocente, DTOLancamentoNotasSolicitacao dtoSolicitacaoParaLancamento);
		bool ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(short ano, short periodo, short subperiodoAtual, string disciplina, string turma);
		bool PodeLancarNotaNaTurma(string disciplina, string turma, short ano, short periodo, short subperiodo);

		List<DTOItemLancamentoNotaFrequenciaAluno> ListaLancamentoNotaFrequenciaAlunoPor(string disciplina, string turma, short ano, short periodo, short subperiodo);

		//Salvamento
		void VerificaPermissaoParaSalvarNota(DTOLancamentoNotasSalvamento dtoSolicitacaoParaLancamento, DTOProvaParaLancamento dtoProvaParaLancamento);
		
		List<DTONotaSalva> ListaNotasPreviamenteSalvasPor(short ano, short periodo, string codigoTurma, string codigoDisciplina, string tipoProva);
		List<DTOFaltaSalva> ListaFaltasPreviamenteSalvasPor(short ano, short periodo, string codigoTurma, string codigoDisciplina, string codigoFrequencia);

		void AtualizaOuRemoveOuInsereNota(List<DTONota> dtoNota, List<DTONotaSalva> dtoNotasSalvas, string matricula);
		void AtualizarLancamentoEAtualizarAulas(DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao);

		void AtualizaOuInsereFalta(List<DTOFalta> dtoFaltas, List<DTOFaltaSalva> dtoFaltasSalvas);

		void VerificaNotasEFaltasParaAtualizacaoNotas(string matricula, int? aulasDadas, int? aulasPrevistas, List<DTOFalta> dtoFaltas, List<DTONota> dtoNotas, DTOProtocoloNota dtoProtocolaNota);

		int InsereProtocolo(DTOProtocoloNota dtoProtocolaNota);

		void ProcessaNotasFaltasProtocolo(string matricula, List<DTOFalta> dtoFaltas, List<DTONota> dtoNotas, ref DTOProtocoloNota dtoProtocolaNota, List<DTONotaSalva> dtoNotasSalvas, List<DTOFaltaSalva> dtoFaltasSalvas, DTOLancamentoAtualizacaoAulas dtoLancamentoAtualizacao);

		List<DTOItemLancamentoNotaFrequenciaAluno> AtualizaEVerificaListaDeAlunosExistentesComAlunosEnviadosPeloProfessor(List<DTOItemLancamentoNotaFrequenciaAluno> alunosDaTurma, List<DTOItemSalvaNotaFrequenciaAluno> alunosEnviadosPeloProfessor);

        IList<DTOItemJustificativa> ListarItemJustificativa();

        string MensagemFrequenciaNotaFalta(string CodigoDisciplina);

        Disciplina DisciplinaFrequenciaNota(string CodigoDisciplina);

        DTOLancamentoNotasConsolidado ObtemLancamentoNotasConsolidado(string disciplina, string turma, short ano, short periodo);
	}
}
