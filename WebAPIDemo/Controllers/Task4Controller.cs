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
    public class Task4Controller : ApiController
    {
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();

            //What planet in Star Wars universe provided largest number of vehicle pilots?
            string query = @"SELECT distinct 'Planet: ' + lpt.name + ' - Pilots: (' + cast(Count(lp.id) as varchar) + ') ' + 
                             stuff((SELECT ','+ convert(varchar,p.name + ' - '  + s.name) as PeopleSpecies 
                             FROM Planets pt inner join films_planets fp on pt.id = fp.planet_id
                             inner join films_vehicles fv on fv.film_id = fp.film_id
                             inner join vehicles_pilots vp on vp.vehicle_id = fv.vehicle_id
                             inner join People P on p.id = vp.people_id
                             inner join species_people sp on sp.people_id = p.id
                             inner join species s on s.id = sp.species_id
                             where lpt.name = pt.name
                             group by Pt.name,p.name,s.name
                                         FOR XML PATH(''), TYPE).value('.', 'varchar(max)'),1,1,'') AS PeopleAndSpecies
                             			FROM Planets lpt inner join films_planets fp on lpt.id = fp.planet_id
                             inner join films_vehicles fv on fv.film_id = fp.film_id
                             inner join vehicles_pilots vp on vp.vehicle_id = fv.vehicle_id
                             inner join People lP on lp.id = vp.people_id
                             inner join species_people sp on sp.people_id = lp.id
                             inner join species s on s.id = sp.species_id
                             group by lpt.name
                             having COUNT(lp.id)=(select max(X.PilotCount) as PilotCount from
                             (select   pt.name,count(p.id) as PilotCount from
                             People P right outer join vehicles_pilots vp on p.id=vp.people_id
                             inner join species_people sp on sp.people_id = p.id
                             inner join species s on s.id = sp.species_id
                             inner join films_vehicles fv on fv.vehicle_id = vp.vehicle_id 
                             inner join films_planets fp on fv.film_id = fp.film_id
                             inner join Planets pt on pt.id = fp.planet_id
                             group by pt.name) as X)";

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
