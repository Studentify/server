using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Studentify.Data;
using Studentify.Models;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public InfoController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/Infos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Info>>> GetInfos()
        {
            return await _context.Infos.ToListAsync();
        }

        // GET: api/Infos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Info>> GetInfo(int id)
        {
            var info = await _context.Infos.FindAsync(id);

            if (info == null)
            {
                return NotFound();
            }

            return info;
        }

        // PUT: api/Infos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInfo(int id, Info info)
        {
            if (id != info.Id)
                return BadRequest();

            _context.Entry(info).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InfoExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Infos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Info>> PostInfo(InfoDto infoDto)
        {
            var username = User.Identity.Name;
            StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            var account = await accountManager.FindAccountByUsername(username);

            var info = new Info()
            {
                Name = infoDto.Name,
                ExpiryDate = infoDto.ExpiryDate,
                MapPoint = new Point(infoDto.Longitude, infoDto.Latitude) {SRID = 4326},
                Address = infoDto.Address,
                Description = infoDto.Description,
                AuthorId = account.Id,
                Category = infoDto.Category,
            };
            
            info.CreationDate = DateTime.Now;
            
            _context.Infos.Add(info);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInfo), new { id = info.Id }, info);
        }



        private bool InfoExists(int id)
        {
            return _context.Infos.Any(e => e.Id == id);
        }
    }
}
