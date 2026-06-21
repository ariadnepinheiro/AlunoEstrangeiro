using System;
using System.Data;

namespace Techne.Data
{
    public delegate void TDataRowUpdateEventHandler(object sender, TDataRowUpdateEventArgs args);

    public class TDataRowUpdateEventArgs
    {
        private readonly TCommand command;

        // A propriedade Row não pode ser TDataRow porque o TDataAdapter pode ser utilizado
        // em datasets que não são do tipo TechneDS (é o caso do controle MxNAddRemove).
        private readonly CommandOperation commandOperation;

        private readonly DataRow row;

        public TDataRowUpdateEventArgs(TCommand command, 
                                       DataRow row, 
                                       Exception errors, 
                                       UpdateStatus status, 
                                       StatementType statementType) : this(command, row, errors, status, CommandOperation.Select)
        {
            switch (statementType)
            {
                case StatementType.Select:
                    return;
                case StatementType.Insert:
                    this.commandOperation = CommandOperation.Insert;
                    return;
                case StatementType.Update:
                    this.commandOperation = CommandOperation.Update;
                    return;
                case StatementType.Delete:
                    this.commandOperation = CommandOperation.Delete;
                    return;
            }

            throw new NotImplementedException();
        }

        public TDataRowUpdateEventArgs(TCommand command, 
                                       DataRow row, 
                                       Exception errors, 
                                       UpdateStatus status, 
                                       CommandOperation commandOperation)
        {
            this.command = command;
            this.row = row;
            this.Errors = errors;
            this.Status = status;
            this.commandOperation = commandOperation;
        }

        public TCommand Command
        {
            get
            {
                return this.command;
            }
        }

        public CommandOperation CommandOperation
        {
            get
            {
                return this.commandOperation;
            }
        }

        public Exception Errors { get; set; }

        public DataRow Row
        {
            get
            {
                return this.row;
            }
        }

        public UpdateStatus Status { get; set; }
    }
}