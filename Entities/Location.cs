using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PhotocopierInformationManagementServices.Entities
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public string CostCentre { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string Area { get; set; }

        public string OfficeType { get; set; }
    }
}