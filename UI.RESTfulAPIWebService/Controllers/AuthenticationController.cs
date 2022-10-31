using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UI.RESTfulAPIWebService.Models;

namespace UI.RESTfulAPIWebService.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userMgr;
        private readonly SignInManager<IdentityUser> _signInMgr;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr, IConfiguration configuration)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            _configuration = configuration;
        }

        [HttpPost("api/signin")]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationCredentials authenticationCredentials)
        {
            var user = await _userMgr.FindByNameAsync(authenticationCredentials.Nr);
            if (user != null)
            {
                if ((await _signInMgr.PasswordSignInAsync(user,
                    authenticationCredentials.Password, false, false)).Succeeded)
                {
                    var securityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = (await _signInMgr.CreateUserPrincipalAsync(user)).Identities.First(),
                        Expires = DateTime.Now.AddMinutes(int.Parse(_configuration["BearerTokens:ExpiryMinutes"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["BearerTokens:Key"])), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);

                    return Ok(new { Succes = true, Token = handler.WriteToken(securityToken) });
                }
            }

            return BadRequest();
        }

        [HttpPost("api/signout")]
        public async Task<IActionResult> SignOut()
        {
            await _signInMgr.SignOutAsync();
            return Ok();
        }
    }
}
