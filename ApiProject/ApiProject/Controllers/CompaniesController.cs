using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Models;

using ApiProject.Modelviews;
using Microsoft.AspNetCore.Authorization;

namespace ApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDb _context;


        [BindProperty]
        public CompaniesViewModel CompaniesVM { get; set; }
        public CompaniesController(ApplicationDb db)
        {

            _context = db;
            CompaniesVM = new CompaniesViewModel()
            {
                TransportationTypes = _context.TransportationTypes.ToList(),
                Companies = new Models.Companies()
            };
        }

        // GET: api/Companies
        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult<IEnumerable<Companies>>> Index()
        {
            var Companies = _context.Companies.Include(m => m.TransportationTypes.Name);
            
            
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult<Companies>> Details(int id)
        {
            var companies = await _context.Companies.FindAsync(id);

            if (companies == null)
            {
                return Ok("Not Found");
            }

            // return companies;

            return Ok(new
            {
                id = companies.Id,
                name = companies.Name,
                type = companies.TransportationTypes.Name 
            });
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.

        //[Route("Edit/{id}")]
        //public async Task<IActionResult> Edit(int id, Companies companiesvm)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var CompanyFromDb = _context.Companies.Where(m => m.Id == id).FirstOrDefault();
        //        if(CompanyFromDb != null)
        //        {
        //            return NotFound();
        //        }
        //        CompanyFromDb.Name = companiesvm.Name;
        //        CompanyFromDb.TransportationTypeId = companiesvm.TransportationTypeId;

        //        await _context.SaveChangesAsync();

        //        return StatusCode(StatusCodes.Status200OK);
        //    }

        //    return BadRequest();
        //}


        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Companies companies)
        {
            if (id != companies.Id)
            {
                return BadRequest();
            }

            _context.Entry(companies).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompaniesExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("AddCompany")]
        public async Task<ActionResult<Companies>> AddCompany(Companies companies)
        {
            _context.Companies.Add(companies);
            await _context.SaveChangesAsync();

            return Ok("Added Successfully");
        }

        // DELETE: api/Companies/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult<Companies>> DeleteCompanies(int id)
        {
            var companies = await _context.Companies.FindAsync(id);
            if (companies == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(companies);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }

        private bool CompaniesExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}

