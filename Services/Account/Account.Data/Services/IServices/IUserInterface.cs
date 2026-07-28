using Account.DTO.Response;
using Account.DTO.Resquest;
using ProjectCommonCode;
using ProjectCommonCode.RegisterDependency;

namespace Account.Data.Services.IServices
{
   // [RegisterSingleton]
   // [RegisterTransient]
    [RegisterScoped]
    public interface IUserInterface
    {

        Task<UserRequest> Create(UserRequest request);

        Task<UserResponse?> Update(int id,UserRequest request);

        Task<PageResults<UserResponse>> GetAll(
            int pageNumber = 1,

            int pageSize=10,

            string? search= null,

            string? sortBy= null,
            string? sortOrder= "desc"
            );
        Task<UserResponse?> GetById(int id);

        Task<bool> Delete(int id);
    }
}
