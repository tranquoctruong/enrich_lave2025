using System;
using User.API.Model;

namespace User.API.Services
{
    public class UserService : IUserService
    {
        public async Task UpdateSomethingFromProduct(ProductCreatedMessage msg)
        {
            Console.WriteLine($"Get value ok {msg.Name}");
        }
    }
}
