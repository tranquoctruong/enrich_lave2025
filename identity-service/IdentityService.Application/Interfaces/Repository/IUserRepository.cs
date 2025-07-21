using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        // Thêm các method đặc thù UserRepository nếu cần
        Task<User?> GetByUsernameAsync(string username);
    }
}
