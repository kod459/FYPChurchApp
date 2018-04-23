using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMS.Entities
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Picture of Invoice is required")]
        public byte[] PictureOfInvoice { get; set; }

        public string ImageFileName { get; set; }

        [Required(ErrorMessage = "Date Recieved is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateReceived { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        public virtual Church Church { get; set; }
        
        public virtual Administration Admins { get; set; }
    }
}