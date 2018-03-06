using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        [ForeignKey("Church")]
        public int ChurchId { get; set; }


        public virtual Church Church { get; set; }
        
        public virtual Administration Admins { get; set; }
    }
}