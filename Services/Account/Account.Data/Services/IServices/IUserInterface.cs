using Account.DTO.Response;
using Account.DTO.Resquest;

namespace Account.Data.Services.IServices
{
    public interface IUserInterface
    {

        Task<UserRequest> Create(UserRequest request);

        Task<UserResponse> Update(int id,UserRequest request);

        Task<IEnumerable<UserResponse>> GetAll();
        Task<UserResponse> GetById(int id);

        Task<bool> Delete(int id);
    }
}
