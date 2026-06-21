using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class Paging<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public int TotalItems { get; set; }
        public IList<T> Items { get; set; }
		public string Title { get; set; }
    }
}