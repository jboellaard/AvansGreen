using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.RESTfulAPIWebService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class PacketReservationController : ControllerBase
    {
        private readonly ILogger<PacketReservationController> _logger;
        private readonly IPacketRepository _packetRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPacketService _packetService;

        public PacketReservationController(ILogger<PacketReservationController> logger,
            IPacketRepository packetRepository,
            IStudentRepository studentRepository,
            IPacketService packetService)
        {
            _logger = logger;
            _packetRepository = packetRepository;
            _studentRepository = studentRepository;
            _packetService = packetService;
        }

        [HttpGet("reservations")]
        public ActionResult<List<Packet>> GetReservations()
        {
            var user = User.Identity;
            if (user == null) return BadRequest("User not logged in.");
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                return student.ReservedPackets.ToList();
            }
            return BadRequest("The logged in user could not be found in the student database.");
        }


        [HttpPost("packets/{id}/reservation")]
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

        [HttpDelete("packets/{id}/reservation")]
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
