using Account.Data.Services.IServices;
using Account.DTO.Response;
using Account.DTO.Resquest;
using Microsoft.AspNetCore.Mvc;
using ProjectCommonCode;

namespace Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userService;

        public UserController(IUserInterface userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserRequest>> Create([FromBody] UserRequest request)
        {
            if (request == null)
            {
                return BadRequest("User data cannot be null");
            }
            
            var createdUser = await _userService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet]
        public async Task<ActionResult<PageResults<UserResponse>>> GetAll(
           int pageNumber = 1,

            int pageSize = 10,

            string? search = null,

            string? sortBy = null,
            string? sortOrder = "desc"
            )
        {
            var users = await _userService.GetAll(
                pageNumber, pageSize, search, sortBy, sortOrder
                );
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UserRequest request)
        {
            if (request == null)
            {
                return BadRequest("User data cannot be null");
            }

            var updatedUser = await _userService.Update(id, request);
            if (updatedUser == null)
            {
                return NotFound($"User with ID {id} not found");
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            if (!result)
            {
                return NotFound($"User with ID {id} not found");
            }
            return NoContent();
        }
    }
}
