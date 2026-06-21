using System;
using System.Data;

namespace Techne.Data
{
    public abstract class TDataColumn : DataColumn
    {
        private string sqlServerType;

        /// <summary>
        ///   tabalias.coluna
        /// </summary>
        public abstract string FullCol { get; }

        public abstract bool IsAux { get; }

        public abstract string OriginalCol { get; }

        public abstract bool PrimaryKey { get; }

        public virtual object MaxValue
        {
            get
            {
                return null;
            }
        }

        public virtual object MinValue
        {
            get
            {
                return null;
            }
        }

        public virtual bool NotNull
        {
            get
            {
                return false;
            }
        }

        public virtual int Precision
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        ///   tabela.coluna
        /// </summary>
        public virtual string RefCol
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual int Scale
        {
            get
            {
                return -1;
            }
        }

        public virtual int Size
        {
            get
            {
                return 0;
            }
        }

        public virtual string SqlServerType
        {
            get
            {
                if (this.sqlServerType == null)
                {
                    if (this.DataType == typeof (string))
                    {
                        if (this.Size == 0)
                        {
                            this.sqlServerType = "TEXT";
                        }
                        else
                        {
                            this.sqlServerType = "VARCHAR(" + this.Size + ")";
                        }
                    }
                    else if (this.DataType == typeof (decimal))
                    {
                        this.sqlServerType = "NUMERIC(" + this.Precision +
                                             (this.Scale > 0 ? ", " + this.Scale : string.Empty) +
                                             ")";
                    }
                    else if (this.DataType == typeof (DateTime))
                    {
                        this.sqlServerType = "DATETIME";
                    }
                    else if (this.DataType == typeof (byte[]))
                    {
                        if (this.Size == 0)
                        {
                            this.sqlServerType = "IMAGE";
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }

                return this.sqlServerType;
            }
        }

        public string GetCaption(string idioma)
        {
            var caption = this.GetCaption(); // 1) Caption

            if (caption.Length == 0)
            {
                caption = this.GetName(); // 2) Names
                if (caption.Length == 0)
                {
                    caption = this.ColumnName; // 3) ColumnName
                }

                caption = StrLib.ToProper(caption);
            }

            return caption;
        }

        public string GetDescricao(string idioma)
        {
            var description = this.GetDescricao(); // 1) Descricao
            if (description.Length == 0)
            {
                description = this.GetName(); // 2) Names
            }

            if (description.Length == 0)
            {
                description = this.GetCaption(); // 3) Caption
            }

            if (description.Length == 0)
            {
                description = StrLib.ToProper(this.ColumnName); // 4) ColumnName
            }

            return description;
        }

        protected internal abstract string GetCaption();

        protected internal abstract string GetDescricao();

        protected internal abstract string GetName();
    }
}