namespace Core.Domain
{
    public class AGUser
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }

        public AGUser(string emailAddress, string name)
        {
            EmailAddress = emailAddress;
            Name = name;
        }

        public AGUser(string emailAddress) : this(emailAddress, emailAddress.Split("@")[0]) { }
    }
}
