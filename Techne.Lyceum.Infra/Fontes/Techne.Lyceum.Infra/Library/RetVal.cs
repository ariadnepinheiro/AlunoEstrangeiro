using System;
using Techne.Library;

namespace Techne
{
    [Serializable]
    public class RetVal
    {
        private readonly ErrorList _errors;

        private readonly string message;

        private readonly bool success;

        public RetVal(bool success, string message, ErrorList errors)
        {
            if (message == null)
            {
                this.message = string.Empty;
            }
            else
            {
                this.message = message.Trim();
            }

            this.success = success;

            if (errors == null)
            {
                this._errors = new ErrorList();
            }
            else
            {
                this._errors = (ErrorList)((ICloneable)errors).Clone();
            }
        }

        public RetVal(bool success, string message) : this(success, message, null)
        {
        }

        public string Error
        {
            get
            {
                if (!this.success)
                {
                    return this.message;
                }

                return string.Empty;
            }
        }

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

        public static RetVal Success(string message)
        {
            return new RetVal(true, message);
        }

        public static implicit operator RetVal(string error)
        {
            return new RetVal(error == null || error.Trim().Length == 0, error);
        }
    }

    internal class RetNumber : RetVal
    {
        private readonly Number value;

        protected RetNumber(bool success, string message, Number value) : base(success, message)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        public Number Value
        {
            get
            {
                if (!this.Ok)
                {
                    return DBNull.Value;
                }

                return this.value;
            }
        }

        public static RetNumber Success(Number value, string message)
        {
            return new RetNumber(true, message, value);
        }

        public static implicit operator RetNumber(Number value)
        {
            return new RetNumber(true, string.Empty, value);
        }

        public static implicit operator RetNumber(decimal value)
        {
            return new RetNumber(true, string.Empty, value);
        }

        public static implicit operator RetNumber(int value)
        {
            return new RetNumber(true, string.Empty, value);
        }

        public static implicit operator RetNumber(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um Number em caso de sucesso.");
            }

            return new RetNumber(false, error, DBNull.Value);
        }
    }

    internal class RetVarChar : RetVal
    {
        private readonly VarChar value;

        protected RetVarChar(bool success, string message, VarChar value) : base(success, message)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        public VarChar Value
        {
            get
            {
                if (!this.Ok)
                {
                    return DBNull.Value;
                }

                return this.value;
            }
        }

        public static RetVarChar Success(VarChar value, string message)
        {
            return new RetVarChar(true, message, value);
        }

        public static implicit operator RetVarChar(VarChar value)
        {
            return new RetVarChar(true, string.Empty, value);
        }

        public static implicit operator RetVarChar(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um VarChar em caso de sucesso.");
            }

            return new RetVarChar(false, error, DBNull.Value);
        }
    }

    internal class RetDate : RetVal
    {
        private readonly Date value;

        protected RetDate(bool success, string message, Date value) : base(success, message)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        public Date Value
        {
            get
            {
                if (!this.Ok)
                {
                    return DBNull.Value;
                }

                return this.value;
            }
        }

        public static RetDate Success(Date value, string message)
        {
            return new RetDate(true, message, value);
        }

        public static implicit operator RetDate(Date value)
        {
            return new RetDate(true, string.Empty, value);
        }

        public static implicit operator RetDate(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um Date em caso de sucesso.");
            }

            return new RetDate(false, error, DBNull.Value);
        }
    }

    internal class RetBool : RetVal
    {
        private readonly bool value;

        protected RetBool(bool success, string message, bool value) : base(success, message)
        {
            this.value = value;
        }

        public bool Value
        {
            get
            {
                if (!this.Ok)
                {
                    return false;
                }

                return this.value;
            }
        }

        public static RetBool Success(bool value, string message)
        {
            return new RetBool(true, message, value);
        }

        public static implicit operator RetBool(bool value)
        {
            return new RetBool(true, string.Empty, value);
        }

        public static implicit operator RetBool(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um bool em caso de sucesso.");
            }

            return new RetBool(false, error, false);
        }
    }

    internal class RetRaw : RetVal
    {
        private readonly Raw value;

        protected RetRaw(bool success, string message, Raw value) : base(success, message)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            this.value = value;
        }

        public Raw Value
        {
            get
            {
                if (!this.Ok)
                {
                    return DBNull.Value;
                }

                return this.value;
            }
        }

        public static RetRaw Success(Raw value, string message)
        {
            return new RetRaw(true, message, value);
        }

        public static implicit operator RetRaw(Raw value)
        {
            return new RetRaw(true, string.Empty, value);
        }

        public static implicit operator RetRaw(string error)
        {
            if (error == null || error.Trim().Length == 0)
            {
                throw new ArgumentException("Deve-se informar uma mensagem de erro não vazia, ou um Raw em caso de sucesso.");
            }

            return new RetRaw(false, error, DBNull.Value);
        }
    }

    internal class Ret<T> : RetVal
    {
        private readonly T value;

        protected Ret(bool success, string message, T value) : base(success, message)
        {
            this.value = value;
        }

        public T Value
        {
            get
            {
                if (!this.Ok)
                {
                    return default(T);
                }

                return this.value;
            }
        }

        public static Ret<T> Success(T value, string message)
        {
            return new Ret<T>(true, message, value);
        }

        public static implicit operator Ret<T>(T value)
        {
            return new Ret<T>(true, string.Empty, value);
        }
    }
}