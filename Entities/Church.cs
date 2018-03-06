using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Church
    {
        [Key]
        public int ChurchId { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Parish { get; set; }
    }
}