using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Studentify.Data;
using Studentify.Models.HttpBody;
using Studentify.Models.StudentifyEvents;

namespace Studentify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeOfferController : ControllerBase
    {
        private readonly StudentifyDbContext _context;

        public TradeOfferController(StudentifyDbContext context)
        {
            _context = context;
        }

        // GET: api/TradeOffers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeOffer>>> GetTradeOffers()
        {
            return await _context.TradeOffers.ToListAsync();
        }

        // GET: api/TradeOffers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeOffer>> GetTradeOffer(int id)
        {
            var tradeOffer = await _context.TradeOffers.FindAsync(id);

            if (tradeOffer == null)
            {
                return NotFound();
            }

            return tradeOffer;
        }

        // PUT: api/TradeOffers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTradeOffer(int id, TradeOffer tradeOffer)
        {
            if (id != tradeOffer.Id)
                return BadRequest();

            _context.Entry(tradeOffer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeOfferExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/TradeOffers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TradeOffer>> PostTradeOffer(TradeOfferDTO tradeOfferDto)
        {
            var username = User.Identity.Name;
            StudentifyAccountManager accountManager = new StudentifyAccountManager(_context);
            var account = await accountManager.FindAccountByUsername(username);

            var tradeOffer = new TradeOffer()
            {
                Name = tradeOfferDto.Name,
                ExpiryDate = tradeOfferDto.ExpiryDate,
                Location = new Point(tradeOfferDto.Longitude, tradeOfferDto.Latitude) {SRID=4326},
                Description = tradeOfferDto.Description,
                StudentifyAccountId = account.Id,
                Price = tradeOfferDto.Price,
                Offer = tradeOfferDto.Offer,
            };
            
            tradeOffer.CreationDate = DateTime.Now;
            
            _context.TradeOffers.Add(tradeOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTradeOffer), new { id = tradeOffer.Id }, tradeOffer);
        }
        
        private bool TradeOfferExists(int id)
        {
            return _context.TradeOffers.Any(e => e.Id == id);
        }
    }
}
