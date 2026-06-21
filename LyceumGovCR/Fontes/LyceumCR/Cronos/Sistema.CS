//// HadesLyc vProducao
//// Gerado por:
////   debora.matteo em 19/02/2009 às 09:44h
////   CronosRN, Version=1.1.3334.34401, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
//// Último load por leo em 01/12/2008 às 17:29:58 do banco SQLServer (Cronos@Techne17\Usuarios_SQL:HadCronosProducao)

//using System;
//using System.Collections;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Data;
//using System.Data.OleDb;
//using System.Globalization;
//using System.Reflection;

//using Techne.Data;
//using Techne.Library;
//using Techne.Library.MultiLang;

//namespace Techne.HadesLyc.CR
//{
//    public class Sistema : TDataSet
//    {
//        private const string DataSetName_Def = "Sistema";
//        private static ConstructorInfo ctorTCustom = null;
//        private CustomBase tcustom = null;
//        #region Variáveis private para as tabelas deste dataset
//        private Hd_modulo pvTabHd_modulo;
//        private Hd_padtrans pvTabHd_padtrans;
//        private Hd_sistema pvTabHd_sistema;
//        private Hd_transacao pvTabHd_transacao;
//        #endregion

//        [Obsolete("Utilize o construtor normal. O método estático Create() será deimplementado.")]
//        public static Sistema Create()
//        {
//            throw new NotSupportedException("Utilize o construtor normal. O método estático Create() será deimplementado.");
//        }

//        static Sistema()
//        {
//            ctorTCustom = CRCustomLib.GetTCustomConstructor("Sistema");
//        }

//        public Sistema()
//            : base("pt-BR")
//        {
//            DataSetName = DataSetName_Def;

//            pvTabHd_modulo = new Hd_modulo();
//            pvTabHd_padtrans = new Hd_padtrans();
//            pvTabHd_sistema = new Hd_sistema();
//            pvTabHd_transacao = new Hd_transacao();

//            Tables.Add(pvTabHd_modulo);
//            Tables.Add(pvTabHd_padtrans);
//            Tables.Add(pvTabHd_sistema);
//            Tables.Add(pvTabHd_transacao);

//            CreateRelations();

//            if (ctorTCustom != null) tcustom = (CustomBase)ctorTCustom.Invoke(new object[0]);
//        }


//        [DefaultValue(DataSetName_Def)]
//        public new string DataSetName
//        {
//            get { return base.DataSetName; }
//            set { base.DataSetName = value == null ? string.Empty : value; }
//        }

//        private void CreateRelations()
//        {
//            Relations.Add("fk_modulo_sistema",
//                          new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
//                          new DataColumn[] { pvTabHd_modulo.Columns["sis"] }, false);
//            Relations.Add("fk_transacao_modulo",
//                          new DataColumn[] { pvTabHd_modulo.Columns["sis"], pvTabHd_modulo.Columns["modulo"] },
//                          new DataColumn[] { pvTabHd_transacao.Columns["sis"], pvTabHd_transacao.Columns["modulo"] }, false);
//            Relations.Add("fk_transacao_sistema",
//                          new DataColumn[] { pvTabHd_sistema.Columns["sis"] },
//                          new DataColumn[] { pvTabHd_transacao.Columns["sis"] }, false);
//        }

//        public override TConnection CreateConnection() { return Config.CreateConnection(); }

//        public override TConnectionWritable CreateWritableConnection(TPermission permission) { return Config.CreateWritableConnection(permission); }

//        public virtual string Put(TConnectionWritable connection)
//        {
//            connection.Open(true);
//            try
//            {
//                if (tcustom != null)
//                    return tcustom.Put(this, connection);

//                else
//                {
//                    DataRow row;
//                    pvTabHd_modulo.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_modulo); if (row != null) return DataLib.RowErrorsToString(row);
//                    pvTabHd_padtrans.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_padtrans); if (row != null) return DataLib.RowErrorsToString(row);
//                    pvTabHd_sistema.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_sistema); if (row != null) return DataLib.RowErrorsToString(row);
//                    pvTabHd_transacao.Put(connection); row = DataLib.FirstRowWithError(pvTabHd_transacao); if (row != null) return DataLib.RowErrorsToString(row);
//                    return string.Empty;
//                }
//            }
//            catch
//            {
//                connection.Rollback();
//                throw;
//            }
//            finally
//            {
//                if (connection.State == ConnectionState.Open) connection.Close();
//            }
//        }


//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public new DataRelationCollection Relations { get { return base.Relations; } }


//        #region Propriedades para acesso às tabelas
//        [
//          Browsable(false),
//        ]
//        public Hd_modulo Hd_modulo { get { return pvTabHd_modulo; } }

//        [
//          Browsable(false),
//        ]
//        public Hd_padtrans Hd_padtrans { get { return pvTabHd_padtrans; } }

//        [
//          Browsable(false),
//        ]
//        public Hd_sistema Hd_sistema { get { return pvTabHd_sistema; } }

//        [
//          Browsable(false),
//        ]
//        public Hd_transacao Hd_transacao { get { return pvTabHd_transacao; } }

//        #endregion

//        public abstract class CustomBase
//        {
//            public abstract string Put(Techne.HadesLyc.CR.Sistema ds, TConnectionWritable connection);
//        }

//    }

//}

