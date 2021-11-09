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
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<PaginationApiResult<ReviewsListDTO>>> GetReviews(
            int? top = null, string id = null, int pageIndex = 0, int pageSize = 3,
            string authorId = null)
        {
            var elements = _context.Reviews.Select(r => new ReviewsListDTO()
            {
                Id = r.Id.ToString(),
                Rate = r.Rate,
                Description = r.Description,
                AuthorName = r.Author.Name,
                AuthorProfilePicFilename = r.Author.ProfilePicFilename,
                TargetId = r.TargetId,
                TargetProfilePicFilename = r.Target.ProfilePicFilename,
                SentAt = r.SentAt,
                AuthorId = r.AuthorId
            }).OrderByDescending(r => r.SentAt);

            if (top.HasValue)
            {
                pageSize = (int)top;
                elements = (IOrderedQueryable<ReviewsListDTO>)elements.Take((int)top);
            }

            if (!string.IsNullOrEmpty(id))
            {
                elements = (IOrderedQueryable<ReviewsListDTO>)elements.Where(r => r.TargetId == id);
            }

            if (!string.IsNullOrEmpty(authorId))
            {
                elements = (IOrderedQueryable<ReviewsListDTO>)elements.Where(r => r.AuthorId == authorId);
            }

            return await PaginationApiResult<ReviewsListDTO>.CreateAsync(
                elements, pageIndex, pageSize);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(Guid id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(Guid id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
