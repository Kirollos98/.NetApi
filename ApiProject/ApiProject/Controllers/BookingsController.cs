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
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDb _context;


        [BindProperty]
        public BookingViewModel bookingvm { get; set; }
        public BookingsController(ApplicationDb context)
        {
            _context = context;

            bookingvm = new BookingViewModel()
            {
                CitiesTO = _context.Cities.ToList(),
                CityFrom = _context.Cities.ToList(),
                CompanyAssets = _context.CompanyAssets.ToList(),
                TransportationCategories = _context.TransportationCategories.ToList(),
                Companies = _context.Companies.ToList(),
                Trips = _context.Trips.ToList(),
                Users = _context.Users.ToList(),
                //Trips = _context.Trips.ToList(),
                Bookings = new List<Bookings>()
            };
        }


        //BookTrip 

        [HttpPost]
        [Route("BookTrip")]
        public async Task<ActionResult<Bookings>> BookTrip(BookingTripViewModel model)
        {

            //bookingvm.Bookings = await _context.Bookings.Include(x => x.Trips).ToListAsync();
            var availableSeats = _context.Trips.Where(x => x.Id == model.TripId).Select(x => x.AvailableSeats).First();
            if (model.demandeseats < availableSeats)
            {
                var booking = new Bookings { TripId = model.TripId, UserId = model.userId, DemandeSeats = model.demandeseats };

                availableSeats = availableSeats - model.demandeseats;
                var tripFromDb = _context.Trips.Where(m => m.Id == model.TripId).FirstOrDefault();
                tripFromDb.AvailableSeats = availableSeats;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return Ok("Happielly there are enough seats for You");
            }
            return Ok("Opps seats are not enough");

        }


        // GET: api/Bookings/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Bookings>> Details(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);

            if (bookings == null)
            {
                return NotFound();
            }

            return Ok(new {

                tripnum = bookings.Trips.TripNum,
                cityfrom = bookings.Trips.CityFrom.Name,
                cityto = bookings.Trips.CityTo.Name,
                depDate = bookings.Trips.DepDate,
                arrivalDate = bookings.Trips.ArrivalDate,
                depTime = bookings.Trips.DepTime,
                arrivalTime = bookings.Trips.ArrivalTime,
                price = bookings.Trips.Price,
                availableSeats = bookings.Trips.AvailableSeats,
                companyCategory = bookings.Trips.CompanyAssets.TransportationCategories.CategoryName,
                companyCompany = bookings.Trips.CompanyAssets.Companies.Name,
                username = bookings.ApplicationUser.UserName,
                phoneNumber = bookings.ApplicationUser.PhoneNumber,
                Email = bookings.ApplicationUser.Email,
                ssn = bookings.ApplicationUser.ssn

            

            });
        }




        // DELETE: api/Bookings/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<Bookings>> Delete(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);
            if (bookings == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(bookings);
            await _context.SaveChangesAsync();

            return bookings;
        }

        private bool BookingsExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }



        /*// GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookings>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bookings>> GetBookings(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);

            if (bookings == null)
            {
                return NotFound();
            }

            return bookings;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookings(int id, Bookings bookings)
        {
            if (id != bookings.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingsExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bookings>> PostBookings(Bookings bookings)
        {
            _context.Bookings.Add(bookings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookings", new { id = bookings.Id }, bookings);
        }

        */

        //check user`s info, cancel book valid
    }
}
