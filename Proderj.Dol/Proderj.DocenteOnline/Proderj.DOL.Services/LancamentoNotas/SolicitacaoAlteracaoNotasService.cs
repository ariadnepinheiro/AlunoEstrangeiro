using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public class SolicitacaoAlteracaoNotasService : ISolicitacaoAlteracaoNotasService
	{
		ISolicitacaoAlteracaoNotasRepository repositorioSolicitacao;

		public SolicitacaoAlteracaoNotasService(ISolicitacaoAlteracaoNotasRepository repositorioSolicitacao)
		{
			this.repositorioSolicitacao = repositorioSolicitacao;
		}

		public bool ExisteSolicitacaoAlteracaoNotaValidaEAprovada(DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao)
		{

			var solicitacaoParaVerificar = new SolicitacaoAlteracaoNotas
			{
				UnidadeEnsino = dtoConsultaSolicitacao.CodigoUnidadeEnsino,
				Ano = dtoConsultaSolicitacao.Ano,
				SubPeriodo = dtoConsultaSolicitacao.SubPeriodo,
				NumeroFuncionario = dtoConsultaSolicitacao.NumeroFuncionarioDocente,
				Status = "Aprovado",
				Turma = dtoConsultaSolicitacao.CodigoTurma,
				Disciplina = dtoConsultaSolicitacao.CodigoDisciplina,
			};

			bool solicitacaoValidaExistente = repositorioSolicitacao.ExisteSolicitacaoAlteracaoNotasValido(solicitacaoParaVerificar);

			return solicitacaoValidaExistente;
		}

		public DateTime? ObtemDataDaSolicitacaoAlteracaoNotaAguardandoAprovacao(DTOSolicitacaoAlteracaoNotas_ConsultaTurma dtoConsultaSolicitacao)
		{
			var solicitacaoParaVerificar = new SolicitacaoAlteracaoNotas
			{
				UnidadeEnsino = dtoConsultaSolicitacao.CodigoUnidadeEnsino,
				Ano = dtoConsultaSolicitacao.Ano,
				SubPeriodo = dtoConsultaSolicitacao.SubPeriodo,
				NumeroFuncionario = dtoConsultaSolicitacao.NumeroFuncionarioDocente,
				Status = "Aguardando",
				Turma = dtoConsultaSolicitacao.CodigoTurma,
				Disciplina = dtoConsultaSolicitacao.CodigoDisciplina,
			};

			DateTime? dataUltimaSolicitacao = repositorioSolicitacao.ObtemDataPor(solicitacaoParaVerificar);

			return dataUltimaSolicitacao;
		}


		public void InsereSolicitacaoReabertura(DTOSolicitacaoReabertura solicitacaoReabertura)
		{
			DateTime data = DateTime.Now;
			var solicitacao = new SolicitacaoAlteracaoNotas
			{
				Ano = solicitacaoReabertura.Ano,
				DataSolicitacao = data,
				DataStatus = data,
				Disciplina = solicitacaoReabertura.CodigoDisciplina,
				Justificativa = solicitacaoReabertura.JustificativaReabertura,
				NumeroFuncionario = solicitacaoReabertura.NumeroFuncionario,
				Periodo = solicitacaoReabertura.Periodo,
				Status = "Aguardando",
				SubPeriodo = solicitacaoReabertura.Subperiodo,
				Turma = solicitacaoReabertura.CodigoTurma,
				UnidadeEnsino = solicitacaoReabertura.CodigoUnidadeEnsino
			};

			repositorioSolicitacao.InsereSolicitacaoReabertura(solicitacao);

		}
	}
}
