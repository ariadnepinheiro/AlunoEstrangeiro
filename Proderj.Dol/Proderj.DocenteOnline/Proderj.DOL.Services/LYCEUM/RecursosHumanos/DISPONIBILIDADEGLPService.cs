using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
	public class DISPONIBILIDADEGLPService : IDISPONIBILIDADEGLPService
	{
        private readonly IDISPONIBILIDADEGLPRepository repositorioDISPONIBILIDADEGLP;

        public DISPONIBILIDADEGLPService(IDISPONIBILIDADEGLPRepository repositorioDISPONIBILIDADEGLP)
		{
            this.repositorioDISPONIBILIDADEGLP = repositorioDISPONIBILIDADEGLP;
		}

        public IEnumerable<DTOListaDISPONIBILIDADEGLP> ListaPor(long num_func, int ano)
        {
            return repositorioDISPONIBILIDADEGLP.ListaPor<DTOListaDISPONIBILIDADEGLP>(num_func, ano);
        }

        public DISPONIBILIDADEGLP Inclui(DTOIncluiDISPONIBILIDADEGLP dto) 
        {
            try
            {
                //*** repositorioDISPONIBILIDADEGLP.Inclui(obj); ***
                //NÃO VAI ADIANTAR USAR O NHIBERNATE para fazer o insert da entidade.
                //Após o insert de DISPONIBILIDADEGLP, ele tenta obter o ID gerado para esta entidade
                //através do @@IDENTITY ou @SCOPE_IDENTITY, porém sempre retorna ZERO por causa
                //da execução da trigger de insert.
                //SOLUÇÃO: fazer o insert na mão, usando instruções SQL, à moda antiga.

                Mapper.CreateMap<string, LY_GRUPO_HABILITACAO>()
                    .ForMember(d => d.AGRUPAMENTO, opts => opts.MapFrom(s => s));

                Mapper.CreateMap<int, LY_DOCENTE>()
                    .ForMember(d => d.NUM_FUNC, opts => opts.MapFrom(s => s));

                Mapper.CreateMap<DiaDaSemanaEnum, DISPONIBILIDADEGLP_DIASEMANA>()
                    .ConvertUsing(src => new DISPONIBILIDADEGLP_DIASEMANA { DIASEMANA = (int)src, });

                Mapper.CreateMap<string, DISPONIBILIDADEGLP_MODALIDADE>()
                    .ConvertUsing(src => new DISPONIBILIDADEGLP_MODALIDADE { MODALIDADE = src, });

                Mapper.CreateMap<string, DISPONIBILIDADEGLP_TURNO>()
                    .ConvertUsing(src => new DISPONIBILIDADEGLP_TURNO { TURNO = src, });

                Mapper.CreateMap<string, DISPONIBILIDADEGLP_UNIDADEENSINO>()
                    .ConvertUsing(src => new DISPONIBILIDADEGLP_UNIDADEENSINO { UNIDADE_ENS = src, });

                Mapper.CreateMap<DTOIncluiDISPONIBILIDADEGLP, DISPONIBILIDADEGLP>()
                    .ForMember(d => d.LY_GRUPO_HABILITACAO, opts => opts.MapFrom(s => s.AGRUPAMENTO))
                    .ForMember(d => d.LY_DOCENTE, opts => opts.MapFrom(s => s.NUM_FUNC))
                    .ForMember(d => d.DISPONIBILIDADEGLP_DIASEMANA, opts => opts.MapFrom(s => s.DIASEMANA))
                    .ForMember(d => d.DISPONIBILIDADEGLP_MODALIDADE, opts => opts.MapFrom(s => s.MODALIDADE))
                    .ForMember(d => d.DISPONIBILIDADEGLP_TURNO, opts => opts.MapFrom(s => s.TURNO))
                    .ForMember(d => d.DISPONIBILIDADEGLP_UNIDADEENSINO, opts => opts.MapFrom(s => s.UNIDADE_ENS))
                    .ForMember(d => d.DATAALTERACAO, opts => opts.MapFrom(s => s.DATACADASTRO))
                    ;

                var obj = Mapper.Map<DISPONIBILIDADEGLP>(dto);

                Validacao validaInsercao = ValidaInsercao(obj);
                if (!validaInsercao.Valido)
                    throw new System.Exception(validaInsercao.Mensagem.Replace(Environment.NewLine, "<br />"));

                repositorioDISPONIBILIDADEGLP.InsereAuditada(obj);

                repositorioDISPONIBILIDADEGLP.AtualizaEntidade(obj);

                return obj;
            }
            catch
            {
                throw;
            }
        }

        public void Exclui(int disponibilidadeGlpId, string unidadeEnsino) 
        {
            repositorioDISPONIBILIDADEGLP.ExcluiAuditada(disponibilidadeGlpId, unidadeEnsino);
        }

        private Validacao ValidaInsercao(DISPONIBILIDADEGLP entidade)
        {
            try
            {
                List<string> mensagens = new List<string>();

                if (entidade.LY_DOCENTE == null)
                {
                    mensagens.Add("DOCENTE não definido.");
                }

                if (!entidade.DISPONIBILIDADEGLP_UNIDADEENSINO.Any())
                {
                    mensagens.Add("Informe a UNIDADE ESCOLAR.");
                }

                if (entidade.LY_GRUPO_HABILITACAO == null || string.IsNullOrWhiteSpace(entidade.LY_GRUPO_HABILITACAO.AGRUPAMENTO))
                {
                    mensagens.Add("Informe a DISCIPLINA.");
                }

                if (!entidade.DISPONIBILIDADEGLP_MODALIDADE.Any())
                {
                    mensagens.Add("Informe a MODALIDADE.");
                }

                if (!entidade.DISPONIBILIDADEGLP_DIASEMANA.Any())
                {
                    mensagens.Add("Informe o DIA DA SEMANA.");
                }

                if (!entidade.DISPONIBILIDADEGLP_TURNO.Any())
                {
                    mensagens.Add("Informe o TURNO.");
                }

                if (!repositorioDISPONIBILIDADEGLP.EhDisciplinaHabilitadaPor(entidade.LY_GRUPO_HABILITACAO.AGRUPAMENTO, entidade.LY_DOCENTE.NUM_FUNC))
                {
                    mensagens.Add("DISCIPLINA não habilitada no Sistema Conexão Educação. Orientamos encaminhar a documentação para a Regional a fim de que seja analisada pela Inspeção Escolar e inserida no Sistema Conexão Educação pelo Coordenador de Gestão de Pessoas.");
                }

                string msg = string.Empty;
                foreach (var unidadeEnsino in entidade.DISPONIBILIDADEGLP_UNIDADEENSINO) 
                {
                    foreach (var modalidade in entidade.DISPONIBILIDADEGLP_MODALIDADE) 
                    {
                        foreach (var diaSemana in entidade.DISPONIBILIDADEGLP_DIASEMANA) 
                        {
                            foreach (var turno in entidade.DISPONIBILIDADEGLP_TURNO) 
                            {
                                if (repositorioDISPONIBILIDADEGLP.ExistePor(
                                    entidade.LY_DOCENTE.NUM_FUNC,
                                    entidade.ANO,
                                    entidade.LY_GRUPO_HABILITACAO.AGRUPAMENTO,
                                    unidadeEnsino.UNIDADE_ENS,
                                    modalidade.MODALIDADE,
                                    diaSemana.DIASEMANA,
                                    turno.TURNO
                                ))
                                {
                                    if (string.IsNullOrEmpty(msg))
                                    {
                                        msg = @"Prezado(a) professor(a),
                                            Já existe uma ou mais disponibilizações para GLP com as mesmas características indicadas.";
                                    }
                                }

                                if (!string.IsNullOrEmpty(msg))
                                    break;
                            }
                            
                            if (!string.IsNullOrEmpty(msg))
                                break;
                        }

                        if (!string.IsNullOrEmpty(msg))
                            break;
                    }

                    if (!string.IsNullOrEmpty(msg))
                        break;
                }
                
                if (!string.IsNullOrEmpty(msg))
                    mensagens.Add(msg);

                return new Validacao(mensagens.Any() ? string.Join(Environment.NewLine, mensagens.ToArray()) : null);
            }
            catch
            {
                throw;
            }
        }

        private class Validacao
        {
            public Validacao(string mensagem)
            {
                mensagem = string.IsNullOrWhiteSpace(mensagem) ? null : mensagem;

                this.Valido = mensagem == null;
                this.Mensagem = mensagem;
            }

            public bool Valido { get; private set; }
            public string Mensagem { get; private set; }
        }
	}
}
