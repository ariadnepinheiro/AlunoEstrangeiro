using System;
using Techne.Library;

namespace Techne.Lyceum.RN
{

    public class RetValue
    {
        // Fields
        private string message;
        private bool success;
        private ErrorList _errors = null;


        // Methods
        public RetValue(bool success, string message, ErrorList errors)
        {
            if (message == null)
            {
                this.message = string.Empty;
            }
            else
            {
                this.message = message.Trim();
            }

            if (errors == null)
                this._errors = new ErrorList();
            else
                this._errors = errors;

            this.success = success;
        }

        public static RetValue Success(string message)
        {
            return new RetValue(true, message, null);
        }

        // Properties
        public ErrorList Errors
        {
            get
            {
                return this._errors;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public bool Ok
        {
            get
            {
                return this.success;
            }
        }
    }
    public class RetValue<T> : RetValue
    {

        private T _value;

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        // Methods
        public RetValue(bool success, string message, ErrorList errors)
            : base(success, message, errors)
        {

        }

        public new static RetValue<T> Success(string message)
        {
            return new RetValue<T>(true, message, null);
        }
    }
}