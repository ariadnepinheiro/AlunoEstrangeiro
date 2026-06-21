using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class ControleProcessamentoService : SingletonBase<ControleProcessamentoService>
    {
        private static readonly ControleProcessamentoQuery controleProcessamentoQuery = ControleProcessamentoQuery.Instancia;
        private static readonly LogControleProcessamentoQuery logControleProcessamentoQuery = LogControleProcessamentoQuery.Instancia;
        private const string USUARIO_INTEGRADOR = "ZEUS";

        public DateTime[] ObtemDatas(string nomeProcesso)
        {
            DateTime[] datas = new DateTime[2];

            ControleProcessamento controleProcessamento =
                controleProcessamentoQuery.ObtemUltimaAtualizacaoPor(nomeProcesso);

            if (controleProcessamento.ControleProcessamentoId > 0)
            {
                datas[0] = controleProcessamento.DataFimMovimento.Value;
                datas[1] = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            }
            else
            {
                datas[0] = DateTime.Now.Subtract(TimeSpan.FromDays(2));
                datas[1] = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            }

            return datas;
        }

        public void Agendar(string nome, DateTime inicio, string intervalo)
        {
            ControleProcessamento controleProcessamento = new ControleProcessamento
            {
                DataAgenda = inicio,
                Usuario = USUARIO_INTEGRADOR
            };

            switch (nome)
            {
                case "Registro Retorno":
                    controleProcessamento.NomeProcesso = RegistrosRetornoService.NOMEPROCESSO_REGISTROS_RETORNO;
                    break;
                case "Duplicidades de Matrícula":
                    controleProcessamento.NomeProcesso = DuplicidadeMatriculaService.NOMEPROCESSO_DUPLICIDADE;
                    break;
                case "Cartão":
                    controleProcessamento.NomeProcesso = CartaoService.NOMEPROCESSO_CARTAO;
                    break;
            }

            switch (intervalo)
            {
                case "A cada hora":
                case "Apenas uma vez":
                    controleProcessamento.Frequencia = "E";
                    break;
                case "Diariamente":
                    controleProcessamento.Frequencia = "D";
                    break;
                case "Semanalmente":
                    controleProcessamento.Frequencia = "S";
                    break;
                case "Mensalmente":
                    controleProcessamento.Frequencia = "M";
                    break;
                case "Anualmente":
                    controleProcessamento.Frequencia = "A";
                    break;
            }

            try
            {
                controleProcessamentoQuery.Insere(controleProcessamento);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Insere(ControleProcessamentoDTO controleProcessamentoDTO)
		{
            ControleProcessamento controleProcessamento;
            LogControleProcessamento logControleProcessamento;

            try
            {
                controleProcessamento = new ControleProcessamento
                {
                    DataAgenda = controleProcessamentoDTO.DataAgenda,
                    Frequencia = controleProcessamentoDTO.Frequencia,
                    NomeProcesso = controleProcessamentoDTO.NomeProcesso,
                    Usuario = USUARIO_INTEGRADOR,
                    DataInicioMovimento = controleProcessamentoDTO.DtIniMov,
                    DataFimMovimento = controleProcessamentoDTO.DtFimMov
                };

                logControleProcessamento = new LogControleProcessamento
                {
                    DataProcessamento = controleProcessamentoDTO.LogControleProcessamento.DtProc,
                    RegistroInicial = controleProcessamentoDTO.LogControleProcessamento.RegInicial,
                    RegistroFinal = controleProcessamentoDTO.LogControleProcessamento.RegFinal,
                    QuantidadeRegistrosRetornados = Convert.ToInt32(controleProcessamentoDTO.LogControleProcessamento.QtdRegsRetornados),
                    UltimoRegistroProcessado = Convert.ToInt32(controleProcessamentoDTO.LogControleProcessamento.UltRegProc),
                    SituacaoProcessamento = controleProcessamentoDTO.LogControleProcessamento.StProc
                };

                controleProcessamentoQuery.Insere(controleProcessamento, logControleProcessamento);
            }
            catch (Exception e)
            {                 
                throw e;
            }
		}

        public void RemovePor(string nomeProcesso, DateTime dataAgenda)
		{
            ControleProcessamento controleProcessamento = controleProcessamentoQuery.ObtemPor(nomeProcesso, dataAgenda);

            try
            {          
                
            }
            catch (Exception e)
            {                 
                throw e;
            }

		}

        public bool PodeProcessar(string nomeProcesso, TimeSpan horarioProcessamento)
        {
            return controleProcessamentoQuery.PodeProcessar(nomeProcesso, horarioProcessamento);            
        }

        public void GeraLogErro(string nomeProcesso)
        {
            LogControleProcessamento log = new LogControleProcessamento();
            log.ControleProcessamentoId = controleProcessamentoQuery.ObtemPor(nomeProcesso).ControleProcessamentoId;
            logControleProcessamentoQuery.InsereErro(log);
        }
    }
}
