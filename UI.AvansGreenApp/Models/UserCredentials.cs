namespace UI.AvansGreenApp.Models
{
    public class UserCredentials
    {
        public int Id { get; set; } = -1;
        public string Email { get; set; } = "";
        public TypeOfUser TypeOfUser { get; set; } = TypeOfUser.Anonymous;

        public UserCredentials(int id, string email, TypeOfUser typeOfUser)
        {
            Id = id;
            Email = email;
            TypeOfUser = typeOfUser;
        }

        public UserCredentials() { }
    }
}
