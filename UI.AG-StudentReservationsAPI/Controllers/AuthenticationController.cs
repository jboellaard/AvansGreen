using Core.Domain;
using Core.DomainServices.IRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UI.AG_StudentReservationsAPI.Models;
using UI_Infra.Security;

namespace UI.AG_StudentReservations.Controllers
{
    /// <summary>
    /// Authentication for users of Avans Green
    /// </summary>
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

        /// <summary>
        /// Sign in for a student of Avans
        /// </summary>
        /// <remarks>
        /// Example input:
        /// 
        ///     POST /signin
        ///     {
        ///        "Nr": "s0000000",
        ///        "Password": "APassword"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns the bearer token that the user needs to manage reservations</response>
        /// <response code="400">Returned if the credentials are invalid</response>
        /// <response code="404">Returned if the user cannot be found in the student database</response>
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

        /// <summary>
        /// Sign out for a student
        /// </summary>
        /// <response code="200">Even if there is no signed in user, the method returns 200 as the result is no logged in users.</response>
        [HttpPost("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await _signInMgr.SignOutAsync();
            return Ok();
        }
    }
}
