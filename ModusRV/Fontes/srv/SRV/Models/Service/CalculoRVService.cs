using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.IO;
using SRV.Models.Domain;

namespace SRV.Models.Service
{
    public class CalculoRVService : BaseService
    {
        public ExecucaoCalculo FindUltimaExecucao()
        {
            ExecucaoCalculo result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ExecucaoCalculoMapper mapper = new ExecucaoCalculoMapper();
                mapper.connection = conn;

                result = mapper.FindUltimaExecucao();
            }

            return result;
        }

        public void UpdateStatusEmExecucao(UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ExecucaoCalculoMapper mapper = new ExecucaoCalculoMapper();
                mapper.connection = conn;

                Usuario usuarioExecucao = new Usuario();
                usuarioExecucao.Id = usuario.Id;
                ExecucaoCalculo execucaoCalculo = new ExecucaoCalculo();
                execucaoCalculo.Usuario = usuarioExecucao;

                execucaoCalculo = mapper.Insert(execucaoCalculo);
            }
        
        }

        public void ExecutarCalculo(int anoReferencia, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    CalculoRVMapper mapper = new CalculoRVMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    AuditCalculo(usuario, OperacaoAuditoria.InicioCalculo, trans);

                    mapper.ExecutarSobreposicaoData(anoReferencia, 1);

                    mapper.ExecutarSobreposicaoData(anoReferencia, 0);

                    mapper.ExecutarCalculo(anoReferencia);

                    AuditCalculo(usuario, OperacaoAuditoria.FimCalculo, trans);

                    trans.Commit();
                }
            }
        }

        public string ExportCoeficienteServidor(int anoReferencia)
        {
            IList<CoeficienteServidor> list = new List<CoeficienteServidor>();

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                CalculoRVMapper mapper = new CalculoRVMapper();
                mapper.connection = conn;

                list = mapper.ListCoeficienteServidor(anoReferencia);
            }

            StringWriter result = new StringWriter();

            foreach (var item in list)
            {
                result.WriteLine(String.Format("{0};{1};{2};{3};{4}", item.AnoReferencia, item.IdServidor, item.Coeficiente, item.IdFuncional, item.Vinculo));
            }

            return result.ToString();
        }

        private void AuditCalculo(UserState usuario, OperacaoAuditoria operacao, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = "Cálculo RV";
            logAuditoria.TipoOperacao = operacao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            logMapper.Insert(logAuditoria);
        }

		public void PreencherCoeficienteServidor(FiltroFuncaoOcorrenciaServidor filtro, UserState UsuarioLogado)
		{
			CoeficienteServidor coeficienteServidor;
			ExecucaoCalculo execucaoCalculo;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				CalculoRVMapper mapper = new CalculoRVMapper();
				mapper.connection = conn;

				coeficienteServidor = mapper.ObterCoeficienteServidor(filtro.IdServidor.Value);
				execucaoCalculo = FindUltimaExecucao();

				if (coeficienteServidor != null && coeficienteServidor.Coeficiente != null)
					filtro.CoeficienteServidor = coeficienteServidor.Coeficiente;

				if (execucaoCalculo != null)
					filtro.DataUltimoProcessamento = execucaoCalculo.DtExecucao;
			}
		}
	}
}