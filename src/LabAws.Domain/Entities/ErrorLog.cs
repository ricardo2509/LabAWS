using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Domain.Entities
{
    [Table("error_logs")]
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public DateTime ExceptionDateTime { get; set; }
    }
}
