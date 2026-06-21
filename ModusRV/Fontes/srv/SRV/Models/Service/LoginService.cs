using System.Collections.Generic;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Common.Exceptions;
using SRV.Models.DTO;
using SRV.Security;
using System.Configuration;
using System;
using SRV.Models.Domain;

namespace SRV.Models.Service
{
    public class LoginService : BaseService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoginService));

        public UserState Login(Login login, string clientIP)
        {
            UserState usuario;

            using (SqlConnection conn = GetConnection())
            {
                string senhaDb = Crypt.Encrypt(login.Senha);

                conn.Open();
                LoginMapper mapper = new LoginMapper();
                mapper.connection = conn;

                usuario = mapper.Login(login.Usuario, senhaDb);

                if (usuario == null)
                {
                    string msg = string.Format("Falha no login do usuário {0}", login.Usuario);
                    log.Warn(msg);

                    throw new BusinessException("Usuário ou senha inválida");
                }

                ValidaDisponibilidade(usuario);

                usuario.IPCliente = clientIP;
                AuditLogin(usuario, conn);

                usuario.Ciclo = login.Ciclo;

            }

            return usuario;
        }

        /// <summary>
        /// Verifica se o sistema está disponível para o usuário, coforme seu perfil de acesso, 
        /// caso o cálculo esteja em execução
        /// </summary>
        /// <param name="usuario"></param>
        private void ValidaDisponibilidade(UserState usuario)
        {
            if (usuario.Perfil != Perfil.Administrador && usuario.Perfil != Perfil.Secretaria)
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    ExecucaoCalculoMapper mapper = new ExecucaoCalculoMapper();
                    mapper.connection = conn;

                    ExecucaoCalculo execucao = mapper.FindUltimaExecucao();

                    if (execucao != null && execucao.StatusExecucao == StatusExecucao.EmExecucao)
                    {
                        throw new BusinessException("Sistema indisponível. Tente novamente após alguns instantes.");
                    }
                }            
            }
        }

        private void AuditLogin(UserState usuario, SqlConnection conn)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = "Login";
            logAuditoria.TipoOperacao = OperacaoAuditoria.Login;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = conn;

            logMapper.Insert(logAuditoria);
        }


    }
}