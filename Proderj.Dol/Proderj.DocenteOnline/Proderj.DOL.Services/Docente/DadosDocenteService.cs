using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    public class DadosDocenteService: IDadosDocenteService
    {
        private IDadosGeraisDocenteRepository repositorioDadosGerais;
        private IDadosFormacaoDocenteRepository repositorioFormacaoDocente;
        private IDadosCapacitacaoDocenteRepository repositorioDadosCapacitacaoDocente;
        private IDadosTurmaDocenteRepository repositorioDadosTurmaDocente;
        private IDadosAcessoDocenteRepository repositorioDadosAcessoDocente;

        public DadosDocenteService(
            IDadosGeraisDocenteRepository repositorioDadosGerais,
            IDadosFormacaoDocenteRepository repositorioFormacaoDocente,
            IDadosCapacitacaoDocenteRepository repositorioDadosCapacitacaoDocente,
            IDadosTurmaDocenteRepository repositorioDadosTurmaDocente,
            IDadosAcessoDocenteRepository repositorioDadosAcessoDocente
        )
        {
            this.repositorioDadosGerais = repositorioDadosGerais;
            this.repositorioFormacaoDocente = repositorioFormacaoDocente;            
            this.repositorioDadosCapacitacaoDocente = repositorioDadosCapacitacaoDocente;
            this.repositorioDadosTurmaDocente = repositorioDadosTurmaDocente;
            this.repositorioDadosAcessoDocente = repositorioDadosAcessoDocente;

            Mapper.CreateMap<DadosGeraisDocente, DadosGeraisDocenteDTO>();
            Mapper.CreateMap<DadosTurmaDocente, DadosTurmaDocenteDTO>();
            Mapper.CreateMap<DadosAcessoDocente, DadosAcessoDocenteDTO>();
        }
		
		//public override IQueryable<DadosGeraisDocenteDTO> ListaQueryable()
       // {
       //     return base.ListaQueryable();
        //}

        public DadosGeraisDocenteDTO ObtemPor(string matricula)
        {
            DadosGeraisDocente dadosGeraisDocente = repositorioDadosGerais.ObtemPor(matricula);
            DadosGeraisDocenteDTO dto = Mapper.Map<DadosGeraisDocente, DadosGeraisDocenteDTO>(dadosGeraisDocente);

            return dto;
        }

        public List<DadosFormacaoDocenteDTO> ListaFormacaoPor(string matricula, TipoFormacaoEnum tipoFormacao)
        {
            var dadosGraduacaoDocente = repositorioFormacaoDocente.ListaFormacaoPor(matricula, tipoFormacao).ToList();
            List<DadosFormacaoDocenteDTO> dto = Mapper.Map<List<DadosFormacaoDocente>, List<DadosFormacaoDocenteDTO>>(dadosGraduacaoDocente);

            return dto;
        }

        public List<DadosCapacitacaoDocenteDTO> ListaCapacitacaoPor(string matricula)
        {
            List<DadosCapacitacaoDocente> dadosCapacitacaoDocente = repositorioDadosCapacitacaoDocente.ListaPor(matricula).ToList();
            List<DadosCapacitacaoDocenteDTO> dto = Mapper.Map<List<DadosCapacitacaoDocente>, List<DadosCapacitacaoDocenteDTO>>(dadosCapacitacaoDocente);

            return dto;
        }

        public List<DadosTurmaDocenteDTO> ListaTurmaPor(string matricula)
        {
            var dadosTurmaDocente = repositorioDadosTurmaDocente.ListaPor(matricula).ToList();
            var dto = Mapper.Map<List<DadosTurmaDocente>, List<DadosTurmaDocenteDTO>>(dadosTurmaDocente);

            return dto;
        }

        public List<DadosAcessoDocenteDTO> ListaAcessoPor(string matricula)
        {
            var dadosAcessoDocente = repositorioDadosAcessoDocente.ListaPor(matricula).ToList();
            var dto = Mapper.Map<List<DadosAcessoDocente>, List<DadosAcessoDocenteDTO>>(dadosAcessoDocente);

            return dto;
        }
    }
}
