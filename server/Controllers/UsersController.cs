using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.DTO;
using server.ViewModels;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly KanbanContext _context;

        public UsersController(KanbanContext context)
        {
            _context = context;
        }

        // POST /api/Users/token
        [HttpPost("token")]
        [Produces("application/json")]
        [SwaggerResponse(400, "Invalid username or password", typeof(ErrorViewModel))]
        public async Task<ActionResult<UserAccessViewModel>> Token(CreateTokenDTO dto)
        {
            var (user, identity) = (await GetIdentity(dto.Username, dto.Password)).GetValueOrDefault();

            if (identity == null)
            {
                return BadRequest(new ErrorViewModel { ErrorText = "Invalid username or password" });
            }

            var now = DateTime.UtcNow;

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encoded = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new UserAccessViewModel
            {
                AccessToken = encoded,
                Username = identity.Name,
                UserId = user.Id,
            }; ;
        }

        private async Task<ValueTuple<User, ClaimsIdentity>?> GetIdentity(string username, string password)
        {
            var person = await _context.Users.Where(x => x.Login == username && x.Password == password).FirstOrDefaultAsync();
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, "Token");

                return (person, identity);
            }

            // если пользователя не найдено
            return null;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
        {
            return await _context.Users
                .Select(o => new UserViewModel { Id = o.Id, Email = o.Email, Login = o.Login })
                .ToListAsync();
        }

        // GET: api/Users/:id
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserViewModel {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login
            };
        }

        // PUT api/Users/:id
        [HttpPatch("{id}")]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] PatchUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = dto.IsFieldPresent(nameof(user.Email)) ? dto.Email : user.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users/register
        [HttpPost("register")]
        [Produces("application/json")]
        [SwaggerResponse(409, "User already exists", typeof(ErrorViewModel))]
        public async Task<ActionResult<UserAccessViewModel>> PostUser([FromBody] RegisterUserDTO dto)
        {
            if (_context.Users.Any(o => o.Login == dto.Username || o.Email == dto.Email))
            {
                return Conflict(new ErrorViewModel { ErrorText = "User already exists" });
            }

            var user = new User
            {
                Email = dto.Email,
                Login = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var view = new UserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                Login = user.Login
            };

            return CreatedAtAction(nameof(Token), new { Username = user.Login, user.Password }, view);
        }


        // DELETE: api/Users/:id
        [HttpDelete("{id}")]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
