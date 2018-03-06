using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        public string Id;

        public string Name { get; set; }
        
        public string DocumentType { get; set; }

        public long? Size { get; set; }

        public long? Version { get; set; }

        public DateTime? UploadDate { get; set; }

        //public byte[] Document { get; set; }

        [ForeignKey("Admins")]
        public int AdministrationId { get; set; }

        public virtual Administration Admins { get; set; }
    }
}