using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysAidArchive
{
    public class SysAidTicket
    {
        public int ID { get; set; }
        public string ComputerID { get; set; }
        public string ProblemType { get; set; }
        public string ProblemSubType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusID { get; set; }
        public string Assignee { get; set; }
        public string Reporter { get; set; }
        public string Submitter { get; set; }
        public int Urgency { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public string Resolution { get; set; }
        public string Solution { get; set; }
        public string Location { get; set; }

        public DateTime WhenCreated { get; set; }
        public DateTime WhenModified { get; set; }
        public DateTime WhenClosed { get; set; }
        public string LastUpdatedBy { get; set; }
        
    }


}