using System.ComponentModel;

namespace Techne.Web
{
    public class TSearchClientSideEvents
    {
        private string _valueChanged = string.Empty;

        [Localizable(false), DefaultValue(""), NotifyParentProperty(true)]
        public string ValueChanged
        {
            get
            {
                return this._valueChanged;
            }

            set
            {
                this._valueChanged = value == null ? string.Empty : value;
            }
        }
    }
}