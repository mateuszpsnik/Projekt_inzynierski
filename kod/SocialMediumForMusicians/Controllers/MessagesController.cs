using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginationApiResult<MessageDTO>>> GetMessages(
            string id = null, string authorId = null, int pageIndex = 0, int pageSize = 10)
        {
            var elements = _context.Messages.Select(m => new MessageDTO()
            {
                Id = m.Id.ToString(),
                AuthorId = m.AuthorId,
                RecipentId = m.RecipentId,
                Content = m.Content,
                Read = m.Read,
                SentAt = m.SentAt,
                AuthorName = m.Author.Name,
                AuthorImgFilename = m.Author.ProfilePicFilename
            });

            // protect from access to all messagess
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(authorId))
            {
                elements = elements.Where(m => m.RecipentId == id).OrderByDescending(m => m.SentAt);
            }
            else
            {
                // take all messages in a thread (both directions)
                elements = elements.Where(m => ((m.RecipentId == id && m.AuthorId == authorId) || 
                    (m.AuthorId == id && m.RecipentId == authorId))).OrderBy(m => m.SentAt);
            }

            return await PaginationApiResult<MessageDTO>.CreateAsync(elements, pageIndex, pageSize);
        }

        // GET: api/Messages/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(Guid id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
