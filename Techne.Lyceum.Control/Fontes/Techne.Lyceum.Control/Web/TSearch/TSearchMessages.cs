namespace Techne.Web
{
    public class TSearchMessages
    {
        private string _keyNotFound = "Código não encontrado";

        private string _queryFailure = "Ocorreu uma falha durante a busca dos registros. Execute a busca novamente.";

        private string _tooManyRows = "Foram encontrados mais de {0} registros. Mostrando os {0} primeiros.";

        public string KeyNotFound
        {
            get
            {
                return this._keyNotFound;
            }

            set
            {
                this._keyNotFound = value == null ? string.Empty : value;
            }
        }

        public string QueryFailure
        {
            get
            {
                return this._queryFailure;
            }

            set
            {
                this._queryFailure = value == null ? string.Empty : value;
            }
        }

        public string TooManyRows
        {
            get
            {
                return this._tooManyRows;
            }

            set
            {
                this._tooManyRows = value == null ? string.Empty : value;
            }
        }
    }
}