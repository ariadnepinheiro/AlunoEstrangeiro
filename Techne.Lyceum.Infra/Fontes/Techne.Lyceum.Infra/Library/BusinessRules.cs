using System;
using Techne.Controls;

namespace Techne.Data
{
    internal enum PutOperationEnum
    {
        Insert, 
        Update, 
        Delete
    }

    internal delegate void PrePutDataRowDelegate(object sender, PrePutDataRowArgs args);

    internal delegate void PostPutDataRowEventHandler(object sender, PostPutDataRowArgs args);

    internal abstract class PutDataRowArgsBase : EventArgs
    {
        private readonly TConnectionWritable connection;

        private readonly IRecordContainer container;

        private readonly PutOperationEnum operation;

        private readonly TDataRow row;

        public PutDataRowArgsBase(PutOperationEnum operation, 
                                  TDataRow row, 
                                  TConnectionWritable connection, 
                                  IRecordContainer container)
        {
            this.connection = connection;
            this.operation = operation;
            this.row = row;
            this.container = container;
        }

        /// <summary>
        ///   Conexão a ser utilizada para operações na mesma transação.
        ///   Esta conexão nunca deverá ser fechada.
        /// </summary>
        public TConnectionWritable Connection
        {
            get
            {
                return this.connection;
            }
        }

        public IRecordContainer Container
        {
            get
            {
                return this.container;
            }
        }

        public PutOperationEnum Operation
        {
            get
            {
                return this.operation;
            }
        }

        public TDataRow Row
        {
            get
            {
                return this.row;
            }
        }
    }

    internal class PrePutDataRowArgs : PutDataRowArgsBase
    {
        public PrePutDataRowArgs(PutOperationEnum operation, 
                                 TDataRow row, 
                                 TConnectionWritable connection, 
                                 IRecordContainer container) : base(operation, row, connection, container)
        {
            this.CancelOperation = false;
            this.SkipRow = false;
        }

        /// <summary>
        ///   Se setado para true, faz rollback na operação inteira.
        ///   Se houverem outros registros a serem processados, nenhum sofrerá alteração.
        /// </summary>
        public bool CancelOperation { get; set; }

        /// <summary>
        ///   Se setado para true, ignora o registro correspondente.
        ///   Se houverem outros registros a serem processados eles serão processados normalmente.
        /// </summary>
        public bool SkipRow { get; set; }
    }

    internal class PostPutDataRowArgs : PutDataRowArgsBase
    {
        private string error;

        public PostPutDataRowArgs(PutOperationEnum operation, 
                                  TDataRow row, 
                                  TConnectionWritable connection, 
                                  IRecordContainer container) : base(operation, row, connection, container)
        {
            this.error = string.Empty;
        }

        public string Error
        {
            get
            {
                return this.error;
            }

            set
            {
                this.error = value == null ? string.Empty : value;
            }
        }
    }
}