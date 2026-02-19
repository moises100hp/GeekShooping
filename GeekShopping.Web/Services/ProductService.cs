using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        public Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> FindProductById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> CreateProduct(ProductModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> UpdateProduct(ProductModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
