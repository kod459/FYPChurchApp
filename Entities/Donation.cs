using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        public string TypeOfDonation { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        [ForeignKey("House")]
        public int HouseId { get; set; }

        public string AddressLine1 { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateRecieved { get; set; }

        public virtual House House { get; set; }
        public virtual Church Church { get; set; }

    }
}