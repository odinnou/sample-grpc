using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Models.Product?> GetProductByDeclinationAndReference(string declination, string reference);
        Task<IEnumerable<Models.Product>> GetProductsByDeclinationAndReferences(string declination, IEnumerable<string> references);
        Task<IEnumerable<Models.Product>> GetPaginatedProductsByDeclination(string declination, int pageIndex, int pageSize);
        Task<int> GetCountProductsByDeclination(string declination);
    }
}
