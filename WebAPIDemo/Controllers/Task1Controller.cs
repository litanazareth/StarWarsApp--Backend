using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebAPIDemo.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebAPIDemo.Controllers
{
    public class Task1Controller : ApiController
    {
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();

            //Which of all Star Wars movies has the longest opening crawl (counted by number          of characters)
            string query = @"select max(LEN(opening_crawl)) as LongestOpeningCrawl from films";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["StarWarsAppDb"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(dt);
            }
             
            return Request.CreateResponse(HttpStatusCode.OK,dt);
        }

    }
}
