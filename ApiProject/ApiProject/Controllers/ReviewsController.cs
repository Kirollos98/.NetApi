using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Models;

namespace ApiProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDb _context;

        public ReviewsController(ApplicationDb context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult<IEnumerable<Reviews>>> Index()
        {
            return await _context.Reviews.ToListAsync();
        }



        //return comment bs 

        // GET: api/Reviews/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Reviews>> Details(int id)
        {
            var reviews = await _context.Reviews.FindAsync(id);

            if (reviews == null)
            {
                return Ok("NotFound");
            }

           

            return Ok(new
            {
                // rate = reviews.reviewrate,
                comment = reviews.Comment
            });
        }




        // POST: api/Reviews
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("AddReview")]
        public async Task<ActionResult<Reviews>> AddReview(Reviews reviews)
        {
            _context.Reviews.Add(reviews);
            await _context.SaveChangesAsync();

            return Ok("Added Successfully");
        }





        /*
        // PUT: api/Reviews/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
       [HttpPut("Edit/{id}")]
        public async Task<IActionResult> PutReviews(int id, Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return BadRequest();
            }

            _context.Entry(reviews).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Edited Successfully");
        }
        */





        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return BadRequest();
            }

            _context.Entry(reviews).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Edited Successfully");
        }

        // DELETE: api/Reviews/5
        [HttpDelete]
        [Route("Delete/{id}")]

        public async Task<ActionResult<Reviews>> DeleteReviews(int id)
        {
            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
        
        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
