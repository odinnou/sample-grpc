using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Product;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        public const string TOTAL_COUNT_HEADER = "X-Total-Count";
        private readonly IProductFetcher iProductFetcher;
        private readonly IMapper iMapper;

        public ProductController(IMapper mapper, IProductFetcher iProductFetcher, IMapper iMapper)
        {
            this.iProductFetcher = iProductFetcher ?? throw new ArgumentNullException(nameof(iProductFetcher));
            this.iMapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{declination}/{reference}")]
        public async Task<ProductDto> GetProductByDeclinationAndReference(string declination, string reference)
        {
            Models.Product product = await iProductFetcher.GetProductByDeclinationAndReference(declination, reference);

            return iMapper.Map<ProductDto>(product);
        }

        [HttpGet("{declination}")]
        public async Task<IEnumerable<ProductDto>> GetPaginatedProductsByDeclination(string declination, int pageIndex, int pageSize)
        {
            (IEnumerable<Models.Product> products, int count) = await iProductFetcher.GetPaginatedProductsByDeclination(declination, pageIndex, pageSize);

            Response.Headers.Add(TOTAL_COUNT_HEADER, count.ToString());

            return iMapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
