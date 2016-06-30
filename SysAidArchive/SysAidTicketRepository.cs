using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SysAidArchive
{
    public class SysAidTicketRepository
    {
        private const string SQLQuery = "SELECT id, computer_id, problem_type, problem_sub_type, title, description, status, contact, responsibility, urgency, priority, notes, resolution, solution, insert_time, update_time, close_time, update_user, submit_user, request_user, location FROM service_req";

        public SysAidTicketRepository()
        { 

        }
        
        private SysAidTicket sqlDataReaderToTicket(SqlDataReader dataReader)
        {
            return new SysAidTicket()
            {
                ID = Parsers.ParseInt(dataReader["id"].ToString().Trim()),
                ComputerID = dataReader["computer_id"].ToString().Trim(),
                ProblemSubType = dataReader["problem_sub_type"].ToString().Trim(),
                ProblemType = dataReader["problem_type"].ToString().Trim(),
                Title = dataReader["title"].ToString().Trim(),
                Description = dataReader["description"].ToString().Trim(),
                StatusID = Parsers.ParseInt(dataReader["status"].ToString().Trim()),
                Assignee = dataReader["responsibility"].ToString().Trim(),
                Reporter = dataReader["request_user"].ToString().Trim(),
                Submitter = dataReader["submit_user"].ToString().Trim(),
                Urgency = Parsers.ParseInt(dataReader["urgency"].ToString().Trim()),
                Priority = Parsers.ParseInt(dataReader["priority"].ToString().Trim()),
                Notes = dataReader["notes"].ToString().Trim(),
                Resolution = dataReader["resolution"].ToString().Trim(),
                Solution = dataReader["solution"].ToString().Trim(),
                Location = dataReader["location"].ToString().Trim(),
                WhenClosed = Parsers.ParseDate(dataReader["close_time"].ToString().Trim()),
                WhenCreated = Parsers.ParseDate(dataReader["insert_time"].ToString().Trim()),
                WhenModified = Parsers.ParseDate(dataReader["update_time"].ToString().Trim()),
                LastUpdatedBy = dataReader["update_user"].ToString().Trim()
            };
        }

        public List<SysAidTicketSearchResult> Find(string searchTerms)
        {
            string searchQuery = SQLQuery + " WHERE title LIKE @STERMS " +
                                 "OR description LIKE @STERMS " +
                                 "OR notes LIKE @STERMS " +
                                 "OR resolution LIKE @STERMS " +
                                 "OR solution LIKE @STERMS " +
                                 "OR computer_id LIKE @STERMS " +
                                 "OR request_user LIKE @STERMS";

            List <SysAidTicketSearchResult> returnMe = new List<SysAidTicketSearchResult>();

            // Split search terms
            List<string> terms = new List<string>();

            terms.Add(searchTerms.ToLower().Trim());

            foreach (string term in searchTerms.Split(new char[] {',', ';'}))
            {
                if (!string.IsNullOrEmpty(term))
                {
                    terms.Add(term.ToLower().Trim());
                }
            }

            List<int> foundTicketIDs = new List<int>();
            List<SysAidTicket> foundTickets = new List<SysAidTicket>();

            foreach (string term in terms)
            {
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = searchQuery;
                        sqlCommand.Parameters.AddWithValue("STERMS", "%" + term + "%");
                        sqlCommand.Connection.Open();
                        SqlDataReader dataReader = sqlCommand.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                int id = Parsers.ParseInt(dataReader["id"].ToString().Trim());

                                if (!foundTicketIDs.Contains(id))
                                {
                                    foundTickets.Add(sqlDataReaderToTicket(dataReader));
                                    foundTicketIDs.Add(id);
                                }
                            }
                        }
                        sqlCommand.Connection.Close();
                    }
                }
            }

            // Go through each search result and attribute a score to it
            foreach (SysAidTicket result in foundTickets)
            {
                SysAidTicketSearchResult searchResult = new SysAidTicketSearchResult(result);
                int score = 0;

                foreach (string term in terms)
                {
                    // Determine which fields the terms were found in
                    if (result.Title.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("title"))
                        {
                            searchResult.FieldHits.Add("title");
                        }

                        score += 100;

                    }

                    if (result.Description.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("description"))
                        {
                            searchResult.FieldHits.Add("description");
                        }

                        score += 10;

                    }

                    if (result.Notes.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("notes"))
                        {
                            searchResult.FieldHits.Add("notes");
                        }

                        score += 50;
                    }

                    if (result.Resolution.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("resolution"))
                        {
                            searchResult.FieldHits.Add("resolution");
                        }

                        score += 50;
                    }

                    if (result.Solution.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("solution"))
                        {
                            searchResult.FieldHits.Add("solution");
                        }

                        score += 50;
                    }


                    if (term.Length > 5)
                    {
                        if (result.ComputerID.ToLower().Contains(term))
                        {
                            if (!searchResult.FieldHits.Contains("computer_id"))
                            {
                                searchResult.FieldHits.Add("computer_id");
                            }

                            score += 1000;
                        }
                    }

                    if (result.Reporter.ToLower().Contains(term))
                    {
                        if (!searchResult.FieldHits.Contains("reporter"))
                        {
                            searchResult.FieldHits.Add("reporter");
                        }

                        score += 10;
                    }

                }

                searchResult.SearchScore = score;
                returnMe.Add(searchResult);
            }

            return returnMe.OrderByDescending(t => t.SearchScore).ThenByDescending(t => t.Ticket.WhenCreated).ToList();
        }

        public SysAidTicket Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQLQuery + " WHERE id=";
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            return sqlDataReaderToTicket(dataReader);
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            return null;
        }

        
    }
}