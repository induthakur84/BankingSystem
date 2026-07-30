using Account.Data.Context;
using Account.Data.Services.IServices;
using Account.Domain;
using Account.DTO.Response;
using Account.DTO.Resquest;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCommonCode;
using ProjectCommonCode.Exceptions;

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
            if (request == null)
            {
                throw new BadRequestException("User request data cannot be null.");
            }

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
                throw new UserNotFoundException(id);
            }
            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PageResults<UserResponse>> GetAll(
            int pageNumber = 1,

            int pageSize = 10,

            string? search = null,

            string? sortBy = null,
            string? sortOrder = "desc"
            )
        {
            //var users = await _context.Users.AsNoTracking().ToListAsync();
            //return _mapper.Map<IEnumerable<UserResponse>>(users);


            var query = _context.Users.AsNoTracking().AsQueryable();


            //Search
            if(!String.IsNullOrWhiteSpace(search))
            {
                search= search.ToLower();


                query = query.Where(x =>
                 x.Name.ToLower().Contains(search) ||
                 x.Email.ToLower().Contains(search)
                );
            }




            ///Sorting
            ///

            query = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
            {
                ("id", "asc") => query.OrderBy(x => x.Id),
                ("id", "desc") => query.OrderByDescending(x => x.Id),
                 _ => query.OrderByDescending(x => x.Id)
            };


            var totalCount = await query.CountAsync();


            //pagination



            var data= await query
                     .Skip((pageNumber-1) * pageSize)
                     .Take(pageSize)
                     .Select(x => _mapper.Map<UserResponse>(x))
                     .ToListAsync();


            return new PageResults<UserResponse>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfRecords = totalCount,
                Results = data
            };





        }

        public async Task<UserResponse?> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> Update(int id, UserRequest request)
        {
            if (request == null)
            {
                throw new BadRequestException("User request data cannot be null.");
            }

            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
            {
                throw new UserNotFoundException(id);
            }
            userEntity.Name = request.Name;
            userEntity.Email = request.Email;
            
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<UserResponse>(userEntity);
        }
    }
}
