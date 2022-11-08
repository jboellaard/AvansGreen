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

    [Route("api")]
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

        /* 
         * Route for signing in. This endpoint is only usable for students of Avans, more specifically students that are in the Avans Green database.
         * Use the student Nr as "Nr" and password as "Password" to get a token for this user. Apart from the token the user is also logged in and their 
         * credentials are used in other calls.
         */
        /// <summary>
        /// Gets the price for a ticker symbol
        /// </summary>
        [HttpPost("signin")]
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

                        return Ok(new AuthenticationResponse { Succes = true, Token = handler.WriteToken(securityToken), StudentId = student.Id });
                    }
                }
                else
                {
                    return NotFound(new AuthenticationResponse { ErrorMessage = "This user could not be found in the student database." });
                }
            }
            return BadRequest(new AuthenticationResponse { ErrorMessage = "Password or username incorrect." });
        }

        [HttpPost("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await _signInMgr.SignOutAsync();
            return Ok();
        }
    }
}
