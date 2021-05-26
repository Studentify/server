namespace Studentify.Data.Repositories
{
    public class RepositoryBase
    {
        protected readonly StudentifyDbContext Context;
        public RepositoryBase(StudentifyDbContext context)
        {
            Context = context;
        }
    }
}