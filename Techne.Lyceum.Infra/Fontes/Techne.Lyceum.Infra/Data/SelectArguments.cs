namespace Techne.Data
{
    public class PagingArguments
    {
        public int MaximumRows { get; set; }

        public bool RowCountOnly { get; set; }

        public int StartRowIndex { get; set; }
    }

    public class SelectArguments : PagingArguments
    {
        private string _sortExpression = string.Empty;

        public string SortExpression
        {
            get
            {
                return this._sortExpression;
            }

            set
            {
                this._sortExpression = value == null ? string.Empty : value;
            }
        }
    }
}