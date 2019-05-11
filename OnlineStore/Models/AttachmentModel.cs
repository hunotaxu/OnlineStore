using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTL.Solution.Areas.Exams.Models
{
    public class AttachmentModel
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Contents { get; set; }
    }
}