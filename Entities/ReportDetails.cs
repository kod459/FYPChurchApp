using System;
using System.ComponentModel.DataAnnotations;


namespace PIMS.Entities
{

    public class ReportDetails
    {
        public string SerialNumber { get; set; }

        public string AreaLocation { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string TotalUsage { get; set; }

        public string TotalPrice { get; set; }

        public string TotalPriceWithVat { get; set; }

        public float RunningTotalOfPrice { get; set; }

        public float RunningTotalOfUsage { get; set; }

        public float RunningTotalOfVat { get; set; }


    }
}
