using Studentify.Models.StudentifyEvents;

namespace Studentify.Data.Repositories
{
    public class TradeOffersRepository : StudentifyEventRepositorySelectBase<TradeOffer>, ITradeOffersRepository
    {
        public IInsertRepository<TradeOffer> Insert { get; }
        public IUpdateRepository<TradeOffer> Update { get; }
        public TradeOffersRepository(StudentifyDbContext context, 
            ISelectRepository<TradeOffer> selectRepository,
            IInsertRepository<TradeOffer> insertRepository,
            IUpdateRepository<TradeOffer> updateRepository
        ) : base(context, selectRepository)
        {
            Insert = insertRepository;
            Update = updateRepository;
        }
    }
}