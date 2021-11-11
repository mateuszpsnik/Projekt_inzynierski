using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MeetingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginationApiResult<MeetingDTO>>> GetMeetings(
            string hostId = null, string guestId = null, int pageIndex = 0,
            int pageSize = 3)
        {
            var elements = _context.Meetings.Select(m => new MeetingDTO
            {
                Id = m.Id.ToString(),
                HostId = m.HostId,
                GuestId = m.GuestId,
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Notes = m.Notes,
                Accepted = m.Accepted,
                HostName = m.Host.Name,
                GuestName = m.Guest.Name,
                HostImgFilename = m.Host.ProfilePicFilename,
                GuestImgFilename = m.Guest.ProfilePicFilename
            }).OrderByDescending(m => m.StartTime);

            if (!string.IsNullOrEmpty(hostId))
            {
                elements = (IOrderedQueryable<MeetingDTO>)elements.Where(m => m.HostId == hostId);
            }
            else if (!string.IsNullOrEmpty(guestId))
            {
                elements = (IOrderedQueryable<MeetingDTO>)elements.Where(m => m.GuestId == guestId);
            }
            else
            {
                // protect from access to all meetings
                return BadRequest();
            }

            return await PaginationApiResult<MeetingDTO>.CreateAsync(elements, pageIndex, pageSize);
        }

        // GET: api/Meetings/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return meeting;
        }

        // PUT: api/Meetings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting(Guid id, Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return BadRequest();
            }

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
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

        // POST: api/Meetings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Meeting>> PostMeeting(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeeting", new { id = meeting.Id }, meeting);
        }

        // DELETE: api/Meetings/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeetingExists(Guid id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}
