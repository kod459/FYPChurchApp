using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Address Line 2 is required")]
        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

    }
}