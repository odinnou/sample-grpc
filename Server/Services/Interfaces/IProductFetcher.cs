using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IProductFetcher
    {
        Task<Models.Product> GetProductByDeclinationAndReference(string declination, string reference);
        Task<IEnumerable<Models.Product>> GetProductsByDeclinationAndReferences(string declination, IEnumerable<string> references);
        Task<(IEnumerable<Models.Product> products, int count)> GetPaginatedProductsByDeclination(string declination, int pageIndex, int pageSize);
    }
}
