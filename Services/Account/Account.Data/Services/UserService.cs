using Account.Data.Services.IServices;
using Account.DTO.Response;
using Account.DTO.Resquest;

namespace Account.Data.Services
{
    public class UserService : IUserInterface
    {
        public Task<UserRequest> Create(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserResponse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> Update(int id, UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
