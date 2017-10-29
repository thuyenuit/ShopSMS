using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("ErrorLogs")]
    public class ErrorLog
    {
        [Key]
        public int ErrorLogID { get; set; }

        public string ErrorLogMessage { get; set; }

        public string ErrorLogStackTrace { get; set; }

        public DateTime ErrorLogCreateDate { get; set; }
    }
}
