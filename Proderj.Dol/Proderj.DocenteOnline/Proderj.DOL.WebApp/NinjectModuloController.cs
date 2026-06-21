using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.WebApp
{
	public class NinjectModuloController : NinjectModuloBase
	{
		public override void Load()
		{
			Bind<ILoginService>().To<LoginService>();
			Bind<ITurmaService>().To<TurmaService>();
			Bind<IDisciplinaService>().To<DisciplinaService>();
			Bind<IItemTabelaService>().To<ItemTabelaService>();
			Bind<IUnidadeEnsinoService>().To<UnidadeEnsinoService>();
			Bind<ISelecaoTurmasService>().To<SelecaoTurmasService>();
			Bind<IRespostaCurriculoMinimoService>().To<RespostaCurriculoMinimoService>();
			Bind<ILancamentoNotasService>().To<LancamentoNotasService>();
			Bind<ISubPeriodoLetivoService>().To<SubPeriodoLetivoService>();
			Bind<ITermoCompromissoDocenteService>().To<TermoCompromissoDocenteService>();
			Bind<ISolicitacaoAlteracaoNotasService>().To<SolicitacaoAlteracaoNotasService>();
			Bind<ILogCompetenciaHabilidadeDocenteService>().To<LogCompetenciaHabilidadeDocenteService>();
			Bind<IAvaliacaoCurriculoMinimoService>().To<AvaliacaoCurriculoMinimoService>();

			Bind<ICadastroGlpService>().To<CadastroGlpService>();
			Bind<IMunicipioService>().To<MunicipioService>();
			Bind<IPessoaService>().To<PessoaService>();
			Bind<INucleoService>().To<NucleoService>();
            Bind<IRegionalService>().To<RegionalService>();
            Bind<IPeriodoLetivoService>().To<PeriodoLetivoService>();

			Bind<IProtocoloNotaService>().To<ProtocoloNotaService>();
            Bind<IDadosDocenteService>().To<DadosDocenteService>();

            Bind<IHD_PAISService>().To<HD_PAISService>();
            Bind<ITCE_LOGRADOUROService>().To<TCE_LOGRADOUROService>();
            Bind<ITCE_MUNICIPIOService>().To<TCE_MUNICIPIOService>();
            Bind<ILY_PESSOAService>().To<LY_PESSOAService>();
            Bind<ILY_UNIDADE_ENSINOService>().To<LY_UNIDADE_ENSINOService>();
            Bind<IDISPONIBILIDADEGLPService>().To<DISPONIBILIDADEGLPService>();

            Bind<IARMAZEM_LIVRO_2019Service>().To<ARMAZEM_LIVRO_2019Service>();
            Bind<IMaterialEstudoService>().To<MaterialEstudoService>();
            Bind<ITurmaMaterialEstudoService>().To<TurmaMaterialEstudoService>();
            
            Bind<IApiService>().To<ApiService>();
		}
	}
}
