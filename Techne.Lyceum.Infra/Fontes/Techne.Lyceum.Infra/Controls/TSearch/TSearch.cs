using System.ComponentModel;
using System.Web.UI;
using Techne.Library.Sql.Structure;

namespace Techne.Controls
{
    public delegate void SetViewModeDescriptionEventHandler(object sender, SetViewModeDescriptionEventArgs args);

    [ControlValueProperty("DBValue")]
    internal class TSearch : TSearchBase
    {
        [
            Category("Techne"), 
            Description("Permite consultar/alterar a descrição do valor do controle utilizada em modo View."), 
        ]
        public event SetViewModeDescriptionEventHandler SetViewModeDescription;

        [
            Category("Techne"), 
            DefaultValue(null), 
            Description("Caso o valor digitado no controle (associado à coluna Key) não seja encontrado, " +
                        "tenta buscar o valor em colunas alternativas relacionadas nesta propriedade."), 
            TypeConverter(typeof (StringArrayConverter)), 
        ]
        public string[] AlternateKeys
        {
            get
            {
                return base.BaseAlternateKeys;
            }

            set
            {
                base.BaseAlternateKeys = value == null ? new string[0] : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Coluna que será usada para a busca")
        ]
        public string Argument
        {
            get
            {
                return this.BaseArgument;
            }

            set
            {
                this.BaseArgument = value;
            }
        }

        [
            Category("Techne"), 
            PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TSearchColumnCollection GridColumns
        {
            get
            {
                return this.BaseGridColumns;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Nome da coluna que é a chave da tabela sobre a qual será feita a busca. Não suporta chaves compostas")
        ]
        public string Key
        {
            get
            {
                return this.BaseKey;
            }

            set
            {
                this.BaseKey = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue("")
        ]
        public string SqlOrder
        {
            get
            {
                return this.BaseSqlOrder;
            }

            set
            {
                this.BaseSqlOrder = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(null)
        ]
        public SqlSelect SqlSelect
        {
            get
            {
                return this.BaseSqlSelect;
            }

            set
            {
                this.BaseSqlSelect = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue("")
        ]
        public string SqlWhere
        {
            get
            {
                return this.BaseSqlWhere;
            }

            set
            {
                this.BaseSqlWhere = value;
            }
        }

        [
            Browsable(false), 
            DefaultValue(null), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public DbObject[] WhereValues
        {
            get
            {
                return this.BaseWhereValues;
            }

            set
            {
                this.BaseWhereValues = value;
            }
        }

        protected override void OnSetViewModeDescription(SetViewModeDescriptionEventArgs args)
        {
            if (this.SetViewModeDescription != null)
            {
                this.SetViewModeDescription(this, args);
            }
        }
    }
}