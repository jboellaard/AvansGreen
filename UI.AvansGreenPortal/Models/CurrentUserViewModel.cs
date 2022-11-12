namespace UI.AvansGreenPortal.Models
{
    public class CurrentUserViewModel
    {
        public string Nr { get; set; } = "";

        public bool IsAuthenticated { get; set; } = false;

        public string UserType { get; set; } = "Anonymous";
    }
}
