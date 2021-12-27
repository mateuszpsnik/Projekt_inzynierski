using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediumForMusicians.Data
{
    public class MeetingDTO
    {
        public string Id { get; set; }
        public string HostId { get; set; }
        public string GuestId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
        public bool Accepted { get; set; }
        public string HostName { get; set; }
        public string GuestName { get; set; }
        public string HostImgFilename { get; set; }
        public string GuestImgFilename { get; set; }
    }
}
