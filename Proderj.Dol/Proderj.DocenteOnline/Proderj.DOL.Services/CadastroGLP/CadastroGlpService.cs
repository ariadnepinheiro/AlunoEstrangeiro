using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Common;
using Proderj.Foundation.Framework;
using Proderj.DOL.Exception;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class CadastroGlpService : ICadastroGlpService
	{
		private IGrupoHabilitacaoRepository repositorioGrupoHabilitacao;
		private IDocenteRepository repositorioDocente;
		private IDocenteDisponivelGLPRepository repositorioDocenteDisponivelGlp;

		public CadastroGlpService(IGrupoHabilitacaoRepository repositorioGrupoHabilitacao,
			IDocenteRepository repositorioDocente,
			IDocenteDisponivelGLPRepository repositorioDocenteDisponivelGlp)
		{
			this.repositorioGrupoHabilitacao = repositorioGrupoHabilitacao;
			this.repositorioDocente = repositorioDocente;
			this.repositorioDocenteDisponivelGlp = repositorioDocenteDisponivelGlp;
		}

		public List<DTOCadastroGlp_Disciplina> ListaDisciplinasPor()
		{
			List<GrupoHabilitacao> listaGrupoHabilitacao = repositorioGrupoHabilitacao.Enumera().ToList();

			List<DTOCadastroGlp_Disciplina> listaDTODisciplina = listaGrupoHabilitacao.ConvertAll(disciplina => new DTOCadastroGlp_Disciplina
			{
				Agrupamento = disciplina.Agrupamento,
				Descricao = disciplina.Descricao,
				Tipo = disciplina.Tipo
			});

			return listaDTODisciplina;
		}

        public List<DTOCadastroGlp_Disciplina> ListaDisciplinasPor(int num_func)
        {
            List<GrupoHabilitacao> listaGrupoHabilitacao = repositorioGrupoHabilitacao.EnumeraPor(num_func).ToList();

            List<DTOCadastroGlp_Disciplina> listaDTODisciplina = listaGrupoHabilitacao.ConvertAll(disciplina => new DTOCadastroGlp_Disciplina
            {
                Agrupamento = disciplina.Agrupamento,
                Descricao = disciplina.Descricao,
                Tipo = disciplina.Tipo
            });

            return listaDTODisciplina;
        }

		public List<DTOCadastroGlp_DocenteDisponivel> ListaDocentesDisponiveisPor(long numeroFuncionario)
		{
			List<DocenteDisponivelGLP> listaDocentesDisponiveis = repositorioDocenteDisponivelGlp.EnumeraPor(numeroFuncionario).ToList();

			List<DTOCadastroGlp_DocenteDisponivel> listaDTOCadastroDocentesDisponiveis = listaDocentesDisponiveis.ConvertAll(docente => new DTOCadastroGlp_DocenteDisponivel
			{
				CodigoDisciplina = docente.GrupoHabilitacao.Agrupamento,
				DescricaoDisciplina = docente.GrupoHabilitacao.Descricao,
				CodigoMunicipio = docente.Municipio.Codigo,
				NomeMunicipio = docente.Municipio.Nome,
				HoraInicio = docente.HoraInicio,
				HoraFinal = docente.HoraFinal,
				DiaSemana = (DiaDaSemanaEnum)docente.DiaSemana,
				DocenteDisponivelGlpId = docente.Id,
                CodigoRegional = docente.Regional.Codigo,
                DescricaoRegional = docente.Regional.Descricao
			});

			return listaDTOCadastroDocentesDisponiveis;
		}

		public DTOCadastroGlp_DocenteLogadoComTelefone ObtemDocenteComTelefonePor(string matricula)
		{
			Docente docente = repositorioDocente.ObtemPorPessoaPor(matricula);

			if (docente == null)
			{
				throw new CadastroGlpException(CadastroGlpException.TipoEnum.DocenteInexistente);
			}

			return new DTOCadastroGlp_DocenteLogadoComTelefone
			{
				PessoaId = docente.Pessoa.Id,
				Telefone = docente.Pessoa.Telefone
			};
		}

		public void InsereDocenteDisponivel(DTOCadastroGlp_InsereDocenteDisponivel dtoInsereDocente)
		{
			bool existeDisciplina = repositorioGrupoHabilitacao.ExistePor(dtoInsereDocente.Agrupamento);

			if (!existeDisciplina)
			{
				throw new CadastroGlpException(CadastroGlpException.TipoEnum.DisciplinaInvalida);
			}

			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			MunicipioService servicoDeMunicipio = fabricaServico.Obtem<MunicipioService>();

			bool existeMunicipio = servicoDeMunicipio.ExistePor(dtoInsereDocente.CodigoMunicipio);

			if (!existeMunicipio)
			{
				throw new CadastroGlpException(CadastroGlpException.TipoEnum.MunicipioInvalido);
			}

            //Mapper.CreateMap<DTOCadastroGlp_InsereDocenteDisponivel, DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel>();
			DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel dtoValidaDocente = Mapper.Map<DTOCadastroGlp_InsereDocenteDisponivel, DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel>(dtoInsereDocente);

			VerificaPermissaoParaInsercaoDocenteDisponivel(dtoValidaDocente);

            var docenteDisponivel = new DocenteDisponivelGLP
            {
                DiaSemana = dtoInsereDocente.DiaSemana,
                Docente = new Docente { NumeroFuncionario = dtoInsereDocente.NumeroFuncionario },
                GrupoHabilitacao = new GrupoHabilitacao { Agrupamento = dtoInsereDocente.Agrupamento },
                HoraFinal = dtoInsereDocente.HoraFinal,
                HoraInicio = dtoInsereDocente.HoraInicio,
                Municipio = new Municipio { Codigo = dtoInsereDocente.CodigoMunicipio },
                UnidadeEnsino = new UnidadeEnsino { Codigo = dtoInsereDocente.CodigoUnidadeEnsino },
                Regional = new Regional { Codigo = dtoInsereDocente.CodigoRegional }

            };

			repositorioDocenteDisponivelGlp.Insere(docenteDisponivel);
		}

		public void RemoveDocenteDisponivel(int docenteDisponivelId, long numeroFuncionario)
		{
			//confere se o registro é do usuário logado
			bool itemExiste = repositorioDocenteDisponivelGlp.ConfereItemEhDoDocente(docenteDisponivelId, numeroFuncionario);

			if (!itemExiste)
			{
				throw new CadastroGlpException(CadastroGlpException.TipoEnum.ItemInexistente);
			}

			repositorioDocenteDisponivelGlp.RemovePor(docenteDisponivelId);
		}

		public void VerificaPermissaoParaInsercaoDocenteDisponivel(DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel dtoSolicitacaoParaLancamento)
		{
			var cadastroGlpExceptionLista = new List<CadastroGlpException>();
            			
			if (string.IsNullOrEmpty(dtoSolicitacaoParaLancamento.CodigoMunicipio))
			{
				var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.MunicipioObrigatorio);
				cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
			}
			if (string.IsNullOrEmpty(dtoSolicitacaoParaLancamento.Agrupamento))
			{
				var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.DisciplinaObrigatoria);
				cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
			}
			if (Convert.ToDateTime(dtoSolicitacaoParaLancamento.HoraInicio) >= Convert.ToDateTime(dtoSolicitacaoParaLancamento.HoraFinal))
			{
				var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.HoraFinalInferiorAHoraInicial);
				cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
			}

			if (Convert.ToDateTime(dtoSolicitacaoParaLancamento.HoraFinal).Subtract(Convert.ToDateTime(dtoSolicitacaoParaLancamento.HoraInicio)).TotalMinutes < 40)
			{
				var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.DuracaoDeAulaMinima);
				cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
			}
            if (dtoSolicitacaoParaLancamento.CodigoRegional == default(int))
            {
                var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.RegionalObrigatoria);
                cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
            }
			TODocenteDisponivelGLPExisteDisponibilidade disponibilidadeGlp = new TODocenteDisponivelGLPExisteDisponibilidade
			{
				CodigoMunicipio = dtoSolicitacaoParaLancamento.CodigoMunicipio,
				DiaSemana = dtoSolicitacaoParaLancamento.DiaSemana,
				HoraFinal = dtoSolicitacaoParaLancamento.HoraFinal,
				HoraInicio = dtoSolicitacaoParaLancamento.HoraInicio,
				NumeroFuncionario = dtoSolicitacaoParaLancamento.NumeroFuncionario,
                CodigoRegional = dtoSolicitacaoParaLancamento.CodigoRegional
			};

			if (repositorioDocenteDisponivelGlp.ExisteDisponibilidade(disponibilidadeGlp))
			{
				var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.NaoExisteDisponibilidade);
				cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
			}

			var listaDisponibilidade = repositorioDocenteDisponivelGlp.EnumeraDisponibilidadePor(dtoSolicitacaoParaLancamento.DiaSemana, dtoSolicitacaoParaLancamento.CodigoMunicipio, dtoSolicitacaoParaLancamento.CodigoRegional);

			if (listaDisponibilidade.Count() > 0)
			{
				DateTime horaInicio = Convert.ToDateTime(Convert.ToDateTime(listaDisponibilidade.First().HoraInicio));
				DateTime horaFim = Convert.ToDateTime(listaDisponibilidade.First().HoraFinal);
				if ((horaInicio >= dtoSolicitacaoParaLancamento.HoraInicio && horaInicio <= dtoSolicitacaoParaLancamento.HoraFinal) ||
					(horaFim >= dtoSolicitacaoParaLancamento.HoraInicio && horaFim <= dtoSolicitacaoParaLancamento.HoraFinal) ||
					(horaInicio <= dtoSolicitacaoParaLancamento.HoraInicio && horaFim >= dtoSolicitacaoParaLancamento.HoraFinal))
				{
					var excecaoCadastroGlp = new CadastroGlpException(CadastroGlpException.TipoEnum.HorarioOcupado);
					cadastroGlpExceptionLista.Add(excecaoCadastroGlp);
				}
			}

			if (cadastroGlpExceptionLista.Count > 0)
			{
				throw new CadastroGlpListaException(cadastroGlpExceptionLista);
			}
		}
	}
}
