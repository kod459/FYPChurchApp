using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        public string NameOnRecord { get; set; }
        
        public string DocumentType { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UploadDate { get; set; }

        public byte[] Document { get; set; }

        public string UploadedBy { get; set; }
    }
}