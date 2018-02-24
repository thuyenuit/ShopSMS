using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Common.Common
{
    public class ListObject
    {
        public List<ListStatus> lstStatus = new List<ListStatus>();
    }

    public class ListStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }

    public class ListGroupMenu
    {
        public string MenuGroupName { get; set; }
        public List<ListMenu> ListMenu { get; set; }
    }

    public class ListMenu
    {
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public int? OrderBy { get; set; }
    }

    public class Items
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
