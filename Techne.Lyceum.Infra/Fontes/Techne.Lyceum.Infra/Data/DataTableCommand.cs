namespace Techne.Data
{
    public enum CommandOperation
    {
        Select, 
        Insert, 
        Update, 
        Delete, 
        Undelete
    }

    public class DataTableCommand : TCommand
    {
        private readonly CommandOperation operation;

        private DataTableCommand(CommandOperation operation)
        {
            this.operation = operation;
        }

        public CommandOperation Operation
        {
            get
            {
                return this.operation;
            }
        }

        internal static DataTableCommand Create(TConnection connection, CommandOperation operation)
        {
            var cm = new DataTableCommand(operation);
            cm.Connection = connection;
            return cm;
        }
    }
}