using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Bulletins
    {
        [Key]
        public int BulletinsID { get; set; }

        [Required(ErrorMessage = "Bulletin is required")]
        [DataType(DataType.MultilineText)]
        public string DetailsOfBulletin { get; set; }

        public string AdminPosting { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateOfBulletin { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        public virtual Church Church { get; set; }

    }
}