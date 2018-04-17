using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMS.ViewModel
{
    public class ImageViewModel
    {
        public int InvoiceId { get; set; }
        public byte[] ImageByte { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}