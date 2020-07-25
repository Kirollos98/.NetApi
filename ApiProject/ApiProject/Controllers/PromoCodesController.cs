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
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class PromoCodesController : ControllerBase
    {
        private readonly ApplicationDb _context;
              
        public PromoCodesController(ApplicationDb context)
        {
            _context = context;
        }



        /* 7bbti busi bra7a fi 4 7gat functions y3ni hn3mlha brra7a kolo hyb2a shl
         busi yasti awel 7aga 
       done 1 => generate function 
       done 2 => nsgel fdata base eldata eli hyd5lha 
      done  3 => addpromocode 
        4 => disactivated promo

        5 => function to manage User to active promo code 
         */



        [HttpPost]
        [Route("AddPromoCode")]
        public async Task<ActionResult<PromoCodes>> AddPromoCode(PromoCodeViewModel model)
        {

            string code = DateTime.Now.Millisecond.ToString()
                         + DateTime.Now.Second.ToString()
                        + DateTime.Now.Minute.ToString()
                        + DateTime.Now.Millisecond.ToString();
            code = code.Length > 6 ? code.Substring(0, 6) : code;

            var Promo = new PromoCodes
            {
                CompanyId = model.CompanyId,
                Value = model.Value,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PromoCode = code,
                IsActivated = true
            };

            //nsya is Active
            _context.PromoCodes.Add(Promo);
            await _context.SaveChangesAsync();

            bool isSend = SendPromoCode(model.CompanyId, code);

            return Ok("e4ta 3liki");

        }
       [HttpPost]
        [Route("SendPromoCode")]
        //[HttpPut("SendPromoCode/{id}")]
        public bool SendPromoCode(int companyID ,string promocode )
        {
            try
            {
                List<int> companyAssetIDs = _context.CompanyAssets.Where(x => x.CompniesId == companyID).Select(x => x.Id).ToList();
                List<int> tripsIDs = _context.Trips.Where(x => companyAssetIDs.Contains(x.CompanyAssetId)).Select(x => x.Id).ToList();
              
                List<string> usersIDs = _context.Bookings.Where(x => tripsIDs.Contains(x.TripId)).Select(x => x.UserId).ToList();
               // List<string> usersIDs = _context.Bookings.Where(x => x.).Select(x => x.UserId).ToList();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }



        //TO BLOCK USER
        [HttpPut]
        [Route("ToDisActive")]
        public async Task<ActionResult<PromoCodes>> ToDisActive()
        {
            List<int> ISActivatedIDs = _context.PromoCodes.Where(x => x.EndDate > DateTime.Now).Select(x => x.Id).ToList();

            ISActivatedIDs.IsActivated = false;
          
            
            
            await _context.PromoCodes.Update(ISActivatedIDs);
                return Ok(" ");
            
        }




     /*   //?????????
        //public async Task<ActionResult<IEnumerable<PromoCodes>>> ToDisActive ( )
        public void ToDisActive()
        {

            List<bool> IsActivateds = _context.PromoCodes.Where(x => x.EndDate > DateTime.Now).Select(x => x.IsActivated).ToList();
            foreach (IsActivateds in PromoCodes)
            {
                IsActivateds = false;
            }

            // return await _context.PromoCodes.ToListAsync();

        }*/
        /*
         * public void ToDisActive()
        {

            List<bool> IsActivateds = _context.PromoCodes.Where(x => x.EndDate > DateTime.Now).Select(x => x.IsActivated).ToList();
            foreach (IsActivateds in PromoCodes)
            {
                IsActivateds = false;
            }

            // return await _context.PromoCodes.ToListAsync();

        }
         */


        // is used "promo code"   to active promo wkda
        // check eldate 
        // loop the fun to t8yyr mn true w false =>>done,  "bs elfkra lsa front end hyndh 3liha kol yoom  why8yar elvalue  bta3t elist mn true lfalse"

























        // GET: api/PromoCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCodes>>> GetPromoCodes()
        {
            return await _context.PromoCodes.ToListAsync();
        }

        // GET: api/PromoCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromoCodes>> GetPromoCodes(int id)
        {
            var promoCodes = await _context.PromoCodes.FindAsync(id);

            if (promoCodes == null)
            {
                return NotFound();
            }

            return promoCodes;
        }

        // PUT: api/PromoCodes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromoCodes(int id, PromoCodes promoCodes)
        {
            if (id != promoCodes.Id)
            {
                return BadRequest();
            }

            _context.Entry(promoCodes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromoCodesExists(id))
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

        // POST: api/PromoCodes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PromoCodes>> PostPromoCodes(PromoCodes promoCodes)
        {
            _context.PromoCodes.Add(promoCodes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPromoCodes", new { id = promoCodes.Id }, promoCodes);
        }

        // DELETE: api/PromoCodes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PromoCodes>> DeletePromoCodes(int id)
        {
            var promoCodes = await _context.PromoCodes.FindAsync(id);
            if (promoCodes == null)
            {
                return NotFound();
            }

            _context.PromoCodes.Remove(promoCodes);
            await _context.SaveChangesAsync();

            return promoCodes;
        }

        private bool PromoCodesExists(int id)
        {
            return _context.PromoCodes.Any(e => e.Id == id);
        }
    }
}
/*
 string code = DateTime.Now.Millisecond.ToString()
+ DateTime.Now.Second.ToString()
+ DateTime.Now.Minute.ToString() 
+ DateTime.Now.Millisecond.ToString();
  code = code.Length > 6 ? code.Substring(0, 6) : code;
 */
