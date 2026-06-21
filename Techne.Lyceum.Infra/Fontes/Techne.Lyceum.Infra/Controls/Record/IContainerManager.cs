using System.Data;
using Techne.Data;

namespace Techne.Controls
{
    public enum ContainerManagerAction
    {
        Commit, 
        Rollback, 
        EnterAddNew, 
        EnterEdit, 
        Delete, 
        Undelete
    }

    public interface IContainerManagerBase
    {
        bool Commit();

        bool Delete();

        void EnterAddNewMode();

        void EnterEditMode();

        void Rollback();
    }

    public interface IContainerManager : IContainerManagerBase
    {
        BusinessMethodCollection AssociatedMethods { get; }

        bool DataEntry { get; }

        string DataMember { get; }

        object DataSource { get; }

        string DeletedItemCssClass { get; set; }

        string HistoryUrl { get; set; }

        bool IsEmpty { get; }

        bool IsRoot { get; set; }

        int RowCount { get; }

        bool ShowDeletedRecords { get; set; }

        string[] StoreColumns { get; set; }

        TDataTable Table { get; }

        string Title { get; set; }

        string TitleCssClass { get; }

        string TitleRowCssClass { get; }

        string Transacao { get; set; }

        /// <summary>
        ///   Obtém uma conexão que sirva simultaneamente a este e a todos os managers internos.
        ///   Caso não seja possível, gera uma Exception.
        /// </summary>
        TConnection CreateConnection();

        // bool Commit();
        // bool Delete();
        // void EnterAddNewMode();
        // void EnterEditMode();
        RetVal ExecAssociatedMethod(int methodIndex);

        string GetCaption(string fieldName);

        DataTable GetDesignTimeDataTable();

        string[] GetFields();

        /// <summary>
        ///   Devolve o valor do parâmetro ReturnUrl na query string.
        ///   Se o parâmetro não existir, devolve null.
        /// </summary>
        string GetReturnUrl();

        string GetTitle();

        void Refresh();

        // void Rollback();
        bool Undelete();
    }

    internal interface IContainerManagerInternal : IContainerManager
    {
        IRecordContainer[] DeleteContainers { get; }

        bool ProcessUnchangedContainers { get; }

        void AddMessage(string message, bool isError);

        TConnectionWritable CreateWritableConnection();

        void PosCommit(bool commited, bool isRoot);

        void PosDelete(bool deleted);

        void PosEnterAddNewMode();

        void PosEnterEditMode();

        void PosRollback(bool isRoot);

        void PosUndelete(bool undeleted);

        void PreCommit();

        void PreDelete();

        void PreEnterAddNewMode();

        void PreEnterEditMode();

        void PreRollback();

        void PreUndelete();
    }
}