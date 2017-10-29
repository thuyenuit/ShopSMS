using System.Collections.Generic;
using System.Linq;

namespace ShopSMS.Web.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int Page { get; set; }
        public int Count
        {
            get { return Items != null ? Items.Count() : 0; }
        }

        //public int ShowFrom { get; set; }
        // public int ShowTo { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
        public string StrDate { get; set; }
        public string StrHour { get; set; }
        public string StrUser { get; set; }
    }
}