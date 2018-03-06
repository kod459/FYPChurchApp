using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Ceremony
    {
        [Key]
        public int CeremonyId {get; set;}

        public string TypeOfCeremony { get; set; }

        public decimal CeremonyFee { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateOfCeremony { get; set; }

        

    }
}