using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Techne.Web
{
    public class TSearchSettings
    {
        private readonly TSearchColumnCollection _columns = new TSearchColumnCollection();

        private readonly TSearchMessages _messages = new TSearchMessages();

        private readonly TSearchParameterCollection _parameters = new TSearchParameterCollection();

        private string _descriptionField = string.Empty;

        private Unit _gridWidth = Unit.Empty;

        private string _mask = string.Empty;

        private int _maxLength = -1;

        private int _maxRows = 200;

        private string _queryClass = string.Empty;

        private string _queryMethod = string.Empty;

        private string _textField = string.Empty;

        private TSearchDataType _textFieldType = TSearchDataType.String;

        private string _valueField = string.Empty;

        private TSearchDataType _valueFieldType = TSearchDataType.String;

        public string DescriptionField
        {
            get
            {
                return this._descriptionField;
            }

            set
            {
                this._descriptionField = value == null ? string.Empty : value;
            }
        }

        public TSearchColumnCollection GridColumns
        {
            get
            {
                return this._columns;
            }
        }

        public TSearchParameterCollection GridFilterParameters
        {
            get
            {
                return this._parameters;
            }
        }

        public Unit GridWidth
        {
            get
            {
                return this._gridWidth;
            }

            set
            {
                this._gridWidth = value;
            }
        }

        [Description("Caracteres especias: a - Alfabético (A-Z,a-z), 9 - Numérico (0-9), * - Alfanumérico (A-Z,a-z,0-9)")]
        public string Mask
        {
            get
            {
                return this._mask;
            }

            set
            {
                this._mask = value == null ? string.Empty : value;
            }
        }

        public int MaxLength
        {
            get
            {
                if (this._maxLength > -1)
                {
                    return this._maxLength;
                }
                else if (this.GridFilterParameters[this.TextField] != null)
                {
                    return this.GridFilterParameters[this.TextField].MaxLength;
                }
                else
                {
                    return 0;
                }
            }

            set
            {
                this._maxLength = value < 0 ? 0 : value;
            }
        }

        public int MaxRows
        {
            get
            {
                return this._maxRows;
            }

            set
            {
                this._maxRows = value <= 0 ? 0 : value;
            }
        }

        public TSearchMessages Messages
        {
            get
            {
                return this._messages;
            }
        }

        public string QueryMethod
        {
            get
            {
                return this._queryMethod;
            }

            set
            {
                this._queryMethod = value == null ? string.Empty : value;
            }
        }

        public string QueryTypeName
        {
            get
            {
                return this._queryClass;
            }

            set
            {
                this._queryClass = value == null ? string.Empty : value;
            }
        }

        public string TextField
        {
            get
            {
                return this._textField;
            }

            set
            {
                this._textField = value == null ? string.Empty : value;
            }
        }

        public TSearchDataType TextFieldType
        {
            get
            {
                if (this._textFieldType != TSearchDataType.NotSet)
                {
                    return this._textFieldType;
                }
                else if (this.GridFilterParameters[this.TextField] != null)
                {
                    return this.GridFilterParameters[this.TextField].ParameterType;
                }
                else
                {
                    return TSearchDataType.NotSet;
                }
            }

            set
            {
                this._textFieldType = value;
            }
        }

        public string ValueField
        {
            get
            {
                return this._valueField;
            }

            set
            {
                this._valueField = value == null ? string.Empty : value;
            }
        }

        public TSearchDataType ValueFieldType
        {
            get
            {
                if (this._valueFieldType != TSearchDataType.NotSet)
                {
                    return this._valueFieldType;
                }
                else if (this.GridFilterParameters[this.ValueField] != null)
                {
                    return this.GridFilterParameters[this.ValueField].ParameterType;
                }
                else
                {
                    return TSearchDataType.String;
                }
            }

            set
            {
                this._valueFieldType = value;
            }
        }
    }
}