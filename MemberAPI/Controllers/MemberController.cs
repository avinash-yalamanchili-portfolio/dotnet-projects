// Controllers/MemberController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemberAPI.Data;
using MemberAPI.Models;
using MemberAPI.Services;

namespace MemberAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly MemberDbContext _context;
        private readonly EncryptionService _encryption;

        public MemberController(MemberDbContext context, EncryptionService encryption)
        {
            _context = context;
            _encryption = encryption;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _context.Members
                .Include(m => m.Demographics)
                .Include(m => m.Address)
                .ToListAsync();

            foreach (var m in members)
            {
                m.Demographics.SSN = _encryption.Decrypt(m.Demographics.SSN);
            }

            return members;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members
                .Include(m => m.Demographics)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();

            member.Demographics.SSN = _encryption.Decrypt(member.Demographics.SSN);

            return member;
        }

        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember(Member member)
        {
            member.Demographics.SSN = _encryption.Encrypt(member.Demographics.SSN);

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, Member member)
        {
            if (id != member.Id) return BadRequest();

            member.Demographics.SSN = _encryption.Encrypt(member.Demographics.SSN);

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Members.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
