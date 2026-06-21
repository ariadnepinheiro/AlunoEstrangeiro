using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.Domain;

namespace SRV.Models.Service
{
    public class MotivoInelegDocenteUnidAdmService : BaseService
    {
        public MotivoInelegDocenteUnidAdm Find(int idUnidade, int idAnoReferencia, int? idServidor)
        {
            MotivoInelegDocenteUnidAdm motivoInelegDocenteUnidade;
            SqlConnection conn = GetConnection();
            UnidadeAdministrativaMapper unidadeMapper;
            ServidorMapper servidorMapper;
            MotivoInelegUnidAdmMapper motivoInelegUnidAdmMapper;
            MotivoInelegDocenteMapper motivoInelegDocenteMapper;
            IList<MotivoInelegibilidade> motivosDocente;

            try
            {
                motivoInelegDocenteUnidade = new MotivoInelegDocenteUnidAdm();
                motivoInelegDocenteUnidade.IdAnoReferencia = idAnoReferencia;

                conn.Open();

                unidadeMapper = new UnidadeAdministrativaMapper();
                unidadeMapper.connection = conn;

                motivoInelegDocenteUnidade.UnidadeAdministrativa = unidadeMapper.Find(idUnidade);

                // Motivos por unidade
                motivoInelegUnidAdmMapper = new MotivoInelegUnidAdmMapper();
                motivoInelegUnidAdmMapper.connection = conn;
                
                motivoInelegDocenteUnidade.Motivos = motivoInelegUnidAdmMapper.List(idUnidade, idAnoReferencia);

                if (idServidor.HasValue)
                {
                    servidorMapper = new ServidorMapper();
                    servidorMapper.connection = conn;
                    motivoInelegDocenteUnidade.Servidor = servidorMapper.Find(idServidor.Value);

                    // Motivos por servidor e unidade
                    motivoInelegDocenteMapper = new MotivoInelegDocenteMapper();
                    motivoInelegDocenteMapper.connection = conn;

                    motivosDocente = motivoInelegDocenteMapper.List(idServidor.Value, idAnoReferencia, idUnidade);

                    if (motivosDocente != null)
                        motivoInelegDocenteUnidade.Motivos = motivoInelegDocenteUnidade.Motivos.Concat<MotivoInelegibilidade>(motivosDocente).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return motivoInelegDocenteUnidade;
        }
    }
}