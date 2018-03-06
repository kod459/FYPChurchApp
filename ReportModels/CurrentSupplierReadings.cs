using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIMS.ReportModels
{
    public class CurrentSupplierReadings
    {
        public string Supplier { get; set; }

        [Display(Name = "Total Current Readings")]
        public int TotalCurrentReadings { get; set; }
    }
}