using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Studentify.Data;
using Studentify.Data.Repositories;
using Studentify.Models;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IInfosRepository _infosRepository;
        public InfoController(IInfosRepository infosRepository)
        {
            _infosRepository = infosRepository;
        }
        
        // GET: api/Infos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Info>>> GetInfos()
        {
            var infos = await _infosRepository.Select.All();
            return infos.ToList();
        }

        // GET: api/Infos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Info>> GetInfo(int id)
        {
            var info = await _infosRepository.Select.ById(id);

            if (info == null)
            {
                return NotFound();
            }

            return info;
        }

        // PUT: api/Infos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutInfo(int id, InfoDto infoDto)
        {
            Info info;
            
            try
            {
                info = await _infosRepository.Select.ById(id);
            }
            catch (DataException)
            {
                return BadRequest();
            }
            
            info.Name = infoDto.Name;
            info.ExpiryDate = infoDto.ExpiryDate;
            info.MapPoint.X = infoDto.Longitude;
            info.MapPoint.Y = infoDto.Latitude;
            info.Address = infoDto.Address;
            info.Description = infoDto.Description;
            info.Category = infoDto.Category;

            try
            {
                await _infosRepository.Update.One(info, id);
            }
            catch (DataException)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        // POST: api/Infos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Info>> PostInfo(InfoDto infoDto)
        {
            // var username = User.Identity.Name;
            // StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            // var account = await accountManager.FindAccountByUsername(username);
            //
            // var info = new Info()
            // {
            //     Name = infoDto.Name,
            //     ExpiryDate = infoDto.ExpiryDate,
            //     MapPoint = new Point(infoDto.Longitude, infoDto.Latitude) {SRID = 4326},
            //     Address = infoDto.Address,
            //     Description = infoDto.Description,
            //     AuthorId = account.Id,
            //     Category = infoDto.Category,
            // };
            //
            // info.CreationDate = DateTime.Now;
            //
            // _context.Infos.Add(info);
            // await _context.SaveChangesAsync();
            //
            // return CreatedAtAction(nameof(GetInfo), new { id = info.Id }, info);
            throw new NotImplementedException();
        }
    }
}
