using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Techne.Data
{
    public abstract class TDataSet : DataSet
    {
        private const bool CaseSensitive_Def = true;

        public TDataSet(string cultura)
        {
            this.CaseSensitive = CaseSensitive_Def;
        }

        public TDataSet()
        {
            this.CaseSensitive = CaseSensitive_Def;
        }

        // Esta propriedade foi criada somente para substituir o DefaultValue da original
        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new bool CaseSensitive
        {
            get
            {
                return base.CaseSensitive;
            }

            set
            {
                base.CaseSensitive = value;
            }
        }

        [
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new CultureInfo Locale
        {
            get
            {
                return base.Locale;
            }

            set
            {
                base.Locale = value;
            }
        }

        [
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new DataTableCollection Tables
        {
            get
            {
                return base.Tables;
            }
        }

        public abstract TConnection CreateConnection();

        public abstract TConnectionWritable CreateWritableConnection(TPermission permission);
    }
}