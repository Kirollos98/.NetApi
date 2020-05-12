using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Models;
using ApiProject.Modelviews;

namespace ApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ApplicationDb _context;

        public TripsController(ApplicationDb db)
        {
            _context = db;           
        }


        [HttpGet]
        [Route("ListCities")]
        public async Task<ActionResult<IEnumerable<Cities>>> ListCities()
        {
            return await _context.Cities.ToListAsync();
        }



        [HttpPost]
        [Route("Search")]
        public async Task<ActionResult<IEnumerable<Trips>>> Search(SearchTripsViewModel model)
        {

            TripsViewModel Tripsvm = new TripsViewModel()
            {
                Trips = new List<Models.Trips>()
            };
            
            Tripsvm.Trips = await _context.Trips.ToListAsync();

            if (model.FromId != null)
            {
                Tripsvm.Trips = Tripsvm.Trips.Where(x => x.CitiesFromId == model.FromId).ToList();
            }

           if (model.ToId != null)
            {
                Tripsvm.Trips = Tripsvm.Trips.Where(x => x.CitiesToId == model.ToId).ToList();

            }

            if (model.DepDate != null)
            {
                DateTime depdate = Convert.ToDateTime(model.DepDate);
                Tripsvm.Trips = Tripsvm.Trips.Where(a => a.DepDate.ToShortDateString().Equals(depdate.ToShortDateString())).ToList();

            }
           
            return Ok(Tripsvm);
        }




        // GET: api/Trips/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Trips>> Details(int id)
        {
            var trips = await _context.Trips.FindAsync(id);

            if (trips == null)
            {
                return NotFound();
            }

            return Ok(trips);
        }




        // PUT: api/Trips/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Trips trips)
        {
            if (id != trips.Id)
            {
                return BadRequest();
            }

            _context.Entry(trips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Trips
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("AddTrip")]
        public async Task<ActionResult<Trips>> AddTrip(Trips trips)
        {
            _context.Trips.Add(trips);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetTrips", new { id = trips.Id }, trips);
            return Ok("Added Successfully");
        }

        // DELETE: api/Trips/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<Trips>> DeleteTrips(int id)
        {
            var trips = await _context.Trips.FindAsync(id);
            if (trips == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(trips);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }

        private bool TripsExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
