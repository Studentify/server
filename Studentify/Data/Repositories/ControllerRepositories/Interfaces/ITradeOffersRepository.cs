using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public interface ITradeOffersRepository
    {
        public ISelectRepository<TradeOffer> Select { get; }
        public IInsertRepository<TradeOffer> Insert { get; }
        public IUpdateRepository<TradeOffer> Update { get; }
    }
}