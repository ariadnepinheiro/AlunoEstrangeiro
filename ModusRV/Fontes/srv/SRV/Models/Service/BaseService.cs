using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using SRV.Models.DTO;
using SRV.Models.Domain;
using SRV.Models.Mapper;
using SRV.Common.Logging;

namespace SRV.Models.Service
{
    public class BaseService
    {

        //TODO: criar atributo connection
        // Nas subclasses o construtor padrão (chamado dos controllers) chama getConnection e seta no atributo
        // E outro construtor recebe um connection e seta no atributo (para permitir reaproveitar uma conexão)
        //Ou criar um proxy dos services, que faria o controle da conexão

        protected SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

            SqlConnection conn = new SqlConnection(connectionString);

            return conn;
        }


        protected void AuditInsert(object model, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = model.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Inclusao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            int? idLogAuditoria = logMapper.Insert(logAuditoria);

            LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
            logItemMapper.connection = trans.Connection;
            logItemMapper.transaction = trans;

            Dictionary<string, string> properties = GetModelProperties(model);

            foreach (KeyValuePair<string, string> property in properties)
            {
                LogAuditoriaItem item = new LogAuditoriaItem();

                item.DesAtributo = property.Key;
                item.VlrAtual = property.Value;

                logItemMapper.Insert(item, idLogAuditoria.Value);
            }
        }

        protected void AuditUpdate(object model, object modelOld, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = model.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Atualizacao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            int? idLogAuditoria = logMapper.Insert(logAuditoria);

            LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
            logItemMapper.connection = trans.Connection;
            logItemMapper.transaction = trans;


            Dictionary<string, string> pk = GetModelPrimaryKey(model);

            foreach (KeyValuePair<string, string> property in pk)
            {
                LogAuditoriaItem item = new LogAuditoriaItem();

                item.DesAtributo = property.Key;
                item.VlrAtual = property.Value;
                item.VlrAnterior = property.Value;

                logItemMapper.Insert(item, idLogAuditoria.Value);
            }

            Dictionary<string, string> properties = GetModelProperties(model);
            Dictionary<string, string> propertiesOld = GetModelProperties(modelOld);

            foreach (KeyValuePair<string, string> property in properties)
            {
                string oldValue = propertiesOld[property.Key];

                if ((oldValue != null ? oldValue.ToUpper() : null) != (property.Value != null ? property.Value.ToUpper() : null))
                {
                    LogAuditoriaItem item = new LogAuditoriaItem();

                    item.DesAtributo = property.Key;
                    item.VlrAtual = property.Value;
                    item.VlrAnterior = oldValue;

                    logItemMapper.Insert(item, idLogAuditoria.Value);
                }
            }

        }

        protected void AuditDelete(object modelOld, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = modelOld.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Exclusao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            int? idLogAuditoria = logMapper.Insert(logAuditoria);

            LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
            logItemMapper.connection = trans.Connection;
            logItemMapper.transaction = trans;

            Dictionary<string, string> properties = GetModelProperties(modelOld);

            foreach (KeyValuePair<string, string> property in properties)
            {
                LogAuditoriaItem item = new LogAuditoriaItem();

                item.DesAtributo = property.Key;
                item.VlrAnterior = property.Value;

                logItemMapper.Insert(item, idLogAuditoria.Value);
            }

        }

        private Dictionary<string, string> GetModelProperties(object model)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            foreach (var property in model.GetType().GetProperties())
            {
                properties.Add(property.Name, property.GetValue(model, null) != null ? property.GetValue(model, null).ToString() : null);
            }
            return properties;
        }

        private Dictionary<string, string> GetModelPrimaryKey(object model)
        {
            Dictionary<string, string> pk = new Dictionary<string, string>();

            object[] attributes;
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                attributes = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);

                foreach (object attribute in attributes)
                {
                    pk.Add(property.Name, property.GetValue(model, null).ToString());
                }

            }
            return pk;
        }

		protected void AuditUpdateRecursoServidor(object model, object modelOld, UserState usuario, SqlTransaction trans)
		{
			LogAuditoria logAuditoria = new LogAuditoria();
			logAuditoria.DesObjeto = model.GetType().Name;
			logAuditoria.TipoOperacao = OperacaoAuditoria.Atualizacao;
			logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
			logAuditoria.IpCliente = usuario.IPCliente;

			LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
			logMapper.connection = trans.Connection;
			logMapper.transaction = trans;

			int? idLogAuditoria = logMapper.Insert(logAuditoria);

			LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
			logItemMapper.connection = trans.Connection;
			logItemMapper.transaction = trans;


			Dictionary<string, string> pk = GetModelPrimaryKey(model);

			foreach (KeyValuePair<string, string> property in pk)
			{
				LogAuditoriaItem item = new LogAuditoriaItem();

				item.DesAtributo = property.Key;
				item.VlrAtual = property.Value;
				item.VlrAnterior = property.Value;

				logItemMapper.Insert(item, idLogAuditoria.Value);
			}

			Dictionary<string, string> properties = GetModelProperties(model);
			Dictionary<string, string> propertiesOld = GetModelProperties(modelOld);

			foreach (KeyValuePair<string, string> property in properties)
			{
				string oldValue = propertiesOld[property.Key];

				if ((oldValue != null ? oldValue.ToUpper() : null) != (property.Value != null ? property.Value.ToUpper() : null)
					|| property.Key.ToUpper().Equals("SERVIDOR")
					|| property.Key.ToUpper().Equals("RECURSO"))
				{
					LogAuditoriaItem item = new LogAuditoriaItem();

					item.DesAtributo = property.Key;
					item.VlrAtual = property.Value;
					item.VlrAnterior = oldValue;

					logItemMapper.Insert(item, idLogAuditoria.Value);
				}
			}

		}
    }
}
