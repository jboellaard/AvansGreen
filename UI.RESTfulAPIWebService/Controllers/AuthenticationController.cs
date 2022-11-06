using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UI.RESTfulAPIWebService.Models;
using UI.Security;

namespace UI.RESTfulAPIWebService.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AvansGreenUser> _userMgr;
        private readonly SignInManager<AvansGreenUser> _signInMgr;
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<AvansGreenUser> userMgr,
            SignInManager<AvansGreenUser> signInMgr, IStudentRepository studentRepository, IConfiguration configuration)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            _studentRepository = studentRepository;
            _configuration = configuration;
        }

        [HttpPost("api/signin")]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationCredentials authenticationCredentials)
        {
            var user = await _userMgr.FindByNameAsync(authenticationCredentials.Nr);
            if (user != null)
            {
                Student student = _studentRepository.GetByStudentNr(authenticationCredentials.Nr);
                if (student != null)
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

                        return Ok(new { Succes = true, Token = handler.WriteToken(securityToken), studentId = student.Id });
                    }
                }
                else
                {
                    return NotFound(new { Value = "This user could not be found in the student database." });
                }
            }
            return BadRequest("Password or username incorrect.");
        }

        [HttpPost("api/signout")]
        public new async Task<IActionResult> SignOut()
        {
            await _signInMgr.SignOutAsync();
            return Ok();
        }
    }
}
