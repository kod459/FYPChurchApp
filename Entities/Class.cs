using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        public string TypeOfClass { get; set; }

        public string TeacherName { get; set; }

        [ForeignKey("School")]
        public int SchoolId { get; set; }

        //Foregin Key
        public virtual School School { get; set; }
    }
}