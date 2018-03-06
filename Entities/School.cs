using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        public string SchoolAddress { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }
        
        //Foreign Key
        public virtual Church Church { get; set; }
    }
}