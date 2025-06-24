namespace Deliverr.Models;
public partial class Account
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Details { get; set; }
    public string Email { get; set; }
    public Image ProfilePicture { get; set; }
    public Account(string username, string password, string details, string email, Image profilePicture)
    {
        Username = username;
        Password = password;
        Details = details;
        Email = email;
        ProfilePicture = profilePicture;
    }

    public Account()
    {

    }

}
