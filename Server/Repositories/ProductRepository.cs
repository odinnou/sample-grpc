using Microsoft.EntityFrameworkCore;
using Server.Infrastructure;
using Server.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(ProductContext productContext) : base(productContext)
        {
        }

        public async Task<Models.Product?> GetProductByDeclinationAndReference(string declination, string reference)
        {
            return await ProductContext.Products.Where(product => declination.Equals(product.Declination))
                                                .Where(product => reference.Equals(product.Reference))
                                                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Models.Product>> GetProductsByDeclinationAndReferences(string declination, IEnumerable<string> references)
        {
            return await ProductContext.Products.Where(product => declination.Equals(product.Declination))
                                                .Where(product => references.Contains(product.Reference))
                                                .OrderBy(product => product.Order)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Product>> GetPaginatedProductsByDeclination(string declination, int pageIndex, int pageSize)
        {
            return await ProductContext.Products.Where(product => declination.Equals(product.Declination))
                                                .OrderBy(product => product.Order)
                                                .Skip(pageIndex * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync();
        }

        public async Task<int> GetCountProductsByDeclination(string declination)
        {
            return await ProductContext.Products.Where(product => declination.Equals(product.Declination))
                                                .CountAsync();
        }
    }
}
