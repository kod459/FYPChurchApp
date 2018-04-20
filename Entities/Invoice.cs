using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMS.Entities
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public byte[] PictureOfInvoice { get; set; }

        public string ImageFileName { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateReceived { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        public virtual Church Church { get; set; }
        
        public virtual Administration Admins { get; set; }
    }
}