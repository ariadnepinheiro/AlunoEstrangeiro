using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Techne.Web
{
    public class TSearchParameter
    {
        private string _caption = string.Empty;

        private string _mask = string.Empty;

        private int _maxLength;

        private string _name = string.Empty;

        private TSearchDataType _parameterType = TSearchDataType.String;

        private bool _showInFilterPanel = true;
        
        private string _defaultValue = string.Empty;

        public TSearchParameter()
        {
        }

        public TSearchParameter(string name, string caption, TSearchDataType type)
        {
            this.ParameterName = name;
            this.ParameterType = type;
            this.Caption = caption;
        }

        public TSearchParameter(string name, string caption, TSearchDataType type, int maxLength)
        {
            this.ParameterName = name;
            this.ParameterType = type;
            this.Caption = caption;
            this.MaxLength = maxLength;
        }

        public TSearchParameter(string name, string caption, TSearchDataType type, int maxLength, bool showFilter)
        {
            this.ParameterName = name;
            this.ParameterType = type;
            this.Caption = caption;
            this.MaxLength = maxLength;
            this.ShowInFilterPanel = showFilter;
        }

        [Description("Título do parâmetro")]
        [NotifyParentProperty(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Caption
        {
            get
            {
                return this._caption;
            }

            set
            {
                this._caption = value == null ? string.Empty : value;
            }
        }

        [Description("Máscara. Caracteres especiais: a - Alfabético (A-Z,a-z), 9 - Numérico (0-9), * - Alfanumérico (A-Z,a-z,0-9)")]
        [NotifyParentProperty(true)]
        [Localizable(true)]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
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
                return this._maxLength;
            }

            set
            {
                this._maxLength = value < 0 ? 0 : value;
            }
        }
        
        public string DefaultValue 
        {
			get 
			{
				return this._defaultValue;		
			}
			
			set 
			{
				this._defaultValue = value;
			}
        }

        [Description("Nome que identifica o parâmetro para o método que executa a consulta")]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(false)]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public string ParameterName
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value == null ? string.Empty : value;
            }
        }

        [Description("Tipo do parâmetro")]
        [Category("Data")]
        [DefaultValue(TSearchDataType.String)]
        [Localizable(false)]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public TSearchDataType ParameterType
        {
            get
            {
                return this._parameterType;
            }

            set
            {
                this._parameterType = value;
            }
        }

        public bool ShowInFilterPanel
        {
            get
            {
                return this._showInFilterPanel;
            }

            set
            {
                this._showInFilterPanel = value;
            }
        }
    }

    public class TSearchParameterCollection : List<TSearchParameter>
    {
        public TSearchParameter this[string name]
        {
            get
            {
                for (var i = 0; i < this.Count; i++)
                {
                    if (this[i].ParameterName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this[i];
                    }
                }

                return null;
            }
        }

        public void Add(string name, string caption, TSearchDataType type)
        {
            this.Add(new TSearchParameter(name, caption, type));
        }

        public void Add(string name, string caption, TSearchDataType type, int maxLength)
        {
            this.Add(new TSearchParameter(name, caption, type, maxLength, true));
        }
    }

    public class TSearchParameterCollectionEditor : CollectionEditor
    {
        public TSearchParameterCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof (TSearchParameter);
        }
    }
}