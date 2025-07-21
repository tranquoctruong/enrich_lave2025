using User.API.Model;

namespace User.API.Services
{
    public interface IUserService
    {
        Task UpdateSomethingFromProduct(ProductCreatedMessage msg);
    }
}
