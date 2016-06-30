using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SysAidArchive
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        TableRow addSearchResultRow(SysAidTicketSearchResult result)
        {
            TableRow returnMe = new TableRow();
            
            returnMe.Cells.Add(new TableCell()
            {
                Text = "<div style=\"\">" +
                       "<div style=\"font-size: 12pt; font-weight: bold;margin-bottom: 2px;\"><a href=\"Ticket.aspx?id=" + result.Ticket.ID + "\">" + result.Ticket.ID.ToString().PadLeft(5, '0') + ": " + result.Ticket.Title + "</a></div>" +
                       "<div><b>Score: </b>" + result.SearchScore + ", <b>Hits:</b> " + result.FieldHits.ToCommaSeparatedString() + "</div>" +
                       "<div><b>Created: </b>" + result.Ticket.WhenCreated.ToLongDateString() + " " + result.Ticket.WhenCreated.ToShortTimeString() + " by <b>" + result.Ticket.Reporter + "</b></div>" +
                       "<div><b>Type: </b>" + result.Ticket.ProblemType + " / " + result.Ticket.ProblemSubType + "</div>" +
                       "<div><b>Asset: </b>" + result.Ticket.ComputerID + "</div>" +
                       "<br/><div style=\"font-size: 8pt;\">" + result.Ticket.Description + "</div>" +
                       "</div>"
            });

            return returnMe;
        }


        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            SysAidTicketRepository repository = new SysAidTicketRepository();

            List<SysAidTicketSearchResult> results = repository.Find(txtSearchTerms.Text.Trim());

            lblResults.Text = "<div style=\"font-size: 8pt;\">Found: " + results.Count + "</div>";

            tblResults.Rows.Clear();
            foreach (SysAidTicketSearchResult result in results)
            {
                tblResults.Rows.Add(addSearchResultRow(result));
            }

        }
    }
}