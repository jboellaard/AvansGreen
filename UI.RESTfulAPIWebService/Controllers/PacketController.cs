using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.RESTfulAPIWebService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/packets")]
    [ApiController]
    public class PacketController : ControllerBase
    {
        private readonly ILogger<PacketController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPacketService _packetService;

        public PacketController(ILogger<PacketController> logger,
            IPacketRepository packetRepository,
            IStudentRepository studentRepository,
            IPacketService packetService)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _studentRepository = studentRepository;
            _packetService = packetService;
        }

        [HttpPost("{id}/reservations")]
        public ActionResult<Packet> AddReservation(int id)
        {
            var user = User.Identity;
            if (user == null) return BadRequest("User not logged in.");
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                try
                {
                    Packet packet = _packetService.AddReservation(student, id);
                    if (packet != null) return Ok(packet);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("The logged in user could not be found in the student database.");
        }

        [HttpDelete("{id}/reservations")]
        public ActionResult<Packet> DeleteReservation(int id)
        {
            var user = User.Identity;
            if (user == null) return BadRequest("User not logged in.");
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                try
                {
                    Packet packet = _packetService.DeleteReservation(student, id);
                    if (packet != null) return Ok(packet);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("The logged in user could not be found in the student database.");
        }

    }
}
