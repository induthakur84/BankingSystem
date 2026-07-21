using Account.Domain;
using Account.DTO.Response;
using Account.DTO.Resquest;
using AutoMapper;

namespace Account.Data.Automapper
{
    public class UserMapping: Profile
    {

        public UserMapping()
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<UserRequest, User>().ReverseMap();
        }
    }
}
