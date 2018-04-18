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

        public string DetailsOfBulletin { get; set; }

        [ForeignKey("Admins")]
        public int AdministrationId { get; set; }

        public virtual Administration Admins { get; set; }

    }
}