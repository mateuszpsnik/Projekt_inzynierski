using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusiciansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Musicians
        [HttpGet]
        public async Task<ActionResult<PaginationApiResult<MusiciansListDTO>>> GetMusicians(
            int pageIndex = 0, int pageSize = 3, int? type = null,
            string instrument = null, decimal minPrice = 0.0M, decimal maxPrice = 1000.0M,
            double minAvgScore = 0.0, int sort = 0)
        { 
            // get elements from the database, filter and sort them
            return await PaginationApiResult<MusiciansListDTO>.CreateAsync(
                _context.Musicians.Select(m => new MusiciansListDTO()
                {
                    Id = m.Id,
                    Email = m.Email,
                    Name = m.Name,
                    Price = m.Price,
                    ProfilePicFilename = m.ProfilePicFilename,
                    Instruments = m.Instruments,
                    Types = m.Types,
                    AvgScore = m.Reviews.Count != 0 ?
                                (from r in m.Reviews select r.Rate).Average() : 0
                }), pageIndex, pageSize, type, instrument, minPrice, maxPrice,
                minAvgScore, sort);
        }

        // GET: api/Musicians/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicianDTO>> GetMusician(string id)
        {
            var musician = await _context.Musicians.Select(m => new MusicianDTO()
            {
                Id = m.Id,
                Name = m.Name,
                ProfilePicFilename = m.ProfilePicFilename,
                ShortDescription = m.Description,
                LongDescription = m.LongDescription,
                Price = m.Price,
                Instruments = m.Instruments,
                AvgScore = m.Reviews.Count != 0 ?
                                (from r in m.Reviews select r.Rate).Average() : 0
            }).Where(m => m.Id == id).FirstAsync();

            if (musician == null)
            {
                return NotFound();
            }

            return musician;
        }

        // PUT: api/Musicians/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusician(string id, Musician musician)
        {
            if (id != musician.Id)
            {
                return BadRequest();
            }

            _context.Entry(musician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicianExists(id))
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

        // POST: api/Musicians
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Musician>> PostMusician(Musician musician)
        {
            _context.Musicians.Add(musician);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMusician", new { id = musician.Id }, musician);
        }

        // DELETE: api/Musicians/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusician(int id)
        {
            var musician = await _context.Musicians.FindAsync(id);
            if (musician == null)
            {
                return NotFound();
            }

            _context.Musicians.Remove(musician);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicianExists(string id)
        {
            return _context.Musicians.Any(e => e.Id == id);
        }
    }
}
