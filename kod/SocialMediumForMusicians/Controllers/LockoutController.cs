using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public LockoutController(ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Lockout/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id, string adminId, DateTime end)
        {
            var user = await _context.Users.FindAsync(id);
            var admin = await _context.Users.FindAsync(adminId);

            if (user == null)
            {
                return NotFound();
            }
            if (admin == null)
            {
                return BadRequest();
            }

            if (await _userManager.IsInRoleAsync(admin, "Admin"))
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = new DateTimeOffset(end);

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    // delete all reports with the locked-out user
                    var reports = _context.Reports.Select(r => r).Where(r => r.UserId == user.Id);
                    _context.Reports.RemoveRange(reports);

                    await _context.SaveChangesAsync();

                    return user;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
