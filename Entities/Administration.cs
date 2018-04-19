using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Administration
    {
        [Key]
        public int AdministrationId { get; set; }

        public string AdministratorName { get; set; }

        public string AdminUsername { get; set; }
        
        public string Position { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        [ForeignKey("AdminChurch")]
        public int ChurchId { get; set; }

        //For FK I think
        public virtual Church AdminChurch { get; set; }

    }
}