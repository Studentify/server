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
    public class TradeOfferController : ControllerBase
    {
        private readonly ITradeOffersRepository _tradeOffersRepository;
        private readonly IStudentifyAccountsRepository _accountsRepository;
        public TradeOfferController(ITradeOffersRepository tradeOffersRepository, IStudentifyAccountsRepository accountsRepository)
        {
            _tradeOffersRepository = tradeOffersRepository;
            _accountsRepository = accountsRepository;

        }
        
        // GET: api/TradeOffers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeOffer>>> GetTradeOffers()
        {
            var tradeOffers = await _tradeOffersRepository.Select.All();
            return tradeOffers.ToList();
        }

        // GET: api/TradeOffers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeOffer>> GetTradeOffer(int id)
        {
            var tradeOffer = await _tradeOffersRepository.Select.ById(id);

            if (tradeOffer == null)
            {
                return NotFound();
            }

            return tradeOffer;
        }

        // PUT: api/TradeOffers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutTradeOffer(int id, TradeOfferDto tradeOfferDto)
        {
            TradeOffer tradeOffer;
            
            try
            {
                tradeOffer = await _tradeOffersRepository.Select.ById(id);
            }
            catch (DataException)
            {
                return BadRequest();
            }
            
            tradeOffer.Name = tradeOfferDto.Name;
            tradeOffer.ExpiryDate = tradeOfferDto.ExpiryDate;
            tradeOffer.MapPoint.X = tradeOfferDto.Longitude;
            tradeOffer.MapPoint.Y = tradeOfferDto.Latitude;
            tradeOffer.Address = tradeOfferDto.Address;
            tradeOffer.Description = tradeOfferDto.Description;
            tradeOffer.Offer = tradeOfferDto.Offer;
            tradeOffer.Price = tradeOfferDto.Price;
            tradeOffer.BuyerId = tradeOfferDto.BuyerId;
            
            try
            {
                await _tradeOffersRepository.Update.One(tradeOffer, id);
            }
            catch (DataException)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        // POST: api/TradeOffers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TradeOffer>> PostTradeOffer(TradeOfferDto tradeOfferDto)
        {
            var username = User.Identity.Name;
            var account = await _accountsRepository.SelectByUsername(username);

            var tradeOffer = new TradeOffer
            {
                Name = tradeOfferDto.Name,
                ExpiryDate = tradeOfferDto.ExpiryDate,
                MapPoint = new Point(tradeOfferDto.Longitude, tradeOfferDto.Latitude) {SRID = 4326},
                Address = tradeOfferDto.Address,
                Description = tradeOfferDto.Description,
                AuthorId = account.Id,
                CreationDate = DateTime.Now,
                Offer = tradeOfferDto.Offer,
                Price = tradeOfferDto.Price,
                BuyerId = tradeOfferDto.BuyerId
            };


            await _tradeOffersRepository.Insert.One(tradeOffer);
            
            return CreatedAtAction(nameof(GetTradeOffer), new { id = tradeOffer.Id }, tradeOffer);
        }
        
        
        // Patch: api/TradeOffers/5
        // To accept offer
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTradeOffer(int id)
        {
            TradeOffer tradeOffer;
            
            try
            {
                tradeOffer = await _tradeOffersRepository.Select.ById(id);
            }
            catch (DataException)
            {
                return BadRequest();
            }

            var username = User.Identity.Name;
            var currentUserId = _accountsRepository.SelectByUsername(username).Id;
            tradeOffer.BuyerId = currentUserId;
            
            try
            {
                await _tradeOffersRepository.Update.One(tradeOffer, id);
            }
            catch (DataException)
            {
                return NotFound();
            }
            
            return NoContent();
        }
        
    }
}
