using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebAPIDemo.Controllers
{
    public class Task2Controller : ApiController
    {
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();

            //What character (person) appeared in most of the Star Wars films?
            string query = @"select p.name as MostAppearedCharacterinFilms from films_characters fc left outer join People P on P.id=fc.people_id
                            group by p.name having (count(film_id)) = (select MAX(X.MostAppearedCharacterinFilms) as MostAppearedCharacterinFilms from (
                            select people_id,(count(film_id)) as MostAppearedCharacterinFilms from films_characters group by people_id) as X)
                            order by MostAppearedCharacterinFilms desc";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["StarWarsAppDb"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(dt);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dt);

        }
    }
}
