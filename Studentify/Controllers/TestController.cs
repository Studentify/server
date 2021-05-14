using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Studentify.Data;
using Studentify.Models;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public TestController(StudentifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("StudentifyAccountwithLoadedReferences/{id}")]
        public async Task<ActionResult<StudentifyAccount>> GetSAccount(int id)
        {
            var infoEvent = await _context.Infos.FindAsync(id);
            await _context.Entry(infoEvent).Reference(i => i.Author).LoadAsync();
            await _context.Entry(infoEvent.Author).Collection(a => a.Events).LoadAsync();
            return infoEvent.Author;
        }
    }
}
