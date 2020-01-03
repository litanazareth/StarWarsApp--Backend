using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    public class Task3Controller : ApiController
    {
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();

            //What species (i.e. characters that belong to certain species) appeared in the most            number of Star Wars films ?
                        string query = @"select X.Name + ' (' + cast(MostAppearedSpeciesinFilms as varchar) + ')' as MostAppearedSpeciesinFilms from
                                        (select S.name,(count(people_id)) as MostAppearedSpeciesinFilms 
                                        from species_people sp left outer join Species S on S.id=sp.species_id
                                        group by S.name) as X
                                        group by X.Name,MostAppearedSpeciesinFilms
                                        having MostAppearedSpeciesinFilms = (select MAX(X.MostAppearedSpeciesinFilms) as MostAppearedSpeciesinFilms from (
                                        select species_id,(count(people_id)) as MostAppearedSpeciesinFilms from species_people group by species_id) as X)
                                        ";

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
