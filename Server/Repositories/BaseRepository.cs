using Server.Infrastructure;
using Server.Infrastructure.Exceptions;
using Server.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly ProductContext ProductContext;

        protected BaseRepository(ProductContext productContext)
        {
            ProductContext = productContext;
        }

        public async Task SaveChangesIgnoringNumberOfChanges()
        {
            await ProductContext.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            int nbChanges = await ProductContext.SaveChangesAsync();

            if (nbChanges == 0)
            {
                throw new NoRowInsertedOrUpdatedException();
            }
        }
    }
}
