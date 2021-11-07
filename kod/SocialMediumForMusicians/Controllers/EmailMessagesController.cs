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
    public class EmailMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmailMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmailMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailMessage>>> GetEmailMessage()
        {
            return await _context.EmailMessage.ToListAsync();
        }

        // GET: api/EmailMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailMessage>> GetEmailMessage(int id)
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
        public async Task<IActionResult> PutEmailMessage(string id, EmailMessage emailMessage)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailMessage(int id)
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

        private bool EmailMessageExists(string id)
        {
            return _context.EmailMessage.Any(e => e.Id == id);
        }
    }
}
