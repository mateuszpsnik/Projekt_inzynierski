using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediumForMusicians.Data
{
    public class ReportDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ImgFilename { get; set; }
        public string Justification { get; set; }
        public DateTime SentAt { get; set; }
    }
}
