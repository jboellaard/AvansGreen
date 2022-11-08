namespace UI.RESTfulAPIWebService.Models
{
    public class AuthenticationResponse
    {
        public bool Succes { get; set; }
        public string Token { get; set; }
        public int StudentId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
