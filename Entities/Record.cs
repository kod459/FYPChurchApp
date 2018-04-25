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

        [Required(ErrorMessage = "Name on Record is required")]
        public string NameOnRecord { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        public string DocumentType { get; set; }

        [Required(ErrorMessage = "Upload Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UploadDate { get; set; }

        public byte[] Document { get; set; }


        public string UploadedBy { get; set; }
    }
}