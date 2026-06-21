using System.ComponentModel;
using System.Web.UI;
using Techne.Library.Sql.Structure;

namespace Techne.Controls
{
    [ControlValueProperty("DBValue")]
    internal class TDropDownList : TDropDownListBase
    {
        [
            Category("Techne"), 
            Description("Permite elaborar descrição mais elaborada utilizando os valores das colunas especificadas " +
                        "na propriedade SqlSelect, substituindo a descrição fornecida pela coluna indicada na " +
                        "propriedade DataTextField."), 
        ]
        public event SetItemDescriptionEventHandler SetItemDescription;

        [
            Category("Techne"), 
        ]
        public event SetSqlClausesEventHandler SetSqlClauses;

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description(
                "Indica a coluna da propriedade SqlSelect que será mostrada na lista do controle. " +
                "Caso não seja informada e houver mais de uma coluna, a segunda coluna será utilizada. " +
                "Descrições mais elaboradas podem ser construídas utilizando-se o evento SetItemDescription."
                )
        ]
        public string DataTextField
        {
            get
            {
                return base.BaseDataTextField;
            }

            set
            {
                base.BaseDataTextField = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Indica a coluna da propriedade SqlSelect que será devolvida pela propriedade DBValue. Caso não seja informada, a primeira coluna será utilizada.")
        ]
        public string DataValueField
        {
            get
            {
                return base.BaseDataValueField;
            }

            set
            {
                base.BaseDataValueField = value;
            }
        }

        [
            DefaultValue(NotFoundText_Def), 
            Category("Techne - Messages"), 
            Description("Texto a ser usado quando o número de ítens do DropDownList é zero")
        ]
        public string NotFoundText
        {
            get
            {
                return base.BaseNotFoundText;
            }

            set
            {
                base.BaseNotFoundText = value;
            }
        }

        [
            DefaultValue(NullText_Def), 
            Category("Techne - Messages"), 
            Description("Texto a ser usado como opção para indicar o valor DBNull")
        ]
        public string NullText
        {
            get
            {
                return base.BaseNullText;
            }

            set
            {
                base.BaseNullText = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Cláusula where para restringir o select que preenche o controle.")
        ]
        public string SQLOrder
        {
            get
            {
                return base.BaseSqlOrder;
            }

            set
            {
                base.BaseSqlOrder = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Cláusula where para restringir o select que preenche o controle.")
        ]
        public string SQLWhere
        {
            get
            {
                return base.BaseSqlWhere;
            }

            set
            {
                base.BaseSqlWhere = value;
            }
        }

        [
            DefaultValue(SelectAllText_Def), 
            Category("Techne - Messages"), 
            Description("Texto a ser usado como opção para selecionar todos os itens da lista")
        ]
        public string SelectAllText
        {
            get
            {
                return base.BaseSelectAllText;
            }

            set
            {
                base.BaseSelectAllText = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(null), 
            Description("Select SQL para popular a lista de itens")
        ]
        public SqlSelect SqlSelect
        {
            get
            {
                return base.BaseSqlSelect;
            }

            set
            {
                base.BaseSqlSelect = value;
            }
        }

        protected override void OnSetItemDescription(SetItemDescriptionEventArgs args)
        {
            if (this.SetItemDescription != null)
            {
                this.SetItemDescription(this, args);
            }
        }

        protected override void OnSetSqlClauses(SetSqlClausesEventArgs args)
        {
            if (this.SetSqlClauses != null)
            {
                this.SetSqlClauses(this, args);
            }
        }
    }
}