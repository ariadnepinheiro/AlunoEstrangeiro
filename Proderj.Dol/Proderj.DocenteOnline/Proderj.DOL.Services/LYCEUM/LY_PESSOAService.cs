using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class LY_PESSOAService : ILY_PESSOAService
	{
        private ILY_PESSOARepository repositorioPessoa;
        private ILY_FL_PESSOARepository repositorioFlPessoa;
        private IGOOGLEEDUCATIONRepository repositorioGoogleEducation;
        private ITCE_MUNICIPIORepository repositorioMunicipio;
        private IStoredProcedures repositorioStoredProcedures;
        private IDadosGeraisDocenteRepository repositorioDadosGerais;

        public LY_PESSOAService(
            ILY_PESSOARepository repositorioPessoa, 
            ILY_FL_PESSOARepository repositorioFlPessoa,
            IGOOGLEEDUCATIONRepository repositorioGoogleEducation,
            ITCE_MUNICIPIORepository repositorioMunicipio,
            IStoredProcedures repositorioStoredProcedures
        )
		{
			this.repositorioPessoa = repositorioPessoa;
            this.repositorioFlPessoa = repositorioFlPessoa;
            this.repositorioGoogleEducation = repositorioGoogleEducation;
            this.repositorioMunicipio = repositorioMunicipio;
            this.repositorioStoredProcedures = repositorioStoredProcedures;
		}

        public DTOLY_PESSOA ObtemPor(int? pessoa)
        {
            var obj = repositorioPessoa.ObtemPorChavePrimaria(pessoa);
            
            var dto = ObtemPor(obj);

            //obtenção do e-mail educa do usuário
            dto.E_MAIL_EDUCA = ObtemEmailEduca(obj.PESSOA);

            //TCE_MUNICIPIO não está relacionado com LY_PESSOA no BD, então será obtido através de "repositorioMunicipio"
            if (obj != null)
            {
                var objMunicipio = repositorioMunicipio.ObtemPorChavePrimaria(obj.END_MUNICIPIO);
                dto.END_UF = objMunicipio.UF;
            }

            return dto;
        }

        public DTOLY_PESSOA ObtemPor(string matricula)
        {
            //obtenção do idfuncional


            var obj = repositorioPessoa.ListaQueryable().FirstOrDefault(q => q.LY_DOCENTE.Any(r => r.MATRICULA == matricula));
            var dto = ObtemPor(obj);

            var idfuncional = repositorioPessoa.ObtemIdFuncional(dto.PESSOA.ToString());
               

            //obtenção do e-mail educa do usuário
            dto.E_MAIL_EDUCA = ObtemEmailEduca(obj.PESSOA);
            
            dto.E_MAIL_EDUCA = ObtemEmailEduca(obj.PESSOA);

            //TCE_MUNICIPIO não está relacionado com LY_PESSOA no BD, então será obtido através de "repositorioMunicipio"
            if (obj != null)
            {
                var objMunicipio = repositorioMunicipio.ObtemPorChavePrimaria(obj.END_MUNICIPIO);
                if (objMunicipio != null)
                {
                    dto.END_UF = objMunicipio.UF;
                }
            }
            
            //Vínculo do resultado da Stored Procedure à DTO
            dto.REL_CH_SERV_ANO1 = repositorioStoredProcedures.REL_CH_SERV_ANO(idfuncional, DateTime.Now.Year.ToString()).FirstOrDefault();
            if (dto.REL_CH_SERV_ANO1 != null)
            {
                if (dto.REL_CH_SERV_ANO1.SEGUNDA_MATRICULA != null)
                    if (!string.IsNullOrEmpty(dto.REL_CH_SERV_ANO1.SEGUNDA_MATRICULA))
                        dto.REL_CH_SERV_ANO2 = repositorioStoredProcedures.REL_CH_SERV_ANO(dto.REL_CH_SERV_ANO1.SEGUNDO_IDVINCULO, DateTime.Now.Year.ToString()).FirstOrDefault();
            }
            return dto;
        }

        public DTOLY_PESSOA Atualiza(DTOAtualizaPessoa dto)
        {
            var obj = repositorioPessoa.ObtemPorChavePrimaria(dto.PESSOA);

            if (obj == null)
                return null;
            
            Mapper.CreateMap<DTOAtualizaPessoa, LY_PESSOA>()
                .ForMember(d => d.LY_FL_PESSOA, opts => opts.MapFrom(s => repositorioFlPessoa.ListaQueryable().FirstOrDefault(q => q.PESSOA == s.PESSOA)));
            obj = Mapper.Map(dto, obj);

            if (!string.IsNullOrWhiteSpace(dto.FL_FIELD_01) && obj.LY_FL_PESSOA == null)
            {
                obj.LY_FL_PESSOA = new LY_FL_PESSOA();
                obj.LY_FL_PESSOA.PESSOA = obj.PESSOA;
                repositorioFlPessoa.IncluiAuditada(obj.LY_FL_PESSOA);
            }

            obj.LY_FL_PESSOA.FL_FIELD_01 = dto.FL_FIELD_01;

            repositorioPessoa.AlteraAuditada(obj);

            repositorioPessoa.AtualizaEntidade(obj);

            var result = ObtemPor(obj);

            //obtenção do e-mail educa do usuário
            result.E_MAIL_EDUCA = ObtemEmailEduca(obj.PESSOA);

            return result;
        }

        private DTOLY_PESSOA ObtemPor(LY_PESSOA obj)
        {
            Mapper.CreateMap<LY_PESSOA, DTOLY_PESSOA>();
            Mapper.CreateMap<LY_DOCENTE, DTOLY_PESSOA.DTOLY_DOCENTE>();
            Mapper.CreateMap<LY_FL_PESSOA, DTOLY_PESSOA.DTOLY_FL_PESSOA>();
            DTOLY_PESSOA dto = Mapper.Map<DTOLY_PESSOA>(obj);

            return dto;
        }

       private string ObtemEmailEduca(double pessoa)
        {
            var ge = repositorioGoogleEducation.ListaQueryable().FirstOrDefault(q => q.PESSOA == pessoa);
            if (ge == null)
                return null;
            return ge.EMAIL;
        }
	}

}
