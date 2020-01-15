using Server.Infrastructure.Exceptions;
using Server.Repositories.Interfaces;
using Server.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.UseCases
{
    public class ProductFetcher : IProductFetcher
    {
        private readonly IProductRepository iProductRepository;

        public ProductFetcher(IProductRepository iProductRepository)
        {
            this.iProductRepository = iProductRepository ?? throw new ArgumentNullException(nameof(iProductRepository));
        }

        public async Task<Models.Product> GetProductByDeclinationAndReference(string declination, string reference)
        {
            Models.Product? product = await iProductRepository.GetProductByDeclinationAndReference(declination, reference);

            if (product == null)
            {
                throw new ProductNotFoundException(declination, reference);
            }

            return product;
        }

        public async Task<IEnumerable<Models.Product>> GetProductsByDeclinationAndReferences(string declination, IEnumerable<string> references)
        {
            return await iProductRepository.GetProductsByDeclinationAndReferences(declination, references);
        }

        public async Task<(IEnumerable<Models.Product> products, int count)> GetPaginatedProductsByDeclination(string declination, int pageIndex, int pageSize)
        {
            IEnumerable<Models.Product> products = await iProductRepository.GetPaginatedProductsByDeclination(declination, pageIndex, pageSize);
            int count = await iProductRepository.GetCountProductsByDeclination(declination);

            return (products, count);
        }
    }
}
