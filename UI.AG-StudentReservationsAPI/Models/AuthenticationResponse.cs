namespace UI.AG_StudentReservationsAPI.Models
{
    public class AuthenticationResponse
    {
        public bool Succes { get; set; }
        public string Token { get; set; }
        public int StudentId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
