using Account.Data.Context;
using Account.Data.Services.IServices;
using Account.Domain;
using Account.DTO.Response;
using Account.DTO.Resquest;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Account.Data.Services
{
    public class UserService : IUserInterface
    {
        private readonly AccountDBContext _context;
        private readonly IMapper _mapper;

        public UserService(AccountDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRequest> Create(UserRequest request)
        {
            var userEntity = _mapper.Map<User>(request);
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserRequest>(userEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
            {
                return false;
            }
            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse?> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> Update(int id, UserRequest request)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
            {
                return null;
            }
            userEntity.Name = request.Name;
            userEntity.Email = request.Email;
            
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<UserResponse>(userEntity);
        }
    }
}
