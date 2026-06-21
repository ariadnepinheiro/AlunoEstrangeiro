using System;
using Techne.Data;

namespace Techne.Controls
{
    public delegate void PostContainerOperationEventHandler(object sender, PostContainerOperationEventArgs args);

    public interface IRecordContainer
    {
        event PostContainerOperationEventHandler PostContainerOperation;

        bool Changed { get; }

        bool Deleted { get; }

        DateTime HistInsertStamp { get; }

        string HistInsertUser { get; }

        IContainerManager Manager { get; }

        RecordManagerMode Mode { get; }

        DbObject[] PrimaryKeyValues { get; }

        DbObject[] StoredValues { get; }

        DbObject this[string columnName] { get; set; }
    }

    internal interface IRecordContainerInternal : IRecordContainer
    {
        bool HasErrors { get; }

        void AddMessage(string message, bool isError);

        ITControl[] GetControl(string fieldName);

        void OnPostContainerOperation(PostContainerOperationEventArgs args);

        void OnPostPutDataRow(PostPutDataRowArgs args);

        void OnPrePutDataRow(PrePutDataRowArgs args);

        void RegisterControl(ITControl control);

        void SetChanged(bool changed);

        void SetDeleted(bool deleted);

        void SetHistInfo(string insertUser, DateTime insertStamp);

        void SetMode(RecordManagerMode mode);

        void SetMode(RecordManagerMode mode, ContainerManagerAction action);

        void SetPrimaryKeyValues(DbObject[] primaryKeyValues);

        void SetStoredValues(DbObject[] values);
    }

    public class RecordContainerEventArgs : EventArgs
    {
        private readonly IRecordContainer container;

        public RecordContainerEventArgs(IRecordContainer container)
        {
            this.container = container;
        }

        public IRecordContainer Container
        {
            get
            {
                return this.container;
            }
        }
    }

    public class PostContainerOperationEventArgs : RecordContainerEventArgs
    {
        private readonly ContainerManagerAction managerAction;

        private readonly RecordManagerMode oldContainerMode;

        public PostContainerOperationEventArgs(IRecordContainer container, 
                                               RecordManagerMode oldContainerMode, 
                                               ContainerManagerAction managerAction) : base(container)
        {
            this.oldContainerMode = oldContainerMode;
            this.managerAction = managerAction;
        }

        /// <summary>
        ///   Operação realizada no manager (IContainerManager) para ter disparado o evento.
        /// </summary>
        public ContainerManagerAction ManagerAction
        {
            get
            {
                return this.managerAction;
            }
        }

        /// <summary>
        ///   Modo do container (IRecordContainer) antes da operação ter sido realizada
        ///   no manager correspondente.
        /// </summary>
        public RecordManagerMode OldContainerMode
        {
            get
            {
                return this.oldContainerMode;
            }
        }
    }
}