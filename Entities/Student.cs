using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string MotherName {get; set; } //can be null

        public string FatherName { get; set; } //can be null

        public bool PermissionGranted { get; set; }


        [ForeignKey("Class")]
        public int ClassId { get; set; }

        //Foreign Key
        public virtual Class Class { get; set; }

    }
}