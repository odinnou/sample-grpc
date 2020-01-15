using AutoMapper;
using Grpc.Core;
using Server.Infrastructure.Exceptions;
using Server.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Server.Product;

namespace Server.Grpc
{
    public class ProductService : ProductBase
    {
        private readonly IMapper mapper;
        private readonly IProductFetcher iProductFetcher;

        public ProductService(IMapper mapper, IProductFetcher iProductFetcher)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.iProductFetcher = iProductFetcher ?? throw new ArgumentNullException(nameof(iProductFetcher));
        }

        public override async Task<ProductItemResponse?> GetProductByDeclinationAndReference(ProductItemRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Declination) || string.IsNullOrWhiteSpace(request.Reference))
            {
                context.Status = new Status(StatusCode.InvalidArgument, "Arguments are invalid, declination and Reference can't be null or empty");
                return null;
            }

            try
            {
                Models.Product product = await iProductFetcher.GetProductByDeclinationAndReference(request.Declination, request.Reference);

                return mapper.Map<ProductItemResponse>(product);
            }
            catch (ProductNotFoundException exception)
            {
                context.Status = new Status(StatusCode.NotFound, exception.Message);
                return null;
            }
            catch (Exception exception)
            {
                context.Status = new Status(StatusCode.Internal, exception.Message);
                return null;
            }
        }

        public override async Task GetStreamedProductsByDeclinationAndReferences(StreamedProductItemsRequest request, IServerStreamWriter<ProductItemResponse> responseStream, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Declination) || !request.References.Any())
            {
                context.Status = new Status(StatusCode.InvalidArgument, $"Arguments are invalid, declination and Reference can't be null or empty");
            }

            try
            {
                // Normalement c'est dans le cas où l'on peut utiliser un flux asynchrone, ici c'est un peu "fake" (même si ça garde l'intérêt de transmettre paquet par paquet)
                IEnumerable<Models.Product> products = await iProductFetcher.GetProductsByDeclinationAndReferences(request.Declination, request.References.ToList());
                IEnumerator<Models.Product> productsEnumerator = products.GetEnumerator();

                while (!context.CancellationToken.IsCancellationRequested && productsEnumerator.MoveNext())
                {
                    await responseStream.WriteAsync(mapper.Map<ProductItemResponse>(productsEnumerator.Current));
                }
            }
            catch (Exception exception)
            {
                context.Status = new Status(StatusCode.Internal, exception.Message);
            }
        }

        public override async Task<PaginedProductItemsResponse?> GetPaginedProductsByDeclination(PaginedProductItemsRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Declination))
            {
                context.Status = new Status(StatusCode.InvalidArgument, $"Arguments are invalid, declination can't be null or empty");
                return null;
            }

            try
            {
                (IEnumerable<Models.Product> products, int count) = await iProductFetcher.GetPaginatedProductsByDeclination(request.Declination, request.PageIndex, request.PageSize);

                return BuildPaginedResponse(products, count, request);
            }
            catch (Exception exception)
            {
                context.Status = new Status(StatusCode.Internal, exception.Message);
                return null;
            }
        }

        private PaginedProductItemsResponse BuildPaginedResponse(IEnumerable<Models.Product> products, int count, PaginedProductItemsRequest request)
        {
            PaginedProductItemsResponse response = new PaginedProductItemsResponse
            {
                Total = count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            response.Data.Add(mapper.Map<IEnumerable<ProductItemResponse>>(products));

            return response;
        }
    }
}
