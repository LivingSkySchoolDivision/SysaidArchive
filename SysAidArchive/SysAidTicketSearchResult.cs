using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysAidArchive
{
    public class SysAidTicketSearchResult
    {
        public int SearchScore { get; set; }
        public List<string> FieldHits { get; set; }
        public SysAidTicket Ticket { get; set; }
        
        public SysAidTicketSearchResult(SysAidTicket ticket)
        {
            SearchScore = 0;
            FieldHits = new List<string>();
            Ticket = ticket;
        }
        
    }
}