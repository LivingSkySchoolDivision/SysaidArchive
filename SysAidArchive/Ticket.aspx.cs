using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SysAidArchive
{

    public partial class Ticket : System.Web.UI.Page
    {
        private string formatLineBreaks(string input)
        {
            return input.Replace("\n", "<br>");
        }

        private string buildTicketInfoBox(SysAidTicket ticket)
        {
            StringBuilder content = new StringBuilder();

            content.Append("<h1>" + ticket.ID + ": " + ticket.Title + "</h1>");
            content.Append("<br><b>Submitted on: </b>" + ticket.WhenCreated.ToLongDateString() + " " + ticket.WhenCreated.ToShortTimeString());
            content.Append("<br><b>Created by: </b>" + ticket.Submitter);
            content.Append("<br><b>Reported by: </b>" + ticket.Reporter);
            content.Append("<br><b>Urgency: </b>" + ticket.Urgency);
            content.Append("<br><b>Priority: </b>" + ticket.Priority);
            content.Append("<br><b>Category: </b>" + ticket.ProblemType);
            content.Append("<br><b>Subcategory: </b>" + ticket.ProblemSubType);
            content.Append("<br><b>Computer ID: </b>" + ticket.ComputerID);
            content.Append("<br><b>Assignee: </b>" + ticket.Assignee);


            content.Append("<h3>Description</h3><p class=\"description_block\">" + formatLineBreaks(ticket.Description) + "</p>");
            content.Append("<h3>Notes</h3><p class=\"description_block\">" + formatLineBreaks(ticket.Notes) + "</pre></p>");
            content.Append("<h3>Resolution</h3><p class=\"description_block\">" + formatLineBreaks(ticket.Resolution) + "</p>");
            content.Append("<h3>Solution</h3><p class=\"description_block\">" + formatLineBreaks(ticket.Solution) + "</p>");
            
            content.Append("<br><b>Last modified: </b>" + ticket.WhenModified.ToString() + " by " + ticket.LastUpdatedBy);
            content.Append("<br><b>Resolved: </b>" + ticket.WhenClosed);

            return content.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the ticket ID from the querystring

            string ticketID_String = Request.QueryString["id"];

            if (!string.IsNullOrEmpty(ticketID_String))
            {
                int ticketID = Parsers.ParseInt(ticketID_String);

                if (ticketID > 0)
                {
                    SysAidTicketRepository repository = new SysAidTicketRepository();
                    SysAidTicket ticket = repository.Get(ticketID);
                    if (ticket != null)
                    {
                        litTicketInfo.Text = buildTicketInfoBox(ticket);
                    }
                    else
                    {
                        litTicketInfo.Text = "Ticket is null for some reason";
                    }
                }
                else
                {
                    litTicketInfo.Text = "Invalid ticket specified: " + ticketID;
                }
            }
            else
            {
                litTicketInfo.Text = "No ticket specified";
            }
        }

    }
}