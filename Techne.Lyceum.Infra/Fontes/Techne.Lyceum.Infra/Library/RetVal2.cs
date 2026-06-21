using System;
using Techne.Data;

namespace Techne
{
    [Obsolete("Utilize RetSimpleRow no lugar de RetRow.")]
    internal class RetRow : RetSimpleRow
    {
        protected RetRow(bool success, string message, SimpleRow value) : base(success, message, value)
        {
        }

        public static implicit operator RetRow(SimpleRow value)
        {
            return new RetRow(true, string.Empty, value);
        }

        public static implicit operator RetRow(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um SimpleRow em caso de sucesso.");
            }

            return new RetRow(false, error, null);
        }
    }

    internal class RetSimpleRow : RetVal
    {
        private readonly SimpleRow value;

        protected RetSimpleRow(bool success, string message, SimpleRow value) : base(success, message)
        {
            this.value = value;
        }

        public SimpleRow Value
        {
            get
            {
                if (!this.Ok)
                {
                    return null;
                }

                return this.value;
            }
        }

        public static RetSimpleRow Success(SimpleRow value, string message)
        {
            return new RetSimpleRow(true, message, value);
        }

        public static implicit operator RetSimpleRow(SimpleRow value)
        {
            return new RetSimpleRow(true, string.Empty, value);
        }

        public static implicit operator RetSimpleRow(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um SimpleRow em caso de sucesso.");
            }

            return new RetSimpleRow(false, error, null);
        }
    }

    internal class RetDataRow : RetVal
    {
        private readonly TDataRow value;

        protected RetDataRow(bool success, string message, TDataRow value) : base(success, message)
        {
            this.value = value;
        }

        public TDataRow Value
        {
            get
            {
                if (!this.Ok)
                {
                    return null;
                }

                return this.value;
            }
        }

        public static RetDataRow Success(TDataRow value, string message)
        {
            return new RetDataRow(true, message, value);
        }

        public static implicit operator RetDataRow(TDataRow value)
        {
            return new RetDataRow(true, string.Empty, value);
        }

        public static implicit operator RetDataRow(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um TDataRow em caso de sucesso.");
            }

            return new RetDataRow(false, error, null);
        }
    }
}