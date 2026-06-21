using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Resources;

namespace Proderj.DOL.WebApp.Controllers
{
    public class AcompanhamentoClassroomController : ControllerPadrao
    {
        private readonly IDadosDocenteService dadosDocenteServico;

        public AcompanhamentoClassroomController(IDadosDocenteService dadosDocenteServico)
        {
            this.dadosDocenteServico = dadosDocenteServico;

            Mapper.CreateMap<DadosTurmaDocenteDTO, DadosTurmaDocenteViewModel>()
                .ForMember(d => d.UnidadeEscolar, opt => opt.MapFrom(s => s.Section))
                .ForMember(d => d.Turma, opt => opt.MapFrom(s => s.Name));

            Mapper.CreateMap<DadosAcessoDocenteDTO, DateTime>()
                .ConstructUsing(x => ((DadosAcessoDocenteDTO)x.SourceValue).LoginTime);
        }

        [LogadoComTermoAceito]
        public ActionResult Inicial(DocenteLogadoBindModel docenteLogadoBindModel)
        {
            AcompanhamentoClassroomViewModel acompanhamentoClassroomModel = new AcompanhamentoClassroomViewModel(docenteLogadoBindModel);
            acompanhamentoClassroomModel.TituloDaPagina = Recurso.Classroom_TituloPagina;
            acompanhamentoClassroomModel.CabecalhoModelo.TituloCabecalho = Recurso.Classroom_TituloPagina.ToUpper();

            /*
            * Foi necessário especificar o tipo dentro do "ConstructUsing", a fim de evitar o erro de ambiguidade entre as chamadas:
            * - ConstructUsing(Func<AutoMapper.ResolutionContext, TDestination> ctor) 
            * e
            * - ConstructUsing(Func<TSource, TDestination> ctor) 
            * */
            // TODO: Por questões de performance e referência de uso, toda definição de mapeamento deverá ser colocada no Application_Start (em Global.asax.cs), posteriormente.
            Mapper.CreateMap<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>()
                .ConstructUsing(
                    (Func<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>)
                    (x => new DadosGeraisDocenteViewModel(docenteLogadoBindModel))
                );

            DadosGeraisDocenteDTO docente = dadosDocenteServico.ObtemPor(docenteLogadoBindModel.Matricula);
            acompanhamentoClassroomModel.DadosGerais = Mapper.Map<DadosGeraisDocenteDTO, DadosGeraisDocenteViewModel>(docente);

            List<DadosTurmaDocenteDTO> turmas = dadosDocenteServico.ListaTurmaPor(docente.IDFuncional);
            acompanhamentoClassroomModel.Turmas = Mapper.Map<List<DadosTurmaDocenteDTO>, List<DadosTurmaDocenteViewModel>>(turmas);

            List<DadosAcessoDocenteDTO> acessos = dadosDocenteServico.ListaAcessoPor(docente.IDFuncional);
            acompanhamentoClassroomModel.UltimosAcessos = Mapper.Map<List<DadosAcessoDocenteDTO>, List<DateTime>>(acessos);

            return View(acompanhamentoClassroomModel);
        }
    }
}
