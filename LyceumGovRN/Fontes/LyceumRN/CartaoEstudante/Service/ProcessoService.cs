using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Domain;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class ProcessoService : SingletonBase<ProcessoService>
    {
        private static readonly ProcessoQuery processoQuery = ProcessoQuery.Instancia;
        private static readonly ExecucaoQuery execucaoQuery = ExecucaoQuery.Instancia;
        private static readonly ExecucaoLogErroQuery execucaoLogErroQuery = ExecucaoLogErroQuery.Instancia;        

        public bool PodeProcessar(string nomeProcesso, TimeSpan horarioProcessamento)
        {
            bool podeProcessar = true;
            List<Entity.Processo> lista = processoQuery.ListaPor(nomeProcesso);

            foreach (var entityProcesso in lista)
            {
                //podeProcessar = true;
                Domain.Processo domainProcesso = ToDomain(entityProcesso);

                if (domainProcesso.TemRestricaoHorario
                  && (entityProcesso.RestricaoHorarioInicio > horarioProcessamento || entityProcesso.RestricaoHorarioFim < horarioProcessamento))
                {
                    podeProcessar = false;
                }
                else
                {
                    podeProcessar = true;
                    break;
                }
            }
          

            return podeProcessar;
        }

        public void GravarExecucao(string nomeProcesso, DateTime dataInicioExecucao, DateTime dataFimExecucao, StatusExecucaoEnum statusExecucao, string aluno, string descricaoErro)
        {
            Entity.Processo entityProcesso = processoQuery.ObtemPor(nomeProcesso);
            
            if (entityProcesso.ProcessoId > 0)
            {                        
                Execucao execucao = new Execucao
                {
                    ProcessoId = entityProcesso.ProcessoId,
                    DataInicioExecucao = dataInicioExecucao,
                    DataFimExecucao = dataFimExecucao,
                    SituacaoExecucao = Convert.ToBoolean((int)statusExecucao)
                };

                execucao = execucaoQuery.Insere(execucao);

                if (statusExecucao == StatusExecucaoEnum.ERRO)
                {
                    ExecucaoLogErro logErro = new ExecucaoLogErro{
                        Aluno = aluno,
                        Descricao = descricaoErro,
                        ExecucaoId = execucao.ExecucaoId
                    };

                    execucaoLogErroQuery.Insere(logErro);
                }
            }
        }

        internal Domain.Processo ToDomain(Entity.Processo entity)
        {
            Domain.Processo domain = null;
            if (entity != null)
            {
                domain = new Domain.Processo();
                domain.Id = entity.ProcessoId;
                domain.Nome = entity.NomeProcesso;
                domain.RestricaoHorarioSituacao = entity.RestricaoHorarioSituacao;
                domain.SituacaoProcesso = (SituacaoProcessoEnum)entity.SituacaoProcesso;
                domain.RestricaoHorarioInicio = entity.RestricaoHorarioInicio;
                domain.RestricaoHorarioFim = entity.RestricaoHorarioFim;
            }

            return domain;
        }
    }
}
