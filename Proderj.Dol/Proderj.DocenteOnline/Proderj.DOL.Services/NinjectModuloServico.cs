using Ninject.Modules;
using Ninject.Planning.Bindings;
using Proderj.DOL.Repository;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service
{
	public class NinjectModuloServico : NinjectModuloBase
	{
		public override void Load()
		{
			Bind<ILoginRepository>().To<LoginRepository>();
			Bind<IDocenteRepository>().To<DocenteRepository>();
			Bind<IAceiteTermoCompromissoDocenteRepository>().To<AceiteTermoCompromissoDocenteRepository>();
			Bind<IPessoaRepository>().To<PessoaRepository>();
			Bind<ITermoCompromissoDocenteRepository>().To<TermoCompromissoDocenteRepository>();
			Bind<IUltimoResetRepository>().To<UltimoResetRepository>();
			Bind<ISelecaoTurmasRepository>().To<SelecaoTurmasRepository>();
			Bind<ISubPeriodoLetivoRepository>().To<SubPeriodoLetivoRepository>();
			Bind<IUnidadeEnsinoRepository>().To<UnidadeEnsinoRepository>();
			Bind<ILancamentoNotasRepository>().To<LancamentoNotasRepository>();
			Bind<IDisciplinaRepository>().To<DisciplinaRepository>();
			Bind<ISolicitacaoAlteracaoNotasRepository>().To<SolicitacaoAlteracaoNotasRepository>();
			Bind<IProvaRepository>().To<ProvaRepository>();
			Bind<IFrequenciaRepository>().To<FrequenciaRepository>();
			Bind<IItemTabelaRepository>().To<ItemTabelaRepository>();
			Bind<INotaRepository>().To<NotaRepository>();
			Bind<ILogNotaRepository>().To<LogNotaRepository>();
			Bind<IFaltaRepository>().To<FaltaRepository>();
			Bind<IProtocoloNotaRepository>().To<ProtocoloNotaRepository>();
			Bind<ICompetenciaHabilidadeDocenteRepository>().To<CompetenciaHabilidadeDocenteRepository>();
			Bind<ICompetenciaHabilidadeGrupoRepository>().To<CompetenciaHabilidadeGrupoRepository>();
			Bind<ILogCompetenciaHabilidadeDocenteRepository>().To<LogCompetenciaHabilidadeDocenteRepository>();

			Bind<IAvaliacaoCurriculoMinimoRepository>().To<AvaliacaoCurriculoMinimoRepository>();
			Bind<IAvaliacaoCurriculoMinimoDocenteRepository>().To<AvaliacaoCurriculoMinimoDocenteRepository>();
			Bind<IAvaliacaoCurriculoMinimoJustificativaRepository>().To<AvaliacaoCurriculoMinimoJustificativaRepository>();
			Bind<ILogAvaliacaoCurriculoMinimoDocenteRepository>().To<LogAvaliacaoCurriculoMinimoDocenteRepository>();
			Bind<ILogAvaliacaoCurriculoMinimoJustificativaRepository>().To<LogAvaliacaoCurriculoMinimoJustificativaRepository>();

			Bind<IGrupoHabilitacaoRepository>().To<GrupoHabilitacaoRepository>();
			Bind<IDocenteDisponivelGLPRepository>().To<DocenteDisponivelGLPRepository>();
			Bind<IMunicipioRepository>().To<MunicipioRepository>();
			Bind<INucleoRepository>().To<NucleoRepository>();
            Bind<IDeclaracaoSemNotaRepository>().To<DeclaracaoSemNotaRepository>();
            Bind<ILancamentoNotasConsolidadoRepository>().To<LancamentoNotasConsolidadoRepository>();
            Bind<INotaConsolidadoRepository>().To<NotaConsolidadoRepository>();

            Bind<IDadosGeraisDocenteRepository>().To<DadosGeraisDocenteRepository>();
            Bind<IDadosFormacaoDocenteRepository>().To<DadosFormacaoDocenteRepository>();
            Bind<IDadosCapacitacaoDocenteRepository>().To<DadosCapacitacaoDocenteRepository>();             
            Bind<IRegionalRepository>().To<RegionalRepository>();
            Bind<IPeriodoLetivoRepository>().To<PeriodoLetivoRepository>();
            Bind<IDadosTurmaDocenteRepository>().To<DadosTurmaDocenteRepository>();
            Bind<IDadosAcessoDocenteRepository>().To<DadosAcessoDocenteRepository>();

			Bind<IARMAZEM_LIVRO_2019Repository>().To<ARMAZEM_LIVRO_2019Repository>();

            Bind<IHD_PAISRepository>().To<HD_PAISRepository>();
            Bind<ITCE_LOGRADOURORepository>().To<TCE_LOGRADOURORepository>();
            Bind<ITCE_MUNICIPIORepository>().To<TCE_MUNICIPIORepository>();
            Bind<ILY_PESSOARepository>().To<LY_PESSOARepository>();
            Bind<ILY_FL_PESSOARepository>().To<LY_FL_PESSOARepository>();
            Bind<ILY_UNIDADE_ENSINORepository>().To<LY_UNIDADE_ENSINORepository>();
            Bind<ILY_DOCENTERepository>().To<LY_DOCENTERepository>();
            Bind<ILY_GRUPO_HABILITACAORepository>().To<LY_GRUPO_HABILITACAORepository>();
            Bind<IDISPONIBILIDADEGLPRepository>().To<DISPONIBILIDADEGLPRepository>();
            Bind<IGOOGLEEDUCATIONRepository>().To<GOOGLEEDUCATIONRepository>();

            Bind<IStoredProcedures>().To<StoredProcedures>();
            Bind<IMaterialEstudoRepository>().To<MaterialEstudoRepository>();
            Bind<ITurmaMaterialEstudoRepository>().To<TurmaMaterialEstudoRepository>();

            Bind<IApiRepository>().To<ApiRepository>();
		}
	}
}