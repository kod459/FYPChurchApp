using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMS.ReportModels
{
    public class LastReadingReportModel
    {
        public int MeterReadingId { get; set; }

        public int PhotocopierId { get; set; }

        public string SerialNumber { get; set; }

        public DateTime DateOfReading { get; set; }

        public int Reading { get; set; }
    }
}