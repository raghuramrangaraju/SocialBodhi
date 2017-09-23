using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialbodhi.Models
{
    public class poll
    {
        public long ParticipantsId { get; set; }
        public Nullable<long> InstanceId { get; set; }
        public string ParticipantsName { get; set; }
        public string Participantsemail { get; set; }
        public List<Participant> participant { get; set; }

        
    }
}