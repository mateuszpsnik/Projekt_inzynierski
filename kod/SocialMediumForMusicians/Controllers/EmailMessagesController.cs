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
    public class EmailMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmailMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmailMessages
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginationApiResult<EmailMessageDTO>>> GetEmailMessage(
            string id = null, int pageIndex = 0, int pageSize = 3)
        {
            var elements = _context.EmailMessage.Select(m => new EmailMessageDTO()
            {
                Id = m.Id.ToString(),
                AuthorEmail = m.AuthorEmail,
                RecipentId = m.RecipentId,
                Content = m.Content,
                Read = m.Read,
                SentAt = m.SentAt
            });

            // protect from access to all messagess
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            elements = elements.Where(m => m.RecipentId == id).OrderByDescending(m => m.SentAt);

            return await PaginationApiResult<EmailMessageDTO>.CreateAsync(elements, pageIndex, pageSize);
        }

        // GET: api/EmailMessages/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailMessage>> GetEmailMessage(Guid id)
        {
            var emailMessage = await _context.EmailMessage.FindAsync(id);

            if (emailMessage == null)
            {
                return NotFound();
            }

            return emailMessage;
        }

        // PUT: api/EmailMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailMessage(Guid id, EmailMessage emailMessage)
        {
            if (id != emailMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(emailMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailMessageExists(id))
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

        // POST: api/EmailMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmailMessage>> PostEmailMessage(EmailMessage emailMessage)
        {
            _context.EmailMessage.Add(emailMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmailMessage", new { id = emailMessage.Id }, emailMessage);
        }

        // DELETE: api/EmailMessages/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailMessage(Guid id)
        {
            var emailMessage = await _context.EmailMessage.FindAsync(id);
            if (emailMessage == null)
            {
                return NotFound();
            }

            _context.EmailMessage.Remove(emailMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmailMessageExists(Guid id)
        {
            return _context.EmailMessage.Any(e => e.Id == id);
        }
    }
}
