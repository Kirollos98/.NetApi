using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Models;
using Microsoft.AspNetCore.Authorization;
using ApiProject.Modelviews;

namespace ApiProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        private readonly ApplicationDb _context;


        [BindProperty]
        public ComplainsViewModel ComplainsVM { get; set; }
        public ComplainsController(ApplicationDb context)
        {
            _context = context;
            ComplainsVM = new ComplainsViewModel()
            {
                Users = _context.Users.ToList(),
                Bookings = _context.Bookings.ToList(),
                Complains = new Models.Complains()
            };

        }

        // GET: api/Complains
        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult<IEnumerable<Complains>>> Index()
        {
            var Complains = _context.Complains.Include(m => m.ApplicationUser.UserName).Include(m => m.Bookings.TripId);
            return await _context.Complains.ToListAsync();
        }

        // GET: api/Complains/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Complains>> Details(int id)
        {
            var complains = await _context.Complains.FindAsync(id);

            if (complains == null)
            {
                return NotFound();
            }

            return Ok(new
            {
               username = complains.ApplicationUser.UserName,
               tripId = complains.Bookings.TripId,
                complain = complains.Complain
            });
        }

        /*  // PUT: api/Complains/5
          // To protect from overposting attacks, please enable the specific properties you want to bind to, for
          // more details see https://aka.ms/RazorPagesCRUD.
          [HttpPut("{id}")]
          public async Task<IActionResult> PutComplains(int id, Complains complains)
          {
              if (id != complains.Id)
              {
                  return BadRequest();
              }

              _context.Entry(complains).State = EntityState.Modified;

              try
              {
                  await _context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException)
              {
                  if (!ComplainsExists(id))
                  {
                      return NotFound();
                  }
                  else
                  {
                      throw;
                  }
              }

              return NoContent();
          }*/

        // POST: api/Complains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.


        [HttpPost]
        [Route("Addcomplain")]
        public async Task<ActionResult<Complains>> Addcomplain(Complains complains)
        {
            _context.Complains.Add(complains);
            await _context.SaveChangesAsync();

            return Ok("Added Successfully");
        }

        /* // DELETE: api/Complains/5
         [HttpDelete("{id}")]
         public async Task<ActionResult<Complains>> DeleteComplains(int id)
         {
             var complains = await _context.Complains.FindAsync(id);
             if (complains == null)
             {
                 return NotFound();
             }

             _context.Complains.Remove(complains);
             await _context.SaveChangesAsync();

             return Ok("Deleted Successfully");
         }*/

        private bool ComplainsExists(int id)
        {
            return _context.Complains.Any(e => e.Id == id);
        }
    }
}
