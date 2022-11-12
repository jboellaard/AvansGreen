using Core.Domain;
using Core.DomainServices.IRepos;
using Core.DomainServices.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.AG_StudentReservationsAPI.Models;

namespace UI.AG_StudentReservationsAPI.Controllers
{
    /// <summary>
    /// Managing reservations of an authenticated student
    /// </summary>
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

        /// <summary>
        /// Getting reservations of logged in user
        /// </summary>
        /// <response code="200">Returns the list of packets this user has reservations for.</response>
        /// <response code="403">Returned if there is no logged in user</response>
        /// <response code="404">Returned if the user cannot be found in the student database</response>
        [HttpGet("reservations")]
        [Produces("application/json", "application/xml")]
        public ActionResult<List<PacketDTO>> GetReservations()
        {
            var user = User.Identity;
            if (user == null) return Unauthorized(new AuthenticationResponse { ErrorMessage = "User not logged in." });
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                List<PacketDTO> packets = new List<PacketDTO>();
                foreach (Packet packet in student.ReservedPackets)
                {
                    packets.Add(new PacketDTO(packet));
                }
                Response.StatusCode = 200;
                return packets;
            }
            return NotFound(new AuthenticationResponse { ErrorMessage = "This user could not be found in the student database." });
        }

        /// <summary>
        /// Add a reservation for a logged in user
        /// </summary>
        /// <remarks>
        /// The structure of this endpoint only needs the packet id of the packet that the logged in user wants to make a reservation for. 
        /// Because a packet only has one reservation at most, reservation is singular. The chosen method is POST (not PUT), because you create a reservation. 
        /// </remarks>
        /// <response code="200">Returns the packet if the reservation was succesful</response>
        /// <response code="403">Returned if the user is not authorized to create the reservation</response>
        /// <response code="404">Returned if the user cannot be found in the student database</response>
        /// <response code="500">Returned if there is an internal problem with the server and the reservation could not be made</response>
        [HttpPost("packets/{id}/reservation")]
        [Produces("application/json", "application/xml")]
        public ActionResult<PacketDTO> AddReservation(int id)
        {
            var user = User.Identity;
            if (user == null) return Unauthorized("User not logged in.");
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                try
                {
                    Packet packet = _packetService.AddReservation(student, id);
                    if (packet != null) return Ok(new PacketDTO(packet));
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }
            }
            return NotFound("The logged in user could not be found in the student database.");
        }

        /// <summary>
        /// Delete a reservation of a logged in user
        /// </summary>
        /// <remarks>
        /// The structure of this endpoint only needs the packet id of the packet that the logged in user wants to make a reservation for. 
        /// Because a packet only has one reservation at most, reservation is singular. The chosen method is DELETE (not PUT), because you delete a reservation. 
        /// </remarks>
        /// <response code="200">Returns the packet if removing the reservation was succesful</response>
        /// <response code="403">Returned if the user is not authorized to delete the reservation</response>
        /// <response code="404">Returned if the user cannot be found in the student database</response>
        /// <response code="500">Returned if there is an internal problem with the server and the reservation could not be deleted</response>
        [HttpDelete("packets/{id}/reservation")]
        [Consumes("application/json")]
        public ActionResult<PacketDTO> DeleteReservation(int id)
        {
            var user = User.Identity;

            if (user == null) return Unauthorized("User not logged in.");
            Student student = _studentRepository.GetByStudentNr(user.Name);
            if (student != null)
            {
                try
                {
                    Packet packet = _packetService.DeleteReservation(student, id);
                    if (packet != null) return Ok(new PacketDTO(packet));
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }
            }
            return NotFound("The logged in user could not be found in the student database.");
        }

    }
}
